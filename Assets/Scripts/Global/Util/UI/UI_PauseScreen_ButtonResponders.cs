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

public class UI_PauseScreen_ButtonResponders : MonoBehaviour
{
	public void OnButtonResumePressed()
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
		GameMode.UnPauseGame();
	}

	public void OnQuitButtonPressed()
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
		GameMode.UnPauseGame();
		Messenger.Invoke(Types.s_sUI_HidePauseScreen);
		Messenger.Invoke(Types.s_sGF_BeginExitGame);
	}
}
