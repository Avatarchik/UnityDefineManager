using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace caneva20.UnityDefineManager.Editor
{
	[SuppressMessage("ReSharper", "NotAccessedField.Local")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class ScriptingDefineObject : ScriptableObject
	{
		[SerializeField]
		Compiler m_Compiler;

		[SerializeField]
		BuildTargetGroup m_BuildTarget;

		[SerializeField]
		string[] m_Defines;

		[SerializeField]
		bool m_IsApplied;
	}
}
