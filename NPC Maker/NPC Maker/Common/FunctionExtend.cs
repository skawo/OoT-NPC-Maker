using Microsoft.CSharp;
using NPC_Maker.Controls;
using NPC_Maker.Windows;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace NPC_Maker.Common
{
    public class FunctionExtend
    {
        public enum FuncExtendHooks
        {
            OnStart,
            OnEnd,
            OnMainWindowOpen,
            BeforeMainWindowClose,
            OnMainWindowClose,
            OnJsonSerialize,
            OnJsonSave,
            OnJsonParse,
        }

        public struct OnJsonParse
        {
            public NPCFile file;
            public string fileName;
        }

        public struct OnJsonSerialize
        {
            public NPCFile file;
            public bool isBackup;
        }

        public struct OnJsonSave
        {
            public string json;

            public bool isBackup;
        }

        public struct OnEnd
        {
            public int retVar;
        }

        public struct GenericTool
        {
            public MainWindow window;
            public ProgressWithLabel progressControl;
        }

        public static Dictionary<string, object> extraTools = new Dictionary<string, object>();

        public static void GetTagExtensions()
        {
            StringBuilder sb = new StringBuilder();
            bool Errors = false;

            string pathCur = Path.GetFullPath("funcextend");

            if (Directory.Exists(pathCur))
            {
                string[] files = Directory.GetFiles(pathCur);
                string fshared = "";

                string fsharedPath = Path.Combine(pathCur, "fshared");

                if (File.Exists(fsharedPath))
                    fshared = File.ReadAllText(fsharedPath);

                foreach (string file in files)
                {
                    string fName = Path.GetFileNameWithoutExtension(file);

                    if (file == fsharedPath || extraTools.ContainsKey(fName))
                        continue;

                    string[] code = File.ReadAllLines(file);

                    Dictionary<string, string> providerOptions = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };

                    CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

                    CompilerParameters compilerParams = new CompilerParameters
                    {
                        GenerateInMemory = true,
                        GenerateExecutable = false,
                    };

                    for (int i = 0; i < code.Length; i++)
                    {
                        string line = code[i];

                        if (line.StartsWith("include "))
                        {
                            compilerParams.ReferencedAssemblies.Add(line.Substring(8));
                            code[i] = "";
                        }
                    }

                    string endCode = String.Join(Environment.NewLine, code);
                    endCode = endCode.Replace("##FSHARED##", fshared);

                    CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, endCode);

                    if (results.Errors.HasErrors)
                    {
                        foreach (CompilerError m in results.Errors)
                        {
                            if (!m.IsWarning)
                            {
                                Errors = true;
                                sb.Append($"{fName}:{m.ErrorText}{Environment.NewLine}");
                            }
                        }
                    }
                    else
                    {
                        object o = results.CompiledAssembly.CreateInstance("FuncExtend.Function");
                        extraTools.Add(fName, o);
                    }
                }

                if (Errors)
                    BigMessageBox.Show(sb.ToString());
            }
        }

        public static void RunExtendFunc(string Name, object argStruct, string MethodName = "ToolProcess")
        {
            if (extraTools.ContainsKey(Name))
            {
                object o = extraTools[Name];
                MethodInfo mi = o.GetType().GetMethod(MethodName);
                mi.Invoke(o, new object[] { argStruct });
            }
        }

        public static object RunExtendFuncWithRet(string Name, object argStruct, string MethodName = "ToolProcess")
        {
            if (extraTools.ContainsKey(Name))
            {
                object o = extraTools[Name];
                MethodInfo mi = o.GetType().GetMethod(MethodName);
                return mi.Invoke(o, new object[] { argStruct });
            }

            return null;
        }
    }

}
