﻿// Cecconoid by Triple Eh? Ltd -- 2019
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

public class UI_MainMenu_Eugatron_ButtonResponders : MonoBehaviour
{
	public void OnButtonPlayPressed()
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
		Messenger.Invoke(Types.s_sMenu_BeginTransitionToGame);
	}


	public void OnButtonHighScoresPress()
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
		Messenger.Invoke(Types.s_sMenu_ShowHighScoreTableEugatron);
	}

	public void OnButtonExitGamePressed()
	{
		Messenger.Invoke(Types.s_sMenu_ExitToSelect);
	}
}
