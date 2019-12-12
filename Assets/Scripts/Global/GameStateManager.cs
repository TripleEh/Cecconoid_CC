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

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// Game States are individual C# classes that are dynamically attached to a game object
// controlled by this class. Only one GameState can be active at any one time.
// GameStates sit above any logic that exists for a level, or individual room (etc) as 
// they are responsible for the overall flow of the game, from the SplashScreen, through 
// the Main Menu, and into (and out of) the game itself.

// Level controllers sit below game states, and contain specific logic for driving 
// the (localised) gameplay.

// DEVMODE:
// If play is pressed in the editor, but the XXXX_Persistent scene hasn't been loaded
// we store the name of the active scene in the editor, here, load our persistent stuff, and 
// kick off straight into gs_GameIn (skipping the splashscreen, menu etc). Then we can 
// reload (additively) the scene that was active in the editor, removing the need to have 
// any duplicate objects in level scenes, or additional logic in the Persistent scene to 
// check if multiple scenes are active in the editor...



// Game states available. 
public enum EGameStates
{
	_SPLASHSCREEN_ENTER,
	_SPLASHSCREEN_IN,
	_SPLASHSCREEN_EXIT,
	_MAINMENU_ENTER,
	_MAINMENU_IN,
	_MAINMENU_EXIT,
	_GAME_ENTER,
	_GAME_IN,
	_GAME_EXIT,
	_DEVMODE_ENTER_LEVEL,
	_GAME_SELECT_ENTER,
	_GAME_SELECT_IN,
	_GAME_SELECT_EXIT,
	_EUGATRON_MAINMENU_ENTER,
	_EUGATRON_MAINMENU_IN,
	_EUGATRON_MAINMENU_EXIT,
	_EUGATRON_GAME_ENTER,
	_EUGATRON_GAME_IN,
	_EUGATRON_GAME_EXIT,
	_EUGATRON_HIGHSCORE_ENTER,
	_EUGATRON_HIGHSCORE_IN,
	_EUGATRON_HIGHSCORE_EXIT,
	_CECCONOID_HIGHSCORE_ENTER,
	_CECCONOID_HIGHSCORE_IN,
	_CECCONOID_HIGHSCORE_EXIT,
}

// This class requires the GameInstance component to be attached to the same game object!
[RequireComponent(typeof(GameInstance))]
public class GameStateManager : MonoBehaviour
{
	// GameObject that holds the  active GameState
	private static GameObject m_goGameState;

	// Hold the name of the active scene, IF play is pressed in the Editor. 
	private static string m_sDevmodeRoomToLoad = "";
	
	// DevModeCheck can flag that we're entering Eugatron, rather than the main game mode, so track it here... 
	public bool m_bEugatronGameMode = false;

	// Store of the currently loaded scene, so it can be unloaded when changing state!
	private static Scene m_LoadedScene;

	// If we're waiting to load, block any other state changes.
	private bool m_bWaitingForLoad = false;

	// Store the name of the next state. 
	private EGameStates m_iNewState;

	// Reference to the game instance component
	private GameInstance m_gcGameInstance;

	// Names we'll automatically set the object containing the game state to, so it can be found in the editor Hierarchy. 
	// These should match EGameStates in order...
	//
	// GNTODO: This and EGameStates should have been wrapped in a struct so there'd be no 
	// issue with names deviating from state indexes. 
	private static string[] m_aGameStateNames = new string[] 
	{
		"GS_SplashScreen_Enter",
		"GS_SplashScreen_In",
		"GS_SplashScreen_Exit",
		"GS_MainMenu_Enter",
		"GS_MainMenu_In",	
		"GS_MainMenu_Exit",
		"GS_Game_Enter",
		"GS_Game_In",
		"GS_Game_Exit",
		"GS_DevModeEnterLevel",
		"GS_Game_Select_Enter",
		"GS_Game_Select_In",
		"GS_Game_Select_Exit",
		"GS_Eugatron_MainMenu_Enter",
		"GS_Eugatron_MainMenu_In",
		"GS_Eugatron_MainMenu_Exit",
		"GS_Eugatron_Game_Enter",
		"GS_Eugatron_Game_In",
		"GS_Eugatron_Game_Exit",
		"GS_Eugatron_Highscore_Enter",
		"GS_Eugatron_Highscore_In",
		"GS_Eugatron_Highscore_Exit",
		"GS_Cecconoid_Highscore_Enter",
		"GS_Cecconoid_Highscore_In",
		"GS_Cecconoid_Highscore_Exit",
	};





	// Awake function needs to set some defaults and then check for evidence of a previously loaded scene!
	//
	void Awake()
	{
		// Apply default quality settings. 
		{
			Application.targetFrameRate = -1;
			Assert.raiseExceptions = true;
			Time.fixedDeltaTime = 1.0f / 60.0f;
		}


		// Check for "Development Mode":
		// If an object called P_SceneID exists then it's a hangover from a scene that was loaded in the editor
		// when Play was pressed. If this exists then we're in dev mode. Shortcut the menus and setup everything
		// so we can play the game immediately.
		if (Application.isEditor)
		{
			GameObject go = GameObject.Find("P_SceneID");
			if (null != go)
			{
				DevModeCheck gc = go.GetComponent<DevModeCheck>();
				if (null != gc)
				{
					m_sDevmodeRoomToLoad = gc.m_sSceneName;
					m_bEugatronGameMode = gc.m_bIsEugatron;
					Destroy(go);
				}
			}
		}

		// Set our delegate for scene loaded events
		SceneManager.sceneLoaded += SceneLoaded;
	}






