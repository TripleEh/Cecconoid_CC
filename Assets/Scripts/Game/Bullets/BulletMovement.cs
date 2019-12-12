// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.


using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : MonoBehaviour, Types.IRoom_EnemyObject
{
	// To be set in the Prefab. Audio SFX to play when fired and hitting something. 
	//
	[Header("Audio")]
	[SerializeField] protected EGameSFX m_iSpawnEffect = EGameSFX._SFX_PLAYER_BULLET_WEAK;
	[SerializeField] protected EGameSFX m_iDespawnEffect = EGameSFX._SFX_IMPACT_BULLET_WEAK;

	// Effects to spawn in...
	//
	[Header("Particle Effects")]
	[SerializeField] protected GameObject m_goSpawnParticleEffect = null;
	[SerializeField] protected GameObject m_goImpactParticleEffect = null;

	[Header("Bullet")]
	[SerializeField] protected uint m_iBulletDamage = 1;

	// Referenced components
	protected GameInstance m_gcGameInstance;
	protected Rigidbody2D m_gcRgdBdy = null;
	protected BoxCollider2D m_gcCollider = null;

	// Bullet damage and status
	protected int m_iDamageRemaining;
	protected bool m_bIsAlive = false;

	// Movement params
	protected float m_fMovementSpeed;
	protected Vector2 m_vTrajectory = Vector2.zero;



	// Implement the Enemy Object Interface, but we only need to 
	// track the status of the player; Alive or Dead
	//
	public void OnRoomEnter() { }
	public void OnRoomExit() { }
	public void OnPlayerHasDied() { DeInitBullet(); }
	public void OnPlayerRespawn() { }
	public void OnReset() { }



	// Setup with the references we need
	//
	protected void Awake()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		m_gcCollider = GetComponent<BoxCollider2D>();
		m_gcGameInstance = GameInstance.Object;
		GAssert.Assert(null != m_gcGameInstance, "Bullet, unable to get GameInstance!");
		m_iDamageRemaining = (int)m_iBulletDamage;
	}



	// Gun calls this to set the initial direction of the bullet. 
	//
	virtual public void InitBullet(Vector3 vTraj, float fMovementSpeed, bool bPlayAudio = true)
	{
		m_bIsAlive = true;
		m_vTrajectory = vTraj.normalized;
		m_fMovementSpeed = fMovementSpeed;
		m_iDamageRemaining = (int)m_iBulletDamage;

		if (null != m_gcRgdBdy) m_gcRgdBdy.WakeUp();
		if (null != m_gcCollider) m_gcCollider.enabled = true;
		if (null != m_goSpawnParticleEffect) Instantiate(m_goSpawnParticleEffect, transform.position, Quaternion.identity);
		if (bPlayAudio) m_gcGameInstance.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSpawnEffect);

		float fAngle = Mathf.Atan2(vTraj.y, vTraj.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
	}




	// Deactivate the gameObject for reuse later...
	//
	virtual public void DeInitBullet()
	{
		// If we've ever been alive (fired) then we can add camera shake
		if (m_bIsAlive)
		{
			AddCameraShake gc = GetComponent<AddCameraShake>();
			if (null != gc)
				gc.AddShakeToCamera();
		}

		// Set defaults
		m_bIsAlive = false;
		m_vTrajectory = Vector2.zero;
		m_fMovementSpeed = 0.0f;

		// Send physics to sleep and cleanup...
		{
			if (null != m_gcRgdBdy)
			{
				m_gcRgdBdy.velocity = Vector2.zero;
				m_gcRgdBdy.Sleep();
				gameObject.SetActive(false);
			}

			if (null != m_gcCollider) m_gcCollider.enabled = false;
		}
	}




	// Move in the direction we're pointing at!
	//
	virtual public void FixedUpdate()
	{
		// Skip if we're paused
		if (TimerManager.IsPaused()) return;

		// Bounds check to the room we're in, kill if out of bounds
		if (!GameMode.BoundsCheckPosition(transform.position, m_gcCollider.size.x)) DeInitBullet();


		// Update position
		Vector2 vTraj = (m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime;
		m_gcRgdBdy.MovePosition(new Vector2(transform.position.x + vTraj.x, transform.position.y + vTraj.y));
	}




	// Pay audio / effects, and return to the pool
	//
	virtual public void OnCollisionEnter2D(Collision2D collision)
	{
		if (m_bIsAlive)
		{
			// Now do some checks to see what we hit. 
			{


				// Env always killed the bullet, no matter where it comes from
				if (collision.gameObject.CompareTag(Types.s_sTAG_Environment) || collision.gameObject.CompareTag(Types.s_sTAG_Doorway)) m_iDamageRemaining = 0;

				// Enemy Bullets always subtract from Damage Remaining
				else if (collision.gameObject.CompareTag(Types.s_sTag_EnemyBullets)) m_iDamageRemaining = (int)MathUtil.Clamp(m_iDamageRemaining - 1, 0, Types.s_iPLAYER_MaxDamage);

				// Enemies and destructible items should subtract their hitpoints 
				else if (collision.gameObject.CompareTag(Types.s_sTag_Enemy) || collision.gameObject.CompareTag(Types.s_sTag_Destructible))
				{


					// But... if we're a player bullet we also need to tell the Enemy how much damage we're doing! 
					// Enemy bullets should just die on impact...
					if (!gameObject.CompareTag(Types.s_sTag_PlayerBullets))
						m_iDamageRemaining = 0;
					else
					{
						TrackHitPoints gc = collision.gameObject.GetComponent<TrackHitPoints>();
						if (null != gc)
						{
							int iHitPoints = gc.GetHitPointsRemaining();
							int iDamageAfter = (int)MathUtil.Clamp(m_iDamageRemaining - iHitPoints, 0, Types.s_iPLAYER_MaxDamage);
							gc.DoOnImpactFromPlayer((uint)m_iDamageRemaining);
							m_iDamageRemaining = iDamageAfter;
						}
						// If an enemy doesn't have TrackHitPoints it's invulnerable, so kill this bullet
						else m_iDamageRemaining = 0;
					}


				}

				// Anything remaining is untagged, so kill the bullet just in case...
				else m_iDamageRemaining = 0;
			}


			// Die if we need to...
			if (m_iDamageRemaining == 0)
			{
				Vector3 vPos = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, Types.s_fPOS_FrontLayerZ);
				vPos += new Vector3(collision.contacts[0].normal.x, collision.contacts[0].normal.y, 0.0f) * Types.s_fPOS_ImpactEffectSpawnOffset;
				if (null != m_goImpactParticleEffect) Instantiate(m_goImpactParticleEffect, vPos, Quaternion.identity);

				GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iDespawnEffect);
				DeInitBullet();
			}
		}
	}
}
