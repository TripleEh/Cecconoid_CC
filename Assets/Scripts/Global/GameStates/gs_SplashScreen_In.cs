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

public class gs_SplashScreen_In : GameState
{
	public override void Awake()
	{
		m_sStateName = "SplashScreen_In";
		base.Awake();
	}

	void Start()
	{
		TimerManager.AddTimer(12f, ChangeState);
	}

	public void ChangeState()
	{
		if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._SPLASHSCREEN_EXIT);
	}
}
