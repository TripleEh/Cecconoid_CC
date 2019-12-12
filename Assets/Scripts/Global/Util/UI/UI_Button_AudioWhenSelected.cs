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
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Button_AudioWhenSelected : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
	public void OnSelect(BaseEventData eventData)
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_SELECTED);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._UI_BUTTON_SELECTED);
	}
}
