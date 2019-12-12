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

[RequireComponent(typeof(Rigidbody2D))]
public class CircularMovementWithSinusoidalSpeed : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] protected float m_fOrbitRotationSpeed = 0.1f;
	[SerializeField] protected float m_fMinMovementSpeed = -1.0f;
	[SerializeField] protected float m_fMaxMovementSpeed = 2.0f;
	[SerializeField] protected float m_fSpeedModifierInc = 0.1f;


	protected float m_fMovementSpeed = 0.5f;
	protected float m_fSpeedModAngle = 0.0f;
	protected float m_fOrbitAngle = 0.0f;

	protected Vector3 m_vTrajectory = Vector3.zero;

	protected Rigidbody2D m_gcRgdBdy = null;

	protected PlayerState m_gcPlayerState;

	public virtual void Start()
	{
		// Validate
		{
			m_gcRgdBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRgdBdy, "OrbitRoomOrigin: setup with no rigidbody...");
			m_gcPlayerState = GameInstance.Object.GetPlayerState();
			GAssert.Assert(null != m_gcPlayerState, "Unable to get Game Instance");
		}

		// Randomise the angle offsets
		m_fOrbitAngle = Random.Range(0f, 359f) * Mathf.Deg2Rad;
		m_fSpeedModAngle = Random.Range(0f, 359f) * Mathf.Deg2Rad;

		// Generate the initial trajectory
		m_vTrajectory = new Vector3(Mathf.Sin(m_fOrbitAngle), Mathf.Cos(m_fOrbitAngle), 0.0f);
	}

	// Update is called once per frame
	public virtual void FixedUpdate()
	{
		// Skip if we're paused
		if (TimerManager.IsPaused()) return;

		// Don't move if the player is dead...
		if (!m_gcPlayerState.PlayerIsAlive()) return;

		float fDelta = TimerManager.fFixedDeltaTime;

		// Update the rotational angle
		m_fOrbitAngle += m_fOrbitRotationSpeed * fDelta;

		// Update the movement speed angle
		m_fSpeedModAngle += m_fSpeedModifierInc * fDelta;

		// Generate the movement params...
		m_fMovementSpeed = Mathf.Clamp(Mathf.Sin(m_fSpeedModAngle) * m_fMaxMovementSpeed, m_fMinMovementSpeed, m_fMaxMovementSpeed);
		m_vTrajectory = new Vector3(Mathf.Sin(m_fOrbitAngle), Mathf.Cos(m_fOrbitAngle), 0.0f);

		// Do the movement
		Vector3 vNewPos = transform.position + ((m_vTrajectory * m_fMovementSpeed) * fDelta);
		m_gcRgdBdy.MovePosition(vNewPos);


		Debug.DrawLine(transform.position, transform.position + m_vTrajectory * m_fMovementSpeed, Color.white);
	}
}
