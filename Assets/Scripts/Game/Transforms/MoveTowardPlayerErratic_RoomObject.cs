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

public class MoveTowardPlayerErratic_RoomObject : MoveTowardPlayerErratic, Types.IRoom_EnemyObject
{

	private Vector3 m_vInitPosition = Vector3.zero;
	private bool m_bIsActive = false;

	private void Start()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GetDirToPlayer();
		m_vInitPosition = transform.position;
	}



	public void OnRoomEnter()
	{
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


	private void FixedUpdate()
	{
		if (!m_bIsActive) return;
		if (TimerManager.IsPaused()) return;
		if (!GameMode.PlayerIsAlive()) return;
		if (TimerManager.fGameTime > m_fNextEventTime) GetDirToPlayer();

		m_gcRgdBdy.MovePosition(transform.position + ((m_vTraj * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
	}
}
