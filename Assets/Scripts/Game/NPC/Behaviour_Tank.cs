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

// Tanks just wander around, and take infinite damage.

public class Behaviour_Tank : EnemyObjectBase
{
	override public void OnPlayerHasDied()
	{
		m_bBehaviourCanUpdate = false;

		TrackHitPoints gc = GetComponent<TrackHitPoints>();
		if (null != gc) gc.PlayDeathEffects();

		base.OnReset();
	}



	override public void OnPlayerRespawn()
	{
		m_bBehaviourCanUpdate = true;
	}



	private void FixedUpdate()
	{
		// Skip if we're paused
		if (TimerManager.IsPaused()) return;
		Vector3 vPos = transform.position;
		vPos.z = Types.s_fPOS_RearLayerZ;
		transform.position = vPos;
	}
}
