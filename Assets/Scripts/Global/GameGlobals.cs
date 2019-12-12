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

// This class tracks the flags for Global Game Events / Params that may be 
// referred to by classes / managers / UI throughout the game. 
// 
// Most settings in this class will be saved/reloaded so would be persistent 
// for the player. 
//

public static class GameGlobals
{
	// Bitfields to track game events
	private static ulong m_iGameStateFlags_01;
	private static ulong m_iAchievementStateFlags_01;

	// Player Room Transitions, TransitionTo needs to be saved out as the player's last spawn point!
	public static Vector3 s_vRoomTransitionTo = Vector3.zero;
	public static Vector3 s_vRoomTransitionFrom = Vector3.zero;

	// User Preferences
	public static float s_fVOL_Master = Types.s_fVOL_MaxAttenuation;
	public static float s_fVOL_SFX = Types.s_fVOL_MaxAttenuation;
	public static float s_fVOL_Music = Types.s_fVOL_DefaultMusic;

	// Slider values to match the above...
	public static float s_fUI_SliderMaster = 1.0f;
	public static float s_fUI_SliderMusic = 1.0f;
	public static float s_fUI_SliderSFX = 1.0f;

	// Scores
	public static ulong s_iEugatronCurrentScore = 0;
	public static ulong s_iCecconoidCurrentScore = 0;
	public static ulong s_iEugatronPrevGameScore = 0;
	public static ulong s_iCecconoidPrevGameScore = 0;
	public static bool s_bEugatronHighScoreEntryThisGo = false;
	public static bool s_bCEcconoidHighScoreEntryThisGo = false;

	public static Types.SHighScore[] s_aEugatronHighScores = new Types.SHighScore[10];
	public static Types.SHighScore[] s_aCecconoidHighScores = new Types.SHighScore[10];


	public static void CheckEugatronHighScore(ulong iNewScore)
	{
		// Quick reference for the High Score splash...
		s_iEugatronPrevGameScore = s_iEugatronCurrentScore;
		s_iEugatronCurrentScore = iNewScore;

		// Is the current score higher than the worst in the table?
		if (s_iEugatronCurrentScore > s_aEugatronHighScores[0]._iScore) s_bEugatronHighScoreEntryThisGo = true;
		else s_bEugatronHighScoreEntryThisGo = false;
	}



	public static void LoadHighScores_Local()
	{
		if(PlayerPrefs.HasKey("EUGESCORENAME9"))
		{
			s_aEugatronHighScores[9]._sName = PlayerPrefs.GetString("EUGESCORENAME9");
			s_aEugatronHighScores[8]._sName = PlayerPrefs.GetString("EUGESCORENAME8");
			s_aEugatronHighScores[7]._sName = PlayerPrefs.GetString("EUGESCORENAME7");
			s_aEugatronHighScores[6]._sName = PlayerPrefs.GetString("EUGESCORENAME6");
			s_aEugatronHighScores[5]._sName = PlayerPrefs.GetString("EUGESCORENAME5");
			s_aEugatronHighScores[4]._sName = PlayerPrefs.GetString("EUGESCORENAME4");
			s_aEugatronHighScores[3]._sName = PlayerPrefs.GetString("EUGESCORENAME3");
			s_aEugatronHighScores[2]._sName = PlayerPrefs.GetString("EUGESCORENAME2");
			s_aEugatronHighScores[1]._sName = PlayerPrefs.GetString("EUGESCORENAME1");
			s_aEugatronHighScores[0]._sName = PlayerPrefs.GetString("EUGESCORENAME0");

			s_aCecconoidHighScores[9]._sName = PlayerPrefs.GetString("CECCOSCORENAME9");
			s_aCecconoidHighScores[8]._sName = PlayerPrefs.GetString("CECCOSCORENAME8");
			s_aCecconoidHighScores[7]._sName = PlayerPrefs.GetString("CECCOSCORENAME7");
			s_aCecconoidHighScores[6]._sName = PlayerPrefs.GetString("CECCOSCORENAME6");
			s_aCecconoidHighScores[5]._sName = PlayerPrefs.GetString("CECCOSCORENAME5");
			s_aCecconoidHighScores[4]._sName = PlayerPrefs.GetString("CECCOSCORENAME4");
			s_aCecconoidHighScores[3]._sName = PlayerPrefs.GetString("CECCOSCORENAME3");
			s_aCecconoidHighScores[2]._sName = PlayerPrefs.GetString("CECCOSCORENAME2");
			s_aCecconoidHighScores[1]._sName = PlayerPrefs.GetString("CECCOSCORENAME1");
			s_aCecconoidHighScores[0]._sName = PlayerPrefs.GetString("CECCOSCORENAME0");

			s_aEugatronHighScores[9]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE9");
			s_aEugatronHighScores[8]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE8");
			s_aEugatronHighScores[7]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE7");
			s_aEugatronHighScores[6]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE6");
			s_aEugatronHighScores[5]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE5");
			s_aEugatronHighScores[4]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE4");
			s_aEugatronHighScores[3]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE3");
			s_aEugatronHighScores[2]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE2");
			s_aEugatronHighScores[1]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE1");
			s_aEugatronHighScores[0]._iScore = (ulong)PlayerPrefs.GetInt("EUGESCORE0");

			s_aCecconoidHighScores[9]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE9");
			s_aCecconoidHighScores[8]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE8");
			s_aCecconoidHighScores[7]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE7");
			s_aCecconoidHighScores[6]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE6");
			s_aCecconoidHighScores[5]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE5");
			s_aCecconoidHighScores[4]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE4");
			s_aCecconoidHighScores[3]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE3");
			s_aCecconoidHighScores[2]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE2");
			s_aCecconoidHighScores[1]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE1");
			s_aCecconoidHighScores[0]._iScore = (ulong)PlayerPrefs.GetInt("CECCOSCORE0");
		}
		else
		{
			for (int i = 9; i >= 0; --i)
			{
				s_aCecconoidHighScores[i]._iScore = (ulong)(i + 1) * 100000;
				s_aEugatronHighScores[i]._iScore = (ulong)(i + 1) * 100000;
			}

			s_aEugatronHighScores[9]._sName = "KOR";
			s_aEugatronHighScores[8]._sName = "HOF";
			s_aEugatronHighScores[7]._sName = "STE";
			s_aEugatronHighScores[6]._sName = "SCO";
			s_aEugatronHighScores[5]._sName = "YAK";
			s_aEugatronHighScores[4]._sName = "JED";
			s_aEugatronHighScores[3]._sName = "RIK";
			s_aEugatronHighScores[2]._sName = "DAW";
			s_aEugatronHighScores[1]._sName = "ANT";
			s_aEugatronHighScores[0]._sName = "GS!";

			s_aCecconoidHighScores[9]._sName = "KOR";
			s_aCecconoidHighScores[8]._sName = "HOF";
			s_aCecconoidHighScores[7]._sName = "STE";
			s_aCecconoidHighScores[6]._sName = "SCO";
			s_aCecconoidHighScores[5]._sName = "YAK";
			s_aCecconoidHighScores[4]._sName = "JED";
			s_aCecconoidHighScores[3]._sName = "RIK";
			s_aCecconoidHighScores[2]._sName = "DAW";
			s_aCecconoidHighScores[1]._sName = "ANT";
			s_aCecconoidHighScores[0]._sName = "GS!";

			SavePersistentScores();
		}
	}



