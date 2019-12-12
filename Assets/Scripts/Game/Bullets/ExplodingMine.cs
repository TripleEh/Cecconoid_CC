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

// Tiny explosion when destroyed by the player, massive one when exploding itself!

[RequireComponent(typeof(SpriteRenderer))]
public class ExplodingMine : MonoBehaviour
{
	[Header("Explosion")]
	[SerializeField] private float m_fCountdownSeconds = 1;
	[SerializeField] private GameObject m_goExplosionPrefab = null;

	[Header("Spike Settings")]
	[SerializeField] private int m_iSpikeCount = 1;
	[SerializeField] private float m_fAngleOffsetRadians= 0f;
	[SerializeField] private GameObject m_goSpike = null;
	[SerializeField] private float m_fSpawnOffset = Types.s_fPixelSize * 8.0f;


	[Header("Pixel Shatter")]
	[SerializeField] private GameObject m_goPixelShatterPrefab = null;
	[SerializeField] private Types.EPixelShatterLife m_iPixelShatterTTL = Types.EPixelShatterLife._SHORT;

	[Header("Audio")]
	[SerializeField] private EGameSFX m_iSFX_Windup = EGameSFX._SFX_MINE_WINDUP_SMALL;
	[SerializeField] private EGameSFX m_iSFX_Explosion = EGameSFX._SFX_EXPLOSION_NPC_SMALL;

	private ulong m_iTimerHandle = 0;



	private void Start()
	{
		GAssert.Assert(null != m_goSpike, "Exploding mine in level without a spike prefab defined! " + gameObject.name);
	}



	public void OnDisable()
	{
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, Explode);
	}



	// When we've been triggered, warn the player with an Audio cue and set a timer to Explode
	//
	public void StartExplosionCountdown()
	{
		GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Windup);
		m_iTimerHandle = TimerManager.AddTimer(m_fCountdownSeconds, Explode);
	}



	public void Explode()
	{
		// Create the explosion
		if (null != m_goExplosionPrefab) Instantiate(m_goExplosionPrefab, transform.position, Quaternion.identity);
		SpriteToParticleSystem.ExplodeSprite(transform.position, Types.s_fVEL_PixelShatterVelocity, m_goPixelShatterPrefab, GetComponent<SpriteRenderer>().sprite, m_iPixelShatterTTL);

		// Play audio and add shake. Mines always add maxium shake...
		GameInstance gi = GameInstance.Object;
		gi.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX_Explosion);
		gi.GetGameCamera().AddShake(1.0f);


		// Spawn the Spikes, if we have any
		{
			GameObject go = null;
			Vector3 vTraj = Vector3.zero;
			BulletMovement gcMovement = null;

			float fAngle = m_fAngleOffsetRadians;
			for(int i = 0; i < m_iSpikeCount; ++i)
			{
				vTraj = new Vector3(Mathf.Sin(fAngle), Mathf.Cos(fAngle), 0f);
				go = Instantiate(m_goSpike, transform.position + (vTraj * m_fSpawnOffset), Quaternion.identity);
				gcMovement = go.GetComponent<BulletMovement>(); if (null != gcMovement) gcMovement.InitBullet(vTraj, Types.s_fVEL_MineSpikeMovementVelocity, false);
				fAngle += (2 * Mathf.PI) / m_iSpikeCount;
			}
		}


		// And die...
		Destroy(gameObject);
	}
}
