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
public class MoveTowardPlayerRigidBodyLimitedRotation : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMovementSpeed = 0.1f;
	[SerializeField] private float m_fRotationSpeed = 0.1f;

	private Rigidbody2D m_gcRgdBdy = null;
	private GameObject m_goPlayer = null;
	private PlayerState m_gcPlayerState = null;
	private Vector3 m_vTrajectory = Vector3.zero;

	void Awake()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();

		if (null != GameInstance.Object)
		{
			m_goPlayer = GameInstance.Object.GetPlayerObject();
			GAssert.Assert(null != m_goPlayer, "Unable to get player object from GameInstance");

			m_gcPlayerState = GameInstance.Object.GetPlayerState();
			GAssert.Assert(null != m_gcPlayerState, "Unable to get player state!");
		}
	}

	void Update()
	{
		if (null == m_goPlayer) return;
		if (TimerManager.IsPaused()) return;
		if (null == m_gcPlayerState || !m_gcPlayerState.PlayerIsAlive()) return;

		Vector3 vNewTraj = (m_goPlayer.transform.position - transform.position).normalized;
    float fAngle = Mathf.Atan2(transform.position.y - m_goPlayer.transform.position.y, transform.position.x - m_goPlayer.transform.position.x) * Mathf.Rad2Deg;
    Quaternion qAngle  = Quaternion.Euler (new Vector3(0.0f,0.0f,fAngle+90.0f));
    transform.rotation = Quaternion.Slerp (transform.rotation, qAngle, m_fRotationSpeed * TimerManager.fGameDeltaTime);
		transform.position += (transform.up * m_fMovementSpeed) * TimerManager.fGameDeltaTime;
	}
}