	public static void CheckCecconoidHighScore(ulong iNewScore)
	{
		s_iCecconoidPrevGameScore = s_iCecconoidCurrentScore;
		s_iCecconoidCurrentScore = iNewScore;

		if (s_iCecconoidCurrentScore > s_aCecconoidHighScores[0]._iScore) s_bCEcconoidHighScoreEntryThisGo = true;
		else s_bCEcconoidHighScoreEntryThisGo = false;
	}



	public static void SaveEugatronScore(string sName)
	{
		// Find the index in the list
		int iTopIndex = 9;
		int iBotIndex = 0;
		while (s_iEugatronCurrentScore < s_aEugatronHighScores[iTopIndex]._iScore) iTopIndex--;

		// Copy everything down
		while (iBotIndex < iTopIndex)
		{
			s_aEugatronHighScores[iBotIndex] = s_aEugatronHighScores[iBotIndex + 1];
			++iBotIndex;
		}

		// Store the score...
		s_aEugatronHighScores[iTopIndex]._sName = sName;
		s_aEugatronHighScores[iTopIndex]._iScore = s_iEugatronCurrentScore;
		SavePersistentScores();
	}



	public static void SaveCecconoidScore(string sName)
	{
		// Find the index in the list
		int iTopIndex = 9;
		int iBotIndex = 0;
		while (s_iCecconoidCurrentScore < s_aCecconoidHighScores[iTopIndex]._iScore) iTopIndex--;

		// Copy everything down
		while (iBotIndex < iTopIndex)
		{
			s_aCecconoidHighScores[iBotIndex] = s_aCecconoidHighScores[iBotIndex + 1];
			++iBotIndex;
		}

		// Store the score...
		s_aCecconoidHighScores[iTopIndex]._sName = sName;
		s_aCecconoidHighScores[iTopIndex]._iScore = s_iCecconoidCurrentScore;
		SavePersistentScores();
	}



