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
public class MoveTowardPlayerRigidBodyWithOffset : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] protected float m_fMaxMovementSpeed = 1.0f;
	[SerializeField] protected float m_fMinMovementSpeed = 0.1f;
	[SerializeField] protected float m_fMovementSpeedFallOffDistance = 0.5f;

	protected GameObject m_goPlayer;
	protected Rigidbody2D m_gcRgdBdy = null;
	protected GameInstance m_gcGameInstance = null;
	protected Vector3 m_vTrajectory = Vector3.zero;
	protected Vector3 m_vOffset = Vector3.zero;
	protected float m_fMovementSpeed = 0.0f;

	public virtual void Start()
	{
		// Validate 
		{
			m_gcRgdBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRgdBdy, "Unable to find Rigid Body!");

			m_gcGameInstance = GameInstance.Object;
			GAssert.Assert(null != m_gcGameInstance, "Unable to setup game instance!");

			m_goPlayer = m_gcGameInstance.GetPlayerObject();
			GAssert.Assert(null != m_goPlayer, "Unable to get player object");
		}

		UpdatePixelOffset();
	}

	public void UpdatePixelOffset()
	{
		m_vOffset.x = Random.Range(-m_fMovementSpeedFallOffDistance*3, m_fMovementSpeedFallOffDistance*3);
		m_vOffset.y = Random.Range(-m_fMovementSpeedFallOffDistance*3, m_fMovementSpeedFallOffDistance*3);
	}

	public void SetPixelOffset(Vector3 vOffset)
	{
		m_vOffset = vOffset;
	}

	public float GetMaxMovementSpeed()
	{
		return m_fMaxMovementSpeed;
	}

	public float GetFalloffDistance()
	{
		return m_fMovementSpeedFallOffDistance;
	}

	virtual protected void FixedUpdate()
	{
		if (null == m_goPlayer) return;
		if (TimerManager.IsPaused()) return;
		if (!GameInstance.Object.GetPlayerState().PlayerIsAlive()) return;

		// Get the Direction vector, offset and actual
		Vector3 vDir = (m_goPlayer.transform.position + m_vOffset) - transform.position;
		Vector3 vDist = m_goPlayer.transform.position - transform.position;

		// Get Player's trajectory and scale it randomly so we have some noise
		Vector2 vPlayerTraj = m_gcGameInstance.GetPlayerTrajectory();
		Vector3 vTraj = new Vector3(vPlayerTraj.x, vPlayerTraj.y, 0f) * Random.Range(0f, 1f);

		// Calc the movement speed (Inversely proportional to distance from the player)
		m_fMovementSpeed = Mathf.Abs(vDist.magnitude / m_fMovementSpeedFallOffDistance) * m_fMinMovementSpeed;
		m_fMovementSpeed = Mathf.Clamp(m_fMovementSpeed, m_fMinMovementSpeed, m_fMaxMovementSpeed);

		// Calc the final trajectory
		m_vTrajectory = ((vDir + vTraj) * m_fMovementSpeed) * TimerManager.fFixedDeltaTime;

		// Move!
		{
			Vector3 vWantedPos;
			if (GameInstance.Object.m_bIsCecconoid)
				vWantedPos = MathUtil.GetBoundsCheckedVector(new Vector2(transform.position.x + m_vTrajectory.x, transform.position.y + m_vTrajectory.y), 0.08f);
			else
				vWantedPos = new Vector2(transform.position.x + m_vTrajectory.x, transform.position.y + m_vTrajectory.y);
			m_gcRgdBdy.MovePosition(vWantedPos);
		}
	}
}
