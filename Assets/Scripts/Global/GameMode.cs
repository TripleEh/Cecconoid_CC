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

public static class GameMode
{
	// UI and parts of the game flow should block the player from 
	// being able to use the pause button
	private static bool m_bCanPause = false;
	#if GDEBUG
	private static bool m_bDevMenuActive = false;
	#endif

	// Track which room's controller is currently active...
	private static RoomControllerBase m_gcActiveRoomController = null;


	// Instantly set player's position to the given location. No error checking
	//
	public static void TeleportPlayer(Vector3 vNewPos, ref GameObject goRoomTo)
	{
		GameInstance gcGameInstance = GameInstance.Object;
		Messenger.Invoke(Types.s_sGF_BeginRoomTransition);

		// Transition  (Camera is NOT responsible for calling Begin/EndRoomTransition!)
		{
			gcGameInstance.GetPlayerState().LockPlayer();
			gcGameInstance.GetPlayerController().BeginTeleport();
			gcGameInstance.GetGameCamera().BeginTeleport(goRoomTo.transform.position);
		}

		// Let the rooms cleanup...
		{
			m_gcActiveRoomController.OnRoomExit();
			m_gcActiveRoomController = goRoomTo.GetComponent<RoomControllerBase>();
			GAssert.Assert(null != m_gcActiveRoomController, "There's no room controller on this room!");
			m_gcActiveRoomController.OnRoomEnter();
		}


		// Set player..
		{
			gcGameInstance.GetPlayerState().UnlockPlayer();
			gcGameInstance.GetPlayerController().EndTeleport();
			GameInstance.Object.GetPlayerObject().transform.position = vNewPos;
			GameGlobals.s_vRoomTransitionTo = vNewPos;
		}

		Messenger.Invoke(Types.s_sGF_EndRoomTransition);
	}



	// Centre of the current room (although technically, it should be a few pixels lower, because of the HUD)
	//
	public static Vector2 GetRoomOrigin()
	{
		return GameInstance.Object.GetGameCamera().vCameraOrigin;
	}



	public static Vector3 GetRespawnPosition()
	{
		return GameGlobals.s_vRoomTransitionTo;
	}

	// Get the active room controller
	//
	public static RoomControllerBase GetRoomController()
	{
		return m_gcActiveRoomController;
	}



	// Block pausing 
	//
	public static void SetCanPause(bool bState)
	{
		m_bCanPause = bState;
	}



	// Game-wide handler for the pause button. Stops our game timer and 
	// sends out the appropriate messages. We don't need to care what
	// responds to these...
	//
	public static void PauseGame()
	{
		if (!m_bCanPause) return;
		TimerManager.PauseGame();
		Messenger.Invoke(Types.s_sHUD_Hide);
		Messenger.Invoke(Types.s_sUI_ShowPauseScreen);
	}



	// As above, but to return the player back to the game. Again, we
	// don't need to care what actually responds to these messages
	//
	public static void UnPauseGame()
	{
		if (!m_bCanPause) return;
		TimerManager.UnPauseGame();
		Messenger.Invoke(Types.s_sHUD_ResetHud);
		Messenger.Invoke(Types.s_sUI_HidePauseScreen);
	}



	public static void ToggleDevMenu()
	{
#if GDEBUG
		if (!m_bCanPause) return;
		if (!GameInstance.Object.m_bIsCecconoid) return;

		if (!m_bDevMenuActive)
		{

			TimerManager.PauseGame();
			Messenger.Invoke(Types.s_sHUD_Hide);
			Messenger.Invoke(Types.s_sMenu_DevMenuShow);
			m_bDevMenuActive = true;
		}
		else
		{
			TimerManager.UnPauseGame();
			Messenger.Invoke(Types.s_sHUD_ResetHud);
			Messenger.Invoke(Types.s_sMenu_DevMenuHide);
			m_bDevMenuActive = false;
		}

		Cursor.visible = m_bDevMenuActive;
#endif
	}



	// Checks that a position is within the camera viewport (room bounds)
	// Set fFudge if you want objects to move outside the camera viewport!
	//
	public static bool BoundsCheckPosition(Vector2 vPos, float fFudge = 0.0f)
	{
		float fBoundsX = Types.s_fRoomBoundsX + fFudge;
		float fBoundsY = Types.s_fRoomBoundsY + fFudge;
		Vector3 vOrigin = GetRoomOrigin();

		if (vPos.x > vOrigin.x + fBoundsX || vPos.x <= vOrigin.x - fBoundsX) return false;
		if (vPos.y > vOrigin.y + fBoundsY || vPos.y <= vOrigin.y - fBoundsY) return false;

		return true;
	}



