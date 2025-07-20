using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace PeepsCompress
{
    class YAZ0
    {
        public static byte[] Compress(byte[] file, int offset)
        {
            List<byte> layoutBits = new List<byte>();
            List<byte> dictionary = new List<byte>();

            List<byte> uncompressedData = new List<byte>();
            List<int[]> compressedData = new List<int[]>();

            int maxDictionarySize = 4096;
            int minMatchLength = 3;
            int maxMatchLength = 255 + 0x12;
            int decompressedSize = 0;

            for (int i = 0; i < file.Length; i++)
            {

                if (dictionary.Contains(file[i]))
                {
                    //compressed data
                    int[] matches = FindAllMatches(ref dictionary, file[i]);
                    int[] bestMatch = FindLargestMatch(ref dictionary, matches, ref file, i, maxMatchLength);

                    if (bestMatch[1] >= minMatchLength)
                    {
                        layoutBits.Add(0);
                        bestMatch[0] = dictionary.Count - bestMatch[0];

                        for (int j = 0; j < bestMatch[1]; j++)
                        {
                            dictionary.Add(file[i + j]);
                        }

                        i = i + bestMatch[1] - 1;

                        compressedData.Add(bestMatch);
                        decompressedSize += bestMatch[1];
                    }
                    else
                    {
                        //uncompressed data
                        layoutBits.Add(1);
                        uncompressedData.Add(file[i]);
                        dictionary.Add(file[i]);
                        decompressedSize++;
                    }
                }
                else
                {
                    //uncompressed data
                    layoutBits.Add(1);
                    uncompressedData.Add(file[i]);
                    dictionary.Add(file[i]);
                    decompressedSize++;
                }

                if (dictionary.Count > maxDictionarySize)
                {
                    int overflow = dictionary.Count - maxDictionarySize;
                    dictionary.RemoveRange(0, overflow);
                }
            }

            return BuildYaz0Block(ref layoutBits, ref uncompressedData, ref compressedData, decompressedSize, offset);
        }

        private static byte[] BuildYaz0Block(ref List<byte> layoutBits, ref List<byte> uncompressedData, ref List<int[]> offsetLengthPairs, int decompressedSize, int offset)
        {
            List<byte> finalYAZ0Block = new List<byte>();
            List<byte> layoutBytes = new List<byte>();
            List<byte> compressedDataBytes = new List<byte>();
            List<byte> extendedLengthBytes = new List<byte>();

            //add Yaz0 magic number
            finalYAZ0Block.AddRange(Encoding.ASCII.GetBytes("Yaz0"));

            byte[] decompressedSizeArray = BitConverter.GetBytes(decompressedSize);
            Array.Reverse(decompressedSizeArray);
            finalYAZ0Block.AddRange(decompressedSizeArray);

            //add 8 0's per format specification
            for (int i = 0; i < 8; i++)
            {
                finalYAZ0Block.Add(0);
            }

            //assemble layout bytes
            while (layoutBits.Count > 0)
            {
                while (layoutBits.Count < 8)
                {
                    layoutBits.Add(0);
                }

                string layoutBitsString = layoutBits[0].ToString() + layoutBits[1].ToString() + layoutBits[2].ToString() + layoutBits[3].ToString()
                        + layoutBits[4].ToString() + layoutBits[5].ToString() + layoutBits[6].ToString() + layoutBits[7].ToString();

                byte[] layoutByteArray = new byte[1];
                layoutByteArray[0] = Convert.ToByte(layoutBitsString, 2);
                layoutBytes.Add(layoutByteArray[0]);
                layoutBits.RemoveRange(0, (layoutBits.Count < 8) ? layoutBits.Count : 8);

            }

            //assemble offsetLength shorts
            foreach (int[] offsetLengthPair in offsetLengthPairs)
            {
                //if < 18, set 4 bits -2 as matchLength
                //if >= 18, set matchLength == 0, write length to new byte - 0x12

                int adjustedOffset = offsetLengthPair[0];
                int adjustedLength = (offsetLengthPair[1] >= 18) ? 0 : offsetLengthPair[1] - 2; //vital, 4 bit range is 0-15. Number must be at least 3 (if 2, when -2 is done, it will think it is 3 byte format), -2 is how it can store up to 17 without an extra byte because +2 will be added on decompression

                if (adjustedLength == 0)
                {
                    extendedLengthBytes.Add((byte)(offsetLengthPair[1] - 18));
                }

                int compressedInt = ((adjustedLength << 12) | adjustedOffset - 1);

                byte[] compressed2Byte = new byte[2];
                compressed2Byte[0] = (byte)(compressedInt & 0XFF);
                compressed2Byte[1] = (byte)((compressedInt >> 8) & 0xFF);

                compressedDataBytes.Add(compressed2Byte[1]);
                compressedDataBytes.Add(compressed2Byte[0]);
            }

            //add rest of file
            for (int i = 0; i < layoutBytes.Count; i++)
            {
                finalYAZ0Block.Add(layoutBytes[i]);

                BitArray arrayOfBits = new BitArray(new byte[1] { layoutBytes[i] });

                for (int j = 7; ((j > -1) && ((uncompressedData.Count > 0) || (compressedDataBytes.Count > 0))); j--)
                {
                    if (arrayOfBits[j] == true)
                    {
                        finalYAZ0Block.Add(uncompressedData[0]);
                        uncompressedData.RemoveAt(0);
                    }
                    else
                    {
                        if (compressedDataBytes.Count > 0)
                        {
                            int length = compressedDataBytes[0] >> 4;

                            finalYAZ0Block.Add(compressedDataBytes[0]);
                            finalYAZ0Block.Add(compressedDataBytes[1]);
                            compressedDataBytes.RemoveRange(0, 2);

                            if (length == 0)
                            {
                                finalYAZ0Block.Add(extendedLengthBytes[0]);
                                extendedLengthBytes.RemoveAt(0);
                            }
                        }
                    }
                }


            }

            return finalYAZ0Block.ToArray();
        }

        private static int[] FindAllMatches(ref List<byte> dictionary, byte match)
        {
            List<int> matchPositons = new List<int>();

            for (int i = 0; i < dictionary.Count; i++)
            {
                if (dictionary[i] == match)
                {
                    matchPositons.Add(i);
                }
            }

            return matchPositons.ToArray();
        }

        private static int[] FindLargestMatch(ref List<byte> dictionary, int[] matchesFound, ref byte[] file, int fileIndex, int maxMatch)
        {
            int[] matchSizes = new int[matchesFound.Length];

            for (int i = 0; i < matchesFound.Length; i++)
            {
                int matchSize = 1;
                bool matchFound = true;

                while (matchFound && matchSize < maxMatch && (fileIndex + matchSize < file.Length) && (matchesFound[i] + matchSize < dictionary.Count)) //NOTE: This could be relevant to compression issues? I suspect it's more related to writing
                {
                    if (file[fileIndex + matchSize] == dictionary[matchesFound[i] + matchSize])
                    {
                        matchSize++;
                    }
                    else
                    {
                        matchFound = false;
                    }

                }

                matchSizes[i] = matchSize;
            }

            int[] bestMatch = new int[2];

            bestMatch[0] = matchesFound[0];
            bestMatch[1] = matchSizes[0];

            for (int i = 1; i < matchesFound.Length; i++)
            {
                if (matchSizes[i] > bestMatch[1])
                {
                    bestMatch[0] = matchesFound[i];
                    bestMatch[1] = matchSizes[i];
                }
            }

            return bestMatch;

        }
    }
}

