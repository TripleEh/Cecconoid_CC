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

public class UI_DevMenu_ButtonResponders : MonoBehaviour
{
	public void OnGodPressed(bool bState)
	{
		if (null != GameInstance.Object && null != GameInstance.Object.GetPlayerState())
			GameInstance.Object.GetPlayerState().SetPlayerIsGod(bState);
	}

	public void OnGiveAllPressed(bool bState)
	{
		PlayerState gc = GameInstance.Object.GetPlayerState();
		GAssert.Assert(null != gc, "Unable to get player state!");
		gc.SetPlayerInventoryAll(bState);
	}

	public void OnGiveSomePressed(bool bState)
	{
		PlayerState gc = GameInstance.Object.GetPlayerState();
		GAssert.Assert(null != gc, "Unable to get player state!");
		gc.SetPlayerInventorySome(bState);
	}

	public void OnNeverDropPressed(bool bState)
	{
		PlayerState gc = GameInstance.Object.GetPlayerState();
		GAssert.Assert(null != gc, "Unable to get player state!");
		gc.SetPlayerCanDrop(!bState);
	}

	public void OnMuteMusicPressed(bool bState)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.MuteMusic(bState);
	}

	public void OnMuteSFXPressed(bool bState)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.MuteSFX(bState);
	}

	public void OnVolumeSlider(float fValue)
	{

	}

	public void OnSFXSlider(float fValue)
	{

	}

	public void OnMusicSlider(float fValue)
	{

	}

	public void RoomJumpButtonResponder(Vector3 vPos, ref GameObject goRoomController)
	{
		GameMode.TeleportPlayer(vPos, ref goRoomController);
		GameMode.ToggleDevMenu();	
	}
}
