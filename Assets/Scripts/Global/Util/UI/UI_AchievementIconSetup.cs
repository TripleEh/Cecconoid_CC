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

public class UI_AchievementIconSetup : MonoBehaviour
{
	[Header("Setup")]
	[SerializeField] private Text m_gcText = null;
	[SerializeField] private Image m_gcIcon = null;

  public void SetInfo(ref Types.SAchievementInfo info)
	{
		m_gcText.text = info._sAchievementTitle;
		m_gcIcon.sprite = info._Assets._gcSprite;
	}
}
