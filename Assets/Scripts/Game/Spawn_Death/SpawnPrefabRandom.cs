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

public class SpawnPrefabRandom : SpawnPrefab
{
	[SerializeField] private GameObject[] m_aPrefabs = null;
	[SerializeField] private float m_fSpawnPercentageChance = 1f;

	override public void DoSpawnPrefab()
	{
		// Disabled components can still be called. It's just the Unity functions that 
		// don't run :)
		if(!this.enabled) return;

		// Quick dice roll to see if we should spawn anything at all...
		if(Random.Range(1f,100f) < (100f - m_fSpawnPercentageChance)) return;

		// We been setup correctly?
		GAssert.Assert(m_aPrefabs.Length != 0, "No prefabs setup in SpawnPrefabRandom component");
		
		Vector3 vPos = Vector3.zero;
		Vector2 vRand = Vector2.zero;

		// Pick a prefab 
		m_goPrefab = m_aPrefabs[Random.Range(0, m_aPrefabs.Length - 1)];

		// And spawn...
		for(int i = 0; i < m_fSpawnCount; ++i)
		{
			vPos = transform.position; 
			vRand = Random.insideUnitCircle * m_fSpawnRadius;
			vPos.x += vRand.x;
			vPos.y += vRand.y;
			Instantiate(m_goPrefab, vPos, Quaternion.identity);
		}
	}
}
