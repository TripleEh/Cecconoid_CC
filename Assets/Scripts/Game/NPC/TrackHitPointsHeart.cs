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

public class TrackHitPointsHeart : TrackHitPoints
{
	private bool m_bUpdateLoop = false;
	private float m_fExplosionTime = 0f;

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
	public override void DoOnImpactFromPlayer(uint iDamage, bool bIsPlayerOption = false, bool bPlayerPhysicalCollision=false)
	{
		if (m_bUpdateLoop || bIsPlayerOption) return;

		GameInstance gi = GameInstance.Object;
		GAssert.Assert(null != gi, "Unable to get Game Instance!");

		m_iHitPointsRemaining = (int)MathUtil.Clamp(m_iHitPointsRemaining - (int)iDamage, 0, Types.s_iPLAYER_MaxDamage);
		if (m_iHitPointsRemaining <= 0)
		{
			// Turn off the sprite and collider (Bespoke to the heart)
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;

			// Move particles closer to camera, so they appear over everything
			Vector3 vPos = transform.position;
			vPos.z = Types.s_fPOS_FrontLayerZ;

			// Spawn the effects
			if (m_bPixelShatter) SpriteToParticleSystem.ExplodeSprite(transform.position, Types.s_fVEL_PixelShatterVelocity, m_goPixelShatterPrefab, m_gcSprite, m_iPixelShatterTTL);
			if (m_bPlaySFXOnDeath) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Explosion);
			if (m_bSpawnExplosionEffect && null != m_goExplosionPrefab) Instantiate(m_goExplosionPrefab, transform.position, Quaternion.identity);


			// Add optionals
			AddScore gcScore = GetComponent<AddScore>(); if (null != gcScore) gcScore.AddScoreToPlayer();
			AddCameraShake gcShake = GetComponent<AddCameraShake>(); if (null != gcShake) gcShake.AddShakeToCamera();
			
			SpawnPrefab[] aPrefabSpawns = GetComponents<SpawnPrefab>();
			if (aPrefabSpawns.Length > 0)	for (int i = 0; i < aPrefabSpawns.Length; ++i) aPrefabSpawns[i].DoSpawnPrefab();

			m_bUpdateLoop = true;
			GameMode.BeginCompletionSequence();
		}

		// Didn't die? Play the impact ting audio...
		if (m_bPlaySFXOnHit) gi.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Impact);
	}




	private void Update()
	{
		if(!m_bUpdateLoop) return;
		m_fExplosionTime += ((1f / Time.deltaTime)/2) * TimerManager.fGameDeltaTime;

		if (m_fExplosionTime < 1f) return;
		m_fExplosionTime = 0;

		Vector3 vPos;
		vPos = new Vector3(Random.Range(-Types.s_fRoomBoundsX, Types.s_fRoomBoundsX), Random.Range(-Types.s_fRoomBoundsY, Types.s_fRoomBoundsY), -0.4f);
		vPos += transform.position;
		Instantiate(m_goExplosionPrefab, vPos, Quaternion.identity);
	}
}
