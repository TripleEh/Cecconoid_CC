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

public class HighScore_Euganoid : MonoBehaviour
{
	[Header("UI Elmenents -- SETUP IN EDITOR")]
	[SerializeField] protected CanvasGroup m_gcEntryCanvas = null;
	[SerializeField] protected Text m_gcInitial1 = null;
	[SerializeField] protected Text m_gcInitial2 = null;
	[SerializeField] protected Text m_gcInitial3 = null;

	[SerializeField] protected CanvasGroup m_gcTableCanvas = null;
	[SerializeField] protected Text[] m_aInitials = new Text[10];
	[SerializeField] protected Text[] m_aScores = new Text[10];


	private void Start()
	{
		GAssert.Assert(null != m_gcInitial1, "HighScore_Eugatron: components not setup in editor!");
		GAssert.Assert(null != m_gcInitial2, "HighScore_Eugatron: components not setup in editor!");
		GAssert.Assert(null != m_gcInitial3, "HighScore_Eugatron: components not setup in editor!");
		GAssert.Assert(null != m_gcEntryCanvas, "HighScore_Eugatron: components not setup in editor!");
		GAssert.Assert(null != m_gcTableCanvas, "HighScore_Eugatron: components not setup in editor!");

		OnHideInstant();
	}



	public void OnHideInstant()
	{
		m_gcEntryCanvas.alpha = 0;
		m_gcEntryCanvas.gameObject.SetActive(false);
		m_gcTableCanvas.alpha = 0;
		m_gcTableCanvas.gameObject.SetActive(false);
	}




	public virtual void OnShowInstant()
	{
		ResetInitialsEntry();

		// New high?
		if (GameGlobals.s_bEugatronHighScoreEntryThisGo)
		{
			m_gcEntryCanvas.alpha = 1;
			m_gcEntryCanvas.gameObject.SetActive(true);

			m_gcTableCanvas.alpha = 0;
			m_gcTableCanvas.gameObject.SetActive(false);
		}
		// Just display the table...
		else OnShowTable();
	}



	public virtual void OnShowTable(bool bCheckScore = true)
	{
		m_gcEntryCanvas.alpha = 0;
		m_gcEntryCanvas.gameObject.SetActive(false);

		m_gcTableCanvas.alpha = 1;
		m_gcTableCanvas.gameObject.SetActive(true);

		for (int i = 9; i >= 0; --i)
		{
			m_aInitials[i].text = GameGlobals.s_aEugatronHighScores[i]._sName;
			m_aScores[i].text = GameGlobals.s_aEugatronHighScores[i]._iScore.ToString("D10");

			// Do we set the score to flash? 
			if(bCheckScore)
			{
				if (GameGlobals.s_aEugatronHighScores[i]._iScore == GameGlobals.s_iEugatronCurrentScore)
				{
					m_aScores[i].GetComponent<UI_FlashTextColour>().m_bIsEnabled = true;
					m_aInitials[i].GetComponent<UI_FlashTextColour>().m_bIsEnabled = true;
				}
				else
				{
					m_aScores[i].GetComponent<UI_FlashTextColour>().m_bIsEnabled = false;
					m_aInitials[i].GetComponent<UI_FlashTextColour>().m_bIsEnabled = false;
				}
			}
		}
	}



	public void ResetInitialsEntry()
	{
		m_gcInitial1.text = "_";
		m_gcInitial2.text = "_";
		m_gcInitial3.text = "_";
	}



	// GNTODO: Convert this to a struct of text entries, rather than an index and switch...
	public Text GetTextEntry(uint iIndex)
	{
		GAssert.Assert(iIndex < 3, "GetTextEntry called with invalid index");

		Text gcRet = null;
		switch (iIndex)
		{
			case 0: gcRet = m_gcInitial1; break;
			case 1: gcRet = m_gcInitial2; break;
			case 2: gcRet = m_gcInitial3; break;
		}
		return gcRet;
	}


	public string GetFinalString()
	{
		string sRet = m_gcInitial1.text.Substring(0, 1).ToUpper()
								+ m_gcInitial2.text.Substring(0, 1).ToUpper()
								+ m_gcInitial3.text.Substring(0, 1).ToUpper();

		return sRet;
	}
}
