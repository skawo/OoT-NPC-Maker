using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker
{
    public class NPCFile
    {
        public int Version { get; set; }
        public List<NPCEntry> Entries { get; set; }

        public NPCFile()
        {
            Version = 2;
            Entries = new List<NPCEntry>();
        }
    }

    public class NPCEntry
    {
        public string NPCName { get; set; }
        public bool IsNull { get; set; }

        public UInt16 ObjectID { get; set; }
        public UInt32 Hierarchy { get; set; }
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

        public byte NumVars { get; set; }
        public byte NumFVars { get; set; }

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
            LightColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
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
            TimedPathStart = 0;
            TimedPathEnd = Helpers.GetOcarinaTime("23:59");

            Animations = new List<AnimationEntry>();
            AnimationType = 0;

            Scripts = new List<ScriptEntry>();

            Segments = new List<List<SegmentEntry>>();

            Messages = new List<MessageEntry>();

            EnvironmentColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            BlinkPattern = "";
            TalkPattern = "";
            BlinkSegment = 8;
            TalkSegment = 8;
            BlinkSpeed = 1;
            TalkSpeed = 1;

            ExtraDisplayLists = new List<DListEntry>();
            DisplayListColors = new List<ColorEntry>();

            VisibleUnderLensOfTruth = false;
            Invisible = false;
            DEBUGShowCols = false;

            NumVars = 2;
            NumFVars = 2;
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

                default: break;
            }
        }
    }

    public class ScriptEntry
    {
        public string Text { get; set; }
        public List<string> ParseErrors { get; set; }
        public string Name { get; set; }

        public ScriptEntry()
        {
            Text = "";
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

        public AnimationEntry()
        {
            Name = "";
            Address = 0;
            Speed = 1.0f;
            ObjID = -1;
            StartFrame = 0;
            EndFrame = 0xFF;
        }
        public AnimationEntry(string _Name, UInt32 _Address, float _Speed, Int16 _ObjectID, byte _StartFrame, byte _EndFrame)
        {
            Name = _Name;
            Address = _Address;
            Speed = _Speed;
            ObjID = _ObjectID;
            StartFrame = _StartFrame;
            EndFrame = _EndFrame;
        }
    }

    public class SegmentEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public Int16 ObjectID { get; set; }

        public SegmentEntry()
        {
            Name = "";
            Address = 0;
            ObjectID = -1;
        }
        public SegmentEntry(string _Name, UInt32 _Address, Int16 _ObjectID)
        {
            Name = _Name;
            Address = _Address;
            ObjectID = _ObjectID;
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
            Color = System.Drawing.Color.FromArgb(0, 0, 0, 0);
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
        public int Type { get; set; }
        public int Position { get; set; }

        public MessageEntry()
        {
        }

        // Below taken and edited from 
        // https://github.com/Sage-of-Mirrors/Ocarina-Text-Editor
        // Rewrite it.

        public byte GetMessageTypePos()
        {
            byte Out = (byte)(Type << 4);
            return (byte)(Out | Position);
        }

        public List<byte> ConvertTextData()
        {
            List<byte> data = new List<byte>();

            for (int i = 0; i < MessageText.Length; i++)
            {
                if (MessageText[i] == '\r')
                {
                    MessageText = MessageText.Remove(i, 1);
                    i--;
                }
            }

            for (int i = 0; i < MessageText.Length; i++)
            {
                // Not a control code, copy char to output buffer
                if (MessageText[i] != '<')
                {
                    if (Dicts.MessageControlCodes.ContainsValue(MessageText[i].ToString()))
                    {
                        data.Add((byte)Dicts.MessageControlCodes.First(x => x.Value == MessageText[i].ToString()).Key);
                    }
                    else if (MessageText[i] == '\n')
                    {
                        try
                        {
                            data.Add((byte)Lists.MsgControlCode.Line_Break);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            data.Add((byte)Lists.MsgControlCode.Line_Break);
                        }
                    }
                    else if (MessageText[i] == '\r')
                    {
                        // Do nothing
                    }
                    else
                    {
                        data.Add((byte)MessageText[i]);
                    }
                    continue;
                }
                // Control code end tag. This should never be encountered on its own.
                else if (MessageText[i] == '>')
                {
                    // This should be an error handler
                }
                // We've got a control code
                else
                {
                    // Buffer for the control code
                    List<char> controlCode = new List<char>();

                    while (MessageText[i] != '>')
                    {
                        // Add code chars to the buffer
                        controlCode.Add(MessageText[i]);
                        // Increase i so we can skip the code when we're done parsing
                        i++;
                    }

                    // Remove the < chevron from the beginning of the code
                    controlCode.RemoveAt(0);

                    string parsedCode = new string(controlCode.ToArray());

                    if (parsedCode.ToLower() == "new box")
                    {
                        data.RemoveAt(data.Count - 1); // Removes the last \n, which was added during import
                        i++; // Skips next \n, added at import
                    }

                    data.AddRange(GetControlCode(parsedCode.Split(':')));
                }
            }

            data.Add(0x02);

            return data;
        }

        private List<byte> GetControlCode(string[] code)
        {
            List<byte> output = new List<byte>();

            switch (code[0].ToLower())
            {
                case "line break":
                    output.Add((byte)Lists.MsgControlCode.Line_Break);
                    break;
                case "box break":
                    output.Add((byte)Lists.MsgControlCode.Box_Break);
                    break;
                case "w":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.W);
                    break;
                case "r":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.R);
                    break;
                case "g":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.G);
                    break;
                case "b":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.B);
                    break;
                case "c":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.C);
                    break;
                case "m":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.M);
                    break;
                case "y":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.Y);
                    break;
                case "blk":
                    output.Add(5);
                    output.Add((byte)Lists.MsgColor.Blk);
                    break;
                case "pixels right":
                    output.Add((byte)Lists.MsgControlCode.Spaces);
                    output.Add(Convert.ToByte(code[1]));
                    break;
                    /* Jump will not work for messages like this.
                case "jump":
                    output.Add((byte)Lists.MsgControlCode.Jump);
                    byte[] jumpIDBytes = BitConverter.GetBytes(short.Parse(code[1], System.Globalization.NumberStyles.HexNumber));
                    output.Add(jumpIDBytes[1]);
                    output.Add(jumpIDBytes[0]);
                    break;
                    */
                case "di":
                    output.Add((byte)Lists.MsgControlCode.Draw_Instant);
                    break;
                case "dc":
                    output.Add((byte)Lists.MsgControlCode.Draw_Char);
                    break;
                case "shop description":
                    output.Add((byte)Lists.MsgControlCode.Shop_Description);
                    break;
                case "event":
                    output.Add((byte)Lists.MsgControlCode.Event);
                    break;
                case "delay":
                    output.Add((byte)Lists.MsgControlCode.Delay);
                    output.Add(Convert.ToByte(code[1]));
                    break;
                case "fade":
                    output.Add((byte)Lists.MsgControlCode.Fade);
                    output.Add(Convert.ToByte(code[1]));
                    break;
                case "player":
                    output.Add((byte)Lists.MsgControlCode.Player);
                    break;
                case "ocarina":
                    output.Add((byte)Lists.MsgControlCode.Ocarina);
                    break;
                case "sound":
                    output.Add((byte)Lists.MsgControlCode.Sound);
                    short soundValue = 0;
                    switch (code[1].ToLower())
                    {
                        case "item fanfare":
                            soundValue = (short)Lists.MsgSound.Item_Fanfare;
                            break;
                        case "frog ribbit 1":
                            soundValue = (short)Lists.MsgSound.Frog_Ribbit_1;
                            break;
                        case "frog ribbit 2":
                            soundValue = (short)Lists.MsgSound.Frog_Ribbit_2;
                            break;
                        case "deku squeak":
                            soundValue = (short)Lists.MsgSound.Deku_Squeak;
                            break;
                        case "deku cry":
                            soundValue = (short)Lists.MsgSound.Deku_Cry;
                            break;
                        case "generic event":
                            soundValue = (short)Lists.MsgSound.Generic_Event;
                            break;
                        case "poe vanishing":
                            soundValue = (short)Lists.MsgSound.Poe_Vanishing;
                            break;
                        case "twinrova 1":
                            soundValue = (short)Lists.MsgSound.Twinrova_1;
                            break;
                        case "twinrova 2":
                            soundValue = (short)Lists.MsgSound.Twinrova_2;
                            break;
                        case "navi hello":
                            soundValue = (short)Lists.MsgSound.Navi_Hello;
                            break;
                        case "talon ehh":
                            soundValue = (short)Lists.MsgSound.Talon_Ehh;
                            break;
                        case "carpenter waaaa":
                            soundValue = (short)Lists.MsgSound.Carpenter_Waaaa;
                            break;
                        case "navi hey":
                            soundValue = (short)Lists.MsgSound.Navi_HEY;
                            break;
                        case "saria giggle":
                            soundValue = (short)Lists.MsgSound.Saria_Giggle;
                            break;
                        case "yaaaa":
                            soundValue = (short)Lists.MsgSound.Yaaaa;
                            break;
                        case "zelda heh":
                            soundValue = (short)Lists.MsgSound.Zelda_Heh;
                            break;
                        case "zelda awww":
                            soundValue = (short)Lists.MsgSound.Zelda_Awww;
                            break;
                        case "zelda huh":
                            soundValue = (short)Lists.MsgSound.Zelda_Huh;
                            break;
                        case "generic giggle":
                            soundValue = (short)Lists.MsgSound.Generic_Giggle;
                            break;
                        case "unused 1":
                            soundValue = (short)Lists.MsgSound.Unused_1;
                            break;
                        case "moo":
                            soundValue = (short)Lists.MsgSound.Moo;
                            break;
                    }
                    byte[] soundIDBytes = BitConverter.GetBytes(soundValue);
                    output.Add(soundIDBytes[1]);
                    output.Add(soundIDBytes[0]);
                    break;
                case "icon":
                    output.Add((byte)Lists.MsgControlCode.Icon);
                    output.Add(Convert.ToByte(code[1]));
                    break;
                case "speed":
                    output.Add((byte)Lists.MsgControlCode.Speed);
                    output.Add(Convert.ToByte(code[1]));
                    break;
                case "background":
                    output.Add((byte)Lists.MsgControlCode.Background);
                    byte[] backgroundIDBytes = BitConverter.GetBytes(Convert.ToInt32(code[1]));
                    output.Add(backgroundIDBytes[2]);
                    output.Add(backgroundIDBytes[1]);
                    output.Add(backgroundIDBytes[0]);
                    break;
                case "marathon time":
                    output.Add((byte)Lists.MsgControlCode.Marathon_Time);
                    break;
                case "race time":
                    output.Add((byte)Lists.MsgControlCode.Race_Time);
                    break;
                case "points":
                    output.Add((byte)Lists.MsgControlCode.Points);
                    break;
                case "gold skulltulas":
                    output.Add((byte)Lists.MsgControlCode.Gold_Skulltulas);
                    break;
                case "ns":
                    output.Add((byte)Lists.MsgControlCode.No_Skip);
                    break;
                case "two choices":
                    output.Add((byte)Lists.MsgControlCode.Two_Choices);
                    break;
                case "three choices":
                    output.Add((byte)Lists.MsgControlCode.Three_Choices);
                    break;
                case "fish weight":
                    output.Add((byte)Lists.MsgControlCode.Fish_Weight);
                    break;
                case "high score":
                    output.Add((byte)Lists.MsgControlCode.High_Score);
                    switch (code[1].ToLower())
                    {
                        case "archery":
                            output.Add((byte)Lists.MsgHighScore.Archery);
                            break;
                        case "poe points":
                            output.Add((byte)Lists.MsgHighScore.Poe_Points);
                            break;
                        case "fishing":
                            output.Add((byte)Lists.MsgHighScore.Fishing);
                            break;
                        case "horse race":
                            output.Add((byte)Lists.MsgHighScore.Horse_Race);
                            break;
                        case "marathon":
                            output.Add((byte)Lists.MsgHighScore.Marathon);
                            break;
                        case "dampe race":
                            output.Add((byte)Lists.MsgHighScore.Dampe_Race);
                            break;
                    }
                    break;
                case "time":
                    output.Add((byte)Lists.MsgControlCode.Time);
                    break;
                case "dash":
                    output.Add((byte)Lists.MsgControlCode.Dash);
                    break;
                case "a button":
                    output.Add((byte)Lists.MsgControlCode.A_Button);
                    break;
                case "b button":
                    output.Add((byte)Lists.MsgControlCode.B_Button);
                    break;
                case "c button":
                    output.Add((byte)Lists.MsgControlCode.C_Button);
                    break;
                case "l button":
                    output.Add((byte)Lists.MsgControlCode.L_Button);
                    break;
                case "r button":
                    output.Add((byte)Lists.MsgControlCode.R_Button);
                    break;
                case "z button":
                    output.Add((byte)Lists.MsgControlCode.Z_Button);
                    break;
                case "c up":
                    output.Add((byte)Lists.MsgControlCode.C_Up);
                    break;
                case "c down":
                    output.Add((byte)Lists.MsgControlCode.C_Down);
                    break;
                case "c left":
                    output.Add((byte)Lists.MsgControlCode.C_Left);
                    break;
                case "c right":
                    output.Add((byte)Lists.MsgControlCode.C_Right);
                    break;
                case "triangle":
                    output.Add((byte)Lists.MsgControlCode.Triangle);
                    break;
                case "control stick":
                    output.Add((byte)Lists.MsgControlCode.Control_Stick);
                    break;
                case "d pad":
                    output.Add((byte)Lists.MsgControlCode.D_Pad);
                    break;
                case "new box":
                    output.Add((byte)Lists.MsgControlCode.Box_Break);
                    break;
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
        public UInt16 Limb { get; set; }
        public Int16 ObjectID { get; set; }

        public DListEntry()
        {
            Name = "";
            //Address = 0;
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
        }
        public DListEntry(string _Name, UInt32 _Address, float _TransX, float _TransY, float _TransZ,
                          Int16 _RotX, Int16 _RotY, Int16 _RotZ, float _Scale, UInt16 _Limb, int _ShowType, Int16 _ObjectID)
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
            ObjectID = _ObjectID;
        }
    }
}
