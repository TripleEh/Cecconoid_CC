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


public class gs_MainMenu_In : GameState
{
  private bool m_bIsLisentingForInput = false;
  private float m_fEventTime = 0f;
  private PlayerActions m_PlayerActionsBindings;

  public override void Awake()
  {
    m_sStateName = "MainMenu_In";
    base.Awake();
		Messenger.AddListener(Types.s_sMenu_BeginTransitionToGame, BeginTransitionToGame);
		Messenger.AddListener(Types.s_sMenu_ExitToSelect, BeginTransitionToGameSelect);
    Messenger.AddListener(Types.s_sMenu_ShowHighScoreTableCecconoid, ShowHighScroreTable);
  }

  private void Start()
  {
    m_PlayerActionsBindings = new PlayerActions();
    m_PlayerActionsBindings.UI_Back.ClearInputState();
    m_PlayerActionsBindings.UI_Confirm.ClearInputState();
    m_PlayerActionsBindings.UI_Pause.ClearInputState();
  }


  public void BeginTransitionToGame()
  {
		GameInstance.Object.GetMainMenuCecconoid().OnHide();
		Messenger.RemoveListener(Types.s_sMenu_BeginTransitionToGame, BeginTransitionToGame);
		Messenger.RemoveListener(Types.s_sMenu_ExitToSelect, BeginTransitionToGameSelect);
    Messenger.RemoveListener(Types.s_sMenu_ShowHighScoreTableCecconoid, ShowHighScroreTable);
    if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._MAINMENU_EXIT);
  }


	public void BeginTransitionToGameSelect()
	{
		GameInstance.Object.GetMainMenuCecconoid().OnHide();
		GameInstance.Object.GetCompletionSequence().OnHideInstant();
		Messenger.RemoveListener(Types.s_sMenu_BeginTransitionToGame, BeginTransitionToGame);
		Messenger.RemoveListener(Types.s_sMenu_ExitToSelect, BeginTransitionToGameSelect);
		TimerManager.AddTimer(1f, EndTransitionToGameSelect);
	}


	public void EndTransitionToGameSelect()
	{
		// Game Select scene is empty, but the transition to it is the only way I've found to 
		// _realiably_  debounce all input... 
		if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._GAME_SELECT_ENTER, "", true);
	}



	public void ShowHighScroreTable()
  {
    m_fEventTime = TimerManager.fGameTime + 7.5f;
    m_bIsLisentingForInput = true;
    GameInstance.Object.GetMainMenuCecconoid().OnHideMainCanvas();
    GameInstance.Object.GetHighScoreCecconoid().OnShowTable(false);
  }


  public void HideHighScoreTable()
  {
    m_bIsLisentingForInput = false;
    GameInstance.Object.GetMainMenuCecconoid().OnShowMainCanvas();
    GameInstance.Object.GetHighScoreCecconoid().OnHideInstant();
  }


  private void Update()
  {
    if (!m_bIsLisentingForInput) return;

    if (TimerManager.fGameTime > m_fEventTime || m_PlayerActionsBindings.UI_Back.WasPressed)
    {
      m_PlayerActionsBindings.UI_Back.ClearInputState();
      m_PlayerActionsBindings.UI_Confirm.ClearInputState();
      m_PlayerActionsBindings.UI_Pause.ClearInputState();
      HideHighScoreTable();
    }
  }
}
