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

// Logic is simplified in this room as we're not going to let 
// the player re-enter once they've exited stage-right!
//
public class rc_R1C3_SwarmRoom : RoomControllerBase
{
	[Header("Room Settings")]
	[SerializeField] private SwarmController m_gcSwarm = null;
	[SerializeField] private TimedLaserBarrier m_gcBarrier = null;
	[SerializeField] private Doorway m_gcDoorRight = null;
	[SerializeField] private Doorway m_gcDoorLeft = null;
	[SerializeField] private Doorway m_gcDoorBottom = null;

	private bool m_bCheckSwarmHealth = false;

	public override void OnRoomEnter()
	{
		// Has this room been completed before?
		if (GameGlobals.TestGameEvent(Types.s_iGE_SwarmKilled))
		{
			m_gcSwarm.OnRoomExit();
			m_gcBarrier.TurnOffInstant(true);
			m_gcDoorLeft.CloseInstant();
			m_gcDoorBottom.CloseInstant();
			return;
		}



		m_gcBarrier.OnRoomEnter();
		m_gcSwarm.OnRoomEnter();
		MoveTowardPlayer_RoomObject gc = m_gcSwarm.GetComponent<MoveTowardPlayer_RoomObject>();
		if(null != gc) gc.OnRoomEnter();

		// First Entry, just shuffle them out of the bottom of the screen
		if (!GameGlobals.TestGameEvent(Types.s_iGE_Robotron2))
		{
			m_gcDoorRight.CloseInstant();
			if (!m_gcDoorLeft.GetIsClosed()) TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay, CloseDoorsFirstEntry);
			m_bCheckSwarmHealth = false;
			m_gcSwarm.gameObject.GetComponent<MoveTowardPlayer_RoomObject>().enabled = false;
		}
		// Second entry, lock them in until they kill the swarm...
		else
		{
			m_gcBarrier.TurnOffInstant(true);
			m_gcDoorLeft.CloseInstant();
			m_gcDoorRight.CloseInstant();
      m_gcSwarm.gameObject.GetComponent<MoveTowardPlayer_RoomObject>().enabled = true;
			TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay, CloseDoorSecondEntry);
			m_bCheckSwarmHealth = true;
		}
	}


	public override void OnRoomExit()
	{
		m_gcSwarm.OnRoomExit();
		m_gcBarrier.OnRoomExit();
		MoveTowardPlayer_RoomObject gc = m_gcSwarm.GetComponent<MoveTowardPlayer_RoomObject>();
		if (null != gc) gc.OnRoomExit();
	}
	

	public void CloseDoorsFirstEntry()
	{
		m_gcDoorLeft.Close();
	}


	public void CloseDoorSecondEntry()
	{
		m_gcDoorBottom.Close();
	}

	void Update()
	{
		if (!m_bCheckSwarmHealth) return;

		if (m_gcSwarm.GetBlobCount() == 0)
		{
			m_gcDoorRight.Open();
			GameGlobals.SetGameEvent(Types.s_iGE_SwarmKilled);
			m_bCheckSwarmHealth = false;
		}
	}
}
