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


public class HighScore_Ceccotron : HighScore_Euganoid
{
	public override void OnShowInstant()
	{
		ResetInitialsEntry();

		if (GameGlobals.s_bCEcconoidHighScoreEntryThisGo)
		{
			m_gcEntryCanvas.alpha = 1;
			m_gcEntryCanvas.gameObject.SetActive(true);

			m_gcTableCanvas.alpha = 0;
			m_gcTableCanvas.gameObject.SetActive(false);
		}
		else OnShowTable();
	}


	public override void OnShowTable(bool bCheckScore = true)
	{
		m_gcEntryCanvas.alpha = 0;
		m_gcEntryCanvas.gameObject.SetActive(false);

		m_gcTableCanvas.alpha = 1;
		m_gcTableCanvas.gameObject.SetActive(true);

		for (int i = 9; i >= 0; --i)
		{
			m_aInitials[i].text = GameGlobals.s_aCecconoidHighScores[i]._sName;
			m_aScores[i].text = GameGlobals.s_aCecconoidHighScores[i]._iScore.ToString("D10");

			if (bCheckScore)
			{
				if (GameGlobals.s_aCecconoidHighScores[i]._iScore == GameGlobals.s_iCecconoidCurrentScore)
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
}
