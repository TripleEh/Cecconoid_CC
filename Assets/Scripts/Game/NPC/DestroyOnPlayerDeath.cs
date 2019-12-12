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


// Any object that should be removed from the 
// gameworld on player death can use this!
//

public class DestroyOnPlayerDeath : MonoBehaviour
{
	private TrackHitPoints m_gcHitPoints = null;


	private void Awake()
	{
		m_gcHitPoints = GetComponent<TrackHitPoints>();
	}


	private void Update()
	{
		// Player can die, so state might not exist...
		PlayerState gc = GameInstance.Object.GetPlayerState();
		if(null == gc ) return;

		if(!gc.PlayerIsAlive())
		{
			if(null != m_gcHitPoints) m_gcHitPoints.DoKilledByEnvironment();
			else Destroy(gameObject);
		}
	}
}
