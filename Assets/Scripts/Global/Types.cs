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

// Contains type definitions / enums / constants / magic numbers / interfaces etc that are referenced globally
//
public static class Types
{
	// Game Constants
	//
	public const string s_sGameName = "Cecconoid";
	public const string s_sSCENE_CecconoidLvl1 = "Lvl_Level1";
	public const string s_sSCENE_Eugatron = "Lvl_Eugatron";
	public const string s_sSCENE_MainMenu = "MainMenu";
	public const string s_sSCENE_GameSelect = "GameSelect";

	// Platform Tweakables 
#if MOBILE
	public const int s_iPoolSize_AudioSFX = 32;
	public const float s_fFixedDeltaTimeUpdate = 1f/60f;
	public const float s_fDeadZone_Movement = 0.15f;
	public const float s_fVOL_MaxAttenuation = -8.5f;
#else
	public const int s_iPoolSize_AudioSFX = 32;
	public const float s_fFixedDeltaTimeUpdate = 1f / 60f;
	public const float s_fDeadZone_Movement = 0.35f;
	public const float s_fVOL_MaxAttenuation = -2.5f;
#endif



	public const int s_iPLAYER_MaxLives = 99;
	public const int s_iPLAYER_MaxMultiplier = 99;
	public const int s_iPLAYER_MaxEnergy = 100;
	public const int s_iPLAYER_MaxDamage = 99;
	public const int s_iPLAYER_BulletPerMultiplier = 6;
	public const long s_iPLAYER_MaxScore = 9999999999;
	public const float s_fPLAYER_TripleShotSpreadDegrees = 8.0f;
	public const float s_fPLAYER_RoomTransitionDuration = s_fCAM_RoomTransitionDuration * 2.0f;
	public const float s_fPLAYER_RespawnDuration = 1f;
	public const float s_fPLAYER_RespawnHalfDur = s_fPLAYER_RespawnDuration / 2.0f;
	public const uint s_iPLAYER_ExtraLifeScore = 125000;


	public const float s_fPixelsPerUnit = 100.0f;
	public const float s_fPixelSize = 1.0f / s_fPixelsPerUnit;
	public const float s_fRoomWidth = 3.84f;
	public const float s_fRoomHeight = 2.16f;
	public const float s_fRoomBoundsX = s_fRoomWidth / 2.0f;
	public const float s_fRoomBoundsY = s_fRoomHeight / 2.0f;
	public const float s_fRoomCollisionBoundsY = 0.8f;
	public const float s_fRoomCollisionBoundsX = 1.76f;
	public const float s_fEugatronRoomCollisionBoundsY = 1f;
	public const float s_fEugatronRoomCollisionBoundsX = 1.9f;
	public const float s_fEugatronRoomBoundsX = 1.7f;
	public const float s_fEugatronRoomBoundsY = 0.8f;
	public const float s_fEugatronHalfRoomBoundsX = s_fEugatronRoomBoundsX / 2.0f;
	public const float s_fEugatronHalfRoomBoundsY = s_fEugatronRoomBoundsY / 2.0f;
	public const float s_fDeadZone_Firing = 0.175f;
	public const float s_fPOS_ImpactEffectSpawnOffset = 0.02f;
	public const float s_fPOS_FrontLayerZ = -0.05f;
	public const float s_fPOS_RearLayerZ = 0.5f;
	public const float s_fPOS_PlayerOptionLayerZ = -0.75f;
	public const float s_fCAM_VerticalPixelOffset = 0.04f;
	public const float s_fCAM_ShakeDistanceScale = s_fPixelSize * 8.0f;
	public const float s_fCAM_ShakeDecay = 1.5f;
	public const float s_fCAM_ShakeDeadzone = 0.20f;
	public const float s_fCAM_SmallEnemyShakeAmount = 0.15f;
	public const float s_fCAM_MedEnemyShakeAmount = 0.25f;
	public const float s_fCAM_LargeEnemyShakeAmount = 0.5f;
	public const float s_fCAM_EmplacementShakeAmount = 0.6f;
	public const float s_fCAM_ProjectileShakeAmount = 0.45f;
	public const float s_fCAM_RoomTransitionDuration = 0.35f;
	public const float s_fCAM_ZOffset = -1.0f;
	public const float s_fBULSPD_PixelBullet = 10.0f;
	public const float s_fDUR_DoorCloseDuration = 0.3f;
	public const float s_fDUR_FinaliseSpawnDelay = 1f;
	public const float s_fDUR_ScreenTransitionDelay = 0.35f;
	public const float s_fDUR_RoomEntryDoorCloseDelay = s_fCAM_RoomTransitionDuration * 0.85f;
	public const float s_fDUR_SparkCollisionSlideDuration = 1.5f;
	public const float s_fDUR_FloatingLaserWarmUp = 1.75f;
	public const float s_fDUR_MenuFadeSpeed = 3.0f;
	public const float s_fDUR_GameOverScreen = 4.5f;
	public const float s_fDUR_EugatronInvulnPeriod = 0.5f;
	public const float s_fVOL_MinAttenuation = -80.0f;
	public const float s_fVOL_DefaultMusic = -2.0f;
	public const float s_fVEL_PixelShatterVelocity = 0.325f;
	public const float s_fVEL_SmallMissleMovementVelocity = 3.0f;
	public const float s_fVEL_MineSpikeMovementVelocity = 2.5f;
	public const float s_fTTL_PixelShatterLifetime_Short = 1.75f;
	public const float s_fTTL_PixelShatterLifetime_Med = 2.75f;
	public const float s_fTTL_PixelShatterLifetime_Long = 5.0f;
	public const float s_fTTL_RoomEntryActivityDelay = 1.0f;
	public const float s_fTTL_Multiplier = 2.0f;
	public const float s_fEUGE_MinDistanceToPlayer = 0.65f;
	public const float s_fEUGE_PlayerDistanceDivisor = 0.65f;

