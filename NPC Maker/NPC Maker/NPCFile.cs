using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPC_Maker
{
    public class NPCFile
    {
        public List<NPCEntry> Entries { get; set; }

        public NPCFile()
        {
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
        public Int16[] ModelOffs { get; set; }
        public float Scale { get; set; }

        public byte LookAtType { get; set; }
        public byte HeadAxis { get; set; }
        public UInt16 DegreesVert { get; set; }
        public UInt16 DegreesHor { get; set; }
        public UInt16 LimbIndex { get; set; }

        public bool Collision { get; set; }
        public bool Shadow { get; set; }
        public bool Switches { get; set; }
        public bool Pushable { get; set; }
        public UInt16 Radius { get; set; }
        public UInt16 Height { get; set; }
        public Int16[] ColOffs { get; set; }

        public bool Targettable { get; set; }
        public UInt16 TargetLimb { get; set; }
        public Int16[] TargOffs { get; set; }

        public byte MovementType { get; set; }
        public UInt16 MovementDistance { get; set; }
        public float MovementSpeed { get; set; }
        public byte PathID { get; set; }
        public Int16 LoopStart { get; set; }
        public Int16 LoopEnd { get; set; }
        public UInt16 LoopDel { get; set; }
        public bool Loop { get; set; }

        public byte AnimationType { get; set; }
        public List<AnimationEntry> Animations { get; set; }

        public string Script { get; set; }
        public List<string> ParseErrors { get; set; }

        public System.Drawing.Color EnvColor { get; set; }
        public List<List<TextureEntry>> Textures { get; set; }
        public string BlinkPattern { get; set; }
        public string TalkPattern { get; set; }
        public byte BlinkSegment { get; set; }
        public byte TalkSegment { get; set; }
        public byte BlinkSpeed { get; set; }
        public byte TalkSpeed { get; set; }
        public List<DListEntry> DLists { get; set; }

        public NPCEntry()
        {
            NPCName = "";
            IsNull = false;

            ObjectID = 0;
            Hierarchy = 0;
            HierarchyType = 0;
            ModelOffs = new Int16[] { 0, 0, 0 };
            Scale = 0.01f;

            LookAtType = 0;
            HeadAxis = 0;
            DegreesVert = 0;
            DegreesHor = 0;
            LimbIndex = 0;

            Collision = false;
            Shadow = false;
            Switches = false;
            Pushable = false;
            Radius = 0;
            Height = 0;
            ColOffs = new Int16[] { 0, 0, 0 };

            Targettable = false;
            TargetLimb = 0;
            TargOffs = new Int16[] { 0, 0, 0 };

            MovementType = 0;
            MovementDistance = 0;
            MovementSpeed = 0;
            PathID = 0;
            LoopStart = -1;
            LoopEnd = -1;
            LoopDel = 0;
            Loop = false;

            Animations = new List<AnimationEntry>();
            AnimationType = 0;

            Script = "";
            ParseErrors = new List<string>();

            Textures = new List<List<TextureEntry>>();

            EnvColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            BlinkPattern = "";
            TalkPattern = "";
            BlinkSegment = 8;
            TalkSegment = 8;
            BlinkSpeed = 1;
            TalkSpeed = 1;

            DLists = new List<DListEntry>();
        }

        public void ChangeValueOfMember(string Member, object Value)
        {
            switch (Member)
            {
                case "NPCNAME": NPCName = (string)Value; break;

                case "OBJID": ObjectID = Convert.ToUInt16(Value); break;
                case "HIERARCHY": Hierarchy = Convert.ToUInt32(Value); break;
                case "HIERARCHYTYPE": HierarchyType = Convert.ToByte(Value); break;
                case "XMODELOFFS": ModelOffs[0] = Convert.ToInt16(Value); break;
                case "YMODELOFFS": ModelOffs[1] = Convert.ToInt16(Value); break;
                case "ZMODELOFFS": ModelOffs[2] = Convert.ToInt16(Value); break;
                case "SCALE": Scale = (float)Convert.ToDecimal(Value); break;

                case "LOOKATTYPE": LookAtType = Convert.ToByte(Value); break;
                case "AXIS": HeadAxis = Convert.ToByte(Value); break;
                case "DEGVERT": DegreesVert = Convert.ToUInt16(Value); break;
                case "DEGHOZ": DegreesHor = Convert.ToUInt16(Value); break;
                case "LIMB": LimbIndex = Convert.ToUInt16(Value); break;

                case "COLLISION": Collision = Convert.ToBoolean(Value); break;
                case "SHADOW": Shadow = Convert.ToBoolean(Value); break;
                case "SWITCHES": Switches = Convert.ToBoolean(Value); break;
                case "PUSHABLE": Pushable = Convert.ToBoolean(Value); break;
                case "COLRADIUS": Radius = Convert.ToUInt16(Value); break;
                case "COLHEIGHT": Height = Convert.ToUInt16(Value); break;

                case "XCOLOFFS": ColOffs[0] = Convert.ToInt16(Value); break;
                case "YCOLOFFS": ColOffs[1] = Convert.ToInt16(Value); break;
                case "ZCOLOFFS": ColOffs[2] = Convert.ToInt16(Value); break;

                case "TARGETTABLE": Targettable = Convert.ToBoolean(Value); break;
                case "TARGETLIMB": TargetLimb = Convert.ToUInt16(Value); break;
                case "XTARGETOFFS": TargOffs[0] = Convert.ToInt16(Value); break;
                case "YTARGETOFFS": TargOffs[1] = Convert.ToInt16(Value); break;
                case "ZTARGETOFFS": TargOffs[2] = Convert.ToInt16(Value); break;

                case "MOVEMENT": MovementType = Convert.ToByte(Value); break;
                case "MOVDISTANCE": MovementDistance = Convert.ToUInt16(Value); break;
                case "MOVSPEED": MovementSpeed = (float)Convert.ToDecimal(Value); break;
                case "PATHID": PathID = Convert.ToByte(Value); break;
                case "LOOPSTART": LoopStart = Convert.ToInt16(Value); break;
                case "LOOPEND": LoopEnd = Convert.ToInt16(Value); break;
                case "LOOPDEL": LoopDel = Convert.ToUInt16(Value); break;
                case "LOOP": Loop = Convert.ToBoolean(Value); break;

                case "ANIMTYPE": AnimationType = Convert.ToByte(Value); break;

                case "TALKSCRIPT": Script = Convert.ToString(Value); break;

                case "BLINKPAT": BlinkPattern = Convert.ToString(Value); break;
                case "TALKPAT": TalkPattern = Convert.ToString(Value); break;
                case "BLINKSEG": BlinkSegment = Convert.ToByte(Value); break;
                case "TALKSEG": TalkSegment = Convert.ToByte(Value); break;
                case "BLINKSPE": BlinkSpeed = Convert.ToByte(Value); break;
                case "TALKSPE": TalkSpeed = Convert.ToByte(Value); break;

                default: break;
            }
        }
    }

    public class AnimationEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public UInt16 ObjID { get; set; }
        public float Speed { get; set; }

        public AnimationEntry()
        {
            Name = "";
            Address = 0;
            Speed = 1.0f;
            ObjID = 0xFFFF;
        }
        public AnimationEntry(string _Name, UInt32 _Address, float _Speed, UInt16 _ObjectID)
        {
            Name = _Name;
            Address = _Address;
            Speed = _Speed;
            ObjID = _ObjectID;
        }
    }

    public class TextureEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public int ObjectID { get; set; }

        public TextureEntry()
        {
            Name = "";
            Address = 0;
            ObjectID = -1;
        }
        public TextureEntry(string _Name, UInt32 _Address, int _ObjectID)
        {
            Name = _Name;
            Address = _Address;
            ObjectID = _ObjectID;
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
        }
        public DListEntry(string _Name, UInt32 _Address, float _TransX, float _TransY, float _TransZ, Int16 _RotX, Int16 _RotY, Int16 _RotZ, float _Scale, UInt16 _Limb, int _ShowType, Int16 _ObjectID)
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
