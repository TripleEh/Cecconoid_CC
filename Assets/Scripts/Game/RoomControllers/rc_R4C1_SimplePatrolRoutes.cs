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


public class rc_R4C1_SimplePatrolRoutes : RoomControllerBase, Types.IRoom_SwitchResponder
{
	public void OnSwitchFlip()
	{
		if (GameGlobals.TestGameEvent(Types.s_iGE_SimplePatrolRouteSwitch)) return;
		GameGlobals.SetGameEvent(Types.s_iGE_SimplePatrolRouteSwitch);
		OpenAllDoors();
	}

	public void OnSwitchFlop()
	{

	}

	public override void OnRoomEnter()
	{
		GAssert.Assert(m_aDoors.Length > 0, "RC_LaserIntro: Door hasn't been setup in the editor");

		base.OnRoomEnter();

		if (GameGlobals.TestGameEvent(Types.s_iGE_SimplePatrolRouteSwitch)) OpenAllDoorsInstant();
		else TimerManager.AddTimer(Types.s_fDUR_DoorCloseDuration, CloseAllDoors);
	}
}
