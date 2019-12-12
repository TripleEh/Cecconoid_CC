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

public class SpawnPrefab : MonoBehaviour
{
	[SerializeField] protected GameObject m_goPrefab=null;
	[SerializeField] protected float m_fSpawnRadius=0.2f;
	[SerializeField] protected float m_fSpawnCount=1.0f;

	// This is only called by the Eugatron game mode atm...
	public void SetSpawn( GameObject go )
	{
		m_goPrefab = go;
		m_fSpawnCount = 1;
		m_fSpawnRadius = 0;
	}

	virtual public void DoSpawnPrefab()
	{
		// Disabled components can still be called. It's just the Unity functions that 
		// don't run :)
		if(!this.enabled) return;

		GAssert.Assert(null != m_goPrefab, "Unable to spawn multipliers, prefab has not been set!");
		
		Vector3 vPos = Vector3.zero;
		Vector2 vRand = Vector2.zero;

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
