using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace caneva20.UnityDefineManager.Editor {
    public static class GlobalDefineUtility {
        // http://forum.unity3d.com/threads/93901-global-define/page2
        // Do not modify these paths
        private const string C_SHARP_PATH = "Assets/mcs.rsp";
        private const string EDITOR_PATH = "Assets/gmcs.rsp";

        public static string[] GetDefines(Compiler compiler) {
            switch (compiler) {
                case Compiler.CSharp: return ParseRspFile(C_SHARP_PATH);
                case Compiler.Editor: return ParseRspFile(EDITOR_PATH);
                default: return null;
            }
        }

        public static void SetDefines(Compiler compiler, string[] defs) {
            switch (compiler) {
                case Compiler.CSharp:
                    WriteDefines(C_SHARP_PATH, defs);
                    break;

                case Compiler.Editor:
                    WriteDefines(EDITOR_PATH, defs);
                    break;
            }

            var first = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories).FirstOrDefault();

            if (!string.IsNullOrEmpty(first)) {
                AssetDatabase.ImportAsset(first);
            }
        }

        public static string[] ParseRspFile(string path) {
            if (!File.Exists(path)) {
                return new string[0];
            }

            var lines = File.ReadAllLines(path);
            var defs = new List<string>();

            foreach (var line in lines) {
                if (line.StartsWith("-define:")) {
                    defs.AddRange(line.Replace("-define:", "").Split(';'));
                }
            }

            return defs.ToArray();
        }

        public static void WriteDefines(string path, string[] defs) {
            if (defs == null || defs.Length < 1 && File.Exists(path)) {
                File.Delete(path);
                File.Delete(path + ".meta");
                AssetDatabase.Refresh();
                return;
            }

            var sb = new StringBuilder();

            sb.Append("-define:");

            for (var i = 0; i < defs.Length; i++) {
                sb.Append(defs[i]);
                
                if (i < defs.Length - 1) {
                    sb.Append(";");
                }
            }

            using (var writer = new StreamWriter(path, false)) {
                writer.Write(sb.ToString());
            }
        }
    }
}