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

public class MoveTowardPlayer_RoomObject : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMovementSpeed = 0.1f;

	private GameObject m_goPlayer;
	private Vector3 m_vTrajectory = Vector3.zero;
	private Vector3 m_vInitPosition = Vector3.zero;
	private bool m_bIsActive = false;
	
	void Start()
	{
		m_vInitPosition = transform.position;
	}

	public void OnRoomEnter()
	{
		m_goPlayer = GameInstance.Object.GetPlayerObject();
		GAssert.Assert(null!=m_goPlayer, "Unable to get player object");
		transform.position = m_vInitPosition;
		m_bIsActive = true;
	}

	public void OnRoomExit()
	{
		m_bIsActive = false;
		transform.position = m_vInitPosition;
	}


	public void OnPlayerHasDied()
	{
		m_bIsActive = false;
	}


	public void OnPlayerRespawn()
	{
		m_bIsActive = true;
	}



	public void OnReset()
	{
		transform.position = m_vInitPosition;
	}


	void Update()
	{
		if(!m_bIsActive) return;
		if(null == m_goPlayer) return;

		m_vTrajectory = m_goPlayer.transform.position - transform.position;
		m_vTrajectory.Normalize();
		if (null != m_goPlayer) transform.position += (m_vTrajectory * m_fMovementSpeed) * TimerManager.fGameDeltaTime;
	}
}
