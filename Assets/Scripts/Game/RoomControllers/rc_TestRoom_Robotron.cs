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

using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// Robotron rooms are based, funnily enough, on Robotron 2084!
// Except in my version, multiple waves can spawn, on a timer. 
//

public class rc_TestRoom_Robotron : RoomControllerBase
{
	protected enum ERobotronEvents
	{
		_RB1_IckleBaddies,
		_RB2_Enforcers,
		_RB3_FloatingLaserBrains,
		_RB4_LoadsaBaddies,
		_RB5_CircularShotTwats,
	};


	[Header("Robotron Params")]
	[SerializeField] protected GameObject[] m_aAdditionalObjects;
	[SerializeField] protected int m_iEnemiesToSpawn = 16;
	[SerializeField] protected int m_iNumberOfWaves = 3;
	[SerializeField] protected float m_fWaveDelay = 5.0f;
	[SerializeField] protected Vector2 m_vHalfRoomBounds = new Vector2(1.7f, 0.7f);
	[SerializeField] protected float m_fDistanceFromPlayer = 0.2f;
	[SerializeField] protected Types.SEnemySpawnSet m_SpawnSet = new Types.SEnemySpawnSet();
	[SerializeField] protected ERobotronEvents m_iEvent = ERobotronEvents._RB1_IckleBaddies;
  [SerializeField] protected GameObject m_goWarningPrefab = null;

  [Header("Rewards")]
	[SerializeField] protected GameObject[] m_aRewards;
	[SerializeField] protected GameObject m_goRewardEffect;

	protected int m_iWaveNumber = 0;
	protected Vector2[] m_aSpawnPositions;
	protected List<Vector2> m_aRespawnPositions = new List<Vector2>();
	protected Vector2 m_vRoomOrigin = Vector3.zero;
	protected ulong m_iTimerHandle;
	protected bool m_bIsActive = false;
	protected float m_fLastEventTime = 0f;
	protected ulong m_iEventBitField;



	// Disable all additional objects in rooms, as these are placed in-editor 
	// and not spawned in as part of the wave!
	private void Awake()
	{
		foreach (GameObject go in m_aAdditionalObjects) go.SetActive(false);
	}



