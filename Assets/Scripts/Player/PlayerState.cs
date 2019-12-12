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


[RequireComponent(typeof(PlayerController))]
public class PlayerState : MonoBehaviour
{
	// 'Designer' tweakables (in editor)
	[Header("Settings")]
	[SerializeField] private float m_fPlayerMovementSpeed = 0.75f;
	[SerializeField] private float m_fPlayerMovementSpeedPowerUp = 0.15f;

	[Header("Effects")]
	[SerializeField] private GameObject m_goExplosionEffect = null;
	[SerializeField] private GameObject m_goLifeEffect = null;
	[SerializeField] private Material m_gcInvulnerableMat = null;
	[SerializeField] private Material m_gcSpriteDefaultMat = null;

	// What we're tracking in this session
	[Header("Active Game")]
	public ulong m_iScore = 0;
	public uint m_iScoreToNextLife = 0;
	public uint m_iScoreMultiplier;
	public uint m_iLives = 3;
	public uint m_iEnergy = 100;

	// State tracks the equipped weapon and movement speed for simplicity
	private GunBase m_gcEquippedWeapon = null;
	private float m_fCachedMovementSpeed = 0f;

	// Invulnerability timer
	private ulong m_iInvTimerHandle = 0;

	// Main player states...
	// GNTODO: Convert to a bitfield
	private bool m_bCanFire = false;
	private bool m_bCanMove = false;
	private bool m_bCanAdd = false;
	public bool m_bCanDrop = true;
	private bool m_bIsGod = false;
	private bool m_bCheatModeGod = false;
	private bool m_bIsAlive = false;
	private bool m_bIsFirstDeath = true;
	private bool m_bCheckedForAMillion = false;
	private bool m_bCheckedFor99 = false;
	private bool m_bPlayerMarkedForDeath = false;


	// Achievement Only
	private int m_iNumberOfDeaths = 0;

	public float GetPlayerMovementSpeed() { return m_fCachedMovementSpeed; }
	public bool GetPlayerCanMove() { return m_bCanMove; }
	public bool GetPlayerCanFire() { return m_bCanFire; }
	public bool PlayerIsAlive() { return m_bIsAlive; }
	public bool PlayerIsGod() { return m_bIsGod || m_bCheatModeGod; }
	public void SetPlayerCanMove(bool bState) { m_bCanMove = bState; }
	public void SetPlayerCanFire(bool bState) { m_bCanFire = bState; m_gcEquippedWeapon.SetCanFire(bState); }
	public void SetPlayerIsAlive(bool bState) { m_bIsAlive = bState; }
	public void SpawnExitKey() { m_gcInventory.SpawnExitKey(); }
	public void DespawnExitKey() { m_gcInventory.DespawnExitKey(); }

	private PlayerInventory m_gcInventory = null;



	// Order is important here, this must be called AFTER PlayerInventory in the GameInstance!
	// 
	public void SetDefaults()
	{
		m_bCanFire = m_bCanMove = m_bIsGod = m_bCanAdd = false;
		m_iScore = 0;
		m_iScoreToNextLife = 0;
		m_iScoreMultiplier = 1;
		m_iLives = 3;
		m_iEnergy = 100;
		m_fCachedMovementSpeed = m_fPlayerMovementSpeed;
		m_bIsAlive = true;
		m_bIsFirstDeath = true;
		m_bCheatModeGod = false;
		m_bCheckedForAMillion = false;
		m_bCheckedFor99 = false;

		m_gcInventory = GetComponent<PlayerInventory>();
		GAssert.Assert(null != m_gcInventory, "PlayerState: Unable to get player Inventory!");
		m_gcEquippedWeapon = m_gcInventory.GetEquippedWeapon();
		GAssert.Assert(null != m_goExplosionEffect, "PlayerState has no prefab for explosions!");
	}



	// This is a cheat mode function, can be accessed through the DevMenu
	//
	public void SetPlayerIsGod(bool bState)
	{
		m_bCheatModeGod = bState;
	}



	// As Above
	//
	public void SetPlayerCanDrop(bool bState)
	{
		m_bCanDrop = bState;
	}



