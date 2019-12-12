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


public class gs_MainMenu_Exit : GameState
{
	private float m_fEventTime = 0;

  public override void Awake()
  {
    m_sStateName = "MainMenu_Exit";
    base.Awake();
  }

  void Start()
  {
		UnityEngine.Cursor.visible = false;
		m_fEventTime = TimerManager.fGameTime;
  }

  void Update()
  {
    if (m_gcGameStateManager.CanChangeState() && TimerManager.fGameTime - m_fEventTime > 0.6f) m_gcGameStateManager.ChangeState(EGameStates._GAME_ENTER, Types.s_sSCENE_CecconoidLvl1);
  }
}
