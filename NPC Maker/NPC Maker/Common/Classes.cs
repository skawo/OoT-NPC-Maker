using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Common
{ 
    public class ProgressReport
    {
        public string Status;
        public float Value;

        public ProgressReport(string status, float val)
        {
            Status = status;
            Value = val;
        }
    }

    public class CompilationEntryData
    {
        public int offset = 0;
        public int compressedSize = 0;
        public int decompressedSize = 0;
        public List<byte> data;

        public CompilationEntryData(List<byte> _data)
        {
            data = _data;
        }
    }

    public class PreprocessedEntry
    {
        public string identifier;
        public object data;

        public PreprocessedEntry(string st, object dt)
        {
            identifier = st;
            data = dt;
        }
    }

    public class HDefine
    {
        public string Name1;
        public string Value1String;
        public UInt32? Value1;
        public string Name2;
        public string Value2String;
        public UInt32? Value2;

        public HDefine(string _Name1, string _Value1Str, string _Name2 = "", string _Value2Str = "")
        {
            Name1 = _Name1;
            Value1String = _Value1Str;
            Name2 = _Name2;
            Value2String = _Value2Str;

            try
            {
                if (_Value1Str == "")
                    Value1 = null;
                else
                    Value1 = Helpers.HexConvertToUInt32(_Value1Str);

                if (_Value2Str == "")
                    Value2 = null;
                else
                    Value2 = Helpers.HexConvertToUInt32(_Value2Str);
            }
            catch (Exception)
            {
                Value1 = null;
                Value2 = null;
            }

        }

        public override string ToString()
        {
            if ((Value2 == null) && (Value1 == null))
                return "";
            else if (Value2 == null)
                return $"{Name1}";
            else if (Value1 == null)
                return $"{Name2};";
            else
                return $"{Name2};{Name1}";
        }
    }

    public class SavedMsgPreviewData
    {
        public System.Drawing.Image previewImage;
        public List<List<byte>> MessageArrays;
        public int Type;
        public int Position;
    }
}