	// Another cheat mode function, will give all to Players. DevMenu
	//
	public void SetPlayerInventoryAll(bool bState)
	{
#if GDEBUG
		GAssert.Assert(null != m_gcInventory, "Player Inventory not set!");
		if (bState)
		{
			m_gcInventory.AddAll();
			if (m_gcEquippedWeapon != m_gcInventory.GetEquippedWeapon())
			{
				m_gcEquippedWeapon.SetCanFire(false);
				m_gcEquippedWeapon = m_gcInventory.GetEquippedWeapon();
			}
			// Update the cached speed multiplier
			m_fCachedMovementSpeed = m_fPlayerMovementSpeed + (m_fPlayerMovementSpeedPowerUp * m_gcInventory.GetSpeedMultiplier());
		}
		else m_gcInventory.SetDefaults();
#endif
	}


	// Annnnnnd another cheat mode!
	//
	public void SetPlayerInventorySome(bool bState)
	{
#if GDEBUG
		GAssert.Assert(null != m_gcInventory, "Player Inventory not set!");
		if (bState)
		{
			AddPowerUp(Types.EPowerUp._OPTION);
			AddPowerUp(Types.EPowerUp._LASER_DOUBLE);
			AddPowerUp(Types.EPowerUp._SPEED2);

			if (m_gcEquippedWeapon != m_gcInventory.GetEquippedWeapon())
			{
				m_gcEquippedWeapon.SetCanFire(false);
				m_gcEquippedWeapon = m_gcInventory.GetEquippedWeapon();
			}
			// Update the cached speed multiplier
			m_fCachedMovementSpeed = m_fPlayerMovementSpeed + (m_fPlayerMovementSpeedPowerUp * m_gcInventory.GetSpeedMultiplier());
		}
		else m_gcInventory.SetDefaults();
#endif
	}





	// Default state for Eugatron is one speed up and double shot
	// This is just an initial balance thing (can make the base game harder)
	// Multipliers are reset so we can keep scores sorta reasonable
	//
	public void EugatronResetPlayer()
	{
		GAssert.Assert(null != m_gcInventory, "Player Inventory not set!");
		m_gcInventory.SetDefaults();

		// These will make the player invulnerable
		AddPowerUp(Types.EPowerUp._SHOT_DOUBLE);
		AddPowerUp(Types.EPowerUp._SPEED1);

		// So lets block that...
		TimerManager.ClearTimerHandler(m_iInvTimerHandle, EndInvulnerablePeriod);
		EndInvulnerablePeriod();

		ResetMultiplier();
	}



	public void UnlockPlayer()
	{
		m_bCanFire = m_bCanMove = m_bCanAdd = true;
	}



	public void LockPlayer()
	{
		m_bCanFire = m_bCanMove = m_bCanAdd = false;
	}



	public bool AddScore(uint iScore)
	{
		if (!m_bCanAdd) return false;

		// Do score...
		m_iScore = MathUtil.Clamp(m_iScore + (iScore * m_iScoreMultiplier), m_iScore, Types.s_iPLAYER_MaxScore);
		Messenger.Invoke(Types.s_sHUD_ScoreUpdated);

		// Check for extra life if we're in Eugatron
		if (!GameInstance.Object.m_bIsCecconoid)
		{
			m_iScoreToNextLife += (iScore * m_iScoreMultiplier);
			while (m_iScoreToNextLife > Types.s_iPLAYER_ExtraLifeScore)
			{
				m_iScoreToNextLife = MathUtil.Clamp(m_iScoreToNextLife - Types.s_iPLAYER_ExtraLifeScore, 0, 9999999);
				if (null != m_goLifeEffect) AddLives(1);
			}
		}


		// Check for Achievements
		if (m_iScore >= 1000000 && !m_bCheckedForAMillion)
		{
			m_bCheckedForAMillion = true;
			if (!GameInstance.Object.m_bIsCecconoid) GameMode.AchievementUnlocked(Types.EAchievementIdentifier._MillionInEugatron);
			else GameMode.AchievementUnlocked(Types.EAchievementIdentifier._MillionInCecconoid);
		}

		return true;
	}



