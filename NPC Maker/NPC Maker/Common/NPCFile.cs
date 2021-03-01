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
        public byte CutsceneID { get; set; }

        public byte LookAtType { get; set; }

        public byte HeadLimb { get; set; }
        public byte HeadHorizAxis { get; set; }
        public byte HeadVertAxis { get; set; }

        public byte WaistLimb { get; set; }
        public byte WaistHorizAxis { get; set; }
        public byte WaistVertAxis { get; set; }

        public UInt16 DegreesVert { get; set; }
        public UInt16 DegreesHor { get; set; }
        public Int16[] LookAtOffs { get; set; }

        public bool Collision { get; set; }
        public bool Switches { get; set; }
        public bool Pushable { get; set; }
        public bool AlwActive { get; set; }
        public bool AlwDraw { get; set; }
        public bool JustScript { get; set; }
        public bool ReactAttacked { get; set; }
        public bool OpenDoors { get; set; }
        public UInt16 ColRadius { get; set; }
        public UInt16 Height { get; set; }
        public Int16[] ColOffs { get; set; }

        public bool Shadow { get; set; }
        public UInt16 ShRadius { get; set; }

        public bool Targettable { get; set; }
        public byte TargetDist { get; set; }
        public byte TargetLimb { get; set; }
        public Int16[] TargOffs { get; set; }
        public float TalkRadius { get; set; }

        public byte MovementType { get; set; }
        public UInt16 MovementDistance { get; set; }
        public float MovementSpeed { get; set; }
        public float GravityForce { get; set; }
        public byte PathID { get; set; }
        public Int16 LoopStart { get; set; }
        public Int16 LoopEnd { get; set; }
        public UInt16 LoopDel { get; set; }
        public bool Loop { get; set; }
        public bool TimedPath { get; set; }
        public UInt16 TimedPathStart { get; set; }
        public UInt16 TimedPathEnd { get; set; }

        public byte AnimationType { get; set; }
        public List<AnimationEntry> Animations { get; set; }

        public string Script { get; set; }
        public List<string> ParseErrors { get; set; }

        public string Script2 { get; set; }
        public List<string> ParseErrors2 { get; set; }

        public System.Drawing.Color EnvColor { get; set; }
        public List<List<TextureEntry>> Textures { get; set; }
        public string BlinkPattern { get; set; }
        public string TalkPattern { get; set; }
        public byte BlinkSegment { get; set; }
        public byte TalkSegment { get; set; }
        public byte BlinkSpeed { get; set; }
        public byte TalkSpeed { get; set; }
        public List<DListEntry> DLists { get; set; }

        //public List<ColorEntry> Colors { get; set; }

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
            HeadLimb = 0;
            HeadHorizAxis = 0;
            HeadVertAxis = 0;
            WaistLimb = 0;
            WaistHorizAxis = 0;
            WaistVertAxis = 0;
            DegreesVert = 0;
            DegreesHor = 0;
            LookAtOffs = new Int16[] { 0, 0, 0 };

            Collision = false;
            Switches = false;
            Pushable = false;
            AlwActive = false;
            JustScript = false;
            ReactAttacked = false;
            OpenDoors = false;
            ColRadius = 0;
            Height = 0;
            ColOffs = new Int16[] { 0, 0, 0 };

            Shadow = false;
            ShRadius = 0;

            Targettable = false;
            TargetDist = 1;
            TargetLimb = 0;
            TargOffs = new Int16[] { 0, 0, 0 };
            TalkRadius = 150.0f;

            MovementType = 0;
            MovementDistance = 0;
            MovementSpeed = 1.0f;
            GravityForce = 1.0f;
            PathID = 0;
            LoopStart = -1;
            LoopEnd = -1;
            LoopDel = 0;
            Loop = false;
            TimedPathStart = 0;
            TimedPathEnd = Helpers.GetOcarinaTime("23:59");

            Animations = new List<AnimationEntry>();
            AnimationType = 0;

            Script = "";
            ParseErrors = new List<string>();

            Script2 = "";
            ParseErrors2 = new List<string>();

            Textures = new List<List<TextureEntry>>();

            EnvColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            BlinkPattern = "";
            TalkPattern = "";
            BlinkSegment = 8;
            TalkSegment = 8;
            BlinkSpeed = 1;
            TalkSpeed = 1;

            DLists = new List<DListEntry>();
            //Colors = new List<ColorEntry>();
        }

        /*
        public List<OutputColorEntry> ParseColorEntries()
        {
            List<OutputColorEntry> Out = new List<OutputColorEntry>();

            foreach (ColorEntry CE in this.Colors)
            {
                if (CE.Limbs != "")
                {
                    string[] Limbs = CE.Limbs.Split(',');

                    foreach (string LimbE in Limbs)
                    {
                        if (LimbE.Contains("-"))
                        {
                            string[] MinMax = LimbE.Split('-');

                            int Min = Convert.ToInt32(MinMax[0]);
                            int Max = Convert.ToInt32(MinMax[1]);

                            for (int f = Min; f < Max + 1; f++)
                            {
                                if (Out.Find(x => x.LimbID == Convert.ToByte(f)) == null)
                                    Out.Add(new OutputColorEntry(Convert.ToByte(f), CE.Color.R, CE.Color.G, CE.Color.B));
                            }
                        }
                        else
                        {
                            if (Out.Find(x => x.LimbID == Convert.ToByte(LimbE)) == null)
                                Out.Add(new OutputColorEntry(Convert.ToByte(LimbE), CE.Color.R, CE.Color.G, CE.Color.B));
                        }
                    }
                }
            }

            return Out;
        }
        */

        public enum Members
        {
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
            PUSHABLE,
            COLRADIUS,
            COLHEIGHT,
            SHADOW,
            SHADOWRADIUS,
            XCOLOFFS,
            YCOLOFFS,
            ZCOLOFFS,
            TARGETTABLE,
            TARGETLIMB,
            XTARGETOFFS,
            YTARGETOFFS,
            ZTARGETOFFS,
            MOVEMENT,
            MOVDISTANCE,
            MOVSPEED,
            PATHID,
            LOOPSTART,
            LOOPEND,
            LOOPDEL,
            LOOP,
            ANIMTYPE,
            TALKSCRIPT,
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
        }

        public void ChangeValueOfMember(Members Member, object Value)
        {
            switch (Member)
            {
                case Members.NPCNAME: NPCName = (string)Value; break;

                case Members.OBJID: ObjectID = Convert.ToUInt16(Value); break;
                case Members.HIERARCHY: Hierarchy = Convert.ToUInt32(Value); break;
                case Members.HIERARCHYTYPE: HierarchyType = Convert.ToByte(Value); break;
                case Members.XMODELOFFS: ModelOffs[0] = Convert.ToInt16(Value); break;
                case Members.YMODELOFFS: ModelOffs[1] = Convert.ToInt16(Value); break;
                case Members.ZMODELOFFS: ModelOffs[2] = Convert.ToInt16(Value); break;
                case Members.SCALE: Scale = (float)Convert.ToDecimal(Value); break;
                case Members.CUTSCENEID: CutsceneID = Convert.ToByte(Value); break;

                case Members.LOOKATTYPE: LookAtType = Convert.ToByte(Value); break;
                case Members.HEADLIMB: HeadLimb = Convert.ToByte(Value); break;
                case Members.HEADVERTAXIS: HeadVertAxis = Convert.ToByte(Value); break;
                case Members.HEADHORIZAXIS: HeadHorizAxis = Convert.ToByte(Value); break;
                case Members.WAISTLIMB: WaistLimb = Convert.ToByte(Value); break;
                case Members.WAISTVERTAXIS: WaistVertAxis = Convert.ToByte(Value); break;
                case Members.WAISTHORIZAXIS: WaistHorizAxis = Convert.ToByte(Value); break;
                case Members.DEGVERT: DegreesVert = Convert.ToUInt16(Value); break;
                case Members.DEGHOZ: DegreesHor = Convert.ToUInt16(Value); break;
                case Members.XLOOKATOFFS: LookAtOffs[0] = Convert.ToInt16(Value); break;
                case Members.YLOOKATOFFS: LookAtOffs[1] = Convert.ToInt16(Value); break;
                case Members.ZLOOKATOFFS: LookAtOffs[2] = Convert.ToInt16(Value); break;

                case Members.COLLISION: Collision = Convert.ToBoolean(Value); break;
                case Members.SWITCHES: Switches = Convert.ToBoolean(Value); break;
                case Members.PUSHABLE: Pushable = Convert.ToBoolean(Value); break;
                case Members.COLRADIUS: ColRadius = Convert.ToUInt16(Value); break;
                case Members.COLHEIGHT: Height = Convert.ToUInt16(Value); break;

                case Members.SHADOW: Shadow = Convert.ToBoolean(Value); break;
                case Members.SHADOWRADIUS: ShRadius = Convert.ToUInt16(Value); break;

                case Members.XCOLOFFS: ColOffs[0] = Convert.ToInt16(Value); break;
                case Members.YCOLOFFS: ColOffs[1] = Convert.ToInt16(Value); break;
                case Members.ZCOLOFFS: ColOffs[2] = Convert.ToInt16(Value); break;

                case Members.TARGETTABLE: Targettable = Convert.ToBoolean(Value); break;
                case Members.TARGETLIMB: TargetLimb = Convert.ToByte(Value); break;
                case Members.XTARGETOFFS: TargOffs[0] = Convert.ToInt16(Value); break;
                case Members.YTARGETOFFS: TargOffs[1] = Convert.ToInt16(Value); break;
                case Members.ZTARGETOFFS: TargOffs[2] = Convert.ToInt16(Value); break;

                case Members.MOVEMENT: MovementType = Convert.ToByte(Value); break;
                case Members.MOVDISTANCE: MovementDistance = Convert.ToUInt16(Value); break;
                case Members.MOVSPEED: MovementSpeed = (float)Convert.ToDecimal(Value); break;
                case Members.PATHID: PathID = Convert.ToByte(Value); break;
                case Members.LOOPSTART: LoopStart = Convert.ToInt16(Value); break;
                case Members.LOOPEND: LoopEnd = Convert.ToInt16(Value); break;
                case Members.LOOPDEL: LoopDel = Convert.ToUInt16(Value); break;
                case Members.LOOP: Loop = Convert.ToBoolean(Value); break;

                case Members.ANIMTYPE: AnimationType = Convert.ToByte(Value); break;

                case Members.TALKSCRIPT: Script = Convert.ToString(Value); break;

                case Members.BLINKPAT: BlinkPattern = Convert.ToString(Value); break;
                case Members.TALKPAT: TalkPattern = Convert.ToString(Value); break;
                case Members.BLINKSEG: BlinkSegment = Convert.ToByte(Value); break;
                case Members.TALKSEG: TalkSegment = Convert.ToByte(Value); break;
                case Members.BLINKSPE: BlinkSpeed = Convert.ToByte(Value); break;
                case Members.TALKSPE: TalkSpeed = Convert.ToByte(Value); break;

                case Members.ACTIVE: AlwActive = Convert.ToBoolean(Value); break;
                case Members.DRAWOUTOFCAM: AlwDraw = Convert.ToBoolean(Value); break;
                case Members.TARGETDIST: TargetDist = Convert.ToByte(Value); break;

                case Members.JUSTSCRIPT: JustScript = Convert.ToBoolean(Value); break;
                case Members.REACTATT: ReactAttacked = Convert.ToBoolean(Value); break;
                case Members.TIMEDPATH: TimedPath = Convert.ToBoolean(Value); break;
                case Members.OPENDOORS: OpenDoors = Convert.ToBoolean(Value); break;

                case Members.TALKRADIUS: TalkRadius = Convert.ToUInt16(Value); break;
                case Members.GRAVITYFORCE: GravityForce = (float)Convert.ToDecimal(Value); break;

                case Members.PATHSTARTTIME: TimedPathStart = Helpers.GetOcarinaTime((string)Value); break;
                case Members.PATHENDTIME: TimedPathEnd = Helpers.GetOcarinaTime((string)Value); break;

                default: break;
            }
        }
    }

    public class AnimationEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public Int16 ObjID { get; set; }
        public float Speed { get; set; }
        public byte[] Frames { get; set; }

        public AnimationEntry()
        {
            Name = "";
            Address = 0;
            Speed = 1.0f;
            ObjID = -1;
            Frames = new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF };
        }
        public AnimationEntry(string _Name, UInt32 _Address, float _Speed, Int16 _ObjectID, byte[] _Frames)
        {
            Name = _Name;
            Address = _Address;
            Speed = _Speed;
            ObjID = _ObjectID;
            Frames = _Frames;
        }
    }

    public class TextureEntry
    {
        public string Name { get; set; }
        public UInt32 Address { get; set; }
        public Int16 ObjectID { get; set; }

        public TextureEntry()
        {
            Name = "";
            Address = 0;
            ObjectID = -1;
        }
        public TextureEntry(string _Name, UInt32 _Address, Int16 _ObjectID)
        {
            Name = _Name;
            Address = _Address;
            ObjectID = _ObjectID;
        }
    }

    /*
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
      }*/

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
