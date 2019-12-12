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

public class UI_SetNewHighScoreText : MonoBehaviour
{
	[Header("SETUP IN EDITOR")]
	[SerializeField] private Text m_gcNew=null;
	[SerializeField] private Text m_gcOld=null;

	// Start is called before the first frame update
	void OnEnable()
	{
		if(GameInstance.Object.m_bIsCecconoid)
		{
			m_gcNew.text = "NEW: " + GameGlobals.s_iCecconoidCurrentScore.ToString();
			m_gcOld.text = "PREV: " + GameGlobals.s_iCecconoidPrevGameScore.ToString();
		}
		else
		{
			m_gcNew.text = "NEW: " + GameGlobals.s_iEugatronCurrentScore.ToString();
			m_gcOld.text = "PREV: " + GameGlobals.s_iEugatronPrevGameScore.ToString();
		}
	}
}
