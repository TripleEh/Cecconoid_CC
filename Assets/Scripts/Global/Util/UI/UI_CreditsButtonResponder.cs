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

public class UI_CreditsButtonResponder : MonoBehaviour
{
	[SerializeField] private UI_InputModuleActions m_gcActions = null;
	[SerializeField] private UI_MainMenu_Cecconoid_ButtonResponders m_gcButtonResponder = null;

	private void Start()
	{
		GAssert.Assert(null != m_gcActions, "Credits button responder not setup in editor!");
		GAssert.Assert(null != m_gcButtonResponder, "Credits button repsonder not setup in editor!");
	}

	void Update()
	{
		if(m_gcActions.AnyActionButtonPressed()) m_gcButtonResponder.OnCreditsBackButtonPressed();
	}
}
