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
    public enum MsgValueTypes
    {
        x,  // 1 byte
        h,  // 2 bytes
        t,  // 3 bytes
        w,  // 4 bytes
        d,  // definition
    }

    public class MessageConfig
    {
        public List<Tag> Entries { get; set; }
        public int EndMessage { get; set; }

        public string EndMessageType { get; set; }

        public int NewLine { get; set; }

        public string NewLineType { get; set; }

        public string ExtraFont { get; set; }

        public MessageConfig()
        {
            EndMessage = (int)ZeldaMessage.Data.MsgControlCode.END;
            NewLine = (int)ZeldaMessage.Data.MsgControlCode.LINE_BREAK;
            EndMessageType = MsgValueTypes.x.ToString();
            NewLineType = MsgValueTypes.x.ToString();
            ExtraFont = "";
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

        public string TypeToken { get; set; }

        public int VarNum { get; set; }

        public string DefaultValue { get; set; }

        public TagValueEntry(uint? value, string typeToken, string defaultValue = "", int varNum = -1)
        {
            Value = value;
            TypeToken = typeToken;
            VarNum = varNum;
            DefaultValue = defaultValue;
        }

        public MsgValueTypes GetValueType()
        {
            switch (TypeToken)
            {
                case "x": return MsgValueTypes.x;
                case "h": return MsgValueTypes.h;
                case "t": return MsgValueTypes.t;
                case "w": return MsgValueTypes.w;
                default: return MsgValueTypes.d;
            }
        }

        public byte[] ToBytes()
        {
            if (!Value.HasValue)
                return new byte[0];

            uint val = Value.Value;
            MsgValueTypes type = GetValueType();

            switch (type)
            {
                case MsgValueTypes.x:
                    return new byte[] { (byte)val };
                case MsgValueTypes.h:
                    return Program.BEConverter.GetBytes((short)val);
                case MsgValueTypes.t:
                    {
                        byte[] vals = Program.BEConverter.GetBytes((int)val);
                        byte[] result = new byte[vals.Length - 1];
                        Array.Copy(vals, 1, result, 0, result.Length);
                        return result;
                    }
                case MsgValueTypes.w:
                    return Program.BEConverter.GetBytes((int)val);
                default:
                    throw new Exception($"The definition {TypeToken} could not be resolved.");
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

            if (trimmedValue.IsHex())
            {
                if (!byte.TryParse(trimmedValue.Substring(2), NumberStyles.HexNumber, null, out value))
                    throw new Exception(string.Format("Could not convert hex value {0} to byte", trimmedValue));
            }
            else
            {
                if (!byte.TryParse(trimmedValue, out value))
                    throw new Exception(string.Format("Could not convert {0} to byte", trimmedValue));
            }

            Values.Add(new TagValueEntry(value, MsgValueTypes.x.ToString()));
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

            // Normalize tag if it is a "tag", else keep as is
            normalized = isTag ? t.Trim().TrimStart('<').TrimEnd('>').ToUpper().Replace(' ', '_') : t;

            // If tag contains a colon, only consider the part before it
            string searchTag = isTag ? normalized.Split(':')[0] : normalized;

            int tagIndex = tagDict.Entries.FindIndex(x => x.Entry.TagName == searchTag && x.Entry.isTag == isTag);

            return tagIndex;
        }

        public List<byte> ToBytes(string Language)
        {
            var data = new List<byte>();
            var tagDict = Dicts.LanguageDefs.TryGetValue(Language, out var dict)
                ? dict
                : Dicts.LanguageDefs[Dicts.DefaultLanguage];

            var tokens = Tokenize();
            bool ignoreNextLinebreak = false;

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                if (token == "\n")
                {
                    if (ignoreNextLinebreak)
                    {
                        ignoreNextLinebreak = false;
                        continue;
                    }

                    // Skip this linebreak if the next tag makes a new tag
                    // (This is so that this looks nicer in the editor)
                    if (i + 1 < tokens.Count &&
                        GetTagIndex(tagDict, tokens[i + 1], out _) is int nextTagIndex &&
                        nextTagIndex != -1 &&
                        tagDict.Entries[nextTagIndex].NewBoxSpecialHandling)
                    {
                        continue;
                    }

                    data.AddRange(new TagValueEntry((uint)tagDict.NewLine, tagDict.NewLineType).ToBytes());
                    continue;
                }

                ignoreNextLinebreak = false;
                int tagIndex = GetTagIndex(tagDict, token, out string normalized);

                if (tagIndex == -1)
                {
                    if (token.Length != 1)
                        throw new Exception($"Could not convert tag: {token}");

                    data.Add((byte)token[0]);
                }
                else
                {
                    var entry = tagDict.Entries[tagIndex];

                    foreach (var val in entry.Entry.Values)
                    {
                        var workCopy = Helpers.Clone<TagValueEntry>(val);
                        ProcessTagValue(workCopy, normalized);
                        data.AddRange(workCopy.ToBytes());
                    }

                    if (entry.NewBoxSpecialHandling)
                        ignoreNextLinebreak = true;
                }
            }

            data.AddRange(new TagValueEntry((uint)tagDict.EndMessage, tagDict.EndMessageType).ToBytes());
            return data;
        }

        private void ProcessTagValue(TagValueEntry workCopy, string normalized)
        {
            MsgValueTypes vt = workCopy.GetValueType();

            if (vt == MsgValueTypes.d)
                ProcessDictionaryValue(workCopy, normalized);
            else if (workCopy.VarNum != -1)
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

            if (workCopy.VarNum + 1 > subTokens.Length)
                throw new Exception($"Malformed tag: {normalized}");

            string chosenSubToken = subTokens[workCopy.VarNum + 1];

            if (workCopy.TypeToken == Lists.SoundsDictType)
            {
                workCopy.TypeToken = MsgValueTypes.h.ToString();
                workCopy.Value = GetValueFromDictOrConvert(Dicts.SFXes.Forward, normalized, chosenSubToken);
            }
            else
            {
                int defIdx = Dicts.MsgDefinitions.FindIndex(x => x.Identifier == workCopy.TypeToken);

                if (defIdx == -1)
                    throw new Exception($"Couldn't find dictionary {workCopy.TypeToken}");
                else
                {
                    var dict = Dicts.MsgDefinitions[defIdx];
                    workCopy.TypeToken = dict.ValueType;
                    workCopy.Value = GetValueFromDictOrConvert(dict.Entries, normalized, chosenSubToken);
                }
            }
        }

        private void ProcessVariableValue(TagValueEntry workCopy, string normalized)
        {
            string[] subTokens = normalized.Split(':');
            string chosenSubToken;

            if (workCopy.VarNum + 1 >= subTokens.Length)
            {
                if (String.IsNullOrWhiteSpace(workCopy.DefaultValue))
                    throw new Exception($"Malformed tag: {normalized}");
                else
                    chosenSubToken = workCopy.DefaultValue;
            }
            else
                chosenSubToken = subTokens[workCopy.VarNum + 1];

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
