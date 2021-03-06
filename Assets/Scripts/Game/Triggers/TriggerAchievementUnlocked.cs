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

public class TriggerAchievementUnlocked : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private Types.EAchievementIdentifier m_iID = Types.EAchievementIdentifier._WelcomeToCecconoid;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag(Types.s_sTag_Player)) GameMode.AchievementUnlocked(m_iID);
	}
}