	public bool AddLives(uint iAmount)
	{
		if (!m_bCanAdd) return false;

		// Make the sprite face the correct way...
		GameObject go = Instantiate(m_goLifeEffect, transform.position, Quaternion.identity);
		SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
		SpriteRenderer mySr = GetComponent<SpriteRenderer>();
		if (null != sr && null != mySr) sr.sprite = mySr.sprite;

		// Add the life...
		m_iLives = MathUtil.Clamp(m_iLives + iAmount, 0, Types.s_iPLAYER_MaxLives);
		Messenger.Invoke(Types.s_sHUD_LivesUpdated);
		GameInstance.Object.GetAudioManager().PlayAudio(EGameSFX._SFX_PLAYER_EXTRA_LIFE);


		// Check for Achievement
		if (!GameInstance.Object.m_bIsCecconoid && m_iLives >= 10) GameMode.AchievementUnlocked(Types.EAchievementIdentifier._10LevelsEugatron);

		return true;
	}



	public bool AddMultiplier(uint iAmount)
	{
		if (!m_bCanAdd) return false;
		m_iScoreMultiplier = MathUtil.Clamp(m_iScoreMultiplier + iAmount, 0, Types.s_iPLAYER_MaxMultiplier);
		Messenger.Invoke(Types.s_sHUD_MultiplierUpdated);

		// Check For Achievement
		{
			if (GameInstance.Object.m_bIsCecconoid && m_iScoreMultiplier >= 99 && !m_bCheckedFor99)
			{
				GameMode.AchievementUnlocked(Types.EAchievementIdentifier._99MultCecconoid);
				m_bCheckedFor99 = true;
			}
			else if (!GameInstance.Object.m_bIsCecconoid && m_iScoreMultiplier >= 50)
			{
				GameMode.AchievementUnlocked(Types.EAchievementIdentifier._50MultEugatron);
				m_bCheckedFor99 = true;
			}
		}

		return true;
	}



	public void ResetMultiplier()
	{
		m_iScoreMultiplier = 1;
		// Do not need to invoke MultiplierUpdated here, as the HUD is 
		// removed and reset by the RoomController during transition...
	}



	public bool AddPowerUp(Types.EPowerUp iPowerUp)
	{
		GAssert.Assert(null != m_gcInventory, "Player Inventory not set!");

		// Set the inventory state for the new pickup
		switch (iPowerUp)
		{
			case Types.EPowerUp._SHOT_DOUBLE: m_gcInventory.SetInventoryItem(Types.s_iINV_Shot_Double); break;
			case Types.EPowerUp._SHOT_TRIPLE: m_gcInventory.SetInventoryItem(Types.s_iINV_Shot_Triple); break;
			case Types.EPowerUp._LASER_SINGLE: m_gcInventory.SetInventoryItem(Types.s_iINV_Laser_Single); break;
			case Types.EPowerUp._LASER_DOUBLE: m_gcInventory.SetInventoryItem(Types.s_iINV_Laser_Double); break;
			case Types.EPowerUp._LASER_TRIPLE: m_gcInventory.SetInventoryItem(Types.s_iINV_Laser_Triple); break;
			case Types.EPowerUp._OPTION: m_gcInventory.SetInventoryItem(Types.s_iINV_Option); break;
			case Types.EPowerUp._SPEED1: m_gcInventory.SetInventoryItem(Types.s_iINV_Speed1); break;
			case Types.EPowerUp._SPEED2: m_gcInventory.SetInventoryItem(Types.s_iINV_Speed2); break;
		}

		// Update the weapon, in case we've upgraded...
		if (m_gcEquippedWeapon != m_gcInventory.GetEquippedWeapon())
		{
			m_gcEquippedWeapon.SetCanFire(false);
			m_gcEquippedWeapon = m_gcInventory.GetEquippedWeapon();
		}

		// Update the cached speed multiplier
		m_fCachedMovementSpeed = m_fPlayerMovementSpeed + (m_fPlayerMovementSpeedPowerUp * m_gcInventory.GetSpeedMultiplier());

		// If this was the option, enable it!
		if (iPowerUp == Types.EPowerUp._OPTION) m_gcInventory.GetPlayerOption().SetActive(true);

		// In Eugatron pickups give you a little invulnerable period...
		if (!GameInstance.Object.m_bIsCecconoid && !m_bIsGod) StartInvulnerablePeriod();

		// Check for Achievement
		if (m_gcInventory.IsFullPower() && GameInstance.Object.m_bIsCecconoid) GameMode.AchievementUnlocked(Types.EAchievementIdentifier._FullPowerCecconoid);

		// We've always added something...
		return true;
	}



	public bool DecMultiplier(uint iAmount)
	{
		if (!m_bCanAdd) return false;
		m_iScoreMultiplier = MathUtil.Clamp(m_iScoreMultiplier - iAmount, 1, Types.s_iPLAYER_MaxMultiplier);
		Messenger.Invoke(Types.s_sHUD_MultiplierUpdated);
		return true;
	}



