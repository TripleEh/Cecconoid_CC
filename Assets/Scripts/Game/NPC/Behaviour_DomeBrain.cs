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

// Dome Brains just move between random spots near to them. After
// a random period they'll fire cruise missiles at the player.
//
// They have a fixed amount of ammunition, but also a small chance
// of reloading. :D

public class Behaviour_DomeBrain : EnemyObjectBase
{
	[Header("Enemy")]
	[SerializeField] private GameObject m_goCruiseMissile = null;
	[SerializeField] private uint m_iNumMissiles = 3;

	[Header("Spawn Settings")]
	[SerializeField] private float m_fSpawnTimeMin = 1.5f;
	[SerializeField] private float m_fSpawnTimeMax = 10f;

	private ulong m_iTimerHandle = 0;
	private uint m_iSpawned = 0;
	private float m_fInitSpawnDelay = 2.0f;



	void OnDestroy()
	{
		OnRoomExit();
	}



	void SpawnMissile()
	{

		GameObject go = Instantiate(m_goCruiseMissile, transform.position, Quaternion.identity);
		//if (m_bEugatronSpecific) GameMode_Eugatron.AddRoomObjectToRoomController(go);

		// Spawn a missile if we've got any left...
		if (m_iSpawned < m_iNumMissiles) ++m_iSpawned;
		// What about a bit of a chance to reload?
		else if (Random.Range(0, 99) > 97) m_iSpawned = 0;

		m_iTimerHandle = TimerManager.AddTimer(Random.Range(m_fSpawnTimeMin, m_fSpawnTimeMax), SpawnMissile);
	}



	public override void OnRoomEnter()
	{
		base.OnRoomEnter();

		m_fInitSpawnDelay = Random.Range(1, 10);

		// We don't have to spawn missiles. Make it a reasonable chance that we will though
		if (Random.Range(0, 99) < 90) m_iTimerHandle = TimerManager.AddTimer(m_fInitSpawnDelay + Random.Range(0.0f, m_fSpawnTimeMin), SpawnMissile);
	}



	public override void OnRoomExit()
	{
		base.OnRoomExit();
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, SpawnMissile);
	}



	public override void OnPlayerHasDied()
	{
		if (m_bEugatronSpecific) base.OnPlayerHasDied();
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, SpawnMissile);

		m_bBehaviourCanUpdate = false;
		m_iSpawned = 0;
	}



	public override void OnPlayerRespawn()
	{
		base.OnPlayerRespawn();
		m_iTimerHandle = TimerManager.AddTimer(Random.Range(m_fSpawnTimeMin, m_fSpawnTimeMax), SpawnMissile);
	}
}
