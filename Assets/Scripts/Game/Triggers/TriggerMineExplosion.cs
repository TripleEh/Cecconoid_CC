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

[RequireComponent(typeof(ExplodingMine))]
public class TriggerMineExplosion  : TriggerInRangeOfPlayer, Types.IRoom_EnemyObject
{
	[SerializeField] private float m_fRaycastOffset = 0.12f;


	// If the player has died, we need to explode instantly
	// The GameMode will have cleared all timers, so we tell the mine explicitly...
	public override void OnPlayerHasDied()
	{
		if(m_bPlayerInRange || false == this.enabled)
		{
			ExplodingMine gcMine = GetComponent<ExplodingMine>();
			if (null != gcMine) gcMine.Explode();
			m_bIsActive = false;

		}
	}



	protected new void Update()
	{
		if (!m_bIsActive) return;
		if(null == m_goPlayerObject) return;
		if(TimerManager.IsPaused()) return;

		Vector3 vDist = m_goPlayerObject.transform.position - transform.position;

		m_bPlayerInRange = false;

		// If we're respecting colliders, then we need to have raycast line-of-sight to the player
		if(m_bRespectColliders)
		{
			RaycastHit2D Hit = Physics2D.Raycast(transform.position + (vDist.normalized * m_fRaycastOffset) , vDist.normalized, (m_fTriggerDistance - m_fRaycastOffset) );
			Debug.DrawRay(transform.position + (vDist.normalized * 0.12f), vDist.normalized * (m_fTriggerDistance - m_fRaycastOffset));
			if(null != Hit.collider && Hit.collider.CompareTag("Player")) m_bPlayerInRange = true;
		}
		// Otherwise just explode if we're close enough!
		else if (vDist.magnitude <= m_fTriggerDistance) m_bPlayerInRange = true;


		if (m_bPlayerInRange)
		{
				// Tell the mine to start the countdown to explode
				ExplodingMine gcMine = GetComponent<ExplodingMine>();
				if(null != gcMine) gcMine.StartExplosionCountdown();
			
				// Change to the active material, if present
				SpriteRenderer gcRenderer = GetComponent<SpriteRenderer>();
				if(null != m_gcActiveMaterial && null != gcRenderer) gcRenderer.material = m_gcActiveMaterial;

				// If we've been triggered, don't let the player destroy the mine
				TrackHitPoints gcHitP = GetComponent<TrackHitPoints>();
				if(null != gcHitP) gcHitP.enabled = false;


				// Work here is done...
				this.enabled = false;
		}
	}
}
