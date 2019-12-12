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


// GameInstance
//
// Global class that is set in editor to run before Default Time, ensuring that this is the first 
// class to be processed each frame... 
//


[RequireComponent(typeof(GameAudioManager))]
public class GameInstance : MonoBehaviour
{
	// Prefab References
	//
	[Header("Prefab References")]
	public GameObject m_goPlayerPrefab = null;


	// In-Game Spawned Object References
	//
	private GameObject m_goPlayerObjectReference = null;
	private PlayerState m_gcPlayerState = null;
	private PlayerController m_gcPlayerController = null;
	private PlayerInventory m_gcPlayerInventory = null;

	// List of achievements...
	//
	private Achievements m_gcAchievementsList;

	// Class References (Set these in the editor! They're attached to objects in the persistent scene)
	//
	[Header("Class References")]
	[SerializeField] private GameAudioManager m_gcAudioManager = null;
	[SerializeField] private GameCamera m_gcGameCamera = null;
	[SerializeField] private InGameHud m_gcGameHud = null;
	[SerializeField] private InGamePauseScreen m_gcPauseScreen = null;
	[SerializeField] private MainMenu m_gcMainMenu_Cecconoid = null;
	[SerializeField] private GameOverScreen m_gcGameOverScreen = null;
	[SerializeField] private DevMenu m_gcDevMenu = null;
	[SerializeField] private HighScore_Ceccotron m_gcHighScore_CecconoidMenu = null;
	[SerializeField] private CompletionSequenceGetter m_gcCompletionSequence = null;
	[SerializeField] private UI_AchievementIconSpawner m_gcAchievementSpawner = null;


	// Debug
	//
	#if GDEBUG
	[Header("Tweakables")]
	[SerializeField] private bool m_bDisplayDebugHud = true;
	private string m_sActiveRoom = "Room Name Not Set!";
	#endif

	//------------------------------------------------------------------------------------------

	// For the FPS display in debug mode...
	private float m_fDeltaTime;



	// Track the GameMode
	private bool m_bGameModeIsCecconoid = true;
	public bool m_bIsCecconoid
	{
		get { return m_bGameModeIsCecconoid; }
		set { m_bGameModeIsCecconoid = value; }
	}



	// Singleton object.
	private static GameInstance obj;

	public static GameInstance Object
	{
		get
		{
			if (!obj)
			{
				obj = (GameInstance)GameObject.FindObjectOfType<GameInstance>();
#if !UNITY_EDITOR
				if (!obj) Debug.LogError("Unable to find GameInstance.Object! Awake function possibly calling for .Object before persistent scene loaded?");
#endif
			}

			return obj;
		}
	}

	//------------------------------------------------------------------------------------------


	public void Awake()
	{
		if (null == obj)
		{
			Debug.Log("GameInstance created! Welcome to " + Types.s_sGameName);
			obj = GetComponent<GameInstance>();
		}


		// Check that our class references have been set correctly. 
		{
			GAssert.Assert(null != m_gcGameCamera, "Game Camera component has not been set in the editor!");
			GAssert.Assert(null != m_gcAudioManager, "Audio Manager component has not been set in the editor!");
			GAssert.Assert(null != m_gcPauseScreen, "Pause Screen component has not been set in the editor!");
			GAssert.Assert(null != m_gcGameHud, "In Game Hud component has not been set in the editor!");
			GAssert.Assert(null != m_gcMainMenu_Cecconoid, "Main Menu component has not been set in the editor!");
			GAssert.Assert(null != m_gcGameOverScreen, "Game Over screen not setup in the editor!");
			GAssert.Assert(null != m_gcDevMenu, "DevMenu not set!");
		}
	}



	public void Start()
	{
		// Run the physics at 60fps. This will never change!
		// Most objects in the world (with colliders on) will move at this delta!
		Time.fixedDeltaTime = Types.s_fFixedDeltaTimeUpdate;

		// Achievements should be attached to this game object. 
		// Build the final achievement array so we know what to pop up on screen...
		m_gcAchievementsList = GetComponent<Achievements>();
		GAssert.Assert(null != m_gcAchievementsList, "GameInstance: Unable to find the list of achievements! Component missing!");
		m_gcAchievementsList.BuildAchievementsList();

		GameGlobals.LoadHighScores_Local();

		// GNTODO: This needs to move to after we have loaded the User Prefs, probably SplashScreen_Exit?
		m_gcAudioManager.LoadUserPrefs();
	}



	public void StartGameCecconoid()
	{
		GameGlobals.s_bCEcconoidHighScoreEntryThisGo = false;
		m_bGameModeIsCecconoid = true;
		m_gcAudioManager.PlayMusic(EGameMusic._CECCONOID_INGAME);
		StartGame();
	}



	public void StartGameEugatron()
	{
		GameGlobals.s_bEugatronHighScoreEntryThisGo = false;
		m_bGameModeIsCecconoid = false;
		StartGame();
		m_gcGameHud.OnHideHud();
	}