	public static void SavePersistentScores()
	{
		PlayerPrefs.SetString("EUGESCORENAME9", s_aEugatronHighScores[9]._sName);
		PlayerPrefs.SetString("EUGESCORENAME8", s_aEugatronHighScores[8]._sName);
		PlayerPrefs.SetString("EUGESCORENAME7", s_aEugatronHighScores[7]._sName);
		PlayerPrefs.SetString("EUGESCORENAME6", s_aEugatronHighScores[6]._sName);
		PlayerPrefs.SetString("EUGESCORENAME5", s_aEugatronHighScores[5]._sName);
		PlayerPrefs.SetString("EUGESCORENAME4", s_aEugatronHighScores[4]._sName);
		PlayerPrefs.SetString("EUGESCORENAME3", s_aEugatronHighScores[3]._sName);
		PlayerPrefs.SetString("EUGESCORENAME2", s_aEugatronHighScores[2]._sName);
		PlayerPrefs.SetString("EUGESCORENAME1", s_aEugatronHighScores[1]._sName);
		PlayerPrefs.SetString("EUGESCORENAME0", s_aEugatronHighScores[0]._sName);

		PlayerPrefs.SetString("CECCOSCORENAME9", s_aCecconoidHighScores[9]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME8", s_aCecconoidHighScores[8]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME7", s_aCecconoidHighScores[7]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME6", s_aCecconoidHighScores[6]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME5", s_aCecconoidHighScores[5]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME4", s_aCecconoidHighScores[4]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME3", s_aCecconoidHighScores[3]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME2", s_aCecconoidHighScores[2]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME1", s_aCecconoidHighScores[1]._sName);
		PlayerPrefs.SetString("CECCOSCORENAME0", s_aCecconoidHighScores[0]._sName);

		PlayerPrefs.SetInt("EUGESCORE9", (int) s_aEugatronHighScores[9]._iScore);
		PlayerPrefs.SetInt("EUGESCORE8", (int) s_aEugatronHighScores[8]._iScore);
		PlayerPrefs.SetInt("EUGESCORE7", (int) s_aEugatronHighScores[7]._iScore);
		PlayerPrefs.SetInt("EUGESCORE6", (int) s_aEugatronHighScores[6]._iScore);
		PlayerPrefs.SetInt("EUGESCORE5", (int) s_aEugatronHighScores[5]._iScore);
		PlayerPrefs.SetInt("EUGESCORE4", (int) s_aEugatronHighScores[4]._iScore);
		PlayerPrefs.SetInt("EUGESCORE3", (int) s_aEugatronHighScores[3]._iScore);
		PlayerPrefs.SetInt("EUGESCORE2", (int) s_aEugatronHighScores[2]._iScore);
		PlayerPrefs.SetInt("EUGESCORE1", (int) s_aEugatronHighScores[1]._iScore);
		PlayerPrefs.SetInt("EUGESCORE0", (int) s_aEugatronHighScores[0]._iScore);

		PlayerPrefs.SetInt("CECCOSCORE9", (int) s_aCecconoidHighScores[9]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE8", (int) s_aCecconoidHighScores[8]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE7", (int) s_aCecconoidHighScores[7]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE6", (int) s_aCecconoidHighScores[6]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE5", (int) s_aCecconoidHighScores[5]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE4", (int) s_aCecconoidHighScores[4]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE3", (int) s_aCecconoidHighScores[3]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE2", (int) s_aCecconoidHighScores[2]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE1", (int) s_aCecconoidHighScores[1]._iScore);
		PlayerPrefs.SetInt("CECCOSCORE0", (int) s_aCecconoidHighScores[0]._iScore);
	}


	public static void SetGameEvent(ulong iFlag)
	{
		m_iGameStateFlags_01 |= iFlag;
	}


	public static bool TestGameEvent(ulong iFlag)
	{
		return (bool)((m_iGameStateFlags_01 & iFlag) != 0);
	}



	public static void ClearGameEvent(ulong iFlag)
	{
		m_iGameStateFlags_01 &= ~(iFlag);
	}


	public static void SetAchievement(Types.EAchievementIdentifier iID)
	{
		int shifter = (int)iID;
		m_iAchievementStateFlags_01 |= ((ulong)0x01 << shifter);
		PlayerPrefs.SetInt("Achievements", (int)m_iAchievementStateFlags_01);
	}



	public static bool TestAchievement(Types.EAchievementIdentifier iID)
	{
		int shifter = (int)iID;
		return (bool)((m_iAchievementStateFlags_01 & ((ulong)0x01 << shifter)) != 0);
	}


	public static void ClearAchievement(Types.EAchievementIdentifier iID)
	{
		int shifter = (int)iID;
		m_iAchievementStateFlags_01 &= ~((ulong)0x01 << shifter);
	}


	public static void SetDefaults()
	{
		m_iGameStateFlags_01 = 0x00;

		// Set this in PlayerSettings->Other Settings->Configuration->Scripting Define Symbols to 
		// enable all achievements, in every run of the game...
		{
		#if TEST_ACHIEVEMENTS
			PlayerPrefs.SetInt("Achievements", 0);
			m_iAchievementStateFlags_01 = (ulong)PlayerPrefs.GetInt("Achievements");
		#else
			if (PlayerPrefs.HasKey("Achievements"))
				m_iAchievementStateFlags_01 = (ulong)PlayerPrefs.GetInt("Achievements");
			else
			{
				m_iAchievementStateFlags_01 = 0x00;
				PlayerPrefs.SetInt("Achievements", (int)m_iAchievementStateFlags_01);
			}
		#endif
		}

		SetGameEvent(Types.s_iGE_IntroShown);
	}
}
