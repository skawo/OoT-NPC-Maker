using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NPC_Maker
{
    public class NPCFile
    {
        public int Version { get; set; }
        public List<NPCEntry> Entries { get; set; }
        public List<ScriptEntry> GlobalHeaders { get; set; }
        public bool SpaceFromFont { get; set; }

        public string CHeader { get; set; }

        public List<string> CHeaderLines { get; set; }

        public NPCFile()
        {
            Version = 7;
            Entries = new List<NPCEntry>();
            GlobalHeaders = new List<ScriptEntry>();
            SpaceFromFont = false;
            CHeader = "";
            CHeaderLines = new List<string>();
        }
    }

    public class NPCEntry
    {
        public string NPCName { get; set; }
        public bool IsNull { get; set; }

        public UInt16 ObjectID { get; set; }
        public UInt32 Hierarchy { get; set; }

        public Int32 FileStart { get; set; }
        public byte HierarchyType { get; set; }
        public Int16[] ModelPositionOffsets { get; set; }
        public float ModelScale { get; set; }
        public byte CutsceneID { get; set; }

        public byte LookAtType { get; set; }

        public byte HeadLimb { get; set; }
        public byte HeadHorizAxis { get; set; }
        public byte HeadVertAxis { get; set; }

        public byte WaistLimb { get; set; }
        public byte WaistHorizAxis { get; set; }
        public byte WaistVertAxis { get; set; }

        public UInt16 LookAtDegreesVertical { get; set; }
        public UInt16 LookAtDegreesHorizontal { get; set; }
        public float[] LookAtPositionOffsets { get; set; }

        public bool HasCollision { get; set; }
        public bool PushesSwitches { get; set; }
        public byte Mass { get; set; }
        public bool IsAlwaysActive { get; set; }
        public bool IsAlwaysDrawn { get; set; }
        public bool ExecuteJustScript { get; set; }
        public bool ReactsIfAttacked { get; set; }
        public Int16 SfxIfAttacked { get; set; }
        public byte EffectIfAttacked { get; set; }
        public bool OpensDoors { get; set; }
        public UInt16 CollisionRadius { get; set; }
        public UInt16 CollisionHeight { get; set; }
        public Int16 CollisionYShift { get; set; }
        public Int16 NPCToRide { get; set; }

        public bool CastsShadow { get; set; }
        public UInt16 ShadowRadius { get; set; }
        public bool VisibleUnderLensOfTruth { get; set; }
        public bool Invisible { get; set; }

        public bool IsTargettable { get; set; }
        public bool FadeOut { get; set; }
        public byte Alpha { get; set; }
        public byte TargetDistance { get; set; }
        public byte TargetLimb { get; set; }
        public Int16[] TargetPositionOffsets { get; set; }
        public float TalkRadius { get; set; }

        public byte MovementType { get; set; }
        public UInt16 MovementDistance { get; set; }
        public float MovementSpeed { get; set; }
        public float GravityForce { get; set; }
        public byte PathID { get; set; }
        public Int16 PathStartNodeID { get; set; }
        public Int16 PathEndNodeID { get; set; }
        public UInt16 MovementDelayTime { get; set; }
        public bool LoopPath { get; set; }
        public bool IgnoreYAxis { get; set; }
        public float SmoothingConstant { get; set; }
        public UInt16 TimedPathStart { get; set; }
        public UInt16 TimedPathEnd { get; set; }
        public UInt16 MaxDistRoam { get; set; }

        public bool GenLight { get; set; }
        public bool Glow { get; set; }
        public byte LightLimb { get; set; }
        public Int16[] LightPositionOffsets { get; set; }
        public System.Drawing.Color LightColor { get; set; }
        public UInt16 LightRadius { get; set; }

        public byte AnimationType { get; set; }
        public List<AnimationEntry> Animations { get; set; }

        public List<ScriptEntry> Scripts { get; set; }

        public System.Drawing.Color EnvironmentColor { get; set; }
        public List<List<SegmentEntry>> Segments { get; set; }
        public string BlinkPattern { get; set; }
        public string TalkPattern { get; set; }
        public byte BlinkSegment { get; set; }
        public byte TalkSegment { get; set; }
        public byte BlinkSpeed { get; set; }
        public byte TalkSpeed { get; set; }
        public List<DListEntry> ExtraDisplayLists { get; set; }

        public List<ColorEntry> DisplayListColors { get; set; }

        public List<MessageEntry> Messages { get; set; }

        public bool DEBUGShowCols { get; set; }
        public bool DEBUGPrintToScreen { get; set; }
        public bool DEBUGLookAtEditor { get; set; }
        public bool DEBUGExDlistEditor { get; set; }

        public byte NumVars { get; set; }
        public byte NumFVars { get; set; }

        public bool ExistInAllRooms { get; set; }

        public float CullForward { get; set; }

        public float CullDown { get; set; }

        public float CullScale { get; set; }

        public byte AnimInterpFrames { get; set; }

        public CCodeEntry EmbeddedOverlayCode { get; set; }

        public NPCEntry()
        {
            NPCName = "";
            IsNull = false;

            ObjectID = 0;
            Hierarchy = 0;  // "Skeleton"
            HierarchyType = 0;  // "Draw type"
            ModelPositionOffsets = new Int16[] { 0, 0, 0 };
            ModelScale = 0.01f;

            LookAtType = 0;
            HeadLimb = 0;
            HeadHorizAxis = 0;
            HeadVertAxis = 0;
            WaistLimb = 0;
            WaistHorizAxis = 0;
            WaistVertAxis = 0;
            LookAtDegreesVertical = 0;
            LookAtDegreesHorizontal = 0;
            LookAtPositionOffsets = new float[] { 0, 0, 0 };

            ExistInAllRooms = false;
            HasCollision = false;
            FadeOut = false;
            Alpha = 255;
            PushesSwitches = false;
            Mass = 254;
            IsAlwaysActive = false;
            ExecuteJustScript = false;
            ReactsIfAttacked = false;
            SfxIfAttacked = -1;
            EffectIfAttacked = 10;
            OpensDoors = false;
            SmoothingConstant = 15.0f;
            IgnoreYAxis = true;
            CollisionRadius = 0;
            CollisionHeight = 0;
            CollisionYShift = 0;
            NPCToRide = -1;

            CastsShadow = false;
            ShadowRadius = 0;

            IsTargettable = false;
            TargetDistance = 1;
            TargetLimb = 0;
            TargetPositionOffsets = new Int16[] { 0, 0, 0 };
            TalkRadius = 150.0f;

            GenLight = false;
            Glow = false;
            LightLimb = 0;
            LightPositionOffsets = new Int16[] { 0, 0, 0 };
            LightColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            LightRadius = 0;

            MovementType = 0;
            MovementDistance = 0;
            MovementSpeed = 1.0f;
            GravityForce = 0.1f;
            PathID = 0;
            PathStartNodeID = -1;
            PathEndNodeID = -1;
            MovementDelayTime = 0;
            LoopPath = false;
            TimedPathStart = Helpers.GetOcarinaTime("00:00");
            TimedPathEnd = Helpers.GetOcarinaTime("23:59");
            MaxDistRoam = 65535;
            CullForward = 1000.0f;
            CullDown = 700.0f;
            CullScale = 350.0f;

            Animations = new List<AnimationEntry>();
            AnimationType = 0;

            Scripts = new List<ScriptEntry>();

            Segments = new List<List<SegmentEntry>>();

            Messages = new List<MessageEntry>();

            EnvironmentColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            BlinkPattern = "";
            TalkPattern = "";
            BlinkSegment = 7;
            TalkSegment = 7;
            BlinkSpeed = 1;
            TalkSpeed = 1;

            ExtraDisplayLists = new List<DListEntry>();
            DisplayListColors = new List<ColorEntry>();

            VisibleUnderLensOfTruth = false;
            Invisible = false;
            DEBUGShowCols = false;
            DEBUGLookAtEditor = false;
            DEBUGPrintToScreen = false;
            DEBUGExDlistEditor = false;

            FileStart = 0;

            NumVars = 2;
            NumFVars = 2;

            AnimInterpFrames = 4;

            EmbeddedOverlayCode = new CCodeEntry();
        }

        public List<OutputColorEntry> ParseColorEntries()
        {
            List<OutputColorEntry> Out = new List<OutputColorEntry>();

            foreach (ColorEntry CE in this.DisplayListColors)
            {
                if (CE.Limbs != "")
                {
                    if (Out.Find(x => x.LimbID == Convert.ToByte(CE.Limbs)) == null)
                        Out.Add(new OutputColorEntry(Convert.ToByte(CE.Limbs), CE.Color.R, CE.Color.G, CE.Color.B));
                }
            }

            return Out;
        }

        public enum Members
        {
            NOMEMBER,
            NPCNAME,
            OBJID,
            HIERARCHY,
            HIERARCHYTYPE,
            XMODELOFFS,
            YMODELOFFS,
            ZMODELOFFS,
            SCALE,
            CUTSCENEID,
            LOOKATTYPE,
            HEADLIMB,
            HEADVERTAXIS,
            HEADHORIZAXIS,
            WAISTLIMB,
            WAISTVERTAXIS,
            WAISTHORIZAXIS,
            DEGVERT,
            DEGHOZ,
            XLOOKATOFFS,
            YLOOKATOFFS,
            ZLOOKATOFFS,
            COLLISION,
            SWITCHES,
            MASS,
            COLRADIUS,
            COLHEIGHT,
            SHADOW,
            SHADOWRADIUS,
            YCOLOFFS,
            TARGETTABLE,
            TARGETLIMB,
            XTARGETOFFS,
            YTARGETOFFS,
            ZTARGETOFFS,
            MOVEMENT,
            MOVDISTANCE,
            MOVSPEED,
            PATHID,
            PATHSTARTID,
            PATHENDID,
            MOVDEL,
            LOOP,
            ANIMTYPE,
            BLINKPAT,
            TALKPAT,
            BLINKSEG,
            TALKSEG,
            BLINKSPE,
            TALKSPE,
            ACTIVE,
            DRAWOUTOFCAM,
            TARGETDIST,
            JUSTSCRIPT,
            REACTATT,
            TALKRADIUS,
            TIMEDPATH,
            PATHSTARTTIME,
            PATHENDTIME,
            GRAVITYFORCE,
            OPENDOORS,
            IGNORENODEYAXIS,
            SMOOTH,
            SFXIFATT,
            EFFIFATT,
            FADEOUT,
            ALPHA,
            NPCTORIDE,
            LIGHT,
            GLOW,
            LIGHTLIMB,
            XLIGHTOFFS,
            YLIGHTOFFS,
            ZLIGHTOFFS,
            DEBUGSHOWCOLS,
            LIGHTRADIUS,
            VISIBLEONLYLENS,
            INVISIBLE,
            SCRIPTVARS,
            SCRIPTFVARS,
            ROAMMAX,
            EXISTALLROOMS,
            FILESTART,
            DEBUGLOOKAT,
            DEBUGPRINTSCR,
            DEBUGDLISTED,
            CULLFWD,
            CULLDWN,
            CULLSCALE,
            ANIMINTERPFRAMES,
        }

        public static Members GetMemberFromTag(object Tag, string PassingObjectName)
        {
            Members Member;

            try
            {
                if (Tag is string t)
                    Member = (Members)Enum.Parse(typeof(NPCEntry.Members), t);
                else if (Tag is Members m)
                    Member = m;
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                Member = Members.NOMEMBER;
                System.Windows.Forms.MessageBox.Show($"Warning: {PassingObjectName} tag is incorrect!");
            }

            return Member;
        }

        public void ChangeValueOfMember(Members Member, object Value)
        {
            switch (Member)
            {
                case Members.NPCNAME: NPCName = (string)Value; break;

                case Members.OBJID: ObjectID = Convert.ToUInt16(Value); break;
                case Members.HIERARCHY: Hierarchy = Convert.ToUInt32(Value); break;
                case Members.HIERARCHYTYPE: HierarchyType = Convert.ToByte(Value); break;
                case Members.XMODELOFFS: ModelPositionOffsets[0] = Convert.ToInt16(Value); break;
                case Members.YMODELOFFS: ModelPositionOffsets[1] = Convert.ToInt16(Value); break;
                case Members.ZMODELOFFS: ModelPositionOffsets[2] = Convert.ToInt16(Value); break;
                case Members.SCALE: ModelScale = (float)Convert.ToDecimal(Value); break;
                case Members.CUTSCENEID: CutsceneID = Convert.ToByte(Value); break;

                case Members.LOOKATTYPE: LookAtType = Convert.ToByte(Value); break;
                case Members.HEADLIMB: HeadLimb = Convert.ToByte(Value); break;
                case Members.HEADVERTAXIS: HeadVertAxis = Convert.ToByte(Value); break;
                case Members.HEADHORIZAXIS: HeadHorizAxis = Convert.ToByte(Value); break;
                case Members.WAISTLIMB: WaistLimb = Convert.ToByte(Value); break;
                case Members.WAISTVERTAXIS: WaistVertAxis = Convert.ToByte(Value); break;
                case Members.WAISTHORIZAXIS: WaistHorizAxis = Convert.ToByte(Value); break;
                case Members.DEGVERT: LookAtDegreesVertical = Convert.ToUInt16(Value); break;
                case Members.DEGHOZ: LookAtDegreesHorizontal = Convert.ToUInt16(Value); break;
                case Members.XLOOKATOFFS: LookAtPositionOffsets[0] = (float)Convert.ToDecimal(Value); break;
                case Members.YLOOKATOFFS: LookAtPositionOffsets[1] = (float)Convert.ToDecimal(Value); break;
                case Members.ZLOOKATOFFS: LookAtPositionOffsets[2] = (float)Convert.ToDecimal(Value); break;

                case Members.COLLISION: HasCollision = Convert.ToBoolean(Value); break;
                case Members.SWITCHES: PushesSwitches = Convert.ToBoolean(Value); break;
                case Members.MASS: Mass = Convert.ToByte(Value); break;
                case Members.COLRADIUS: CollisionRadius = Convert.ToUInt16(Value); break;
                case Members.COLHEIGHT: CollisionHeight = Convert.ToUInt16(Value); break;

                case Members.SHADOW: CastsShadow = Convert.ToBoolean(Value); break;
                case Members.SHADOWRADIUS: ShadowRadius = Convert.ToUInt16(Value); break;
                case Members.YCOLOFFS: CollisionYShift = Convert.ToInt16(Value); break;

                case Members.TARGETTABLE: IsTargettable = Convert.ToBoolean(Value); break;
                case Members.TARGETLIMB: TargetLimb = Convert.ToByte(Value); break;
                case Members.XTARGETOFFS: TargetPositionOffsets[0] = Convert.ToInt16(Value); break;
                case Members.YTARGETOFFS: TargetPositionOffsets[1] = Convert.ToInt16(Value); break;
                case Members.ZTARGETOFFS: TargetPositionOffsets[2] = Convert.ToInt16(Value); break;

                case Members.MOVEMENT: MovementType = Convert.ToByte(Value); break;
                case Members.MOVDISTANCE: MovementDistance = Convert.ToUInt16(Value); break;
                case Members.MOVSPEED: MovementSpeed = (float)Convert.ToDecimal(Value); break;
                case Members.PATHID: PathID = Convert.ToByte(Value); break;
                case Members.PATHSTARTID: PathStartNodeID = Convert.ToInt16(Value); break;
                case Members.PATHENDID: PathEndNodeID = Convert.ToInt16(Value); break;
                case Members.MOVDEL: MovementDelayTime = Convert.ToUInt16(Value); break;
                case Members.LOOP: LoopPath = Convert.ToBoolean(Value); break;
                case Members.IGNORENODEYAXIS: IgnoreYAxis = Convert.ToBoolean(Value); break;

                case Members.ANIMTYPE: AnimationType = Convert.ToByte(Value); break;

                case Members.BLINKPAT: BlinkPattern = Convert.ToString(Value); break;
                case Members.TALKPAT: TalkPattern = Convert.ToString(Value); break;
                case Members.BLINKSEG: BlinkSegment = Convert.ToByte(Value); break;
                case Members.TALKSEG: TalkSegment = Convert.ToByte(Value); break;
                case Members.BLINKSPE: BlinkSpeed = Convert.ToByte(Value); break;
                case Members.TALKSPE: TalkSpeed = Convert.ToByte(Value); break;

                case Members.ACTIVE: IsAlwaysActive = Convert.ToBoolean(Value); break;
                case Members.DRAWOUTOFCAM: IsAlwaysDrawn = Convert.ToBoolean(Value); break;
                case Members.TARGETDIST: TargetDistance = Convert.ToByte(Value); break;

                case Members.JUSTSCRIPT: ExecuteJustScript = Convert.ToBoolean(Value); break;
                case Members.REACTATT: ReactsIfAttacked = Convert.ToBoolean(Value); break;
                case Members.OPENDOORS: OpensDoors = Convert.ToBoolean(Value); break;

                case Members.TALKRADIUS: TalkRadius = Convert.ToUInt16(Value); break;
                case Members.GRAVITYFORCE: GravityForce = (float)Convert.ToDecimal(Value); break;

                case Members.PATHSTARTTIME: TimedPathStart = Helpers.GetOcarinaTime((string)Value); break;
                case Members.PATHENDTIME: TimedPathEnd = Helpers.GetOcarinaTime((string)Value); break;
                case Members.SMOOTH: SmoothingConstant = (float)Convert.ToDecimal(Value); break;
                case Members.SFXIFATT: SfxIfAttacked = Convert.ToInt16(Value); break;
                case Members.EFFIFATT: EffectIfAttacked = Convert.ToByte(Value); break;
                case Members.FADEOUT: FadeOut = Convert.ToBoolean(Value); break;
                case Members.ALPHA: Alpha = Convert.ToByte(Value); break;
                case Members.NPCTORIDE: NPCToRide = Convert.ToInt16(Value); break;

                case Members.GLOW: Glow = Convert.ToBoolean(Value); break;
                case Members.LIGHT: GenLight = Convert.ToBoolean(Value); break;
                case Members.LIGHTLIMB: LightLimb = Convert.ToByte(Value); break;
                case Members.XLIGHTOFFS: LightPositionOffsets[0] = Convert.ToInt16(Value); break;
                case Members.YLIGHTOFFS: LightPositionOffsets[1] = Convert.ToInt16(Value); break;
                case Members.ZLIGHTOFFS: LightPositionOffsets[2] = Convert.ToInt16(Value); break;
                case Members.DEBUGSHOWCOLS: DEBUGShowCols = Convert.ToBoolean(Value); break;
                case Members.LIGHTRADIUS: LightRadius = Convert.ToUInt16(Value); break;
                case Members.VISIBLEONLYLENS: VisibleUnderLensOfTruth = Convert.ToBoolean(Value); break;
                case Members.INVISIBLE: Invisible = Convert.ToBoolean(Value); break;

                case Members.SCRIPTVARS: NumVars = Convert.ToByte(Value); break;
                case Members.SCRIPTFVARS: NumFVars = Convert.ToByte(Value); break;

                case Members.ROAMMAX: MaxDistRoam = Convert.ToUInt16(Value); break;
                case Members.EXISTALLROOMS: ExistInAllRooms = Convert.ToBoolean(Value); break;
                case Members.FILESTART: FileStart = Convert.ToInt32(Value); break;
                case Members.DEBUGLOOKAT: DEBUGLookAtEditor = Convert.ToBoolean(Value); break;
                case Members.DEBUGPRINTSCR: DEBUGPrintToScreen = Convert.ToBoolean(Value); break;
                case Members.DEBUGDLISTED: DEBUGExDlistEditor = Convert.ToBoolean(Value); break;

                case Members.CULLFWD: CullForward = (float)Convert.ToDecimal(Value); break;
                case Members.CULLDWN: CullDown = (float)Convert.ToDecimal(Value); break;
                case Members.CULLSCALE: CullScale = (float)Convert.ToDecimal(Value); break;

                case Members.ANIMINTERPFRAMES: AnimInterpFrames = Convert.ToByte(Value); break;

                default: break;
            }
        }
    }

    public class ScriptEntry
    {
        public string Text { get; set; }

        public List<string> TextLines { get; set; }

        public List<string> ParseErrors { get; set; }
        public string Name { get; set; }

        public ScriptEntry()
        {
            Text = "";
            TextLines = new List<string>();
            ParseErrors = new List<string>();
            Name = "Script";
        }
    }

    public class AnimationEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public Int16 ObjID { get; set; }
        public float Speed { get; set; }
        public byte StartFrame { get; set; }
        public byte EndFrame { get; set; }

        public Int32 FileStart { get; set; }

        public AnimationEntry()
        {
            Name = "";
            Address = 0;
            Speed = 1.0f;
            ObjID = -1;
            StartFrame = 0;
            EndFrame = 0xFF;
            FileStart = -1;
        }
        public AnimationEntry(string _Name, UInt32 _Address, float _Speed, Int16 _ObjectID, byte _StartFrame, byte _EndFrame, Int32 _FileStart)
        {
            Name = _Name;
            Address = _Address;
            Speed = _Speed;
            ObjID = _ObjectID;
            StartFrame = _StartFrame;
            EndFrame = _EndFrame;
            FileStart = _FileStart;
        }
    }

    public class SegmentEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public Int16 ObjectID { get; set; }

        public Int32 FileStart { get; set; }

        public SegmentEntry()
        {
            Name = "";
            Address = 0;
            ObjectID = -1;
            FileStart = -1;
        }
        public SegmentEntry(string _Name, UInt32 _Address, Int16 _ObjectID, Int32 _FileStart)
        {
            Name = _Name;
            Address = _Address;
            ObjectID = _ObjectID;
            FileStart = _FileStart;
        }
    }

    public class OutputColorEntry
    {
        public byte LimbID { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public OutputColorEntry()
        {
            LimbID = 255;
            R = 255;
            G = 255;
            B = 255;
        }
        public OutputColorEntry(byte _LimbID, byte _R, byte _G, byte _B)
        {
            LimbID = _LimbID;
            R = _R;
            G = _G;
            B = _B;
        }
    }

    public class ColorEntry
    {
        public System.Drawing.Color Color { get; set; }
        public string Limbs { get; set; }

        public ColorEntry()
        {
            Color = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            Limbs = "";
        }
        public ColorEntry(string _Limb, System.Drawing.Color _Color)
        {
            Color = _Color;
            Limbs = _Limb;
        }
    }

    public class MessageEntry
    {
        public string Name { get; set; }
        public string MessageText { get; set; }

        public List<string> MessageTextLines { get; set; }

        public int Type { get; set; }
        public int Position { get; set; }

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

        public List<byte> ConvertTextData(string NPCName, bool ShowErrors = true)
        {
            List<byte> data = new List<byte>();
            List<string> errors = new List<string>();

            string wMessageText = MessageText;

            foreach (var s in Dicts.MsgTagOverride)
                wMessageText = wMessageText.Replace(s.Key, s.Value);

            for (int i = 0; i < wMessageText.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (wMessageText[i] != '<' && wMessageText[i] != '>')
                {
                    if (Dicts.MessageControlCodes.ContainsValue(wMessageText[i].ToString()))
                        data.Add((byte)Dicts.MessageControlCodes.First(x => x.Value == wMessageText[i].ToString()).Key);
                    else if (wMessageText[i] == '\n')
                        data.Add((byte)Lists.MsgControlCode.LINE_BREAK);
                    else if (wMessageText[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                        data.Add((byte)wMessageText[i]);

                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (wMessageText[i] == '>')
                    errors.Add($"Message formatting is not valid: found stray >");
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (wMessageText[i] != '>' && i < wMessageText.Length - 1)
                    {
                        // Add code chars to the buffer
                        controlCode.Add(wMessageText[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    if (controlCode.Count == 0)
                        continue;

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());
                    string parsedFixed = parsedCode.Split(':')[0].Replace(" ", "_").ToUpper();

                    if (parsedFixed == Lists.MsgControlCode.NEW_BOX.ToString() || parsedFixed == Lists.MsgControlCode.DELAY.ToString())
                    {
                        if (data.Count != 0)
                            if (data[data.Count - 1] == 0x01)
                                data.RemoveAt(data.Count - 1);

                        if (wMessageText.Length > i + Environment.NewLine.Length)
                        {
                            string s;

                            if (Environment.NewLine.Length == 2)
                                s = String.Concat(wMessageText[i + 1], wMessageText[i + 2]);
                            else
                                s = String.Concat(wMessageText[i + 1]);

                            if (s == Environment.NewLine)
                            {
                                i += Environment.NewLine.Length; // Skips next linebreak
                            }
                        }
                    }

                    data.AddRange(GetControlCode(parsedCode.Split(':'), ref errors));
                }
            }

            data.Add((byte)Lists.MsgControlCode.END);

            if (errors.Count != 0)
            {
                if (ShowErrors)
                    System.Windows.Forms.MessageBox.Show($"Errors parsing message \"{Name}\" in actor \"{NPCName}\": " + Environment.NewLine + String.Join(Environment.NewLine, errors.ToArray()));

                Console.Write($"{Environment.NewLine}{Environment.NewLine}Errors parsing message \"{Name}\":{Environment.NewLine}{String.Join(Environment.NewLine, errors.ToArray())}{Environment.NewLine}");
            }


            if (errors.Count == 0)

                return data;
            else
                return null;
        }

        private List<byte> GetControlCode(string[] code, ref List<string> errors)
        {
            List<byte> output = new List<byte>();

            try
            {
                for (int i = 0; i < code.Length; i++)
                    code[i] = code[i].Replace(" ", "_").ToUpper();

                switch (code[0])
                {
                    case "PIXELS_RIGHT":
                        {
                            output.Add((byte)Lists.MsgControlCode.SHIFT);
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "JUMP":
                        {
                            /* Jump will not work for messages like this.
                            output.Add((byte)Lists.MsgControlCode.JUMP);
                            byte[] jumpIDBytes = BitConverter.GetBytes(short.Parse(code[1], System.Globalization.NumberStyles.HexNumber));
                            output.Add(jumpIDBytes[1]);
                            output.Add(jumpIDBytes[0]);
                            */
                            break;
                        }
                    case "DELAY":
                    case "FADE":
                    case "SHIFT":
                    case "SPEED":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgControlCode), code[0]));
                            output.Add(Convert.ToByte(code[1]));
                            break;
                        }
                    case "FADE2":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgControlCode), code[0]));
                            short soundValue = Convert.ToInt16(code[1]);
                            output.AddRangeBigEndian(soundValue);
                            break;
                        }
                    case "ICON":
                        {
                            output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgControlCode), code[0]));
                            output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgIcon), code[1]));
                            break;
                        }
                    case "BACKGROUND":
                        {
                            output.Add((byte)Lists.MsgControlCode.BACKGROUND);
                            output.AddRange(Program.BEConverter.GetBytes(Convert.ToUInt32(code[1])).Skip(1).Take(3).ToArray());
                            break;
                        }
                    case "HIGH_SCORE":
                        {
                            output.Add((byte)Lists.MsgControlCode.HIGH_SCORE);
                            output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgHighScore), code[1]));
                            break;
                        }
                    case "PAUSE":
                        {
                            // Fake tag to facilitate text pauses
                            // PA:TextCharPrintSpeed:NumOfPauseTextChars>
                            output.Add((byte)Lists.MsgControlCode.SPEED);

                            // Pause length (This number * number of DC characters)
                            if (code.Length == 2)
                                output.Add(Convert.ToByte(code[1]));
                            else
                                output.Add(Convert.ToByte(3));

                            // Number of DC characters
                            UInt32 DCCharsCount = 3;

                            if (code.Length == 3)
                                DCCharsCount = Convert.ToUInt32(code[2]);

                            for (int i = 0; i < DCCharsCount; i++)
                                output.Add((byte)Lists.MsgControlCode.DC);

                            output.Add((byte)Lists.MsgControlCode.SPEED);
                            output.Add(Convert.ToByte(0));
                            break;
                        }
                    case "SOUND":
                        {
                            output.Add((byte)Lists.MsgControlCode.SOUND);

                            if (Dicts.SFXes.ContainsKey(code[1]))
                            {
                                short soundValue = (short)Dicts.SFXes[code[1]];
                                output.AddRangeBigEndian(soundValue);
                            }
                            else
                            {
                                try
                                {
                                    short soundValue = Convert.ToInt16(code[1]);
                                    output.AddRangeBigEndian(soundValue);
                                }
                                catch (Exception)
                                {
                                    errors.Add($"{code[1]} is not a valid sound.");
                                    output.AddRangeBigEndian((UInt16)0);
                                }
                            }
                            break;
                        }
                    case "BYTE":
                        {
                            try
                            {
                                byte b = 0;

                                if (NPC_Maker.Scripts.ScriptHelpers.IsHex(code[1]))
                                    b = Convert.ToByte(code[1].ToUpper().Replace("0X", ""), 16);
                                else
                                    b = Convert.ToByte(code[1]);

                                output.Add(b);
                            }
                            catch (Exception)
                            {
                                errors.Add($"{code[1]} is not a valid byte.");
                                output.AddRangeBigEndian((UInt16)0);
                            }
                            break;
                        }
                    default:
                        {
                            if (Enum.IsDefined(typeof(Lists.MsgColor), code[0]))
                            {
                                output.Add((byte)Lists.MsgControlCode.COLOR);
                                output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgColor), code[0]));
                            }
                            else if (Enum.IsDefined(typeof(Lists.MsgControlCode), code[0]))
                                output.Add((byte)(int)Enum.Parse(typeof(Lists.MsgControlCode), code[0]));
                            else
                                errors.Add($"{code[0]} is not a valid control code.");

                            break;
                        }
                }
            }
            catch (Exception)
            {
                errors.Add($"{String.Join(":", code)} could not be parsed.");
            }

            return output;
        }
    }

    public class DListEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public float TransX { get; set; }
        public float TransY { get; set; }
        public float TransZ { get; set; }
        public Int16 RotX { get; set; }
        public Int16 RotY { get; set; }
        public Int16 RotZ { get; set; }
        public float Scale { get; set; }
        public int ShowType { get; set; }
        public Int16 Limb { get; set; }
        public System.Drawing.Color Color { get; set; }
        public Int16 ObjectID { get; set; }

        public Int32 FileStart { get; set; }

        public DListEntry()
        {
            Name = "";
            Address = 0;
            TransX = 0;
            TransY = 0;
            TransZ = 0;
            RotX = 0;
            RotY = 0;
            RotZ = 0;
            Scale = 0;
            ShowType = 0;
            Limb = 0;
            ObjectID = -1;
            Color = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            FileStart = -1;
        }
        public DListEntry(string _Name, UInt32 _Address, float _TransX, float _TransY, float _TransZ, System.Drawing.Color _Color,
                          Int16 _RotX, Int16 _RotY, Int16 _RotZ, float _Scale, Int16 _Limb, int _ShowType, Int16 _ObjectID, Int32 _FileStart)
        {
            Name = _Name;
            Address = _Address;
            TransX = _TransX;
            TransY = _TransY;
            TransZ = _TransZ;
            RotX = _RotX;
            RotY = _RotY;
            RotZ = _RotZ;
            Scale = _Scale;
            Limb = _Limb;
            ShowType = _ShowType;
            Color = _Color;
            ObjectID = _ObjectID;
            FileStart = _FileStart;
        }
    }

    public class CCodeEntry
    {
        public string Code { get; set; }

        public List<string> CodeLines { get; set; }
        public List<FunctionEntry> Functions { get; set; }

        public int[,] FuncsRunWhen { get; set; }

        public string[] SetFuncNames { get; set; }

        public CCodeEntry(string _Code = "", List<FunctionEntry> _Funcs = null, int[,] _FuncsRunWhen = null)
        {
            Code = _Code;
            CodeLines = new List<string>();
            Functions = _Funcs;

            if (Functions == null)
                Functions = new List<FunctionEntry>();

            FuncsRunWhen = _FuncsRunWhen;
            SetFuncNames = new string[6];

            if (FuncsRunWhen == null)
                FuncsRunWhen = new int[6, 2]
                {
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                };
        }
    }


    public class FunctionEntry
    {
        public string FuncName { get; set; }

        [JsonIgnore]
        public UInt32 Addr { get; set; }

        public FunctionEntry(string _Name = "", UInt32 _Addr = 0xFFFFFFFF)
        {
            FuncName = _Name;
            Addr = _Addr;
        }
    }

    public class JsonIgnoreAttributeIgnorerContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.Ignored = false; 
            return property;
        }
    }

}
