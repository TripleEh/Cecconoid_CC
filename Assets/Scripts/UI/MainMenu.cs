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

public class MainMenu : MonoBehaviour
{
	// For our state machine...	
	protected enum EMainMenuState
	{
		_FADE_MASTER_IN,
		_FADE_MASTER_OUT,
		_FADE_BUTTONS_IN,
		_FADE_SETTINGS_IN,
		_FADE_CREDITS_IN,
		_IDLEIN_BUTTONS,
		_IDLEIN_SETTINGS,
		_IDLEIN_CREDITS,
	}


	[Header("UI Objects - Setup In Editor")]
	[SerializeField] protected GameObject m_goEventSystem = null;
	[SerializeField] protected GameObject m_goCanvas = null;
	[SerializeField] protected CanvasGroup m_gcMasterGroup = null;
	[SerializeField] protected CanvasGroup m_gcButtonsGroup = null;
	[SerializeField] protected CanvasGroup m_gcSettingsGroup = null;
	[SerializeField] protected CanvasGroup m_gcCreditsGroup = null;
	[SerializeField] protected UI_ForceEventSystem_FirstButton m_gcESHandler = null;
	[SerializeField] protected GameObject m_goSettingsPrimaryButton = null;
	[SerializeField] protected GameObject m_goMenuPrimaryButton = null;

	protected EMainMenuState m_iMenuState = EMainMenuState._IDLEIN_BUTTONS;


	// Verify we have what we need 
	// 
	virtual public void Start()
	{
		GAssert.Assert(null != m_goEventSystem, "MainMenu: Class references haven't been setup in the editor!");
		GAssert.Assert(null != m_goCanvas, "MainMenu: Class references haven't been setup in the editor!");
		GAssert.Assert(null != m_gcButtonsGroup, "MainMenu: Class references haven't been setup in the editor!");
		GAssert.Assert(null != m_gcSettingsGroup, "MainMenu: Class references haven't been setup in the editor!");
		GAssert.Assert(null != m_gcCreditsGroup, "MainMenu: Class references haven't been setup in the editor!");
		GAssert.Assert(null != m_gcESHandler, "MainMenu: Class references haven't been setup in the editor!");

		OnHideInstant();
	}



	virtual public void SetDefaults()
	{
		OnHideInstant();
	}



	virtual public void OnShow()
	{
		m_gcMasterGroup.alpha = 0f;						// <-- inverted because we fade in...
		m_gcMasterGroup.interactable = true;
		m_iMenuState = EMainMenuState._FADE_MASTER_IN;
		m_goEventSystem.SetActive(true);
		m_goCanvas.SetActive(true);

		Cursor.visible = true;
	}



	virtual public void OnHide()
	{
		m_gcMasterGroup.alpha = 1f;
		m_gcMasterGroup.interactable = false;
		m_goEventSystem.SetActive(false);
		m_iMenuState = EMainMenuState._FADE_MASTER_OUT;
		Messenger.Invoke(Types.s_sMISC_KillAllParticles);
		Cursor.visible = false;
	}


	virtual public void OnHideMainCanvas()
	{
		m_gcMasterGroup.alpha = 0f;
		m_gcMasterGroup.interactable = false;
		m_goEventSystem.SetActive(false);
	}


	virtual public void OnShowMainCanvas()
	{
		m_gcMasterGroup.alpha = 1f;
		m_gcMasterGroup.interactable = true;
		m_goEventSystem.SetActive(true);
	}

	public void ShowSettings()
	{
		m_iMenuState = EMainMenuState._FADE_SETTINGS_IN;
	}


	public void ShowMainMenu()
	{
		m_iMenuState = EMainMenuState._FADE_BUTTONS_IN;
	}


	public void ShowCredits()
	{
		m_iMenuState = EMainMenuState._FADE_CREDITS_IN;
	}


