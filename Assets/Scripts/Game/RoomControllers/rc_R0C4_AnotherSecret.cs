
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


public class rc_R0C4_AnotherSecret : RoomControllerBase, Types.IRoom_SwitchResponder
{
	public void OnSwitchFlip()
	{
		if (GameGlobals.TestGameEvent(Types.s_iGE_AnotherSecretDoor)) return;

		GameGlobals.SetGameEvent(Types.s_iGE_AnotherSecretDoor);
		m_aDoors[0].Open();
	}

	public void OnSwitchFlop()
	{

	}

	public override void OnRoomEnter()
	{
		GAssert.Assert(m_aDoors.Length > 0, "RC_LaserIntro: Door hasn't been setup in the editor");

		base.OnRoomEnter();

		if (GameGlobals.TestGameEvent(Types.s_iGE_AnotherSecretDoor)) m_aDoors[0].OpenInstant();
		else m_aDoors[0].CloseInstant();
	}
}
