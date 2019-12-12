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

public class DestroyObstacleOnImpact : MonoBehaviour
{
	private float m_fEventTime = 0f;
	private bool m_bKillEverything = false;

	private void Start()
	{
		m_fEventTime = TimerManager.fGameTime;
	}
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



	// This is Eugatron Specific, and while we do want it to kill everything
	// we don't want it wiping out a bunch of obstacles immediately when the player
	// spawns. Do delay by an arbitrary amount so a few more obstacles survive the
	// immediate carnage of the spawn...
	//
	private void Update()
	{
		if (m_bKillEverything) return;
		if (TimerManager.fGameTime - m_fEventTime > Types.s_fDUR_FinaliseSpawnDelay * 3f) m_bKillEverything = true;
	}




	public void OnCollisionEnter2D(Collision2D col)
	{
		// Kill the enemy if we've hit them...
		if (col.gameObject.CompareTag(Types.s_sTag_Enemy))
		{
			TrackHitPoints gc = col.gameObject.GetComponent<TrackHitPoints>();
			if(null != gc) gc.DoKilledByEnvironment();

			if (m_bKillEverything)
			{
				gc = GetComponent<TrackHitPoints>();
				GAssert.Assert(null != gc, "DestroyObstacleOnImpact called on go without a TrackHitPoints componetn!");
				gc.DoKilledByEnvironment();
			}
		}

		// Kill the enemy bullet if we've hit them...
		if (col.gameObject.CompareTag(Types.s_sTag_EnemyBullets))
		{
			Destroy(col.gameObject);

			if (m_bKillEverything)
			{
				TrackHitPoints gc = GetComponent<TrackHitPoints>();
				GAssert.Assert(null != gc, "DestroyObstacleOnImpact called on go without a TrackHitPoints componetn!");
				gc.DoKilledByEnvironment();
			}
		}
	}
}
