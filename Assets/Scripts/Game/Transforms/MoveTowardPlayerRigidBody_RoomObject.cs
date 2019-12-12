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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayerRigidBody_RoomObject : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMovementSpeed = 0.1f;

	private Rigidbody2D m_gcRgdBdy = null;
	private GameObject m_goPlayer = null;
	private Vector3 m_vTrajectory = Vector3.zero;
	private Vector3 m_vInitPosition = Vector3.zero;
	private bool m_bIsActive = false;

	void Awake()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		m_vInitPosition = transform.position;
	}

	public void OnRoomEnter()
	{
		m_goPlayer = GameInstance.Object.GetPlayerObject();
		GAssert.Assert(null != m_goPlayer, "Unable to get player object");
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
		m_gcRgdBdy.MovePosition(m_vInitPosition);
	}

	void FixedUpdate()
	{
		if(!m_bIsActive) return;
		if (null == m_goPlayer) return;
		if (TimerManager.IsPaused()) return;
		if (!GameInstance.Object.GetPlayerState().PlayerIsAlive()) return;

		m_vTrajectory = m_goPlayer.transform.position - transform.position;
		m_vTrajectory.Normalize();
		m_vTrajectory = (m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime;
		Vector2 vPos = new Vector2(transform.position.x + m_vTrajectory.x, transform.position.y + m_vTrajectory.y);
		m_gcRgdBdy.MovePosition(vPos);
	}
}
