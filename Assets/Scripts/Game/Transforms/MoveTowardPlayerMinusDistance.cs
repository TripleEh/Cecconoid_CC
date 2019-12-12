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

public class MoveTowardPlayerMinusDistance : MoveTowardPlayer
{
	[SerializeField] private float m_fDistanceAwayFromPlayer = 0.18f;

	// Update is called once per frame
	void Update()
	{
		if (null == m_goPlayer || !GameInstance.Object.GetPlayerState().PlayerIsAlive()) return;

		Vector3 vVecToPlayer = m_goPlayer.transform.position - transform.position;
		if (null != m_goPlayer && vVecToPlayer.magnitude > m_fDistanceAwayFromPlayer)
		{
			m_vTrajectory = vVecToPlayer;
			m_vTrajectory.Normalize();
			transform.position += (m_vTrajectory * m_fMovementSpeed) * TimerManager.fGameDeltaTime;
		}
	}
}
