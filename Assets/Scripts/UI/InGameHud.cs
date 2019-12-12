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

[RequireComponent(typeof(CanvasGroup))]
public class InGameHud : MonoBehaviour
{
	// Holders for the dynamic HUD elements
	[Header("UI Text Fields (Setup in Editor)")]
	[SerializeField] private Text m_sWaveNumber = null;
	[SerializeField] private Text m_sMultiplier = null;
	[SerializeField] private Text m_sScore = null;
	[SerializeField] private Text m_sLives = null;

	// Canvas group lets us fade in and out
	private CanvasGroup m_gcCanvasGroup;
	private GameInstance m_gcGameInstance;
	private PlayerState m_gcPlayerState;



	// Make sure we get the canvas group early doors
	private void Awake()
	{
		m_gcCanvasGroup = GetComponent<CanvasGroup>();
		GAssert.Assert(null != m_gcCanvasGroup, "Hud unable to get canvas group!");
	}



	// By default make sure the HUD is hidden
	//
	void Start()
	{
		OnHideHud();
	}



	// HUD responds to messages, there's nothing that's really 
	// got a handle to it directly
	//
	public void SetDefaults()
	{
		Messenger.AddListener(Types.s_sHUD_WaveNumberUpdated, OnWaveNumberUpdated);
		Messenger.AddListener(Types.s_sHUD_ScoreUpdated, OnScoreUpdated);
		Messenger.AddListener(Types.s_sHUD_LivesUpdated, OnLivesUpdated);
		Messenger.AddListener(Types.s_sHUD_MultiplierUpdated, OnMultiplierUpdated);
		Messenger.AddListener(Types.s_sHUD_ResetHud, OnResetHud);
		Messenger.AddListener(Types.s_sHUD_Hide, OnHideHud);
	}



	private void OnDisable()
	{
		Messenger.RemoveListener(Types.s_sHUD_WaveNumberUpdated, OnWaveNumberUpdated);
		Messenger.RemoveListener(Types.s_sHUD_ScoreUpdated, OnScoreUpdated);
		Messenger.RemoveListener(Types.s_sHUD_LivesUpdated, OnLivesUpdated);
		Messenger.RemoveListener(Types.s_sHUD_MultiplierUpdated, OnMultiplierUpdated);
		Messenger.RemoveListener(Types.s_sHUD_ResetHud, OnResetHud);
		Messenger.RemoveListener(Types.s_sHUD_Hide, OnHideHud);
	}



	
	public void OnHideHud()
	{
		m_sWaveNumber.text = "";
		m_sMultiplier.text = "";
		m_sScore.text = "";
		m_sLives.text = "";
		m_gcCanvasGroup.alpha = 0f;
	}



	// Trying to avoid localisation, by not using words :D
	//
	public void OnResetHud()
	{
		m_gcPlayerState = GameInstance.Object.GetPlayerState();

		m_sWaveNumber.text = ""; // + GameMode_Eugatron.s_iWaveNumber.ToString();
		m_sMultiplier.text = "X:" + m_gcPlayerState.m_iScoreMultiplier.ToString();
		m_sScore.text = m_gcPlayerState.m_iScore.ToString("000000000000");
		m_sLives.text = "L:" + m_gcPlayerState.m_iLives.ToString();

		m_gcCanvasGroup.alpha = 1f;
	}


	public void HideWaveNumber()
	{
		m_sWaveNumber.text = "";
	}


	public void OnWaveNumberUpdated()
	{
		m_sWaveNumber.text = "";// + GameMode_Eugatron.s_iWaveNumber.ToString();
	}



	public void OnScoreUpdated()
	{
		m_sScore.text = m_gcPlayerState.m_iScore.ToString("000000000000");
	}



	public void OnLivesUpdated()
	{
		m_sLives.text = "L:" + m_gcPlayerState.m_iLives.ToString();
	}



	public void OnMultiplierUpdated()
	{
		m_sMultiplier.text = "X:" + m_gcPlayerState.m_iScoreMultiplier.ToString();
	}
}
