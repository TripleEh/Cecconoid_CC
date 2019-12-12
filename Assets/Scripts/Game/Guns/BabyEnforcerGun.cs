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

// Baby enforcers are based on the Enforcers from Robotron. 
// They fire sparks that 'predict' player position and use
// Player's trajectory to modify bullet paths. Little bastards,
// basically. 
//

[RequireComponent(typeof(TrackHitPoints))]
public class BabyEnforcerGun : GunBase
{
	[SerializeField] protected float m_fMinBulletSpeed = 0.25f;

	protected GameObject m_goPlayer = null;
	protected GameInstance m_gcGameInstance = null;
	protected TrackHitPoints m_gcHealth = null;
	protected float m_fThisBulletSpeed = 0f;

	// If we're "Angry" we're just firing all the time and directly at the player
	protected bool m_bIsAngry = false;



	override protected void Awake()
	{
		base.Awake();

		// Validate!
		{
			m_gcGameInstance = GameInstance.Object;
			GAssert.Assert(null != m_gcGameInstance, "Unable to get GameInstance!");

			m_goPlayer = m_gcGameInstance.GetPlayerObject();
			GAssert.Assert(null != m_goPlayer, "Unable to get player!");

			BulletMovementSpark gc = m_BulletPrefab.GetComponent<BulletMovementSpark>();
			GAssert.Assert(null != gc, "Baby Enforcer Gun must have bullets of type BulletMovementSpark");

			m_gcHealth = GetComponent<TrackHitPoints>();
			GAssert.Assert(null != m_gcHealth, "Unable to get health!");
		}
	}



	// Don't Init the bullet pool, we're not using it
	//
	virtual protected void Start()
	{
		base.SetCanFire(true, Random.Range(0.5f, 3.5f));
		m_bSetup = true;
	}



	private void OnDestroy()
	{
		base.OnDeInit();
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, Fire);
	}



	public void SetIsAngry()
	{
		m_bIsAngry = true;
	}



	override public void Fire()
	{
		if (m_bCanFire)
		{
			// Target position is the player's position
			Vector3 vTargetPos = m_goPlayer.transform.position;
			Vector3 vTargetFuture = Vector3.zero;
			Vector2 vTrajAdd = Vector3.zero;


			if (!m_bIsAngry)
			{
				// Plus where they'll be 15-20 frames from now (which is scarily accurate at these scales, btw :D)
				vTargetFuture = (m_gcGameInstance.GetPlayerTrajectory() * Random.Range(0.1f, 1f))
											* (m_gcGameInstance.GetPlayerState().GetPlayerMovementSpeed() * Random.Range(0.1f, 1.0f))
											* (Random.Range(0f, 15f) * TimerManager.fGameDeltaTime);

				if (Random.Range(1f, 100f) > 80f) vTrajAdd = m_gcGameInstance.GetPlayerTrajectory();
			}
			else vTargetFuture = m_gcGameInstance.GetPlayerTrajectory() * (20.0f * TimerManager.fGameDeltaTime);


			// Add these together for some variance...
			vTargetPos += vTargetFuture; // * Random.Range(0.25f, 1.0f);

			// We have to call this ourselves as there's no behaviour on top of us constantly aiming...
			UpdateFireDirection(vTargetPos - transform.position);

			// Bullet spawn position is gun's position, plus a definable distance, offset in the direction of fire...
			Vector3 vSpawnPos = (transform.position + m_vGunPositionOffset) + (new Vector3(m_vFireDirection.x, m_vFireDirection.y, 0.0f) * m_fBulletSpawnOffset);

			// Bullet's speed needs to be inversely proportional to our distance from the player
			{
				Vector3 vDist = m_goPlayer.transform.position - transform.position;
				m_fThisBulletSpeed = Mathf.Clamp(Mathf.Abs(vDist.magnitude / Types.s_fEUGE_PlayerDistanceDivisor) * m_fMinBulletSpeed, m_fMinBulletSpeed, m_fBulletSpeed);
			}


			// Spawn the Spark
			// Note, because enforcers will be killed so quickly and sparks slide along the environment 
			// we're not pooling these bullets. Player is very likely to kill the Enforcer before the bullet
			// gets going...
			{
				GameObject go = Instantiate(m_BulletPrefab, vSpawnPos, Quaternion.identity);
				BulletMovementSpark gc = go.GetComponent<BulletMovementSpark>();

				if (!m_bIsAngry) gc.SparkInitBullet(m_vFireDirection, vTrajAdd, m_fThisBulletSpeed, Random.Range(0.1f, m_fBulletSpeed / 2.0f));
				else gc.SparkInitBullet(m_vFireDirection, vTrajAdd, m_fBulletSpeed, m_fBulletSpeed / 2.0f);
			}


			// Set a timer for the next shot... (Assuming we're healthy)
			if (m_gcHealth.GetHitPointsRemaining() > 0) m_iTimerHandle = TimerManager.AddTimer(m_fFiringPauseSeconds + Random.Range(-m_fFiringPauseSeconds, m_fFiringPauseSeconds), Fire);
			else m_iTimerHandle = 0;
		}
	}
}
