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

public class rc_R1C2_LaserIntro : RoomControllerBase, Types.IRoom_SwitchResponder
{
	public void OnSwitchFlip()
	{
		if(GameGlobals.TestGameEvent(Types.s_iGE_LaserIntroDoorToggled)) return;

		GameGlobals.SetGameEvent(Types.s_iGE_LaserIntroDoorToggled);
		m_aDoors[0].Open();
	}

	public void OnSwitchFlop()
	{

	}

	public override void OnRoomEnter()
	{
		GAssert.Assert(m_aDoors.Length > 0, "RC_LaserIntro: Door hasn't been setup in the editor");

		base.OnRoomEnter();

		if(GameGlobals.TestGameEvent(Types.s_iGE_LaserIntroDoorToggled)) m_aDoors[0].OpenInstant();
		else m_aDoors[0].CloseInstant();

		TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay, CheckForAchievement);
	}


	// The first room shows images for the controls, so we don't want to overwhelm 
	// the player by firing the first entry achievement at the same time
	// So, now they've figured out how to move to the next room, wait for the 
	// transition to end and give them a little present...
	//
	public void CheckForAchievement()
	{
		// Is this the first entry to the game? Achievement Unlocked! (This timer callback is only called in Cecconoid, see Eugatron room controller for the equiv...)
		GameMode.AchievementUnlocked(Types.EAchievementIdentifier._WelcomeToCecconoid);
	}
}

