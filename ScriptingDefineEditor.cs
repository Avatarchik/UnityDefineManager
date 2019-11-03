using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace caneva20.UnityDefineManager {
    [CustomEditor(typeof(ScriptingDefineObject))]
    public class ScriptingDefineEditor : UnityEditor.Editor {
        private const int COMPILER_COUNT = 3;
        private SerializedProperty _buildTarget;

        private SerializedProperty _compiler;
        private BuildTargetGroup _currentTargetGroup;
        private SerializedProperty _defines;
        private SerializedProperty _isApplied;
        private ReorderableList _reorderableList;

        private void OnEnable() {
            _compiler = serializedObject.FindProperty("_compiler");
            SetCompilerTarget((Compiler) _compiler.intValue);

            _reorderableList = new ReorderableList(serializedObject, _defines);
            _reorderableList.drawHeaderCallback += OnDrawHeader;
            _reorderableList.drawElementCallback += OnDrawListElement;
        }

        private void OnDisable() {
            if (!_isApplied.boolValue) {
                var dialog = EditorUtility.DisplayDialog(
                    "Unsaved Changes",
                    "Would you like to save changes to the scripting defines?",
                    "Yes", "No");

                if (dialog) {
                    ApplyDefines();
                }
            }
        }

        private void SetCompilerTarget(Compiler compiler) {
            _compiler.intValue = (int) compiler;

            _defines = serializedObject.FindProperty("_defines");
            _isApplied = serializedObject.FindProperty("_isApplied");

            if (_compiler.intValue == (int) Compiler.Platform) {
                _buildTarget = serializedObject.FindProperty("_buildTarget");
                _currentTargetGroup = (BuildTargetGroup) _buildTarget.intValue;

                SetBuildTarget(_currentTargetGroup == BuildTargetGroup.Unknown
                    ? BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)
                    : _currentTargetGroup);
            } else {
                var defs = GlobalDefineUtility.GetDefines((Compiler) _compiler.intValue).ToList();

                _defines.arraySize = defs.Count;

                for (var i = 0; i < defs.Count; i++) {
                    _defines.GetArrayElementAtIndex(i).stringValue = defs[i];
                }

                _isApplied.boolValue = true;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void SetBuildTarget(BuildTargetGroup target) {
            _currentTargetGroup = target;
            _buildTarget.intValue = (int) target;

            var defs = GetScriptingDefineSymbols((BuildTargetGroup) _buildTarget.enumValueIndex);
            _defines.arraySize = defs.Length;

            for (var i = 0; i < defs.Length; i++) {
                _defines.GetArrayElementAtIndex(i).stringValue = defs[i];
            }

            _isApplied.boolValue = true;
            serializedObject.ApplyModifiedProperties();
        }

        private static string[] GetScriptingDefineSymbols(BuildTargetGroup group) {
            var res = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            return res.Split(';');
        }

        private void ApplyDefines() {
            var arr = new string[_defines.arraySize];

            for (int i = 0, c = arr.Length; i < c; i++) {
                arr[i] = _defines.GetArrayElementAtIndex(i).stringValue;
            }

            if (_compiler.intValue == (int) Compiler.Platform) {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(_currentTargetGroup, string.Join(";", arr));
            } else {
                GlobalDefineUtility.SetDefines((Compiler) _compiler.intValue, arr);
            }

            _isApplied.boolValue = true;

            serializedObject.ApplyModifiedProperties();

            GUI.FocusControl("");
        }

        private void OnDrawHeader(Rect rect) {
            var cur = ((Compiler) _compiler.intValue).ToString();

            if (_compiler.intValue == (int) Compiler.Platform) {
                cur += " " + (BuildTargetGroup) _buildTarget.intValue;
            }

            GUI.Label(rect, cur, EditorStyles.boldLabel);
        }

        private void OnDrawListElement(Rect rect, int index, bool isactive, bool isfocused) {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUIUtility.labelWidth = 4;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), element);
            EditorGUIUtility.labelWidth = 0;
        }

        public override void OnInspectorGUI() {
            Styles.Init();

            serializedObject.Update();

            var oldColor = GUI.backgroundColor;

            GUILayout.Space(2);

            GUILayout.BeginHorizontal();

            for (var i = 0; i < COMPILER_COUNT; i++) {
                if (i == _compiler.intValue) {
                    GUI.backgroundColor = Color.gray;
                }

                GUIStyle st;
                switch (i) {
                    case 0:
                        st = EditorStyles.miniButtonLeft;
                        break;
                    case COMPILER_COUNT - 1:
                        st = EditorStyles.miniButtonRight;
                        break;
                    default:
                        st = EditorStyles.miniButtonMid;
                        break;
                }

                if (GUILayout.Button(((Compiler) i).ToString(), st)) {
                    _compiler.intValue = i;
                    SetCompilerTarget((Compiler) i);
                }

                GUI.backgroundColor = oldColor;
            }

            GUILayout.EndHorizontal();

            if (_compiler.intValue == (int) Compiler.Platform) {
                var cur = (BuildTargetGroup) _buildTarget.intValue;

                GUILayout.Space(3);

                EditorGUI.BeginChangeCheck();
                cur = (BuildTargetGroup) EditorGUILayout.EnumPopup(cur);

                if (EditorGUI.EndChangeCheck()) {
                    SetBuildTarget(cur);
                }
            }

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginVertical(Styles.listContainer);

            _reorderableList.DoLayoutList();

            if (EditorGUI.EndChangeCheck()) {
                _isApplied.boolValue = false;
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            var wasEnabled = GUI.enabled;

            GUI.enabled = !_isApplied.boolValue;

            if (GUILayout.Button("Apply", EditorStyles.miniButton)) {
                ApplyDefines();
            }

            GUI.enabled = wasEnabled;

            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private static class Styles {
            public static GUIStyle listContainer;
            private static bool _isInitialized;

            public static void Init() {
                if (_isInitialized) {
                    return;
                }

                _isInitialized = true;

                listContainer = new GUIStyle {
                    margin = new RectOffset(4, 4, 4, 4)
                };
            }
        }
    }
}