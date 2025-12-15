using System;

// Translated from https://github.com/snailspeed3/RiiStudio/blob/master/source/szs/src/LibYaz.hpp

namespace RLibrii.Szs
{
    public static class Yaz0
    {
        private const uint YAZ0_MAGIC = 0x59617A30; // "Yaz0"

        private static void WriteU32(byte[] dst, int offset, uint value)
        {
            dst[offset + 0] = (byte)((value >> 24) & 0xFF);
            dst[offset + 1] = (byte)((value >> 16) & 0xFF);
            dst[offset + 2] = (byte)((value >> 8) & 0xFF);
            dst[offset + 3] = (byte)(value & 0xFF);
        }

        private static void CompressionSearch(byte[] src, int srcPos, int srcStart, int srcEnd, int maxLen,
                                              uint searchRange, out int foundPos, out int foundLen)
        {
            foundLen = 0;
            foundPos = -1;

            if (srcPos + 2 >= srcEnd)
                return;

            int search = srcPos - (int)searchRange;
            if (search < srcStart)
                search = srcStart;

            int cmpEnd = srcPos + maxLen;
            if (cmpEnd > srcEnd)
                cmpEnd = srcEnd;

            byte c1 = src[srcPos];

            while (search < srcPos)
            {
                // memchr equivalent
                while (search < srcPos && src[search] != c1)
                    search++;

                if (search >= srcPos)
                    break;

                int cmp1 = search + 1;
                int cmp2 = srcPos + 1;

                while (cmp2 < cmpEnd && src[cmp1] == src[cmp2])
                {
                    cmp1++;
                    cmp2++;
                }

                int len = cmp2 - srcPos;

                if (len > foundLen)
                {
                    foundLen = len;
                    foundPos = search;

                    if (foundLen == maxLen)
                        break;
                }

                search++;
            }
        }

        public static byte[] CompressYaz(byte[] src, byte level)
        {
            uint srcLen = (uint)src.Length;
            byte[] dst = new byte[src.Length * 2 + 16];

            WriteU32(dst, 0x0, YAZ0_MAGIC);
            WriteU32(dst, 0x4, srcLen);
            WriteU32(dst, 0x8, 0);
            WriteU32(dst, 0xC, 0);

            uint searchRange;
            if (level == 0)
                searchRange = 0;
            else if (level < 9)
                searchRange = (uint)(0x10E0 * level / 9 - 0x0E0);
            else
                searchRange = 0x1000;

            int srcPos = 0;
            int srcEnd = src.Length;

            int dstPos = 16;
            int maxLen = 18 + 255;

            if (searchRange == 0)
            {
                while (srcEnd - srcPos >= 8)
                {
                    dst[dstPos++] = 0xFF;
                    Array.Copy(src, srcPos, dst, dstPos, 8);
                    dstPos += 8;
                    srcPos += 8;
                }

                int delta = srcEnd - srcPos;
                if (delta > 0)
                {
                    dst[dstPos++] = (byte)(((1 << delta) - 1) << (8 - delta));
                    Array.Copy(src, srcPos, dst, dstPos, delta);
                    dstPos += delta;
                    srcPos += delta;
                }
            }
            else
            {
                CompressionSearch(src, srcPos, 0, srcEnd, maxLen, searchRange,
                                  out int foundPos, out int foundLen);

                int nextFoundPos = -1;
                int nextFoundLen = 0;

                while (srcPos < srcEnd)
                {
                    int codeBytePos = dstPos;
                    dst[dstPos++] = 0;

                    for (int i = 7; i >= 0; i--)
                    {
                        if (srcPos >= srcEnd)
                            break;

                        if (srcPos + 1 < srcEnd)
                        {
                            CompressionSearch(src, srcPos + 1, 0, srcEnd,
                                              maxLen, searchRange,
                                              out nextFoundPos, out nextFoundLen);
                        }

                        if (foundLen > 2 && nextFoundLen <= foundLen)
                        {
                            int delta = srcPos - foundPos - 1;

                            if (foundLen < 18)
                            {
                                dst[dstPos++] = (byte)((delta >> 8) | ((foundLen - 2) << 4));
                                dst[dstPos++] = (byte)(delta & 0xFF);
                            }
                            else
                            {
                                dst[dstPos++] = (byte)(delta >> 8);
                                dst[dstPos++] = (byte)(delta & 0xFF);
                                dst[dstPos++] = (byte)(foundLen - 18);
                            }

                            srcPos += foundLen;

                            CompressionSearch(src, srcPos, 0, srcEnd,
                                              maxLen, searchRange,
                                              out foundPos, out foundLen);
                        }
                        else
                        {
                            dst[codeBytePos] |= (byte)(1 << i);
                            dst[dstPos++] = src[srcPos++];

                            foundPos = nextFoundPos;
                            foundLen = nextFoundLen;
                        }
                    }
                }
            }

            Array.Resize(ref dst, dstPos);
            return dst;
        }
    }
}
