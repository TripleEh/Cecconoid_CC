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

public class Achievements : MonoBehaviour
{
	// Public array that contains the sprites we've setup in the editor
	//
	[Header("Achievement Icons - (Order is not important)")]
	public Types.SAchievementAssets[] m_aAchievementAssets = new Types.SAchievementAssets[Types.s_iNumAchievements];

	[HideInInspector]
	public Types.SAchievementInfo[] m_aAchievementInfo = new Types.SAchievementInfo[Types.s_iNumAchievements];


	// This is our list of titles. It could be auto-generated from the LOC spreadsheet
	// Order is important here. It should match the ordering in Types.EAchievementID
	//
	private string[] m_aTitles = new string[20]
	{
		"Welcome to Cecconoid",
		"Welcome to Eugatron",
		"Who Wants To Be...",
		"Better than Eugene",
		"Super Nashwan",
		"Mine's a 99",
		"Warmed up...",
		"Now I Ste you...",
		"Are you the gate keeper?",
		"Worthy of the name",
		"Harder, Better, Faster...",
		"Stakker Humanoid",
		"First of many...",
		"Sweet Bang Bang",
		"You'll be needing that",
		"Not Optional",
		"Lets Party Like It's 2084",
		"Smash TV!",
		"Total Carnage!",
		"Get your Llama Trons",
	};


	// Same, but for long descriptions...
	//
	private string[] m_aDescriptions = new string[20]
	{
		"First play of Cecconoid!",
		"First play of Eugatron!",
		"Million Points in Cecconoid",
		"Million Points in Eugatron",
		"Get all the power ups",
		"x99 in Cecconoid",
		"x50 in Eugatron",
		"Postcard from Zub",
		"Collected the key",
		"Completed Cecconoid",
		"Complete 10 level of Eugatron",
		"Have 10 Lives in Eugatron",
		"Die for the first time",
		"Die as many times as Antti, in one day",
		"Every little helps...",
		"Just don't get cocky...",
		"Like it's 2084",
		"Huge Euge",
		"Midway...",
		"Shareware forever...",
	};


	// Should be called by GameInstance, maps the sprite defined in the editor with the correct
	// title and description string...
	//
	public void BuildAchievementsList()
	{
		foreach (Types.SAchievementAssets entry in m_aAchievementAssets)
		{
			m_aAchievementInfo[(int)entry._iAchievementID]._Assets = entry;
			m_aAchievementInfo[(int)entry._iAchievementID]._sAchievementTitle = m_aTitles[(int)entry._iAchievementID];
			m_aAchievementInfo[(int)entry._iAchievementID]._sAcheivementDesc = m_aDescriptions[(int)entry._iAchievementID];
		}
	}
}
