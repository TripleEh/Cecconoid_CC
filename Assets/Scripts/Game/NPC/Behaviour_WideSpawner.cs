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


// This is a spawner variant that sits at the top of rooms and spawns in things above the player
// Doesn't spawn if the player is "too close"
//

public class Behaviour_WideSpawner : Behaviour_Spawner
{
	[SerializeField] private float m_fWidth = 1f;
	[SerializeField] private float m_fPlayerMinDistance = 0.2f;



	public override void Spawn()
	{
		// Early outs...
		if (!m_bIsActive) return;
		if (!GameMode.PlayerIsAlive()) return;
		if (TimerManager.IsPaused()) return;

		// Don't spawn if the player is too close
		{
			Vector3 vPlayerPos = GameInstance.Object.GetPlayerPosition();
			vPlayerPos.x = transform.position.x;
			Vector3 vTraj = vPlayerPos - transform.position;

			if (vTraj.magnitude < m_fPlayerMinDistance * m_fPlayerMinDistance)
			{
				// New Spawn Timer
				m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_ScreenTransitionDelay + Random.Range(m_fMinSpawnTime, m_fMaxSpawnTime), Spawn);
				return;
			}
		}

		// Tbf if we get this far it'd be pretty funny...
		if (m_iSpawnCount >= m_iMaxSpawn) return;

		// Make a random position in our range
		Vector3 vPos = transform.position + m_vSpawnOffset;
		vPos.x += Random.Range(-m_fWidth / 2f, m_fWidth / 2f);

		// Spawn random prefab
		int iIndex = Random.Range(0, m_aSpawnPrefabs.Length);
		GAssert.Assert(null != m_aSpawnPrefabs[iIndex], "Spawner not setup with prefab at index " + iIndex.ToString());
		GameObject go = Instantiate(m_aSpawnPrefabs[iIndex], vPos, Quaternion.identity);

		// Tell it to go
		Types.IRoom_EnemyObject[] aGC = go.GetComponents<Types.IRoom_EnemyObject>();
		foreach (Types.IRoom_EnemyObject gc in aGC) gc.OnRoomEnter();

		// Add it to our list so we can kill everything on exit...
		m_aMyRoomObjects[m_iSpawnCount] = go;
		++m_iSpawnCount;

		// New Spawn Timer
		m_iTimerHandle = TimerManager.AddTimer(Types.s_fDUR_ScreenTransitionDelay + Random.Range(m_fMinSpawnTime, m_fMaxSpawnTime), Spawn);
	}



	new public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + m_vSpawnOffset, new Vector3(m_fWidth, 0.08f, 0.08f));

	}
}
