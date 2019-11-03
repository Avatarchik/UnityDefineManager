using UnityEditor;
using UnityEngine;

namespace caneva20.UnityDefineManager {
    public class ScriptingDefineObject : ScriptableObject {
        [SerializeField] private BuildTargetGroup _buildTarget;
        [SerializeField] private Compiler _compiler;
        [SerializeField] private string[] _defines;
        [SerializeField] private bool _isApplied;
    }
}