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

public class gs_Cecconoid_Highscore_Enter : GameState
{
	public override void Awake()
	{
		m_sStateName = "Cecconoid_HighScore_Enter";
		base.Awake();
	}


	private void Start()
	{
		// Hide the GameOver after the scene has been loaded to remove a glitch...
		m_gcGameInstance.GetGameOverScreen().OnHideInstant();

		var gc = GameInstance.Object.GetHighScoreCecconoid();
		GAssert.Assert(null != gc, "Unable to get Cecconoid High Score Controller!");
		gc.OnShowInstant();
	}



	void Update()
	{
		if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._CECCONOID_HIGHSCORE_IN);
	}
}
