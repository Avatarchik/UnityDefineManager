using UnityEditor;
using UnityEngine;

namespace caneva20.UnityDefineManager.Editor {
    public class ScriptingDefineWindow : EditorWindow {
        [MenuItem("Window/Unity Define Manager")]
        private static void Init() {
            GetWindow<ScriptingDefineWindow>(true, "Unity Define Manager", true);
        }

        private UnityEditor.Editor _editor;
        private ScriptingDefineObject _asset;

        private void OnEnable() {
            _asset = CreateInstance<ScriptingDefineObject>();
            _editor = UnityEditor.Editor.CreateEditor(_asset);
        }

        private void OnDisable() {
            DestroyImmediate(_editor);
            DestroyImmediate(_asset);
        }

        private void OnGUI() {
            _editor.OnInspectorGUI();
        }
    }
}