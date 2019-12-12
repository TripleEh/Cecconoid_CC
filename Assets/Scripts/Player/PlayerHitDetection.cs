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

[RequireComponent(typeof(PlayerState))]
public class PlayerHitDetection : MonoBehaviour
{
	// Need player state to ensure we're not dying when player is already dead 
	// or we're in god mode...
	private PlayerState m_gcPlayerState = null;



	private void Awake()
	{
		m_gcPlayerState = GetComponent<PlayerState>();
		GAssert.Assert(null != m_gcPlayerState, "PlayerHitDetection: Unable to get Player State...");
	}



	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(Types.s_sTag_Enemy) || collision.gameObject.CompareTag(Types.s_sTag_EnemyBullets))
			DoImpact();
	}



	// Because we have a few things (like scanning lasers) that don't have colliders
	// but do know when they've hit the player, this starts player death...
	//
	public void DoImpact()
	{
		if (!m_gcPlayerState.PlayerIsAlive() || m_gcPlayerState.PlayerIsGod()) return;

		GameMode.OnPlayerHasDied();

		// Spawn Particles
		// Play Audio effect
	}
}
