using UnityEditor;

namespace caneva20.UnityDefineManager.Editor {
    public class ScriptingDefineWindow : EditorWindow {
        private ScriptingDefineObject _asset;

        private UnityEditor.Editor _editor;

        [MenuItem("Window/Unity Define Manager")]
        private static void Init() {
            GetWindow<ScriptingDefineWindow>(true, "Unity Define Manager", true);
        }

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