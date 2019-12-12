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

public class InGamePauseScreen : MonoBehaviour
{
	[Header("UI Objects - Setup In Editor")]
	[SerializeField] private GameObject m_goEventSystem = null;
	[SerializeField] private GameObject m_goCanvas= null;

	public void Start()
	{
		OnHide();
	}

	public void SetDefaults()
	{
		Messenger.AddListener(Types.s_sUI_ShowPauseScreen, OnShow);
		Messenger.AddListener(Types.s_sUI_HidePauseScreen, OnHide);
		OnHide();
	}

	private void OnDisable()
	{
		Messenger.RemoveListener(Types.s_sUI_ShowPauseScreen, OnShow);
		Messenger.RemoveListener(Types.s_sUI_HidePauseScreen, OnHide);
	}

	public void OnShow()
	{
		m_goEventSystem.SetActive(true);
		m_goCanvas.SetActive(true);
	} 


	public void OnHide()
	{
		m_goEventSystem.SetActive(false);
		m_goCanvas.SetActive(false);
	}
}


