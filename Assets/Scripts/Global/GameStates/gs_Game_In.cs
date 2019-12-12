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

public class gs_Game_In : GameState
{
	public override void Awake()
	{
		m_sStateName = "Game_In";
		base.Awake();
	}



	void Start()
	{
		// This is called by the Pause Screen!
		Messenger.AddListener(Types.s_sGF_BeginExitGame, OnBeginExitGame);

		// Begin setup of player
		GameMode.TransitionToDefaultRoom();
		Messenger.Invoke(Types.s_sHUD_ResetHud);

#if GDEBUG
		// This is the list of rooms shown in the dev menu. Debug builds only...
		GameInstance.Object.GetDevMenu().PopulateRoomList();
#endif
	}


	void Update()
	{
		if (m_gcGameInstance.GetPlayerState().m_iLives == 0) OnBeginExitGame();
	}



	void LateUpdate()
	{
		// Player is only killed at the end of a frame, not instantly!
		GameMode.DoLateUpdatePlayerDeathCheck();
	}




	public void OnBeginExitGame()
	{
		GameInstance.Object.EndGame();

#if GDEBUG
		GameInstance.Object.GetDevMenu().DepopulateRoomList();
#endif

		TimerManager.ClearAll();
		Messenger.ClearAll();

		m_gcGameStateManager.ChangeState(EGameStates._GAME_EXIT);
	}
}
