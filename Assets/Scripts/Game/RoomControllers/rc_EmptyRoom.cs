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


// Default empty room logic
//

public class rc_EmptyRoom : RoomControllerBase
{
	[Header("Custom Room - SETUP IN EDITOR")]
	[SerializeField] private Doorway m_gcDoorway = null;


	public override void OnRoomEnter()
	{
		GAssert.Assert(null != m_gcDoorway, "rc_EmptyRoom: Doorway for game event not setup!");
		base.OnRoomEnter();

		if(!GameGlobals.TestGameEvent(Types.s_iGE_PassedLaserIntroRoom))
		{
			TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay, DelayedCloseDoor);	
			GameGlobals.SetGameEvent(Types.s_iGE_PassedLaserIntroRoom);
		}
		else m_gcDoorway.CloseInstant();
	}

	public void DelayedCloseDoor()
	{
		m_gcDoorway.Close();
	}
}
