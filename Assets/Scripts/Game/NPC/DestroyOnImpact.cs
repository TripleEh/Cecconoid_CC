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

[RequireComponent(typeof(TrackHitPoints))]
public class DestroyOnImpact : MonoBehaviour
{
	[SerializeField] private bool m_bAnyObject = false;

	// 	Let TrackHitPoints handle our death...
	//
	private void KillInstant(Collision2D collision)
	{
		TrackHitPoints gc = GetComponent<TrackHitPoints>();
		GAssert.Assert(null != gc, "DestroyOnImpact, no TrackHitPoints component on this gameObject!");
		gc.KillInstant();
		this.enabled = false;
	}


	// Missiles kill themselves immediately upon hitting the player, or environment!
	//
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(m_bAnyObject) KillInstant(collision);
		else if ((collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Player"))) KillInstant(collision);
	}


	public void OnCollisionStay2D(Collision2D collision)
	{
		if(m_bAnyObject) KillInstant(collision);
		else if ((collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Player"))) KillInstant(collision);
	}
}
