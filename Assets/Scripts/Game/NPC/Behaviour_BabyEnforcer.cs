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


// Baby Enforcers will, by default, move toward the player with an offset
// This component handles how and when we overrice that behaviour
//
[RequireComponent(typeof(MoveTowardPlayerRigidBodyWithOffset))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BabyEnforcerGun))]
public class Behaviour_BabyEnforcer : EnemyObjectBase
{
	// This MUST be attached...
	protected MoveTowardPlayerRigidBodyWithOffset m_gcMovementComponent = null;
	// At some point, we will override the default movement and head to a corner...
	protected bool m_bThisComponentUpdatesMovement = false;
	// Speed set in the movement component
	protected float m_fMovementSpeed = 0.0f;
	// Trajectory to head into a corner...
	protected Vector3 m_vTrajectory = Vector3.zero;
	// Store for the player
	protected GameObject m_goPlayerObject = null;
	// If we're taking over control, store for the rigid body...
	protected Rigidbody2D m_gcRdgBdy = null;
	// Next Update Time...
	protected float m_fNextEventTime = 0;



	void Start()
	{
		// Validate
		{
			m_gcMovementComponent = GetComponent<MoveTowardPlayerRigidBodyWithOffset>();
			GAssert.Assert(null != m_gcMovementComponent, "Behaviour_BabyEnforcer: Unable to get movement component!");

			m_goPlayerObject = GameInstance.Object.GetPlayerObject();
			GAssert.Assert(null != m_goPlayerObject, "Unable to get Player's gameObject!");

			m_gcRdgBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRdgBdy, "Unable to get hold of RidigBody2D!");
		}

		OnRoomEnter();
	}



	public void OnDisable()
	{
		OnRoomExit();
	}

	

	public void OnDestroy()
	{
		OnRoomExit();
	}



	public override void OnRoomEnter()
	{
		base.OnRoomEnter();
		m_fNextEventTime = TimerManager.fGameTime + Random.Range(1f,2f);
	}



	// Reset is called during screen transition, which we should not persist beyond, so
	// destroy while we're hidden...
	//
	public override void OnPlayerRespawn()
	{
		Destroy(gameObject);
	}



	// When we revert to a corner and keep shooting, we just keep moving in a fixed 
	// direction forever...
	// 
	void FixedUpdate()
	{
		if(m_fNextEventTime < TimerManager.fGameTime) BehaviourUpdate();

		if (!m_bThisComponentUpdatesMovement) return;
		if (TimerManager.IsPaused()) return;

		m_gcRdgBdy.MovePosition( transform.position + ((m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
	}



	// Behaviour changes:
	// - Do a dice roll, are we going to fail and head into the corner shooting?
	// - If not, change offset to the player
	// - Unless we're too close, in which case pick a random position anywhere on screen
	public void BehaviourUpdate()
	{
		if(null == m_goPlayerObject) return;

		// Dice Roll
		{
			if (Random.Range(1f, 100f) > 90f)
			{
				m_fMovementSpeed = m_gcMovementComponent.GetMaxMovementSpeed();
				if (Random.Range(1f, 100f) > 50f) m_vTrajectory.x = 1.0f; else m_vTrajectory.x = -1.0f;
				if (Random.Range(1f, 100f) > 50f) m_vTrajectory.y = 1.0f; else m_vTrajectory.y = -1.0f;
				m_gcMovementComponent.enabled = false;
				m_bThisComponentUpdatesMovement = true;
				BabyEnforcerGun gc = GetComponent<BabyEnforcerGun>();
				GAssert.Assert(null != gc, "Baby Enforcer has no gun?");
				gc.SetIsAngry();
				return;
			}
		}


		// Too close to the player?
		{
			if ((m_goPlayerObject.transform.position - transform.position).magnitude < m_gcMovementComponent.GetFalloffDistance()*2)
			{
				m_gcMovementComponent.SetPixelOffset(new Vector3(Random.Range(-Types.s_fEugatronHalfRoomBoundsX, Types.s_fEugatronHalfRoomBoundsX), Random.Range(-Types.s_fEugatronHalfRoomBoundsY, Types.s_fEugatronHalfRoomBoundsY), 0.0f));
				m_fNextEventTime = TimerManager.fGameTime + Random.Range(2.0f, 5.0f);
				return;
			}
		}

		// Should we change the offset to the player?
		if (Random.Range(1f, 100f) > 20f)	m_gcMovementComponent.UpdatePixelOffset();

		// Next Update In...
		m_fNextEventTime = TimerManager.fGameTime + Random.Range(1.0f, 2.0f);
	}
}
