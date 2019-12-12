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

using System;
using UnityEngine;

public class GunBase : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Gun Properties")]
	// Bullet we're going to fire. 
	[SerializeField] protected GameObject m_BulletPrefab = null;

	// Pause duration, in seconds, between shots.
	[SerializeField] protected float m_fFiringPauseSeconds = 1.0f / 16.0f;

	// Gun's don't fire from the centre of the object, but a certain distance in front
	[SerializeField] protected float m_fBulletSpawnOffset = 0.1f;

	// Size of the bullet pool for this gun
	[SerializeField] protected uint m_iBulletPoolSize = 16;

	// How fast the bullet moves. Quantised to fixed speeds defined in Types.cs
	[SerializeField] protected float m_fBulletSpeed = Types.s_fBULSPD_PixelBullet;



	// Can this gun fire this frame?
	protected bool m_bCanFire = false;

	// Subclassed guns may have active Update functions before they've called
	// OnInit, so we want to block that without complicating the room logic. 
	protected bool m_bSetup = false;

	// This var holds the TimeEvent that TimerManager is currently tracking. 
	protected UInt64 m_iTimerHandle;

	// Trajectory to fire the next bullet.  
	protected Vector2 m_vFireDirection = new Vector2(1.0f, 0.0f);

	// Position offset to fire from, as multi-object NPCs (etc) will have guns that aren't at origin
	protected Vector3 m_vGunPositionOffset = Vector3.zero;

	// Bullet pool for this gun. This will be a circular buffer, sized bigger than the 
	// max number of bullets on screen, so we need do the minimum of state checks 
	// when taking or returning a bullet to the pool...
	protected GameObject[] m_aBulletPool;

	// Index of the next available bullet...
	protected uint m_iPoolIndex;



	// Guns will have different behaviours on player death than this base class does
	// See overrides...
	virtual public void OnRoomEnter() { }
	virtual public void OnRoomExit() { }
	virtual public void OnPlayerHasDied() { OnDeInit(); }
	virtual public void OnPlayerRespawn() { }
	virtual public void OnReset() { }



	// Guns can't fire by default!
	//
	protected virtual void Awake()
	{
		m_bCanFire = false;
		m_bSetup = false;
	}



	// Create the bullet pool. This needs to be explicit, we can't rely on Awake, as 
	// some guns will be fixed-placed in the world and should activate when the room does
	//
	public void OnInit()
	{
		GAssert.Assert(null != m_BulletPrefab, "Gun setup without a bullet prefab. Unable to create the bullet pool...");

		m_aBulletPool = new GameObject[m_iBulletPoolSize];
		for (int i = 0; i < m_iBulletPoolSize; ++i)
		{
			m_aBulletPool[i] = GameObject.Instantiate(m_BulletPrefab, new Vector3(10000.0f, 10000.0f, 0.0f), Quaternion.identity);
			m_aBulletPool[i].GetComponent<BulletMovement>().DeInitBullet();
		}

		m_iPoolIndex = 0;
		m_bSetup = true;
	}



	// Destroy the bullet pool
	//
	public void OnDeInit()
	{
		if (null != m_aBulletPool && m_aBulletPool.Length > 0)
		{
			foreach (GameObject go in m_aBulletPool)
				Destroy(go);
		}

		m_bSetup = false;
		SetCanFire(false);
	}



	private void OnDisable()
	{
		OnDeInit();
	}



	// Change current firing state. If this is changed to true, we fire immediately and 
	// then set a Timer for the next firing event... 
	//
	public void SetCanFire(bool bCanFire, float fDelayStart = -1.0f, bool bBeginFiring = true)
	{
		if (bCanFire)
		{
			if (!m_bCanFire)
			{
				m_bCanFire = true;
				if (fDelayStart > 0.0f) m_iTimerHandle = TimerManager.AddTimer(fDelayStart, Fire);
				else if (bBeginFiring) Fire();
			}
		}
		else
		{
			TimerManager.ClearTimerHandler(m_iTimerHandle, Fire);
			m_bCanFire = false;
		}
	}



	// Set next direction of fire. This has to be passed to any bullets spawned so they 
	// head off in the correct direction...
	//
	virtual public void UpdateFireDirection(Vector2 vDir)
	{
		vDir.Normalize();
		m_vFireDirection = vDir;
	}



	// This is a delegate that TimerManager will call when the gun's m_fFiringPauseSeconds  
	// has elapsed. It spawns a new bullet, sets its trajectory and then sets a new timer
	// for the next shot...
	//
	virtual public void Fire()
	{
		if (m_bCanFire)
		{
			// Bullet spawn position is gun's position, plus a definable distance, offset in the direction of fire...
			Vector3 vSpawnPos = (transform.position + m_vGunPositionOffset) + (new Vector3(m_vFireDirection.x, m_vFireDirection.y, 0.0f) * m_fBulletSpawnOffset);

			// Spawn the bullet
			{
				if (m_aBulletPool[m_iPoolIndex].activeSelf) Debug.LogError(gameObject.name + ": Bullet Pool overrun!");
				m_aBulletPool[m_iPoolIndex].SetActive(true);
				m_aBulletPool[m_iPoolIndex].transform.position = vSpawnPos;
				m_aBulletPool[m_iPoolIndex].GetComponent<BulletMovement>().InitBullet(m_vFireDirection, m_fBulletSpeed);
				++m_iPoolIndex;
				if (m_iPoolIndex >= m_iBulletPoolSize) m_iPoolIndex = 0;
			}

			// Set a timer for the next shot...
			m_iTimerHandle = TimerManager.AddTimer(m_fFiringPauseSeconds, Fire);
		}
	}
}
