using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NPC_Maker
{
    public class MessageConfig
    {
        public List<Tag> Entries { get; set; }
        public int EndMessage { get; set; }

        public string EndMessageType { get; set; }

        public int NewLine { get; set; }

        public string NewLineType { get; set; }

        public MessageConfig()
        {
            EndMessage = 2;
            NewLine = 1;
            EndMessageType = "x";
            NewLineType = "x";
            Entries = new List<Tag>();
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Tag t = new Tag() { Token = $"<{Lists.ByteTagString}:x>", Value = "$1" };
            t.ProcessTag();
            Entries.Add(t);
        }

    }

    public class MessageDefinition
    {
        public string Identifier { get; set; }

        public string ValueType { get; set; }
        public Dictionary<string, int> Entries { get; set; }

        public MessageDefinition()
        {

        }
    }

    public class TagValueEntry
    {
        public UInt32? Value { get; set; }

        public string typeToken { get; set; }

        public int varNum { get; set; }

        public string defaultValue { get; set; }

        public TagValueEntry(UInt32? v, string tt, string dfVal = "", int vn = -1)
        {
            Value = v;
            typeToken = tt;
            varNum = vn;
            defaultValue = dfVal;
        }

        public ValueTypes GetValueType()
        {
            switch (typeToken)
            {
                case "x": return ValueTypes.x;
                case "h": return ValueTypes.h;
                case "t": return ValueTypes.t;
                case "w": return ValueTypes.w;
                default: return ValueTypes.d;
            }
        }

        public byte[] ToBytes()
        {
            if (Value == null)
                return new byte[0];
            else
            {
                ValueTypes type = GetValueType();

                switch (type)
                {
                    case ValueTypes.x: return new byte[1] { (byte)Value };
                    case ValueTypes.h: return Program.BEConverter.GetBytes((short)Value);
                    case ValueTypes.t:
                        {
                            byte[] vals = Program.BEConverter.GetBytes((int)Value);
                            return vals.Skip(1).ToArray();
                        }
                    case ValueTypes.w: return Program.BEConverter.GetBytes((int)Value);
                    default: throw new Exception($"The definition {typeToken} could not be resolved.");
                }
            }
        }
    }

    public class TagEntry
    {
        public string TagName { get; set; }
        public List<TagValueEntry> Values { get; set; }
        public bool isTag { get; set; }

        public TagEntry(string token, string vals, string defaultValues = "")
        {
            if (string.IsNullOrEmpty(token))
                return;

            try
            {
                InitializeFromToken(token, vals, defaultValues);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error parsing token {0}: {1}", token, ex.Message));
            }
        }

        private void InitializeFromToken(string token, string vals, string defaultValues)
        {
            // Check if it's a tag
            isTag = token.StartsWith("<") && token.EndsWith(">");

            // Parse token parts
            var subTokens = token.Split(':');
            if (subTokens.All(x => string.IsNullOrWhiteSpace(x)))
                throw new Exception("Could not find token name");

            // Process tokens and extract tag name
            for (int i = 0; i < subTokens.Length; i++)
            {
                subTokens[i] = Regex.Unescape(subTokens[i].TrimStart('<').TrimEnd('>'));
            }

            TagName = subTokens[0];
            var valueTokens = subTokens.Skip(1).ToArray();

            // Parse values
            ParseValues(vals, valueTokens, defaultValues);
        }

        private void ParseValues(string vals, string[] valueTokens, string defaultValues)
        {
            Values = new List<TagValueEntry>();

            var subValues = vals.Split(';');
            if (subValues.All(x => string.IsNullOrWhiteSpace(x)))
                return;

            var defaultValueTokens = string.IsNullOrEmpty(defaultValues)
                ? new string[0]
                : defaultValues.Split(';');

            foreach (var subValue in subValues)
            {
                var trimmedValue = subValue.Trim();
                if (string.IsNullOrEmpty(trimmedValue))
                    continue;

                if (trimmedValue.StartsWith("$"))
                {
                    ProcessTokenReference(trimmedValue, valueTokens, defaultValueTokens);
                }
                else
                {
                    ProcessDirectValue(trimmedValue);
                }
            }
        }

        private void ProcessTokenReference(string trimmedValue, string[] valueTokens, string[] defaultValueTokens)
        {
            int tokenIndex;
            if (!int.TryParse(trimmedValue.Substring(1), out tokenIndex))
                throw new Exception(string.Format("Invalid token reference: {0}", trimmedValue));

            tokenIndex--; // Convert to 0-based index

            if (tokenIndex < 0 || tokenIndex >= valueTokens.Length)
                throw new Exception(string.Format("Token reference {0} is out of range", trimmedValue));

            string defaultValue = tokenIndex < defaultValueTokens.Length
                ? defaultValueTokens[tokenIndex]
                : string.Empty;

            Values.Add(new TagValueEntry(null, valueTokens[tokenIndex], defaultValue, tokenIndex));
        }

        private void ProcessDirectValue(string trimmedValue)
        {
            byte value;

            if (Scripts.ScriptHelpers.IsHex(trimmedValue))
            {
                if (!byte.TryParse(trimmedValue.Substring(2), NumberStyles.HexNumber, null, out value))
                    throw new Exception(string.Format("Could not convert hex value {0} to byte", trimmedValue));
            }
            else
            {
                if (!byte.TryParse(trimmedValue, out value))
                    throw new Exception(string.Format("Could not convert {0} to byte", trimmedValue));
            }

            Values.Add(new TagValueEntry(value, ValueTypes.x.ToString()));
        }
    }


    public class Tag
    {
        public string Token { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string DefaultValues { get; set; }
        public string ContextGroup { get; set; }
        public string ContextDict { get; set; }
        public bool NewBoxSpecialHandling { get; set; }

        public string ContextName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TagEntry Entry { get; set; }

        public Tag()
        {
            Token = "";
            Value = "";
            Description = "";
            DefaultValues = "";
            ContextGroup = "";
            ContextDict = "";
            ContextName = "";
            NewBoxSpecialHandling = false;

        }

        public void ProcessTag()
        {
            Entry = new TagEntry(Token, Value, DefaultValues);
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ProcessTag();
        }

    }

    public enum ValueTypes
    {
        x,  // 1 byte
        h,  // 2 bytes
        t,  // 3 bytes
        w,  // 4 bytes
        d,  // definition
    }

    public class LocalizationEntry
    {
        public string Language { get; set; }
        public List<MessageEntry> Messages { get; set; }

        public LocalizationEntry()
        {
            Language = "";
            Messages = new List<MessageEntry>();
        }
    }

    public class MessageEntry
    {
        public string Name { get; set; }

        public string Comment { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string MessageText { get; set; }

        public List<string> MessageTextLines { get; set; }

        public int Type { get; set; }
        public int Position { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<byte> tempBytes { get; set; } // This is only temporarily used as a container during compile

        public MessageEntry()
        {
        }

        // Below taken and edited from 
        // https://github.com/Sage-of-Mirrors/Ocarina-Text-Editor

        public byte GetMessageTypePos()
        {
            byte Out = (byte)(Type << 4);
            return (byte)(Out | Position);
        }

        private string GetXString(byte Character)
        {
            return $"\\x{Character.ToString("X2")}\"\"";
        }

        public string ToCString(string Language)
        {
            return "Unimplemented";
        }

        public bool IsTag(string token)
        {
            return token.StartsWith("<") && token.EndsWith(">");
        }

        public int GetTagIndex(MessageConfig tagDict, string t, out string normalized)
        {
            bool isTag = IsTag(t);
            string workTag = isTag ? t.ToUpper().Replace(' ', '_').TrimStart('<').TrimEnd('>') : t;
            int tagIndex = -1;

            if (isTag)
                tagIndex = tagDict.Entries.FindIndex(x => x.Entry.TagName == workTag.Split(':')[0] && x.Entry.isTag);
            else
                tagIndex = tagDict.Entries.FindIndex(x => x.Entry.TagName == workTag && !x.Entry.isTag);

            normalized = workTag;
            return tagIndex;
        }

        public List<byte> ToBytes(string Language)
        {
            List<byte> data = new List<byte>();

            var tagDict = Dicts.LanguageDefs[Dicts.DefaultLanguage];
            if (Dicts.LanguageDefs.ContainsKey(Language))
                tagDict = Dicts.LanguageDefs[Language];

            var tokens = Tokenize();
            bool IgnoreNextLinebreak = false;

            for (int i = 0; i < tokens.Count; i++)
            {
                var t = tokens[i];

                if (t == "\n")
                {
                    if (IgnoreNextLinebreak)
                    {
                        IgnoreNextLinebreak = false;
                        continue;
                    }

                    if (tokens.Count() > i + 1)
                    {
                        int nextTagIndex = GetTagIndex(tagDict, tokens[i + 1], out _);

                        if (nextTagIndex != -1 && tagDict.Entries[nextTagIndex].NewBoxSpecialHandling)
                            continue;
                    }

                    TagValueEntry nL = new TagValueEntry((UInt32)tagDict.NewLine, tagDict.NewLineType);
                    data.AddRange(nL.ToBytes());
                    continue;
                }

                IgnoreNextLinebreak = false;
                int tagIndex = GetTagIndex(tagDict, t, out string normalized);

                if (tagIndex == -1)
                {
                    if (t.Length != 1)
                        throw new Exception($"Could not convert tag: {t}");
                    else
                    {
                        char curChar = t[0];
                        data.Add((byte)curChar);
                    }
                }
                else
                {
                    foreach (var val in tagDict.Entries[tagIndex].Entry.Values)
                    {
                        var workCopy = Helpers.Clone<TagValueEntry>(val);
                        ProcessTagValue(workCopy, normalized);

                        if (tagDict.Entries[tagIndex].NewBoxSpecialHandling)
                            IgnoreNextLinebreak = true;

                        data.AddRange(workCopy.ToBytes());
                    }
                }
            }

            TagValueEntry tg = new TagValueEntry((UInt32)tagDict.EndMessage, tagDict.EndMessageType);
            data.AddRange(tg.ToBytes());

            return data;
        }

        private void ProcessTagValue(TagValueEntry workCopy, string normalized)
        {
            ValueTypes vt = workCopy.GetValueType();

            if (vt == ValueTypes.d)
                ProcessDictionaryValue(workCopy, normalized);
            else if (workCopy.varNum != -1)
                ProcessVariableValue(workCopy, normalized);
        }

        private UInt32 GetValueFromDictOrConvert(Dictionary<string, int> dict, string normalizedToken, string chosenSubToken)
        {
            if (dict.ContainsKey(chosenSubToken))
                return (UInt32)dict[chosenSubToken];
            else
            {
                try
                {
                    return Helpers.HexConvertToUInt32(chosenSubToken);
                }
                catch
                {
                    throw new Exception($"Couldn't convert value: {normalizedToken}");
                }
            }
        }

        private void ProcessDictionaryValue(TagValueEntry workCopy, string normalized)
        {
            string[] subTokens = normalized.Split(':');

            if (workCopy.varNum + 1 > subTokens.Length)
                throw new Exception($"Malformed tag: {normalized}");

            string chosenSubToken = subTokens[workCopy.varNum + 1];

            if (workCopy.typeToken == Lists.SoundsDictType)
            {
                workCopy.typeToken = ValueTypes.h.ToString();
                workCopy.Value = GetValueFromDictOrConvert(Dicts.SFXes, normalized, chosenSubToken);
            }
            else
            {
                int defIdx = Dicts.MsgDefinitions.FindIndex(x => x.Identifier == workCopy.typeToken);

                if (defIdx == -1)
                    throw new Exception($"Couldn't find dictionary {workCopy.typeToken}");
                else
                {
                    var dict = Dicts.MsgDefinitions[defIdx];
                    workCopy.typeToken = dict.ValueType;
                    workCopy.Value = GetValueFromDictOrConvert(dict.Entries, normalized, chosenSubToken);
                }
            }
        }

        private void ProcessVariableValue(TagValueEntry workCopy, string normalized)
        {
            string[] subTokens = normalized.Split(':');
            string chosenSubToken;

            if (workCopy.varNum + 1 >= subTokens.Length)
            {
                if (String.IsNullOrWhiteSpace(workCopy.defaultValue))
                    throw new Exception($"Malformed tag: {normalized}");
                else
                    chosenSubToken = workCopy.defaultValue;
            }
            else
                chosenSubToken = subTokens[workCopy.varNum + 1];

            try
            {
                workCopy.Value = Helpers.HexConvertToUInt32(chosenSubToken);
            }
            catch
            {
                throw new Exception($"Couldn't convert value: {normalized}");
            }
        }

        private List<string> Tokenize()
        {
            var tokens = new List<string>();

            if (string.IsNullOrEmpty(MessageText))
                return tokens;

            int i = 0;

            while (i < MessageText.Length)
            {
                if (MessageText[i] == '<')
                {
                    // Look for the closing chevron
                    int closeIndex = MessageText.IndexOf('>', i);

                    if (closeIndex != -1)
                    {
                        // Extract the complete tag excluding chevrons
                        string tag = MessageText.Substring(i, closeIndex - i + 1);

                        if (tag.Contains("\n"))
                            throw new Exception($"Malformed tag: {tag.Substring(0, tag.IndexOf("\n"))}");

                        tokens.Add(tag);
                        i = closeIndex + 1;
                    }
                    else
                    {
                        // No closing chevron found, treat as single character
                        tokens.Add(MessageText[i].ToString());
                        i++;
                    }
                }
                else
                {
                    // Single character token
                    if (MessageText[i] != '\r')
                        tokens.Add(MessageText[i].ToString());

                    i++;
                }
            }

            return tokens;
        }
    }
}
