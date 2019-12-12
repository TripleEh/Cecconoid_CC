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


public class gs_Game_Enter : GameState
{
	public override void Awake()
	{
		m_sStateName = "Game_Enter";
		base.Awake();
	}

	void Start()
	{
		GameInstance.Object.StartGameCecconoid();	
		
		// If we've loaded a game, then parse that and setup the Game and Player States
		//
		// Initiate the transition animation
		// Setup a timer to change state once the transition is over
		// Spawn the player in the default position but remain Paused
	}

	void Update()
	{
		if (m_gcGameStateManager.CanChangeState()) m_gcGameStateManager.ChangeState(EGameStates._GAME_IN, "", false);
	}
}
