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


public class gs_SplashScreen_Exit : GameState 
{
  public override void Awake()
  {
    m_sStateName = "SplashScreen_Exit";
    base.Awake();
  }

	void Start()
  {
  }

  void Update()
  {
		if(m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._MAINMENU_ENTER, Types.s_sSCENE_MainMenu);
 
  }
}
