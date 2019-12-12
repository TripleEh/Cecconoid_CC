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
public class MoveTowardPlayerRigidBodyWithOffset_RoomObject : MoveTowardPlayerRigidBodyWithOffset, Types.IRoom_EnemyObject
{
	protected bool m_bIsActive = false;

	public override void Start()
	{
		// Do Nothing...
	}

	public void OnRoomEnter()
	{
		// Validate 
		{
			m_gcRgdBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRgdBdy, "Unable to find Rigid Body!");

			m_gcGameInstance = GameInstance.Object;
			GAssert.Assert(null != m_gcGameInstance, "Unable to setup game instance!");

			m_goPlayer = m_gcGameInstance.GetPlayerObject();
			GAssert.Assert(null != m_goPlayer, "Unable to get player object");
		}

		UpdatePixelOffset();
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
		m_bIsActive = true;
	}



	public void OnReset()
	{

	}



	protected override void FixedUpdate()
	{
		if(m_bIsActive)	base.FixedUpdate();
	}
}
