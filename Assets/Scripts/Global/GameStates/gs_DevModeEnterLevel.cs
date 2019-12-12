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

// THIS IS A DEBUG MODE HELPER, FOR USE IN THE EDITOR ONLY!
//
public class gs_DevModeEnterLevel : GameState
{
	public override void Awake()
	{
		m_sStateName = "DevModeEnterLevel";
		base.Awake();
	}

	// Shortcut into the game when we're hitting play in the editor and the persistent scene 
	// hasn't been loaded. 
	//
	void Start()
	{
		if (m_gcGameStateManager.m_bEugatronGameMode) GameInstance.Object.StartGameEugatron();
		else GameInstance.Object.StartGameCecconoid();
	}

	void Update()
	{
		if (m_gcGameStateManager.CanChangeState())
		{
			if (!m_gcGameStateManager.m_bEugatronGameMode) m_gcGameStateManager.ChangeState(EGameStates._GAME_IN, "", false);
			else m_gcGameStateManager.ChangeState(EGameStates._EUGATRON_GAME_IN, "", false);
		}
	}
}