	// Player Controller doesn't know (or care) which weapon is equipped, so 
	// uses this function to make sure the gun state is correct
	//
	public void UpdateEquippedWeapon(Vector2 vTraj, bool bControllerFireState)
	{
		GAssert.Assert(null != m_gcEquippedWeapon, "PlayerState: equipped weapon is null. Last powerup broke something!");
		m_gcEquippedWeapon.UpdateFireDirection(vTraj);
		m_gcEquippedWeapon.SetCanFire(m_bCanFire && bControllerFireState);
	}



	// Game Mode can tell the player that they have died. BUT. Something else might 
	// have happened in the same frame (got a powerup) so don't process death 
	// instantly, mark it now, then check in LateUpdate
	//
	public void MarkPlayerForDeath()
	{
		m_bPlayerMarkedForDeath = true;
	}



	public bool GetPlayerMarkedForDeath()
	{
		return m_bPlayerMarkedForDeath;
	}


	// When the player dies:
	// - Drop inventory items
	// - Reset multiplier
	// - Decrement lives
	// - Trigger events so any message subscribers can react
	//
	public uint OnPlayerHasDied()
	{
		m_bPlayerMarkedForDeath = false;

		LockPlayer();

		// Create the explosion
		Instantiate(m_goExplosionEffect, transform.position, Quaternion.identity);

		// Check for achievement
		++m_iNumberOfDeaths;
		if(m_iNumberOfDeaths > 200) GameMode.AchievementUnlocked(Types.EAchievementIdentifier._SweetBangBang);
		else Debug.Log("Sweet Bang Bang count: " + m_iNumberOfDeaths);


		// Handle Inventory
		{
			GAssert.Assert(null != m_gcInventory, "Player Inventory not set!");
			if (m_bCanDrop)
			{
				m_gcInventory.DropInventory();
				m_gcInventory.GetPlayerOption().SetActive(false);
			}
			m_gcEquippedWeapon = m_gcInventory.GetEquippedWeapon();
			m_fCachedMovementSpeed = m_fPlayerMovementSpeed + (m_fPlayerMovementSpeedPowerUp * m_gcInventory.GetSpeedMultiplier());
		}


		// Update state
		{
			m_bIsAlive = false;
			m_iScoreMultiplier = 1;
			if (m_iLives > 0) --m_iLives;
		}

		// Tell the HUD things have changed...
		Messenger.Invoke(Types.s_sHUD_LivesUpdated);
		Messenger.Invoke(Types.s_sHUD_MultiplierUpdated);


		// Check for the achievement
		if(GameInstance.Object.m_bIsCecconoid && m_bIsFirstDeath)
		{
			GameMode.AchievementUnlocked(Types.EAchievementIdentifier._FirstDeath);
			m_bIsFirstDeath = false;
		}

		// Return lives count. GameMode / GameInstance decide what to do when the
		// player is out of lives. 
		return m_iLives;
	}



	public void OnPlayerRespawn()
	{
		m_bIsAlive = true;
		m_bPlayerMarkedForDeath = false;

		UnlockPlayer();

		if (GameInstance.Object.m_bIsCecconoid) StartInvulnerablePeriod();
	}



	public void StartInvulnerablePeriod()
	{
		// Player can no longer die this frame...
		m_bPlayerMarkedForDeath = false;
		m_bIsGod = true;

		// Reset clear timer
		if (m_bIsGod) TimerManager.ClearTimerHandler(m_iInvTimerHandle, EndInvulnerablePeriod);

		// Switch to the flashing material...
		SpriteRenderer gc = GetComponent<SpriteRenderer>();
		if (null != gc && null != m_gcInvulnerableMat) gc.material = m_gcInvulnerableMat;

		m_iInvTimerHandle = TimerManager.AddTimer(Types.s_fDUR_EugatronInvulnPeriod, EndInvulnerablePeriod);
	}



	public void EndInvulnerablePeriod()
	{
		SpriteRenderer gc = GetComponent<SpriteRenderer>();
		if (null != gc && null != m_gcSpriteDefaultMat) gc.material = m_gcSpriteDefaultMat;

		m_iInvTimerHandle = 0;
		m_bIsGod = false;
	}
}
