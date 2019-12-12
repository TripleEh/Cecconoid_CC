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


// Fixed placement Enemy Spawner
// Randomly spawns a prefab from the list, tracks what it's spawned
// directly so OnRoomExit() it's this that's responsible for cleaning up everything...
//
// GNTODO: Test that this actually cleans up everything OnRoomExit when it's destroyed! :D
//

public class Behaviour_Spawner : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] protected GameObject[] m_aSpawnPrefabs = null;
	[SerializeField] protected float m_fMinSpawnTime = 0.2f;
	[SerializeField] protected float m_fMaxSpawnTime = 0.5f;
	[SerializeField] protected Vector3 m_vSpawnOffset = Vector3.zero;
	[SerializeField] protected uint m_iMaxSpawn = 48;

	protected bool m_bIsActive = false;
	protected ulong m_iTimerHandle = 0;
	protected uint m_iSpawnCount = 0;

	protected GameObject[] m_aMyRoomObjects;



	public void OnDisable()
	{
		//OnRoomExit();
		TimerManager.ClearTimerHandler(m_iTimerHandle, Spawn);
	}



	public void OnRoomEnter()
	{
		m_aMyRoomObjects = new GameObject[m_iMaxSpawn];
		m_bIsActive = true;
		m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_ScreenTransitionDelay + Random.Range(m_fMinSpawnTime, m_fMaxSpawnTime), Spawn);
	}



	public void OnRoomExit()
	{
		m_bIsActive = false;

		// Destroy everything....
		foreach (GameObject go in m_aMyRoomObjects)
		{
			if (null == go) continue;
			Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnRoomExit();
			Destroy(go);
		}


		// Tidy up
		TimerManager.ClearTimerHandler(m_iTimerHandle, Spawn);
	}



	public void OnPlayerHasDied()
	{
		m_bIsActive = false;
		TimerManager.ClearTimerHandler(m_iTimerHandle, Spawn);
	}



	public void OnPlayerRespawn()
	{
		m_bIsActive = true;
		Spawn();
	}



	public void OnReset()
	{

	}



	virtual public void Spawn()
	{
		// Early outs...
		if (!m_bIsActive) return;
		if (!GameMode.PlayerIsAlive()) return;
		if (TimerManager.IsPaused()) return;
		
		// If we've spawned everything, DIE
		// This is mainly to prevent the player from cheesing the 
		// object (through save/reload, when I do it) for infinite
		// points...
		if (m_iSpawnCount >= m_iMaxSpawn)
		{
			TrackHitPoints gc = GetComponent<TrackHitPoints>();
			if(null != gc) gc.DoKilledByEnvironment();
			else Destroy(gameObject);
			return;
		}

		// Spawn random prefab
		int iIndex = Random.Range(0, m_aSpawnPrefabs.Length);
		GAssert.Assert(null != m_aSpawnPrefabs[iIndex], "Spawner not setup with prefab at index " + iIndex.ToString());
		GameObject go = Instantiate(m_aSpawnPrefabs[iIndex], transform.position + m_vSpawnOffset, Quaternion.identity);

		// Tell it to go
		Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
		foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnRoomEnter();

		// Add it to our list so we can kill everything on exit...
		m_aMyRoomObjects[m_iSpawnCount] = go;
		++m_iSpawnCount;

		// New Spawn Timer
		m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_ScreenTransitionDelay + Random.Range(m_fMinSpawnTime, m_fMaxSpawnTime), Spawn);
	}


	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + m_vSpawnOffset, new Vector3(0.08f, 0.08f, 0.08f));
	}
}
