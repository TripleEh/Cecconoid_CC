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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This missile calcs its trajectory on initialisation, and from that 
// point on, moves forward at a set speed, bouncing off the Eugatron Room Bounds
//

[RequireComponent(typeof(Rigidbody2D))]
public class MissileMoveForwardBounceOffEugatronWalls : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMovementSpeed = 0.95f;
	[SerializeField] private int m_iKillAfterNumBounces = 4;

	private Vector3 m_vTraj;
	private Rigidbody2D m_gcRgdBdy = null;
	private int m_iBounceCount = 0;
	private Vector2 m_vRoomBounds;

	// Calculate the initial trajectory...
	//
	private void Start()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		m_vTraj = GameInstance.Object.GetPlayerObject().transform.position - transform.position;
		m_vTraj.Normalize();

		m_vRoomBounds = new Vector2(Types.s_fEugatronRoomBoundsX, Types.s_fEugatronRoomCollisionBoundsY);
		m_vRoomBounds.x += 0.1f;
		m_vRoomBounds.y -= 0.075f;

		m_iBounceCount = 0;
	}


	// Move the object
	//
	private void FixedUpdate()
	{
		if (TimerManager.IsPaused()) return;
		if (!GameMode.PlayerIsAlive()) return;

		

		// Check for room bounds
		if (transform.position.x >= m_vRoomBounds.x || transform.position.x <= -m_vRoomBounds.x)
		{
			m_vTraj.x *= -1;
			++m_iBounceCount;
		}

		if (transform.position.y+0.06f >= m_vRoomBounds.y || transform.position.y+0.06f <= -m_vRoomBounds.y)
		{
			m_vTraj.y *= -1;
			++m_iBounceCount;
		}

		if(m_iBounceCount >= m_iKillAfterNumBounces)
		{
			TrackHitPoints gc = GetComponent<TrackHitPoints>();
			GAssert.Assert(null != gc, "DestroyOnImpact, no TrackHitPoints component on this gameObject!");
			gc.KillInstant();
			this.enabled = false;
		}
		else m_gcRgdBdy.MovePosition(transform.position + ((m_vTraj * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
	}
}