	// GNTODO: There's no bounds checking in the Room controller for these numbers!
	public const int s_iEUGE_MaxRoomObjects = 512;
	public const int s_iEUGE_MaxRoomObstacles = 64;
	public const int s_iEUGE_MaxRoomOptionals = 64;

	// GameObject Tags (Prefabs should all be assigned to one of these!)
	//
	public const string s_sTag_Player = "Player";
	public const string s_sTag_Enemy = "Enemy";
	public const string s_sTag_PlayerBullets = "PlayerBullet";
	public const string s_sTag_EnemyBullets = "EnemyBullet";
	public const string s_sTAG_Environment = "Environment";
	public const string s_sTAG_Doorway = "Doorway";
	public const string s_sTag_Destructible = "Destructible";



	// Default Room Controller name. We need to know this when starting a new game
	// This must match the name of the gameObject in the world!
	//
	public const string s_sRC_DefaultRoomController = "ROOM_SpawnRoom";
	public const string s_sRC_LevelTwoRoomController = "ROOM_SpawnRoomLevel2";


	// Messenger Callback, String Identifiers. 
	//
	public const string s_sGF_BeginExitGame = "_BEING_EXIT_GAME";
	public const string s_sGF_BeginRoomTransition = "_BEGIN_ROOM_TRANSITION";
	public const string s_sGF_EndRoomTransition = "_END_ROOM_TRANSITION";
	public const string s_sGF_CecconoidCompleted = "_CECCONOID_COMPLETED";
	public const string s_sHUD_WaveNumberUpdated = "_ENERGY_UPDATED";
	public const string s_sHUD_LivesUpdated = "_LIVES_UPDATED";
	public const string s_sHUD_ScoreUpdated = "_SCORE_UPDATED";
	public const string s_sHUD_MultiplierUpdated = "_MULTIPLIER_UPDATED";
	public const string s_sHUD_ResetHud = "_RESET_HUD";
	public const string s_sHUD_Hide = "_HIDE_HUD";
	public const string s_sUI_ShowPauseScreen = "_SHOW_PAUSE";
	public const string s_sUI_HidePauseScreen = "_HIDE_PAUSE";
	public const string s_sMISC_KillAllParticles = "_KILL_PARTICLE_SYSTEMS";
	public const string s_sEUGE_ScreenTransitionFull = "_SCREEN_TRANSITION_FULL";
	public const string s_sEUGE_ScreenTransitionEnd = "_SCREEN_TRANSITION_END";
	public const string s_sPLAYER_HasDied = "_PLAYER_HAS_DIED";
	public const string s_sPLAYER_RespawnBegin = "_PLAYER_RESPAWN_BEING";
	public const string s_sPLAYER_RespawnEnd = "_PLAER_RESPAWN_END";
	public const string s_sMenu_BeginTransitionToGame = "_PLAYGAME";
	public const string s_sMenu_DevMenuShow = "_DEVMENU_SHOW";
	public const string s_sMenu_DevMenuHide = "_DEVMENU_HIDE";
	public const string s_sMenu_ExitToSelect = "_EXIT_TO_SELECT_SCREEN";
	public const string s_sMenu_ShowHighScoreTableEugatron = "_SHOW_HIGH_SCORE_TABLE_EUGATRON";
  public const string s_sMenu_ShowHighScoreTableCecconoid = "_SHOW_HIGH_SCORE_TABLE_CECCONOID";
	public const string s_sGF_InduceSlowdown = "_INDUCE_SLOWDOWN";
	public const string s_sGF_ShowCompletionMessage = "_SHOW_COMP_MESSAGE";


	// ----- HIGH SCORE ALPHABET!
	
