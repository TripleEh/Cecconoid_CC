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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameSelect_ButtonResponders : MonoBehaviour
{
	public void OnButtonCecconoidClicked()
	{
		Messenger.Invoke("CECCONOID_SELECTED");
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
	}

	public void OnButtonEugatronClicked()
	{
		Messenger.Invoke("EUGATRON_SELECTED");
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_CLICKED);
	}


	public void OnButtonExitGame()
	{
		GameInstance.Object.ExitToDesktop();
	}
}
