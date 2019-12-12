using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
	private void OnSceneGUI()
	{
		Line line = target as Line;
		Transform handleTransform = line.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
		Vector3 p0 = handleTransform.TransformPoint(line.vStart);
		Vector3 p1 = handleTransform.TransformPoint(line.vEnd);

		Handles.color = Color.white;
		Handles.DrawLine(p0, p1);
		Handles.DoPositionHandle(p0, handleRotation);
		Handles.DoPositionHandle(p1, handleRotation);

		EditorGUI.BeginChangeCheck();
		p0 = Handles.DoPositionHandle(p0, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.vStart = handleTransform.InverseTransformPoint(p0);
		}
		EditorGUI.BeginChangeCheck();
		p1 = Handles.DoPositionHandle(p1, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.vEnd = handleTransform.InverseTransformPoint(p1);
		}
	}
}
