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


public class gs_MainMenu_Enter : GameState 
{
	private ulong m_iTimerHandle;

  public override void Awake()
  {
    m_sStateName = "MainMenu_Enter";
    base.Awake();
  }

  void Start()
  {
		UnityEngine.Cursor.visible = true;
		GameInstance.Object.GetGameOverScreen().OnHideInstant();
		GameInstance.Object.GetMainMenuCecconoid().OnShow();
		m_iTimerHandle = TimerManager.AddTimer( 1f/Types.s_fDUR_MenuFadeSpeed, EnterMainMenu);
  }

  public void EnterMainMenu()
  {
    if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._MAINMENU_IN);
  }  
}
