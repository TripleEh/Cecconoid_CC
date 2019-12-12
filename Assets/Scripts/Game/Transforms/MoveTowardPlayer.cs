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

public class MoveTowardPlayer : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] protected float m_fMovementSpeed = 0.1f;

	protected GameObject m_goPlayer;
	protected Vector3 m_vTrajectory = Vector3.zero;

	
	protected virtual void Start()
	{
		m_goPlayer = GameInstance.Object.GetPlayerObject();
		GAssert.Assert(null!=m_goPlayer, "Unable to get player object");
	}

	void Update()
	{
		if(null == m_goPlayer) return;

		m_vTrajectory = m_goPlayer.transform.position - transform.position;
		m_vTrajectory.Normalize();
		if (null != m_goPlayer) transform.position += (m_vTrajectory * m_fMovementSpeed) * TimerManager.fGameDeltaTime;
	}
}
