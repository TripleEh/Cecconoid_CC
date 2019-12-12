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


// This transform will just reflect off a collision and bounce
// around indefinitely. 
//
// It's not robust, and stuff will get stuck, but that's actually
// pretty handy for the player, so what the hell...
//

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Behaviour_BounceAroundRoom : EnemyObjectBase
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMovementSpeed = 1.0f;
	[SerializeField] private LayerMask m_iCollideAgainst = new LayerMask();

	[Header("Audio")]
	[SerializeField] private bool m_bPlaySFXOnImpact = true;
	[SerializeField] private EGameSFX m_iSFX = EGameSFX._SFX_NPC_BOUNCE_LOW;


	private Vector3 m_vTrajectory = Vector3.zero;
	private Rigidbody2D m_gcRdgBdy = null;
	private CircleCollider2D m_gcCollider = null;


	private void Start()
	{
		m_gcRdgBdy = GetComponent<Rigidbody2D>();
		m_gcCollider = GetComponent<CircleCollider2D>();
	}



	// Set a random direction
	//
	public override void OnRoomEnter()
	{
		base.OnRoomEnter();

		m_vTrajectory.x = 1f;
		m_vTrajectory.y = 1f;
		m_vTrajectory.z = 0f;

		if (Random.Range(-1f, 1f) > 0f) m_vTrajectory.x = -m_vTrajectory.x;
		if (Random.Range(-1f, 1f) > 0f) m_vTrajectory.y = -m_vTrajectory.y;

		m_bBehaviourCanUpdate = true;
	}

	

	// Move Rigid Body...
	//
	private void FixedUpdate()
	{
		if(!m_bBehaviourCanUpdate) return;
		if (TimerManager.IsPaused()) return;

		// Check for room bounds, when there's no collision!
		Vector3 vBounce = MathUtil.GetReflectionVectorFromEugatronBoundsCheck(transform.position, m_gcCollider.radius);
		if (vBounce != Vector3.zero)
		{
			if (m_bPlaySFXOnImpact) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX);
			m_vTrajectory = Vector3.Reflect(m_vTrajectory, vBounce);
		}

		Vector3 vNewPos = transform.position + ((m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime);
		vNewPos.z = 0.0f;
		m_gcRdgBdy.MovePosition(vNewPos);
	}



	private void OnCollisionEnter2D(Collision2D col)
	{
		if(!m_bBehaviourCanUpdate) return;

		Vector2 vPoint = col.contacts[0].point;
		Vector2 vInverseNormal = -col.contacts[0].normal;
		vPoint -= vInverseNormal * 1.5f;

		RaycastHit2D Hit = Physics2D.Raycast(vPoint, vInverseNormal, 2.0f, m_iCollideAgainst);
		m_vTrajectory = Vector3.Reflect(m_vTrajectory, Hit.normal);
		m_vTrajectory.Normalize();

		if (m_bPlaySFXOnImpact) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX);
	}
}
