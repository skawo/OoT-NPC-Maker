using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsPushable { get; set; }
        public bool IsAlwaysActive { get; set; }
        public bool IsAlwaysDrawn { get; set; }
        public bool ExecuteJustScript { get; set; }
        public bool ReactsIfAttacked { get; set; }
        public Int16 SfxIfAttacked { get; set; }
        public byte EffectIfAttacked { get; set; }
        public bool OpensDoors { get; set; }
        public UInt16 CollisionRadius { get; set; }
        public UInt16 CollisionHeight { get; set; }
        public Int16[] CollisionPositionOffsets { get; set; }

        public bool CastsShadow { get; set; }
        public UInt16 ShadowRadius { get; set; }

        public bool IsTargettable { get; set; }
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


        public NPCEntry()
        {
            NPCName = "";
            IsNull = false;

            ObjectID = 0;
            Hierarchy = 0;
            HierarchyType = 0;
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
            PushesSwitches = false;
            IsPushable = false;
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
            CollisionPositionOffsets = new Int16[] { 0, 0, 0 };

            CastsShadow = false;
            ShadowRadius = 0;

            IsTargettable = false;
            TargetDistance = 1;
            TargetLimb = 0;
            TargetPositionOffsets = new Int16[] { 0, 0, 0 };
            TalkRadius = 150.0f;

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

            EnvironmentColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            BlinkPattern = "";
            TalkPattern = "";
            BlinkSegment = 8;
            TalkSegment = 8;
            BlinkSpeed = 1;
            TalkSpeed = 1;

            ExtraDisplayLists = new List<DListEntry>();
            DisplayListColors = new List<ColorEntry>();
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
                case Members.PUSHABLE: IsPushable = Convert.ToBoolean(Value); break;
                case Members.COLRADIUS: CollisionRadius = Convert.ToUInt16(Value); break;
                case Members.COLHEIGHT: CollisionHeight = Convert.ToUInt16(Value); break;

                case Members.SHADOW: CastsShadow = Convert.ToBoolean(Value); break;
                case Members.SHADOWRADIUS: ShadowRadius = Convert.ToUInt16(Value); break;

                case Members.XCOLOFFS: CollisionPositionOffsets[0] = Convert.ToInt16(Value); break;
                case Members.YCOLOFFS: CollisionPositionOffsets[1] = Convert.ToInt16(Value); break;
                case Members.ZCOLOFFS: CollisionPositionOffsets[2] = Convert.ToInt16(Value); break;

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
