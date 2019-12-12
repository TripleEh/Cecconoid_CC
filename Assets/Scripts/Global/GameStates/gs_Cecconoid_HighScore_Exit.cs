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


public class gs_Cecconoid_HighScore_Exit : GameState
{
	public override void Awake()
	{
		m_sStateName = "Cecconoid_HighScore_Exit";
		base.Awake();
	}


	private void Start()
	{
		// GNTODO save
		var gc = GameInstance.Object.GetHighScoreCecconoid();
		GAssert.Assert(null != gc, "Unable to get reference to the high score ui!");
		gc.OnHideInstant();
	}


	private void Update()
	{
		if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._MAINMENU_ENTER);
	}
}
