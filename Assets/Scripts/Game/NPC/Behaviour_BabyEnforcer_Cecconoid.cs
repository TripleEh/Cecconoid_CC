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

// Variation of the Baby Enforcer that works in Cecconoid//

public class Behaviour_BabyEnforcer_Cecconoid : Behaviour_BabyEnforcer
{
	protected PlayerState m_gcPlayerState = null;


	void Start()
	{
		m_vInitPosition = transform.position;
	}




	
	public override void OnRoomEnter()
	{
		base.OnRoomEnter();


		// Validate
		{
			m_gcMovementComponent = GetComponent<MoveTowardPlayerRigidBodyWithOffset>();
			GAssert.Assert(null != m_gcMovementComponent, "Behaviour_BabyEnforcer: Unable to get movement component!");

			m_goPlayerObject = GameInstance.Object.GetPlayerObject();
			GAssert.Assert(null != m_goPlayerObject, "Unable to get Player's gameObject!");

			m_gcRdgBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRdgBdy, "Unable to get hold of RidigBody2D!");

			m_gcPlayerState = GameInstance.Object.GetPlayerState();
			GAssert.Assert(null != m_gcPlayerState, "Unable to get player state!");
		}

		transform.position = m_vInitPosition;
		m_fNextEventTime = TimerManager.fGameTime + Random.Range(1f, 2f);
	}



	public override void OnPlayerRespawn()
	{
		m_fNextEventTime = TimerManager.fGameTime + Random.Range(1f, 2f);
		m_bBehaviourCanUpdate = true;
		m_gcRenderer.enabled = true;
	}



	public override void OnPlayerHasDied()
	{
		m_bBehaviourCanUpdate = false;
	}



	void FixedUpdate()
	{
		if (!m_bThisComponentUpdatesMovement) return;
		if (TimerManager.IsPaused()) return;
		if (!m_gcPlayerState.PlayerIsAlive()) return;

		if (m_fNextEventTime < TimerManager.fGameTime) BehaviourUpdate();

		Vector3 vWantedPos = transform.position + ((m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime);
		vWantedPos = MathUtil.GetBoundsCheckedVector(vWantedPos, 0.08f);
		m_gcRdgBdy.MovePosition(vWantedPos);
	}
}
