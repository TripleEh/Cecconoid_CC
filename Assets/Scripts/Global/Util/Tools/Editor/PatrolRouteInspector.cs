using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolRoute))]
public class PatrolRouteInspector :Editor 
{
	private void OnSceneGUI()
	{
		PatrolRoute route = target as PatrolRoute;
		Transform handleTransform = route.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		if(null == route.m_aPoints) return;

		for(uint i = 0; i < route.m_aPoints.Length-1; ++i)
		{
			Vector3 p0 = handleTransform.TransformPoint(route.m_aPoints[i]);
			Vector3 p1 = handleTransform.TransformPoint(route.m_aPoints[i+1]);

			Handles.color = Color.white;
			Handles.DrawLine(p0, p1);
			Handles.DoPositionHandle(p0, handleRotation);
			Handles.DoPositionHandle(p1, handleRotation);

			EditorGUI.BeginChangeCheck();
			p0 = Handles.DoPositionHandle(p0, handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(route, "Move Point");
				EditorUtility.SetDirty(route);
				route.m_aPoints[i] = handleTransform.InverseTransformPoint(p0);
			}
			EditorGUI.BeginChangeCheck();
			p1 = Handles.DoPositionHandle(p1, handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(route, "Move Point");
				EditorUtility.SetDirty(route);
				route.m_aPoints[i+1] = handleTransform.InverseTransformPoint(p1);
			}
		}
	}
}
