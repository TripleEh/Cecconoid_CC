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


public class gs_Game_Exit : GameState
{
	private float m_fEventTime;
	private PlayerActions m_PlayerActionsBindings;

	public override void Awake()
	{
		m_PlayerActionsBindings = new PlayerActions();
		m_PlayerActionsBindings.UI_Back.ClearInputState();
		m_PlayerActionsBindings.UI_Confirm.ClearInputState();
		m_PlayerActionsBindings.UI_Pause.ClearInputState();

		m_sStateName = "Game_Exit";
		base.Awake();
	}

	private void Start()
	{
		GameInstance.Object.GetGameOverScreen().OnShowInstant(GameInstance.Object.m_bIsCecconoid);
		GameInstance.Object.GetGameCamera().SetDefaults();
		m_fEventTime = TimerManager.fUIGameTime + Types.s_fDUR_GameOverScreen;
	}

	private void Update()
	{
		if (TimerManager.fUIGameTime > m_fEventTime
			|| m_PlayerActionsBindings.UI_Back.WasPressed
			|| m_PlayerActionsBindings.UI_Confirm.WasPressed
			|| m_PlayerActionsBindings.UI_Pause.WasPressed)
			if (m_gcGameStateManager.CanChangeState())
			{
				TimerManager.UnPauseGame();
				GameInstance.Object.GetAudioManager().FadeMusicOutInstant();
				m_gcGameStateManager.ChangeState(EGameStates._CECCONOID_HIGHSCORE_ENTER, Types.s_sSCENE_MainMenu);
			}
	}
}