	// These are the selectable letters, when scrolling through on the high score entries
	// GNTODO: What do we do about localising these? Switch between alphabet sets per locale?
	public const string s_sHS_English = "ABCDEFGHIJKLMNOPQRSTUVWXYZ!";




	// ----- Enums
	//

	// Objects that trigger in relation to their distance from the player 
	// can ignore if the player is above, or below, them
	public enum EPlayerTriggerArea
	{
		_ABOVE,
		_BELOW,
		_ANY,
	};

	
	// Preset pixel shatter lifetimes	
	public enum EPixelShatterLife
	{
		_SHORT,
		_MEDIUM,
		_LONG,
	}

	
	// Preset cam amplitude variants
	public enum ECamShakeAmount
	{
		_SMALL_ENEMY,
		_MID_ENEMY,
		_LARGE_ENEMY,
		_PROJECTILE,
		_EMPLACEMENT,
	}


	// Directions of travel through a doorway...
	public enum EDirection
	{
		_HORIZONTAL_L2R,
		_HORIZONTAL_R2L,
		_VERTICAL_U2D,
		_VERTICAL_D2U,
		_VERTICAL,
		_HORIZONTAL,
	}


	// What AttractToPlayer objects should do when they hit the player
	public enum EPlayerPickupAction
	{
		_NOTHING,
		_GIVE_SCORE,
		_GIVE_MULTIPLIER,
		_GIVE_POWERUP,
		_GIVE_SCORE_AND_MULTIPLIER,
		_GIVE_SCORE_AND_POWERUP,
		_GIVE_LIFE,
		_GAME_EVENT,
		_GIVE_KEY,
	}

	// Objects that flash when close, or when about to disapper
	public enum EWarnState
	{
		_IDLE,
		_WARNINGPLAYER,
	}


	public enum EDoorwayCollisionState
	{
		_IDLE_OPEN,
		_IDLE_CLOSED,
		_OPENING,
		_CLOSING,
	}


	public enum EPowerUp
	{
		_SHOT_DOUBLE,
		_SHOT_TRIPLE,
		_LASER_SINGLE,
		_LASER_DOUBLE,
		_LASER_TRIPLE,
		_OPTION,
		_SPEED1,
		_SPEED2,
	}


	public enum ESWitchState
	{
		_IDLE_INROOM_ACTIVE_FLIP,
		_IDLE_INROOM_ACTIVE_FLOP,
		_IDLE_EXROOM_ACTIVE_FLIP,
		_IDLE_EXROOM_ACTIVE_FLOP,
		_IDLE_INACTIVE,
	}


	public enum EFlickFrameState
	{
		_FLICK,
		_REVERT,
	}




	public enum EHighScoreMenuState
	{
		_IDLE_INACTIVE,
		_ENTER_NEW_INITIAL1,
		_ENTER_NEW_INITIAL2,
		_ENTER_NEW_INITIAL3,
		_SHOW_TABLE,
	}




	
	public enum EAchievementIdentifier
	{
		_WelcomeToCecconoid = 0,
		_WelcomeToEugatron,					// Last Human Family
		_MillionInCecconoid,				// Who wants to be...
		_MillionInEugatron,					// 
		_FullPowerCecconoid,				// 
		_99MultCecconoid,						// Mine's a 99
		_50MultEugatron,						// With A Flake
		_Zub,												// Now I Ste You...
		_GotTheKey,									// Keymaster
		_CompletedCecconoid,				// Worthy of the name
		_10LevelsEugatron,          // 
		_10LivesEugatron,           // Stakker Humanoid
		_FirstDeath,								// First of many
		_SweetBangBang,							// Die as many times as Antti
		_YoullBeNeedingThat,				// Pick up the first extra life...
		_NotOptional,								// Just don't get cocky
		_PartyLike2084,							// Robotron1
		_SmashTV,										// 2
		_TotalCarnage,							// 3
		_Llamatron,									// 4	
		_NumAchievements,
	}
	public const uint s_iNumAchievements = (uint)EAchievementIdentifier._NumAchievements;


	// ------ Structures
	//

	// Any dynamically spawned enemies should warn the player that they're
	// coming in!
	[System.Serializable]
	public struct SEnemySpawnSet
	{
		public GameObject _goEnemyPrefab;
		public GameObject _goSpawnInEffect;
		public GameObject _goSpawnWarningEffect;
		public GameObject _goSpawnWarningEffectShort;
	}


	// Killer objects that spawn in Eugatron rooms...
	[System.Serializable]
	public struct SEugatronStaticObstacleComponent
	{
		public GameObject _goObjectPrefab;
		public int _iSpawnCount;
	}


	// Definition of one of the enemy sets that makes up a wave...
	[System.Serializable]
	public struct SEugatronEnemyComponent
	{
		public SEnemySpawnSet _Enemy;
		public int _iSpawnCount;
	}


