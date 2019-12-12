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

public class TriggerMaxDamageFromPlayer : MonoBehaviour
{
	public void OnTriggerEnter2D(Collider2D col)
	{
		// Destroy Enemy bullets
		if(col.gameObject.CompareTag(Types.s_sTag_EnemyBullets)) Destroy(col.gameObject);

		// Pass max damage to anything else...
		TrackHitPoints gc = col.gameObject.GetComponent<TrackHitPoints>();
		if(null != gc ){  gc.DoOnImpactFromPlayer(Types.s_iPLAYER_MaxDamage, true); return; }	
	}
}