	// Called on first entry into a new scene (level), just after load. 
	// Searches through every gameobject in the scene, so shouldn't be used in 
	// any other circumstance!
	//
	// It is assumed that every scene will have an initial RoomController on a game
	// object called: "ROOM_SpawnRoom"
	//
	public static void TransitionToDefaultRoom()
	{
		// Flag for the pause/UI controls that we're in-game. 
		// Order is important! OnRomEnter() might immediately set this back to false!
		// GNTODO:  it probably does in all cases... Remove this when Cecconoid handles it properly
		m_bCanPause = true;

		// SUPER DUPER SLOW! 
		GameObject goRoomTo = GameObject.Find(Types.s_sRC_DefaultRoomController);
		GAssert.Assert(null != goRoomTo, "GameInstance unable to find the default room controller in this level!");

		// Init the first room!
		m_gcActiveRoomController = goRoomTo.GetComponent<RoomControllerBase>();
		GAssert.Assert(null != m_gcActiveRoomController, "No room controller on the default room gameobject!");
		m_gcActiveRoomController.OnRoomEnter();


		// Because there's always a transition in (Spawn Effect, of screen roll) Player reset needs to delay
		// The rc_RobotronController in Eugatron scene handles all this for us, though...
		if(GameInstance.Object.m_bIsCecconoid) TimerManager.AddTimer(2f, EndTransitionToDefaultRoom);
	}



	public static void WarpPlayerToLevel2()
	{
		GameObject goRoomTo = GameObject.Find(Types.s_sRC_LevelTwoRoomController);
		GAssert.Assert(null != goRoomTo, "GameInstance unable to find the level 2 room controller!");

		GameInstance gcGameInstance = GameInstance.Object;
		Messenger.Invoke(Types.s_sGF_BeginRoomTransition);

		// Transition  (Camera is NOT responsible for calling Begin/EndRoomTransition!)
		{
			gcGameInstance.GetPlayerController().BeginTeleport();
			gcGameInstance.GetGameCamera().BeginTeleport(goRoomTo.transform.position);
		}

		// Let the rooms cleanup...
		{
			m_gcActiveRoomController.OnRoomExit();
			m_gcActiveRoomController = goRoomTo.GetComponent<RoomControllerBase>();
			GAssert.Assert(null != m_gcActiveRoomController, "There's no room controller on this room!");
			m_gcActiveRoomController.OnRoomEnter();
		}

		TimerManager.AddTimer(2f, EndWarpToLevel2);
	}



	public static void EndWarpToLevel2()
	{
		GameInstance gcGameInstance = GameInstance.Object;

		// Set player..
		{
			gcGameInstance.GetPlayerState().UnlockPlayer();
			gcGameInstance.GetPlayerController().EndTeleport();
			GameInstance.Object.GetPlayerObject().transform.position = m_gcActiveRoomController.transform.position;
			GameGlobals.s_vRoomTransitionTo = m_gcActiveRoomController.transform.position;
		}

		Messenger.Invoke(Types.s_sGF_EndRoomTransition);
	}


	public static void EndTransitionToDefaultRoom()
	{
		// Default Room is always at origin, so GameMode can warp the player there instantly... 
		// NOTE: Player is not spawned when the gamestate enters!
		GameInstance.Object.GetPlayerObject().transform.position = Vector2.zero;

		// Unlock the player now, Spawn Room can always lock them again
		GameInstance.Object.GetPlayerState().UnlockPlayer();
	}