	virtual public void OnHideInstant()
	{
		m_iMenuState = EMainMenuState._FADE_MASTER_OUT;

		m_gcButtonsGroup.gameObject.SetActive(true);
		m_gcButtonsGroup.alpha = 1f;
		m_gcButtonsGroup.interactable = true;
		m_gcSettingsGroup.gameObject.SetActive(false);
		m_gcSettingsGroup.alpha = 0f;
		m_gcSettingsGroup.interactable = false;
		m_gcCreditsGroup.gameObject.SetActive(false);
		m_gcCreditsGroup.alpha = 0f;
		m_gcCreditsGroup.interactable = false;

		m_goCanvas.SetActive(false);
		m_goEventSystem.SetActive(false);
	}



	public void Update()
	{
		switch (m_iMenuState)
		{
			case EMainMenuState._FADE_MASTER_IN:
				m_gcMasterGroup.alpha += Types.s_fDUR_MenuFadeSpeed * TimerManager.fGameDeltaTime;

				if (m_gcMasterGroup.alpha >= 0.99f)
				{
					m_iMenuState = EMainMenuState._IDLEIN_BUTTONS;
					m_goEventSystem.SetActive(true);
					m_gcESHandler.ForceNewButton(m_goMenuPrimaryButton);
				}
				break;



			case EMainMenuState._FADE_MASTER_OUT:
				m_gcMasterGroup.alpha -= Types.s_fDUR_MenuFadeSpeed * TimerManager.fGameDeltaTime;

				if (m_gcMasterGroup.alpha <= 0.01f) OnHideInstant();
				break;



			case EMainMenuState._FADE_BUTTONS_IN:
				{
					m_gcButtonsGroup.gameObject.SetActive(true);
					m_gcButtonsGroup.alpha = 1f;
					m_gcButtonsGroup.interactable = true;
					m_gcSettingsGroup.gameObject.SetActive(false);
					m_gcSettingsGroup.alpha = 0f;
					m_gcSettingsGroup.interactable = false;
					m_gcCreditsGroup.gameObject.SetActive(false);
					m_gcCreditsGroup.alpha = 0f;
					m_gcCreditsGroup.interactable = false;
					m_gcESHandler.ForceNewButton(m_goMenuPrimaryButton);
					m_iMenuState = EMainMenuState._IDLEIN_BUTTONS;
				}
				break;



			case EMainMenuState._FADE_SETTINGS_IN:
				{
					m_gcButtonsGroup.gameObject.SetActive(false);
					m_gcButtonsGroup.alpha = 0f;
					m_gcButtonsGroup.interactable = false;
					m_gcSettingsGroup.gameObject.SetActive(true);
					m_gcSettingsGroup.alpha = 1f;
					m_gcSettingsGroup.interactable = true;
					m_gcCreditsGroup.gameObject.SetActive(false);
					m_gcCreditsGroup.alpha = 0f;
					m_gcCreditsGroup.interactable = false;
					m_gcESHandler.ForceNewButton(m_goSettingsPrimaryButton);
					m_iMenuState = EMainMenuState._IDLEIN_SETTINGS;
				}
				break;


			case EMainMenuState._FADE_CREDITS_IN:
				{
					m_gcButtonsGroup.gameObject.SetActive(false);
					m_gcButtonsGroup.alpha = 0f;
					m_gcButtonsGroup.interactable = false;
					m_gcSettingsGroup.gameObject.SetActive(false);
					m_gcSettingsGroup.alpha = 0f;
					m_gcSettingsGroup.interactable = false;
					m_gcCreditsGroup.gameObject.SetActive(true);
					m_gcCreditsGroup.alpha = 1f;
					m_gcCreditsGroup.interactable = true;
					m_gcESHandler.ForceNewButton(m_goSettingsPrimaryButton);
					m_iMenuState = EMainMenuState._IDLEIN_CREDITS;
				}
				break;

			case EMainMenuState._IDLEIN_BUTTONS: 
			case EMainMenuState._IDLEIN_SETTINGS:
			case EMainMenuState._IDLEIN_CREDITS: break;
		}
	}
}
