using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public class NPCMakerSettings
    {
        public int Version { get; set; }

        public Lists.GameVersions GameVersion { get; set; }

        public string GCCFlags { get; set; }

        public CCode.CodeEditorEnum CodeEditor { get; set; }

        public string CustomCodeEditorPath { get; set; }

        public string CustomCodeEditorArgs { get; set; }

        public bool Verbose { get; set; }

        public bool ColorizeScriptSyntax { get; set; }

        public bool CheckSyntax { get; set; }

        public bool ImproveTextMsgReadability { get; set; }

        public bool AutoComp_Save { get; set; }

        public bool AutoComp_ActorSwitch { get; set; }

        public UInt32 CompileTimeout { get; set; }

        public string LastSaveBinaryPath { get; set; }

        public string LastOpenPath { get; set; }

        public UInt32 ParseTime { get; set; }

        public bool UseWine { get; set; }

        public bool AutoSave { get; set; }

        public UInt32 AutoSaveTime { get; set; }

        public bool UseSpaceWithFromFont { get; set; }

        public bool CompileInParallel { get; set; }
        public NPCMakerSettings()
        {
            Version = 3;
            GameVersion = Lists.GameVersions.oot_mq_debug;
            GCCFlags = "-G 0 -Os -fno-builtin -fno-reorder-blocks -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3 -mno-explicit-relocs -mno-memcpy -mno-check-zero-division -fno-optimize-sibling-calls";
            CodeEditor = CCode.CodeEditorEnum.VSCode;
            CustomCodeEditorPath = "";
            CustomCodeEditorArgs = "";
            Verbose = false;
            ColorizeScriptSyntax = true;
            ImproveTextMsgReadability = true;
            CheckSyntax = true;
            AutoComp_Save = true;
            CompileTimeout = 5000;
            LastOpenPath = Environment.CurrentDirectory;
            LastSaveBinaryPath = Environment.CurrentDirectory;
            ParseTime = 1000;
            UseWine = false;
            AutoSave = false;
            AutoSaveTime = 1000;
            UseSpaceWithFromFont = false;
            AutoComp_ActorSwitch = false;
            CompileInParallel = true;
        }

        public enum Members
        {
            CHECKSYNTAX,
            COLORIZESYNTAX,
            IMPROVEMSGPRV,
            VERBOSECODE,
            GAMEVERSION,
            GCCARGS,
            CODEEDITOR,
            CODEEDITORPATH,
            CODEEDITORARGS,
            NOMEMBER,
            COMPILETIMEOUT,
            AUTOCOMP_SAVE,
            PARSETIME,
            USEWINE,
            AUTOSAVE,
            AUTOSAVETIME,
            AUTOSAVESWITCH,
            USESPACEFONT,
            PARALLEL,
        }
        public static Members GetMemberFromTag(object Tag, string PassingObjectName)
        {
            Members Member;

            try
            {
                if (Tag is string t)
                    Member = (Members)Enum.Parse(typeof(Members), t);
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
                case Members.AUTOSAVESWITCH: AutoComp_ActorSwitch = (bool)Value; break;
                case Members.CHECKSYNTAX: CheckSyntax = (bool)Value; break;
                case Members.COLORIZESYNTAX: ColorizeScriptSyntax = (bool)Value; break;
                case Members.IMPROVEMSGPRV: ImproveTextMsgReadability = (bool)Value; break;
                case Members.VERBOSECODE: Verbose = (bool)Value; break;
                case Members.CODEEDITOR: CodeEditor = (CCode.CodeEditorEnum)Enum.Parse(typeof(CCode.CodeEditorEnum), (string)Value); break;
                case Members.CODEEDITORARGS: CustomCodeEditorArgs = (string)Value; break;
                case Members.CODEEDITORPATH: CustomCodeEditorPath = (string)Value; break;
                case Members.GCCARGS: GCCFlags = (string)Value; break;
                case Members.GAMEVERSION: GameVersion = (Lists.GameVersions)Enum.Parse(typeof(Lists.GameVersions), (string)Value); break;
                case Members.COMPILETIMEOUT: CompileTimeout = Convert.ToUInt32(Value); break;
                case Members.PARSETIME: ParseTime = Convert.ToUInt32(Value); break;
                case Members.USEWINE: UseWine = (bool)Value; break;
                case Members.AUTOSAVE: AutoSave = (bool)Value; break;
                case Members.AUTOSAVETIME: AutoSaveTime = Convert.ToUInt32(Value); break;
                case Members.USESPACEFONT: UseSpaceWithFromFont = (bool)Value; break;
                case Members.PARALLEL: CompileInParallel = (bool)Value; break;
                default: break;
            }
        }
    }
}
