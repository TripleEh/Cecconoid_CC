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
using UnityEngine.UI;

public class UI_SetNoHighScoreText : MonoBehaviour
{
	[SerializeField] private Text m_gcNoHigh = null;

	// Start is called before the first frame update
	void OnEnable()
	{
		if(GameInstance.Object.m_bIsCecconoid) m_gcNoHigh.text = GameGlobals.s_iCecconoidCurrentScore.ToString();
		else m_gcNoHigh.text = GameGlobals.s_iEugatronCurrentScore.ToString();
	}
}
