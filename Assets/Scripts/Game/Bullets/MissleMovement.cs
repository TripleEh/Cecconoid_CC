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


// These are vertical / horizontal missiles that can be placed in the world
// Explode on contact...
//

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TrackHitPoints))]
public class MissleMovement : MonoBehaviour // lol typo :D
{
	// To be set in the Prefab. Audio SFX to play when fired and hitting something. 
	//
	[Header("Audio")]
	[SerializeField] private EGameSFX m_iSpawnEffect = EGameSFX._SFX_PLAYER_BULLET_WEAK;

	// Effect to spawn on impact!
	//
	[Header("Particle Effects")]
	[SerializeField] private GameObject m_goSpawnParticleEffect = null;

	// Tweakables...
	//
	[Header("Movement")]
	[SerializeField] private float m_fMovementSpeed = Types.s_fVEL_SmallMissleMovementVelocity;
	[SerializeField] private Vector2 m_vTrajectory = Vector2.zero;


	private bool m_bIsMoving = false;
	private Rigidbody2D m_gcRgdBdy = null;
	private BoxCollider2D m_gcCollider;
	private float m_fEventTime;


	public void StartMovement()
	{
		// Setup variables
		{
			m_bIsMoving = true;
			m_vTrajectory = m_vTrajectory.normalized;
			m_fEventTime = TimerManager.fGameTime;
			m_gcCollider = GetComponent<BoxCollider2D>();
			m_gcRgdBdy = GetComponent<Rigidbody2D>();
		}

		// and go...	
		if (null != m_gcRgdBdy) m_gcRgdBdy.WakeUp();
		if (null != m_goSpawnParticleEffect) Instantiate(m_goSpawnParticleEffect, transform.position, Quaternion.identity);

		GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSpawnEffect);
	}


	// Movement needs to ease in over a curve, so this can't be a fire and forget projectile. 
	// Event Time tracks when movement was started, and we roll over an Easing Curve until 
	// we're hit our max speed. From there, update as normal...
	//
	public void FixedUpdate()
	{
		// Early out...
		if (!m_bIsMoving) return;
		if (TimerManager.IsPaused()) return;

		// Bounds check
		if (!GameMode.BoundsCheckPosition(transform.position, m_gcCollider.size.x)) Destroy(gameObject);

		// Update position
		if (m_bIsMoving)
		{
			float fRatio = (TimerManager.fGameTime - m_fEventTime) / 1.0f;

			Vector2 vTraj;
			if (fRatio <= 1.0f)
				vTraj = (m_vTrajectory * (Easing.EaseIn(fRatio, EEasingType.Quintic) * m_fMovementSpeed)) * TimerManager.fGameDeltaTime;
			else
				vTraj = (m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime;

			Vector2 vPos = new Vector2(transform.position.x + vTraj.x, transform.position.y + vTraj.y);
			m_gcRgdBdy.MovePosition(vPos);
		}
	}
}