	// GameInstance will call this when the player passes through a doorway, 
	// initiating the transition between two rooms...
	//
	// For us, it's the start of the little mini game, so we need to verify 
	// our settings and then begin spawning in waves...
	//
	public override void OnRoomEnter()
	{
		// When the game is finished we set a GameEvent so this mini-game can't be repeated
		switch (m_iEvent)
		{
			case ERobotronEvents._RB1_IckleBaddies: m_iEventBitField = Types.s_iGE_RobotronTest; break;
			case ERobotronEvents._RB2_Enforcers: m_iEventBitField = Types.s_iGE_Robotron2; break;
			case ERobotronEvents._RB3_FloatingLaserBrains: m_iEventBitField = Types.s_iGE_Robotron3; break;
			case ERobotronEvents._RB4_LoadsaBaddies: m_iEventBitField = Types.s_iGE_Robotron4; break;
			case ERobotronEvents._RB5_CircularShotTwats: m_iEventBitField = Types.s_iGE_Robotron5; break;
		}

		// If we've already completed this room, do nothing!
		if (GameGlobals.TestGameEvent(m_iEventBitField)) return;

		// Spawn in the warning Prefab...
		if(null != m_goWarningPrefab) Instantiate(m_goWarningPrefab, transform.position, Quaternion.identity);

    // Validate what we need
    {
			GAssert.Assert(null != m_SpawnSet._goEnemyPrefab, "Robotron Room Controller has no Enemy Prefab Assigned!");
			GAssert.Assert(null != m_SpawnSet._goSpawnInEffect, "Robotron Room Controller has Spawn In Effect");
			GAssert.Assert(null != m_SpawnSet._goSpawnWarningEffect, "Robotron Room Controller has Spawn Warning Prefab");
			GAssert.Assert(null != m_SpawnSet._goSpawnWarningEffectShort, "Robotron Room Controller missing spawn effect short");
		}

		// Init any additional objects, these aren't tracked as part of the unlock sequence
		{
			foreach (GameObject go in m_aAdditionalObjects)
			{
				if (null == go) continue;
				go.SetActive(true);
				Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
				foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnRoomEnter();
			}
		}

		m_vRoomOrigin = GameMode.GetRoomOrigin();
		m_aRoomObjects = new GameObject[m_iEnemiesToSpawn * m_iNumberOfWaves];
		m_bIsActive = true;

		// Wait for the door to shut before we kick things off...
		m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay+0.35f, FinaliseRoomEntry);
	}



	public override void FinaliseRoomEntry()
	{
		CloseAllDoors();
		m_iTimerHandle = TimerManager.AddTimer(0.5f, SpawnWave);
	}



	// On Exit, kill everything!
	// It's probably going to be the case that Robotron rooms can only be left when everything is dead...
	// However, while testing it's better to be safe than sorry...
	//
	public override void OnRoomExit()
	{
		// Destroy anything we spawned...
		foreach (GameObject go in m_aRoomObjects)
			if (null != go) Destroy(go);

		// Just in case! 
		TimerManager.ClearTimerHandler(m_iTimerHandle, FinaliseWaveSpawning);
		m_bIsActive = false;
	}




	// Check for the complete (death) of the wave, spawn rewards and unlock the player!
	//
	public virtual void Update()
	{
		// Early outs
		{
			if (GameGlobals.TestGameEvent(m_iEventBitField)) return;
			if (!m_bIsActive) return;
			if (m_iWaveNumber < m_iNumberOfWaves) return;
		}


		// If there are active gameObjects (we spawned them) then the player can't exit...
		bool bStillPlaying = false;
		foreach (GameObject go in m_aRoomObjects)
			if (null != go) bStillPlaying = true;


		if (!bStillPlaying)
		{
			// DeInit any additional objects
			{
				foreach (GameObject go in m_aAdditionalObjects)
				{
					if (null == go) continue;
					Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
					foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnRoomExit();
					go.SetActive(false);
				}
			}

			// Spawn the rewards!
			foreach (GameObject go in m_aRewards)
			{
				if (null == go) continue;

				Vector2 vPos = Vector2.zero;
				Vector2 vDist = Vector2.zero;
				Vector2 vPlayerPos = GameInstance.Object.GetPlayerPosition();
				bool bChecking = true;

				do
				{
					vPos = new Vector2(Random.Range(-m_vHalfRoomBounds.x + 0.25f, m_vHalfRoomBounds.x - 0.25f), Random.Range(-m_vHalfRoomBounds.y + 0.25f, m_vHalfRoomBounds.y - 0.25f));
					vPos += m_vRoomOrigin;
					vDist = vPlayerPos - vPos;
					if (vDist.magnitude > m_fDistanceFromPlayer) bChecking = false;
				}
				while (bChecking);

				Instantiate(go, vPos, Quaternion.identity);
				if (null != m_goRewardEffect) Instantiate(m_goRewardEffect, vPos, Quaternion.identity);
			}

			// Play Audio
			GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._SFX_ROBOTRON_COMPLETE);


			// Add the achivement
			switch (m_iEvent)
			{
				case ERobotronEvents._RB1_IckleBaddies: GameMode.AchievementUnlocked(Types.EAchievementIdentifier._PartyLike2084); break;
				case ERobotronEvents._RB2_Enforcers: GameMode.AchievementUnlocked(Types.EAchievementIdentifier._SmashTV ); break;
				case ERobotronEvents._RB3_FloatingLaserBrains: GameMode.AchievementUnlocked(Types.EAchievementIdentifier._TotalCarnage); break;
				case ERobotronEvents._RB4_LoadsaBaddies: /* The key will be delivered in this room */ break;
				case ERobotronEvents._RB5_CircularShotTwats: GameMode.AchievementUnlocked(Types.EAchievementIdentifier._Llamatron); break;
			}

			// Save event in GameGlobals...
			OpenAllDoors();
			GameGlobals.SetGameEvent(m_iEventBitField);
			m_bIsActive = false;
		}
	}




	// Enemy spawning is a two part process:
	// 1. Spawn in warning signs, and particle effects to highlight where enemies will be
	// 2. Wait for these to complete
	// 3. Spawn in the actual baddies...
	//
	// We calc the spawn positions in 1, fire off the effects, and add a Timer callback
	// to complete the spawn sequence...
	//	
	public void SpawnWave()
	{
		if (!m_bIsActive) return;

		m_aSpawnPositions = new Vector2[m_iEnemiesToSpawn];
		Vector2 vPos = Vector2.zero;
		Vector2 vDist = Vector2.zero;

		for (int i = 0; i < m_iEnemiesToSpawn; ++i)
		{
			bool bChecking = true;
			Vector2 vPlayerPos = GameInstance.Object.GetPlayerPosition();

			// Pick a random location within the bounds and check it's not too close
			// to the initial position of the player...
			do
			{
				vPos = new Vector2(Random.Range(-m_vHalfRoomBounds.x, m_vHalfRoomBounds.x), Random.Range(-m_vHalfRoomBounds.y, m_vHalfRoomBounds.y));
				vPos += m_vRoomOrigin;
				vDist = vPlayerPos - vPos;
				if (vDist.magnitude > m_fDistanceFromPlayer) bChecking = false;
			}
			while (bChecking);

			// Spawn the warning effect and the particle effect, they will destroy themselves
			Instantiate(m_SpawnSet._goSpawnInEffect, vPos, Quaternion.identity);
			Instantiate(m_SpawnSet._goSpawnWarningEffect, vPos, Quaternion.identity);

			// Store the position for when we finalise the wave
			m_aSpawnPositions[i] = vPos;
		}

		// Alert the player
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._SFX_ROBOTRON_WARN);

		// Effects have been spawned in, but the enemies haven't!
		m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_FinaliseSpawnDelay, FinaliseWaveSpawning);
	}




	// Cycle through the stored spawn positions and instantiate one of our enemy prefabs
	//
	public void FinaliseWaveSpawning()
	{
		if (!m_bIsActive) return;

		for (int i = 0; i < m_iEnemiesToSpawn; ++i)
		{
			m_aRoomObjects[m_iWaveNumber * m_iEnemiesToSpawn + i] = Instantiate(m_SpawnSet._goEnemyPrefab, m_aSpawnPositions[i], Quaternion.identity);
			foreach (EnemyObjectBase gc in m_aRoomObjects[m_iWaveNumber * m_iEnemiesToSpawn + i].GetComponents<EnemyObjectBase>()) gc.OnRoomEnter();
			foreach (Types.IRoom_EnemyObject eo in m_aRoomObjects[m_iWaveNumber * m_iEnemiesToSpawn + i].GetComponents<Types.IRoom_EnemyObject>()) eo.OnRoomEnter();
		}

		// If we have waves left to spawn, do so
		++m_iWaveNumber;
		if (m_iWaveNumber < m_iNumberOfWaves) m_iTimerHandle = TimerManager.AddTimer(m_fWaveDelay, SpawnWave);

		// Record when this happened, so we can spawn the next wave at the right time
		// even when the player dies in the middle...
		m_fLastEventTime = TimerManager.fGameTime;
	}





	// When the player has died we want to:
	// - Find all the live baddies
	// - Clear them
	// - Respawn them in positions away from where the player is respawned
	// - Continue...
	override public void OnPlayerHasDied(uint iPlayerLives)
	{
		// Get rid of the Update()
		m_bIsActive = false;

		// Clear next wave spawn timer
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, SpawnWave);

		// Clear the array of respawn positions
		m_aRespawnPositions.Clear();

		Vector2 vPos = Vector3.zero;
		Vector2 vSpawn = GameMode.GetRespawnPosition();
		Vector2 vDist;
		bool bChecking = true;

		foreach (GameObject go in m_aRoomObjects)
		{
			if (go != null)
			{
				// Find a random-ish point away from player respawn position...
				bChecking = true;
				do
				{
					vPos = new Vector2(Random.Range(-m_vHalfRoomBounds.x, m_vHalfRoomBounds.x), Random.Range(-m_vHalfRoomBounds.y, m_vHalfRoomBounds.y));
					vPos += m_vRoomOrigin;
					vDist = vSpawn - vPos;
					if (vDist.magnitude > m_fDistanceFromPlayer) bChecking = false;
				}
				while (bChecking);

				// Add this position to the list
				m_aRespawnPositions.Add(vPos);

				// Kill existing baddie
				TrackHitPoints gc = go.GetComponent<TrackHitPoints>();
				if (null != gc) gc.DoKilledByEnvironment();
			}
		}


		// Tell any additional objects
		{
			foreach (GameObject go in m_aAdditionalObjects)
			{
				if (null == go) continue;
				Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
				foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnPlayerHasDied();
			}
		}


		// Spawn a short warning icon...
		if (iPlayerLives > 0) foreach (Vector3 vRPos in m_aRespawnPositions) Instantiate(m_SpawnSet._goSpawnWarningEffectShort, vRPos, Quaternion.identity);
	}




	public override void EndPlayerRespawn()
	{
		// Time it takes for a player to die and respawn...
		m_fLastEventTime += Types.s_fPLAYER_RespawnDuration;

		// Respawning all the objects, but making sure that they fill m_aRoomObjects
		// from index 0, with no spaces. Means any subsequent wave won't overwrite
		// any references...
		for (int i = 0; i < m_aRespawnPositions.Count; ++i) m_aRoomObjects[i] = Instantiate(m_SpawnSet._goEnemyPrefab, m_aRespawnPositions[i], Quaternion.identity);

		if (m_iWaveNumber < m_iNumberOfWaves) m_iTimerHandle = TimerManager.AddTimer(m_fWaveDelay - (TimerManager.fGameTime - m_fLastEventTime), SpawnWave);

		m_bIsActive = true;

		// Now wake everything up!
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;
			var eo = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach (var gc in eo) gc.OnRoomEnter();
		}


		// Tell any additional objects
		{
			foreach (GameObject go in m_aAdditionalObjects)
			{
				if (null == go) continue;
				Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
				foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnPlayerRespawn();
			}
		}
	}
}
