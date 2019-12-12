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


// If the player is close enough to a floating pickup object 
// in the world then this class is responsible for what 
// happens when it's collected...
//

[RequireComponent(typeof(AttractToPlayer))]
public class PlayerPickupAction : MonoBehaviour
{
	[Header("Action Settings")]
	[SerializeField] private Types.EPlayerPickupAction m_iTriggerAction = Types.EPlayerPickupAction._NOTHING;
	[SerializeField] private float m_fPickupDistance = 0.04f;

	[Header("Effects")]
	[SerializeField] private EGameSFX m_iAudio = EGameSFX._SFX_COLLECT_MULTIPLIER;
	[SerializeField] private GameObject m_goPickupEffect = null;

	private AttractToPlayer m_gcPlayerAttractor = null;



	void Start()
	{
		m_gcPlayerAttractor = GetComponent<AttractToPlayer>();
		GAssert.Assert(null != m_gcPlayerAttractor, "PlayerPickupAction can't find AttracToPlayer");
	}



	void Update()
	{
		if (m_gcPlayerAttractor.GetDistanceFromPlayer() <= m_fPickupDistance)
		{
			// Grab the optional components. 
			//
			AddScore gcScore = GetComponent<AddScore>();
			AddMultiplier gcMult = GetComponent<AddMultiplier>();
			AddPowerup gcPowerUp = GetComponent<AddPowerup>();
			PlayerState gcPlayerState = GameInstance.Object.GetPlayerState();
			AddGameEvent gcGameEvent = GetComponent<AddGameEvent>();

			bool bDidAdd = false;

			// Call the relevant actions...
			//
			switch (m_iTriggerAction)
			{
				case Types.EPlayerPickupAction._NOTHING:
					break;

				case Types.EPlayerPickupAction._GIVE_SCORE_AND_MULTIPLIER:
					if (null != gcScore) bDidAdd = gcScore.AddScoreToPlayer();
					if (null != gcMult) bDidAdd = gcMult.AddMultiplierToPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_MULTIPLIER:
					if (null != gcMult) bDidAdd = gcMult.AddMultiplierToPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_SCORE:
					if (null != gcScore) bDidAdd = gcScore.AddScoreToPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_SCORE_AND_POWERUP:
					if (null != gcScore) bDidAdd = gcScore.AddScoreToPlayer();
					if (null != gcPowerUp) bDidAdd = gcPowerUp.AddPowerupToPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_POWERUP:
					if (null != gcPowerUp) bDidAdd = gcPowerUp.AddPowerupToPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_LIFE:
					if (null != gcPlayerState) bDidAdd = gcPlayerState.AddLives(1);
					break;

				case Types.EPlayerPickupAction._GAME_EVENT:
					if (null != gcGameEvent) bDidAdd = gcGameEvent.AddGameEventForPlayer();
					break;

				case Types.EPlayerPickupAction._GIVE_KEY:
					if(null != gcGameEvent) bDidAdd = true;
					if (null != gcGameEvent) bDidAdd = gcGameEvent.AddGameEventForPlayer();		// Key prefab should also have an event attached!
					GameInstance.Object.GetPlayerState().SpawnExitKey();
					break;
			};

			// Only play the feedbacks if add was successful
			if (bDidAdd)
			{
				GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iAudio);
				if (null != m_goPickupEffect) Instantiate(m_goPickupEffect, transform.position, Quaternion.identity);
			}

			Destroy(gameObject);
		}
	}
}
