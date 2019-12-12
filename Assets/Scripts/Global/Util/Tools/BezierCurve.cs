// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.
//
//

using UnityEngine;

public class BezierCurve : MonoBehaviour
{
	public Vector3[] points;

	public void Reset()
	{
		points = new Vector3[] {
						 new Vector3(1f, 0f, 0f),
						 new Vector3(2f, 0f, 0f),
						 new Vector3(3f, 0f, 0f),
						 new Vector3(4f, 0f, 0f) };
	}
	
	public Vector3 GetPoint(float t)
	{
		return transform.TransformPoint(MathUtil.GetBezierCurvePoint(points[0], points[1], points[2], points[3], t));
	}
}