	// Called by Doors, to initialise the transition of the player between rooms
	//
	public static void BeginRoomTransition(ref GameObject goRoomFrom, ref GameObject goRoomTo)
	{
		GAssert.Assert(null != goRoomTo, "BeginRoomTransition called from a Door that's not setup with two rooms!");
		GameInstance gcGameInstance = GameInstance.Object;

		// Play Some Audio
		gcGameInstance.GetAudioManager().PlayAudio(EGameSFX._SFX_MISC_ROOM_TRANSITION);

		// Do the transition (Camera is responsible for calling Begin/EndRoomTransition!)
		{
			gcGameInstance.GetPlayerState().LockPlayer();
			gcGameInstance.GetPlayerController().BeginRoomTransition();
			gcGameInstance.GetGameCamera().BeginRoomTransition(goRoomTo.transform.position);
			;
		}


		// NOTE: Order is important here! OnRoomEnter may want to calculate it's origin, which
		//       will only be accurate AFTER Camera.BeginRoomTransition!
		//
		//				Every room MUST have a room controller attached, even if there's no logic!
		//				Doorways assume this, and it simplifies the logic here...
		//
		{
			m_gcActiveRoomController.OnRoomExit();
			m_gcActiveRoomController = goRoomTo.GetComponent<RoomControllerBase>();
			GAssert.Assert(null != m_gcActiveRoomController, "There's no room controller on this room!");
			m_gcActiveRoomController.OnRoomEnter();
		}


		// The player takes longer to transition than the camera does, and we know the duration, so set a timer
		TimerManager.AddTimer(Types.s_fPLAYER_RoomTransitionDuration, EndRoomTransition);
	}



	// Room transition is complete, let the player resume control
	//
	public static void EndRoomTransition()
	{
		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get GameInstance!");

		gi.GetPlayerState().UnlockPlayer();
		gi.GetPlayerController().EndRoomTransition();
		Messenger.Invoke(Types.s_sGF_EndRoomTransition);
	}



	// Tell everything that the player has died. 
	//
	public static void OnPlayerHasDied()
	{
		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get GameInstance!");

		PlayerState ps = gi.GetPlayerState();
		GAssert.Assert(null != ps, "Unable to get player state!");

		// If we're god, then do nothing...
		if(ps.PlayerIsGod()) return;

		// Otherwise mark the player for Death
		ps.MarkPlayerForDeath();
	}



	// Called via gs_Game_In* LateUpdate, kills the player if they're still marked for death by the end of the frame
	public static void DoLateUpdatePlayerDeathCheck()
	{
		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get GameInstance!");

		PlayerState ps = gi.GetPlayerState();
		GAssert.Assert(null != ps, "Unable to get player state!");

		// Only do this if the player is marked for death!
		if(!ps.GetPlayerMarkedForDeath()) return;

		// Player can't pause
		m_bCanPause = false;

		// Do player death...
		uint iPlayerLives = ps.OnPlayerHasDied();

		// Let the room cleanup...
		m_gcActiveRoomController.OnPlayerHasDied(iPlayerLives);

		// And any subscribers...
		Messenger.Invoke(Types.s_sPLAYER_HasDied);

		// Set a respawn timer if we have lives left...
		if (iPlayerLives > 0) TimerManager.AddTimer(Types.s_fPLAYER_RespawnHalfDur, BeginRespawnPlayer);

		// Move player offscreen until we can respawn...
		// This needs to be done last. Things might still need the player position above!
		gi.GetPlayerObject().transform.position = new Vector3(1000f, 1000f, 0f);
	}



	// Begin the respawn sequence
	//
	public static void BeginRespawnPlayer()
	{
		// GNTODO: Spawn particles
		m_gcActiveRoomController.BeginPlayerRespawn();
		TimerManager.AddTimer(Types.s_fPLAYER_RespawnHalfDur, EndRespawnPlayer);
	}



	// 
	public static void EndRespawnPlayer()
	{
		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get GameInstance!");

		gi.GetAudioManager().PlayAudio(EGameSFX._SFX_PLAYER_RESTART_EUGATRON);
		gi.GetPlayerController().MovePlayerInstant(GameGlobals.s_vRoomTransitionTo);
		gi.GetPlayerState().OnPlayerRespawn();
		m_gcActiveRoomController.EndPlayerRespawn();
		m_bCanPause = true;
	}



	public static bool PlayerIsAlive()
	{
		PlayerState ps = GameInstance.Object.GetPlayerState();
		if (null == ps) return false;

		return ps.PlayerIsAlive() && ps.GetPlayerCanMove();
	}



	public static void BeginCompletionSequence()
	{
		GameInstance.Object.GetPlayerState().LockPlayer();
		GameInstance.Object.GetAudioManager().FadeMusicOutForGameOver();
		Messenger.Invoke(Types.s_sGF_InduceSlowdown);
	}




	public static void AchievementUnlocked(Types.EAchievementIdentifier iAchievementID)
	{
		if (!GameGlobals.TestAchievement(iAchievementID))
		{
			GameGlobals.SetAchievement(iAchievementID);
			GameInstance.Object.SpawnAchievementIcon(iAchievementID);
		}
	}

}
