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

[RequireComponent(typeof(Rigidbody2D))]
public class MoveTowardRandomPointInRoom_RoomObject : MoveTowardRandomPointInRoom, Types.IRoom_EnemyObject
{

	override public void Start()
	{
		// Block setup in start!
	}

	// Setup moved to here
	public void OnRoomEnter()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GAssert.Assert(null != m_gcRgdBdy, "Unable to get rigidbody!" );

		// Order is important! We must be active or GetRandomPoint will early out!
		m_bIsActive = true;
		GetRandomPoint();
	}

	public void OnRoomExit()
	{
		base.OnDisable();
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

	}

	override public void FixedUpdate()
	{
		if (!m_bIsActive) return;
		if (TimerManager.IsPaused()) return;
		if (!GameInstance.Object.GetPlayerState().PlayerIsAlive()) return;

		Vector2 vNewPos = MathUtil.GetBoundsCheckedVector(transform.position + ((m_vTraj * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
		m_gcRgdBdy.MovePosition(vNewPos);
	}
}