	private void StartGame()
	{
		// Spawn the player offscreen. SetPlayerDefaults locks it and prevents movement.
		// Player Object remains in the persistent scene, so we don't need to care 
		// about what happens between scene loads...
		{
			m_goPlayerObjectReference = GameInstance.Instantiate(m_goPlayerPrefab, new Vector3(100, 100, 0), Quaternion.identity);
			GAssert.Assert(null != m_goPlayerObjectReference, "Unable to instantiate Player Object");
			m_gcPlayerState = m_goPlayerObjectReference.GetComponent<PlayerState>();
			GAssert.Assert(null != m_gcPlayerState, "Unable to get Player State");
			m_gcPlayerController = m_goPlayerObjectReference.GetComponent<PlayerController>();
			GAssert.Assert(null != m_gcPlayerController, "Unable to get Player Controller");
			m_gcPlayerInventory = m_goPlayerObjectReference.GetComponent<PlayerInventory>();
			GAssert.Assert(null != m_gcPlayerInventory, "Unable to get Player Inventory!");
		}

		// Set Game Defaults
		{
			Messenger.ClearAll();
			TimerManager.SetDefaults(1.0f, 1.0f);
			m_gcPlayerInventory.SetDefaults();
			m_gcPlayerState.SetDefaults();
			m_gcGameCamera.SetDefaults();
			GameGlobals.SetDefaults();
			m_gcGameHud.SetDefaults();
			m_gcPauseScreen.SetDefaults();
			m_gcMainMenu_Cecconoid.SetDefaults();
			m_gcGameOverScreen.SetDefaults();
			m_gcCompletionSequence.SetDefaults();
#if GDEBUG
			m_gcDevMenu.SetDefaults();
#endif

			Messenger.Invoke(Types.s_sHUD_Hide);
		}
	}




	public void EndGame()
	{
		m_gcAudioManager.FadeMusicOutForGameOver();
		m_gcGameHud.OnHideHud();

		// Record High Scores if we have one, and what we achieved this time which the GameOver screen will pickup...
		if (m_bIsCecconoid) GameGlobals.CheckCecconoidHighScore(m_gcPlayerState.m_iScore);
		else GameGlobals.CheckEugatronHighScore(m_gcPlayerState.m_iScore);

		// Clean up the player by deleting it :D
		Destroy(m_goPlayerObjectReference);
		m_gcPlayerState = null;
		m_gcPlayerController = null;
	}



	public void HideHud()
	{
		m_gcGameHud.OnHideHud();
	}



	public void SpawnAchievementIcon(Types.EAchievementIdentifier iID)
	{
		m_gcAchievementSpawner.SpawnAchievement(ref m_gcAchievementsList.m_aAchievementInfo[(int)iID]);
	}




	public void ExitToDesktop()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}



	void Update()
	{
#if GDEBUG
		// Display the Debug Hud!
		if (m_bDisplayDebugHud) m_fDeltaTime += (Time.unscaledDeltaTime - m_fDeltaTime) * 0.1f;
#endif

		// Process the TimerManager! This ensures all timer callbacks are processed
		// at the start of each frame, and our specific DeltaTimes are available and
		// accurate. GameInstance is the highest priority script, set in the editor...
		TimerManager.Update();

		// Audio manager needs to know we've moved on, and that it can reset the list
		// of played samples that it's tracking...
		m_gcAudioManager.NextFrame();
	}



	// Room Controllers can set the name of the active room to be shown in the Debug HUD
	public void SetDebugHudRoomName(string sRoomName)
	{
#if GDEBUG
		if (m_bDisplayDebugHud) m_sActiveRoom = sRoomName;
#endif
	}

	// ------------------------------------------------------------------------------------------------------------------
	// Getters

	public GameAudioManager GetAudioManager() { return m_gcAudioManager; }
	public PlayerState GetPlayerState() { return m_gcPlayerState; }
	public GameObject GetPlayerObject() { return m_goPlayerObjectReference; }
	public PlayerController GetPlayerController() { return m_gcPlayerController; }
	public Vector3 GetPlayerPosition() { if (null == m_goPlayerObjectReference) return Vector3.zero; else return m_goPlayerObjectReference.transform.position; }
	public Vector2 GetPlayerTrajectory() { if (null == m_gcPlayerController) return Vector2.zero; else return m_gcPlayerController.GetMovementTrajectory(); }
	public GameCamera GetGameCamera() { return m_gcGameCamera; }
	public Camera GetGameCameraComponent() { return m_gcGameCamera.gameObject.GetComponent<Camera>(); }
	public MainMenu GetMainMenuCecconoid() { return m_gcMainMenu_Cecconoid; }
	public GameOverScreen GetGameOverScreen() { return m_gcGameOverScreen; }
	public DevMenu GetDevMenu() { return m_gcDevMenu; }
	public HighScore_Ceccotron GetHighScoreCecconoid() { return m_gcHighScore_CecconoidMenu; }
	public CompletionSequenceGetter GetCompletionSequence() { return m_gcCompletionSequence; }


	// ------------------------------------------------------------------------------------------------------------------
	// Debug
#if GDEBUG
	void OnGUI()
	{
		if (!m_bDisplayDebugHud) return;
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = 20;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

		float fMsec = m_fDeltaTime * 1000.0f;
		float fFPS = 1.0f / m_fDeltaTime;
		string sOutput = string.Format(m_sActiveRoom + ": {0:0.0} ms ({1:0.} fps)", fMsec, fFPS);

		Rect vRect = new Rect(50, 50, Screen.width, Screen.height / 50.0f);
		GUI.Label(vRect, sOutput, style);

		style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		vRect.x += 1.0f;
		vRect.y += 1.0f;
		style.fontStyle = FontStyle.Normal;
		GUI.Label(vRect, sOutput, style);
	}
#endif
}
