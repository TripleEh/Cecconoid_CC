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
public class MoveAlongVectorRigidBody : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private Vector2 m_vTrajectory = Vector2.zero;
	[SerializeField] private float m_fDistancePerSecond = 1;

	private Rigidbody2D m_gcRgdBdy = null;



	void Start()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GAssert.Assert(null != m_gcRgdBdy, "Unable to get RigidBody");
	}



	void FixedUpdate()
	{
		m_gcRgdBdy.MovePosition(transform.position + new Vector3(m_vTrajectory.x * m_fDistancePerSecond, m_vTrajectory.y * m_fDistancePerSecond, 0f) * TimerManager.fFixedDeltaTime);
	}
}
