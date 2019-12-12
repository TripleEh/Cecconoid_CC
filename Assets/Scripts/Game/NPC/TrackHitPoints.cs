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


// Every object that takes damage and consequently can be 
// destroyed in the world should attach this component.
// Handles explosions, and calls out to other components
// to add score, multipliers, etc. 
//

public class TrackHitPoints : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] protected uint m_iDefaultHitPoints = 1;
	[SerializeField] protected bool m_bIgnoreEnvironmentalKills = false;
	[SerializeField] protected bool m_bIgnorePlayerOptionKills = false;

	[Header("Explosion")]
	[SerializeField] protected bool m_bSpawnExplosionEffect = true;
	[SerializeField] protected GameObject m_goExplosionPrefab = null;

	// GNTODO: Pixel Shatter should be a struct!
	[Header("Pixel Shatter")]
	[SerializeField] protected bool m_bPixelShatter = true;
	[SerializeField] protected GameObject m_goPixelShatterPrefab = null;
	[SerializeField] protected Types.EPixelShatterLife m_iPixelShatterTTL = Types.EPixelShatterLife._SHORT;


	[Header("Audio")]
	[SerializeField] protected bool m_bPlaySFXOnDeath = true;
	[SerializeField] protected EGameSFX m_iSFX_Explosion = EGameSFX._SFX_EXPLOSION_NPC_SMALL;
	[SerializeField] protected bool m_bPlaySFXOnHit = true;
	[SerializeField] protected EGameSFX m_iSFX_Impact = EGameSFX._SFX_IMPACT_BULLET_MID;

	
	protected Sprite m_gcSprite;

	// This needs to be signed or damage clamping will rollover incorrectly
	protected int m_iHitPointsRemaining = 1;	
	


	void Start()
	{
		// Check the optional components
		if (m_bPixelShatter)
		{
			GAssert.Assert(null != m_goPixelShatterPrefab, "Pixel shatter ticked, but no Prefab particle effect assigned... " + gameObject.name);
			m_gcSprite = GetComponent<SpriteRenderer>().sprite;
			GAssert.Assert(null != m_gcSprite, "Pixel shatter ticked on a gameObject without a Sprite Renderer... " + gameObject.name);
		} 

		m_iHitPointsRemaining = (int)m_iDefaultHitPoints;	
	}



	// Because we can call to kill an object immediately, we've split it out from the collision function
	//
	public virtual void DoOnImpactFromPlayer(uint iDamage, bool bIsPlayerOption = false, bool bPlayerPhysicalCollision = false)
	{
		if(bIsPlayerOption && m_bIgnorePlayerOptionKills) return;

		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get Game Instance!");

		m_iHitPointsRemaining = (int)MathUtil.Clamp(m_iHitPointsRemaining - (int)iDamage, 0, Types.s_iPLAYER_MaxDamage); 
		if (m_iHitPointsRemaining <= 0)
		{
			PlayDeathEffects(); ;
			
			// Add optionals
			AddScore gcScore = GetComponent<AddScore>(); if(null != gcScore) gcScore.AddScoreToPlayer();
			UnlockAchievement gcAchievement = GetComponent<UnlockAchievement>(); if (null != gcAchievement) gcAchievement.AddAchievement();
			AddCameraShake gcShake = GetComponent<AddCameraShake>(); if(null != gcShake) gcShake.AddShakeToCamera();

			// BUT, we only spawn prefabs if we've been shot, not if it's a physical collision
			if(!bPlayerPhysicalCollision)
			{
				SpawnPrefab[] aPrefabSpawns = GetComponents<SpawnPrefab>();
				if (aPrefabSpawns.Length > 0)
				{
					for (int i = 0; i < aPrefabSpawns.Length; ++i) aPrefabSpawns[i].DoSpawnPrefab();
				}
			}

			// Die Die Die!
			Destroy(gameObject);
			return;
		}

		// Didn't die? Play the impact ting audio...
		if (m_bPlaySFXOnHit) gi.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Impact);
	}



	// Eugatron obstacles, or some other thing in the world can call this to kill the 
	// object instantly. No points, or other things will be given to the player!
	//
	public virtual void DoKilledByEnvironment()
	{
		if(m_bIgnoreEnvironmentalKills) return;

		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get Game Instance!");

		m_iHitPointsRemaining=0;
		PlayDeathEffects();
		Destroy(gameObject);
	}



	// Room Controllers can call this to fake a death :D
	public virtual void PlayDeathEffects()
	{
		// Move particles closer to camera, so they appear over everything
		Vector3 vPos = transform.position;
		vPos.z = Types.s_fPOS_FrontLayerZ;

		// Spawn the effects
		if (m_bPixelShatter) SpriteToParticleSystem.ExplodeSprite(transform.position, Types.s_fVEL_PixelShatterVelocity, m_goPixelShatterPrefab, m_gcSprite, m_iPixelShatterTTL);
		if (m_bPlaySFXOnDeath) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Explosion);
		if (m_bSpawnExplosionEffect && null != m_goExplosionPrefab) Instantiate(m_goExplosionPrefab, transform.position, Quaternion.identity);
	}



	// Objects (such as missiles) that kill themselves on impact can call this function to 
	// do an instant death, meaning they don't have to have their own copies of things like
	// the explosion prefabs...
	//
	public virtual void KillInstant(bool bPlayerPhysicalCollision = false)
	{
		// Set to 1, because DoImpact always decrements by 1 BEFORE testing for death...
		m_iHitPointsRemaining = 1; 
		
		// Don't spawn multipliers (if we have any) as this is a suicide call
		SpawnPrefab gcMultiSpawner = GetComponent<SpawnPrefab>(); if(null != gcMultiSpawner) gcMultiSpawner.enabled = false;
		DoOnImpactFromPlayer(99, false, bPlayerPhysicalCollision);
	}



	// This is a catch all for touching the player. 
	// - Player is invulnerable, so destroy this
	// - We've managed to touch the player, so they've died, we should as well...
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(Types.s_sTag_Player) && !GameInstance.Object.m_bIsCecconoid) KillInstant(true);
	}


	
	// Getter, ensures that regardless of collision processing order, some damage is done to the player's bullets by this object!
	//
	public virtual int GetHitPointsRemaining() { if(m_iHitPointsRemaining == 0 ) return 1; else return m_iHitPointsRemaining; }
}
