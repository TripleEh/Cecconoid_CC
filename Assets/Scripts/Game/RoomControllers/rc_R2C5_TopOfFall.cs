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

public class rc_R2C5_TopOfFall : RoomControllerBase
{
	public override void OnRoomEnter()
	{
		base.OnRoomEnter();

		if (!GameGlobals.TestGameEvent(Types.s_iGE_TopOfFallFirstEntry))
		{
			GameGlobals.SetGameEvent(Types.s_iGE_TopOfFallFirstEntry);
			TimerManager.AddTimer(Types.s_fPLAYER_RoomTransitionDuration, DelayedCloseDoor);
		}
		else m_aDoors[0].CloseInstant();
	}


	public void DelayedCloseDoor()
	{
		m_aDoors[0].Close();
	}
}
