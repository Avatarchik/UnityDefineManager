using UnityEditor;
using UnityEngine;

namespace caneva20.UnityDefineManager.Editor {
    public class ScriptingDefineObject : ScriptableObject {
        [SerializeField] private Compiler _compiler;
        [SerializeField] private BuildTargetGroup _buildTarget;
        [SerializeField] private string[] _defines;
        [SerializeField] private bool _isApplied;
    }
}