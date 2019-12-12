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

public class PatrolRoute : MonoBehaviour
{
	public Vector3[] m_aPoints;
	public Vector3 GetPatrolRoutePositionAtIndex(int iIndex) { GAssert.Assert(iIndex < m_aPoints.Length, "Index out of bounds!"); return transform.position + m_aPoints[iIndex]; }
}
