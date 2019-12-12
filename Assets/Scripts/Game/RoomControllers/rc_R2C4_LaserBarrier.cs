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


// Logic in this room:
// - On Second Entry, if Robotron2 complete, only unlock the left door
//

public class rc_R2C4_LaserBarrier :RoomControllerBase 
{
	[Header("Room Logic")]
	[SerializeField] private Doorway m_aDoorRight = null;
	[SerializeField] private TimedLaserBarrier m_aBarrier = null;

	public override void OnRoomEnter()
	{
		base.OnRoomEnter();
		if(GameGlobals.TestGameEvent(Types.s_iGE_Robotron2))
		{
			m_aDoorRight.CloseInstant();
			m_aBarrier.TurnOffInstant(true);
		}
	}
}
