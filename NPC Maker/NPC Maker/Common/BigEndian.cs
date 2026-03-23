using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace NPC_Maker
{
    public static class BigEndian
    {
        public static short ToInt16(byte[] buffer, int index) =>
            BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan(index));
        public static int ToInt32(byte[] buffer, int index) =>
            BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan(index));
        public static uint ToUInt32(byte[] buffer, int index) =>
            BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan(index));
        public static ushort ToUInt16(byte[] buffer, int index) =>
            BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(index));
        public static byte[] GetBytes(int value)
        {
            var buf = new byte[4];
            BinaryPrimitives.WriteInt32BigEndian(buf, value);
            return buf;
        }
        public static byte[] GetBytes(uint value)
        {
            var buf = new byte[4];
            BinaryPrimitives.WriteUInt32BigEndian(buf, value);
            return buf;
        }
        public static byte[] GetBytes(ushort value)
        {
            var buf = new byte[2];
            BinaryPrimitives.WriteUInt16BigEndian(buf, value);
            return buf;
        }
        public static byte[] GetBytes(short value)
        {
            var buf = new byte[2];
            BinaryPrimitives.WriteInt16BigEndian(buf, value);
            return buf;
        }
        public static float ToSingle(byte[] buffer, int index) =>
            BinaryPrimitives.ReadSingleBigEndian(buffer.AsSpan(index));

        public static byte[] GetBytes(float value)
        {
            var buf = new byte[4];
            BinaryPrimitives.WriteSingleBigEndian(buf, value);
            return buf;
        }
    }
}
