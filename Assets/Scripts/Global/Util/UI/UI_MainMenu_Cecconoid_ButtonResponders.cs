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

[RequireComponent(typeof(MainMenu))]
public class UI_MainMenu_Cecconoid_ButtonResponders : MonoBehaviour
{
	public void OnButtonPlayPressed()
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
		Messenger.Invoke(Types.s_sMenu_BeginTransitionToGame);
	}


	public void OnButtonHighScoresPress()
	{
    GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
    Messenger.Invoke(Types.s_sMenu_ShowHighScoreTableCecconoid);
	}


	public void OnSettingsButtonPressed()
	{
		MainMenu gc = GetComponent<MainMenu>();
		GAssert.Assert(null != gc, "Unable to get main menu!");
		gc.ShowSettings();
	}

	public void OnCreditsButtonPressed()
	{
		MainMenu gc = GetComponent<MainMenu>();
		GAssert.Assert(null != gc, "Unable to get main menu!");
		gc.ShowCredits();
	}


	public void OnSettingsBackButtonPressed()
	{
		MainMenu gc = GetComponent<MainMenu>();
		GAssert.Assert(null != gc, "Unable to get main menu!");
		gc.ShowMainMenu();

		//GNTODO: Save user prefs
	}



	public void OnCreditsBackButtonPressed()
	{
		MainMenu gc = GetComponent<MainMenu>();
		GAssert.Assert(null != gc, "Unable to get main menu!");
		gc.ShowMainMenu();
	}


	public void OnExitGamePressed()
	{
		Messenger.Invoke(Types.s_sMenu_ExitToSelect);
	}


	public void OnMuteMusic(bool bState)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.MuteMusic(bState);
	}


	public void OnMuteSFX(bool bState)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.MuteSFX(bState);
	}


	public void OnMasterSlide(float fVal)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.SetMasterVol(fVal);
	}


	public void OnMusicSlider(float fVal)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.SetMusicVol(fVal);
	}


	public void OnSFXSlider(float fVal)
	{
		GameAudioManager gc = GameInstance.Object.GetAudioManager();
		GAssert.Assert(null != gc, "Unable to get Audio Manager");
		gc.SetSFXVol(fVal);
	}
}
