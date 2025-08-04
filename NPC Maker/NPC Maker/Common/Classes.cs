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

    public class HDefine
    {
        public string Name;
        public string ValueString;
        public UInt32? Value;

        public HDefine(string _Name, string _ValueString)
        {
            Name = _Name;
            ValueString = _ValueString;

            try
            {
                if (Scripts.ScriptHelpers.IsHex(_ValueString))
                    Value = Convert.ToUInt32(_ValueString.Substring(2), 16);
                else
                    Value = Convert.ToUInt32(_ValueString);
            }
            catch (Exception)
            {
                Value = null;
            }

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
