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
        public UInt16 MovementSpeed { get; set; }

        public byte AnimationType { get; set; }
        public List<AnimationEntry> Animations { get; set; }

        public string Script { get; set; }
        public List<string> ParseErrors { get; set; }

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

            Animations = new List<AnimationEntry>();
            AnimationType = 0;

            Script = "";
            ParseErrors = new List<string>();
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
                case "MOVSPEED": MovementSpeed = Convert.ToUInt16(Value); break;

                case "ANIMTYPE": AnimationType = Convert.ToByte(Value); break;

                case "TALKSCRIPT": Script = Convert.ToString(Value); break;

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
}
 