	[System.Serializable]
	public struct SDoorwayColliders
	{
		[Header("Triggers / Collision Areas")]
		public BoxCollider2D _gcCollider_L;
		public BoxCollider2D _gcCollider_R;
		public BoxCollider2D _gcCollider_U;
		public BoxCollider2D _gcCollider_D;
		public DoorwayCollision _gcDoorL;
		public DoorwayCollision _gcDoorR;
	}


	[System.Serializable]
	public struct SDoorwaySpawn
	{
		[Header("Spawn Points")]
		public float _fSpawnOffset;
		public EDirection _iDir;
		public GameObject _goOpposingSpawn;
		public GameObject _goRoom;
		// GNTODO: To make things easier to setup, _goRoom should be in the Doorway script
		// The doorway script should pass it to this struct in the triggers...
	}


	// DevMenu populates a button list using the details in this struct to allow for instant
	// teleporting to rooms in the game. No timeline changes are made though!
	//
	[System.Serializable]
	public struct SDevMenuDoorDetails
	{
		public Vector2 _vPos1;
		public Vector2 _vPos2;
		public GameObject _goRoom1;
		public GameObject _goRoom2;
	}

	// Value and name for the highscore, save in persistent storage...
	public struct SHighScore
	{
		public ulong _iScore;
		public string _sName;
	}



	// This struct is the editable info for the assets that appear in-game. 
	// It _should_ match what's setup in steam. For DRM free stores, it's
	// the only representation that appears in-game...
	//
	[System.Serializable]
	public struct SAchievementAssets
	{
		public EAchievementIdentifier _iAchievementID;
		public Sprite _gcSprite;
	}



	// This is the final struct that's passed around in code...
	public struct SAchievementInfo
	{
		public SAchievementAssets _Assets;
		public string _sAchievementTitle;
		public string _sAcheivementDesc;
	}


	// ------ Interfaces


	// Enemies, or other moving objects that should reset if the player dies, should implement
	// this interface so respond to the player's death...
	//
	public interface IRoom_EnemyObject
	{
		void OnRoomEnter();
		void OnRoomExit();
		void OnPlayerHasDied();
		void OnPlayerRespawn();
		void OnReset();
	}


	// Objects that respond to switches implement this interface...
	//
	public interface IRoom_SwitchResponder
	{
		void OnSwitchFlip();
		void OnSwitchFlop();
	}


	// GNTODO:
	// There's a good argument for having a different interface for Dynamic / Static room
	// obejcts, if the Room Controller was able to store spawn positions for dynamic objects
	// and Instatiate/Destroy them as the player passed through the rooms...
	// Levels probably aren't big enough to warrant that atm though...


	// ------- Bit fields shifters, for GameGlobals.m_iGameStateFlags_0x to track events

	// Global Events
	public const ulong s_iGE_IntroShown = 0x01;
	public const ulong s_iGE_EugatronUnlocked = 0x02;
	public const ulong s_iGE_LaserIntroDoorToggled = 0x04;
	public const ulong s_iGE_PassedLaserIntroRoom = 0x08;
	public const ulong s_iGE_TopOfFallFirstEntry = 0x10;
	public const ulong s_iGE_SimplePatrolRouteSwitch = 0x20;
	public const ulong s_iGE_SpawningEnemies = 0x40;
	public const ulong s_iGE_RobotronTest = 0x80;
	public const ulong s_iGE_Robotron2 = 0x100;
	public const ulong s_iGE_SwarmKilled = 0x200;
	public const ulong s_iGE_Robotron3 = 0x400;
	public const ulong s_iGE_Robotron4 = 0x800;
	public const ulong s_iGE_FirstSpawners = 0x1000;
	public const ulong s_iGE_ExitKey = 0x2000;
	public const ulong s_iGE_FirstBrains = 0x4000;
	public const ulong s_iGE_AnotherSecretDoor = 0x8000;
	public const ulong s_iGE_Robotron5 = 0x10000;

	// Inventory Items
	public const ulong s_iINV_Shot_Double = 0x01;
	public const ulong s_iINV_Shot_Triple = 0x02;
	public const ulong s_iINV_Laser_Single = 0x04;
	public const ulong s_iINV_Laser_Double = 0x08;
	public const ulong s_iINV_Laser_Triple = 0x10;
	public const ulong s_iINV_Option = 0x20;
	public const ulong s_iINV_Speed1 = 0x40;
	public const ulong s_iINV_Speed2 = 0x80;
	public const ulong s_iINV_ALL = s_iINV_Shot_Double | s_iINV_Shot_Triple | s_iINV_Laser_Single | s_iINV_Laser_Double | s_iINV_Laser_Triple | s_iINV_Option | s_iINV_Speed1 | s_iINV_Speed2;
}
