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

public class AttractToPlayer : MonoBehaviour
{
	[SerializeField] private float m_fAtttractionDistance=0.2f;
	[SerializeField] private float m_fMaxSpeed = 1.0f;


	private GameObject m_goPlayer = null;
	private Vector3 m_vTrajectory = Vector3.zero;
	private float m_fDistanceFromPlayer = 0.0f;
	private bool m_bCanUpdate = true;



	public float GetDistanceFromPlayer() { return m_fDistanceFromPlayer; }


	private void Start()
	{
		m_goPlayer = GameInstance.Object.GetPlayerObject();
		GAssert.Assert(null != m_goPlayer, "Unable to get player object!");
		Messenger.AddListener(Types.s_sGF_BeginRoomTransition, StopUpdate);
	}



	private void OnDestroy()
	{
		Messenger.RemoveListener(Types.s_sGF_BeginRoomTransition, StopUpdate);
	}



	public void StopUpdate()
	{
		m_bCanUpdate = false;
	}



	private void Update()
	{ 
		// Check if we can move this tick
		if(null == m_goPlayer) return;
		if(!m_bCanUpdate) return;

		// Early out if we're too far away...
		m_fDistanceFromPlayer = (m_goPlayer.transform.position - transform.position).magnitude;
		if (m_fDistanceFromPlayer >= m_fAtttractionDistance) return;

		// Work out how fast to move toward the player
		float fRatio = 1.0f - (m_fDistanceFromPlayer / m_fAtttractionDistance);
		float fEasedVelocity = m_fMaxSpeed * Easing.EaseIn(fRatio, EEasingType.Quintic);

		// Calc the trajectory and apply it
		{
			m_vTrajectory = m_goPlayer.transform.position - transform.position;
			m_fDistanceFromPlayer = m_vTrajectory.magnitude;
			m_vTrajectory.Normalize();
			transform.position += (m_vTrajectory * fEasedVelocity) * TimerManager.fGameDeltaTime;
		}
	}
}
