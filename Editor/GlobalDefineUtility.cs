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

        public static void AddDefine(Compiler compiler, string define) {
            var defines = GetDefines(compiler).ToList();
            defines.Add(define);

            SetDefines(compiler, defines);
        }

        public static void RemoveDefine(Compiler compiler, string define) {
            var defines = GetDefines(compiler).ToList();
            defines.Remove(define);

            SetDefines(compiler, defines);
        }

        public static IEnumerable<string> GetDefines(Compiler compiler) {
            switch (compiler) {
                case Compiler.CSharp: return ParseRspFile(C_SHARP_PATH);
                case Compiler.Editor: return ParseRspFile(EDITOR_PATH);
                default: return Enumerable.Empty<string>();
            }
        }

        public static void SetDefines(Compiler compiler, IEnumerable<string> defs) {
            switch (compiler) {
                case Compiler.CSharp:
                    WriteDefines(C_SHARP_PATH, defs);
                    break;

                case Compiler.Editor:
                    WriteDefines(EDITOR_PATH, defs);
                    break;
            }

            Reimport();
        }

        private static void Reimport() {
            AssetDatabase.Refresh();
            
            var first = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories).FirstOrDefault();

            if (!string.IsNullOrEmpty(first)) {
                AssetDatabase.ImportAsset(first, ImportAssetOptions.ForceUpdate);
            }
        }

        private static IEnumerable<string> ParseRspFile(string path) {
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

            return defs;
        }

        private static void WriteDefines(string path, IEnumerable<string> defs) {
            var defines = defs?.Distinct().ToList() ?? Enumerable.Empty<string>().ToList();

            if (defines.Count <= 0 && File.Exists(path)) {
                File.Delete(path);
                File.Delete(path + ".meta");
                AssetDatabase.Refresh();
                return;
            }

            var sb = new StringBuilder();

            sb.Append("-define:");

            for (var i = 0; i < defines.Count; i++) {
                var value = defines[i].Trim().ToUpper().Replace(" ", "_");

                sb.Append(value);

                if (i < defines.Count - 1) {
                    sb.Append(";");
                }
            }

            using (var writer = new StreamWriter(path, false)) {
                writer.Write(sb.ToString());
            }
        }
    }
}