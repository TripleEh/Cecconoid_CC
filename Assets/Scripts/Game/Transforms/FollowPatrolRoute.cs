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

public class FollowPatrolRoute : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] private PatrolRoute m_gcPatrolRoute = null;
	[SerializeField] private float m_fMovementSpeed = 1.0f;
	[SerializeField] private int m_iNodeOffset = 0;

	private Vector3 m_vEndPos;
	private Vector3 m_vTraj;
	private bool m_bIsActive;
	private int m_iRouteIndex;


	public void OnRoomEnter()
	{
		OnReset();
		m_bIsActive = true;
	}



	public void OnRoomExit()
	{
		m_bIsActive = false;
	}



	public void OnPlayerHasDied()
	{
	}



	public void OnPlayerRespawn()
	{
	}



	public void OnReset()
	{
		GAssert.Assert(null != m_gcPatrolRoute, "Can't follow patrol as none assigned!");
		GAssert.Assert(m_iNodeOffset < m_gcPatrolRoute.m_aPoints.Length, "Node offset is longer than the patrol route node count!");

		m_iRouteIndex = m_iNodeOffset;
		transform.position = m_gcPatrolRoute.GetPatrolRoutePositionAtIndex(m_iRouteIndex);

		if( m_iNodeOffset >= m_gcPatrolRoute.m_aPoints.Length ) m_vEndPos = m_gcPatrolRoute.m_aPoints[0];
		else m_vEndPos = m_gcPatrolRoute.GetPatrolRoutePositionAtIndex(m_iRouteIndex+1);

		m_vTraj = (m_vEndPos - transform.position).normalized;
	}


	void Update()
	{
		if(!m_bIsActive) return;

		// Change to part of the route...
		float vDist = (m_vEndPos - transform.position).magnitude;

		if (vDist <= m_fMovementSpeed * TimerManager.fGameDeltaTime)
		{
			transform.position = m_vEndPos;

			++m_iRouteIndex;
			if(m_iRouteIndex >= m_gcPatrolRoute.m_aPoints.Length) m_iRouteIndex = 0;

			int fNextNode = m_iRouteIndex+1;
			if(fNextNode >= m_gcPatrolRoute.m_aPoints.Length) fNextNode = 0;
	
			m_vEndPos = m_gcPatrolRoute.GetPatrolRoutePositionAtIndex(fNextNode);
			m_vTraj = (m_vEndPos - transform.position).normalized;
		}

		// Move
		transform.position += (m_vTraj * m_fMovementSpeed) * TimerManager.fGameDeltaTime;
	}
}
