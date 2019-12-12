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


public class rc_R4C9_LevelExitRoom : RoomControllerBase
{
	[Header("Room Specific")]
	[SerializeField] private TimedLaserBarrier m_gcExitBarrier = null;


	public override void OnRoomEnter()
	{
		base.OnRoomEnter();
		if(GameGlobals.TestGameEvent(Types.s_iGE_ExitKey)) m_gcExitBarrier.TurnOffInstant();
	}
}