	// Start function kickstarts game into a loaded scene if DevMode was detected during Awake, 
	// or proceeds with a normal bootup into the splashscreen and default game flow. 
	//
	void Start()
	{
		// Are we loading the game normally, or doing a devmode kickstart into a given scene?
		if (m_sDevmodeRoomToLoad != "")
		{
			ChangeState(EGameStates._DEVMODE_ENTER_LEVEL, m_sDevmodeRoomToLoad);
			m_sDevmodeRoomToLoad = "";
		}
		else
		{
			// Get the Game Instance component, which should be attached to the same object 
			m_gcGameInstance = GameInstance.Object;

			// And change to the initial state
			// NOTE: We're not loading a scene at this point, the game state will determine
			// when that happens 
			ChangeState(EGameStates._SPLASHSCREEN_ENTER);
		}
	}





	// This function initialises the change of game state, and the loading of a new
	// state (if required, which isn't always)
	//
	public void ChangeState(EGameStates iState, string sLevelName = "", bool bClearTimers = true)
	{
		// Early out if we're already waiting for a scene to load!
		if (m_bWaitingForLoad)
		{
			Debug.LogError("Change State called while waiting to load!");
			return;
		}

		// Clear all existing timers! No more will be processed this frame...
		// DevModeCheck can override this, as we'll have gone through successive state changes
		// in consecutive frames, some of which will have happened after Start() is called on 
		// the scene objects loaded... 
		if(bClearTimers) TimerManager.ClearAll();

		// Remove the loaded scene if we need to...
		if (sLevelName != "" && m_LoadedScene.IsValid() && m_LoadedScene.name != sLevelName) SceneManager.UnloadSceneAsync(m_LoadedScene);

		// Load the new scene if we need it...
		if (sLevelName != "")
		{
			Debug.Log("Attempting to load: " + sLevelName + "\n");
			m_bWaitingForLoad = true;
			m_iNewState = iState;
			SceneManager.LoadScene(sLevelName, LoadSceneMode.Additive);
			m_LoadedScene = SceneManager.GetSceneByName(sLevelName);
			Assert.IsTrue(m_LoadedScene.IsValid());
		}
		// ...or just change the active game state to the newly requested one. 
		else FinaliseChangeState(iState);
	}

	
	
	// Swaps the old GS object for a new one, with the correct component added. 
	// Note: when this function is called makes a difference as to which scene 
	// it will be in, which is why ChangeState() calls that don't load a new 
	// scene call this function immediately. 
	//
	protected void FinaliseChangeState(EGameStates iState)
	{
		// Cleanout the old state
		Destroy(m_goGameState);
		m_goGameState = new GameObject();
		m_goGameState.name = m_aGameStateNames[(int)iState];
		AddComponentToGameStateObject(iState);
	}

	
	
	// This is a callback delegate that's added to the SceneManager. It's triggered
	// when the async additive load of the next scene has completed, and we're OK
	// to switch the active GameState over to the new one...
	//
	public void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Unblock any additional calls to change state
		m_bWaitingForLoad = false;

		// We never want the persistent scene to be active! Set it to the new one
		SceneManager.SetActiveScene(scene);

		// Now do the swap of the GameState component onto a new GameObject
		FinaliseChangeState(m_iNewState);

		// And then move it into the newly loaded scene
		if (m_LoadedScene.IsValid()) SceneManager.MoveGameObjectToScene(m_goGameState, scene);
	}


	
	// Because scene load takes a minimum of two frames (this one and the next one) game states 
	// should check to make sure we're not currently in the middle of a load before calling
	// ChangeState(), above...
	//
	public bool CanChangeState()
	{
		return !m_bWaitingForLoad;
	}

	

	// Adds the correct component to m_goGameState, based on the requested state. 
	// NOTE: It's assumed that m_goGameState is always an empty gameObject!
	//
	private void AddComponentToGameStateObject(EGameStates iState)
	{
		if (null == m_goGameState) return;

		switch (iState)
		{
			case EGameStates._SPLASHSCREEN_ENTER: m_goGameState.AddComponent<gs_SplashScreen_Enter>(); break;
			case EGameStates._SPLASHSCREEN_IN: m_goGameState.AddComponent<gs_SplashScreen_In>(); break;
			case EGameStates._SPLASHSCREEN_EXIT: m_goGameState.AddComponent<gs_SplashScreen_Exit>(); break;
			case EGameStates._MAINMENU_ENTER: m_goGameState.AddComponent<gs_MainMenu_Enter>(); break;
			case EGameStates._MAINMENU_IN: m_goGameState.AddComponent<gs_MainMenu_In>(); break;
			case EGameStates._MAINMENU_EXIT: m_goGameState.AddComponent<gs_MainMenu_Exit>(); break;
			case EGameStates._GAME_ENTER: m_goGameState.AddComponent<gs_Game_Enter>(); break;
			case EGameStates._GAME_IN: m_goGameState.AddComponent<gs_Game_In>(); break;
			case EGameStates._GAME_EXIT: m_goGameState.AddComponent<gs_Game_Exit>(); break;
			case EGameStates._DEVMODE_ENTER_LEVEL: m_goGameState.AddComponent<gs_DevModeEnterLevel>(); break;
			case EGameStates._CECCONOID_HIGHSCORE_ENTER: m_goGameState.AddComponent<gs_Cecconoid_Highscore_Enter>(); break;
			case EGameStates._CECCONOID_HIGHSCORE_IN: m_goGameState.AddComponent<gs_Cecconoid_HighScore_In>(); break;
			case EGameStates._CECCONOID_HIGHSCORE_EXIT: m_goGameState.AddComponent<gs_Cecconoid_HighScore_Exit>(); break;
		}
	}

	void OnDisable()
	{
		Debug.Log("GameStateManager Disabled.\n");
	}


	public GameState GetActiveGameState()
	{
		return m_goGameState.GetComponent<GameState>();
	}
}
