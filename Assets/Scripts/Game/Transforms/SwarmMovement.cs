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
public class SwarmMovement : MonoBehaviour
{
	// Reference to the controller.
	[Header("Set by Swarm Controller")]
	public SwarmController m_gcSwarmController;
	public Quaternion m_qTrajectory;

	private float m_fPerlinSeed;
	private Rigidbody2D m_gcRgdBdy;

	// Caluculates the separation vector with a target.
	Vector3 GetSeparationVector(Transform target)
	{
		Vector3 vDiff = transform.position - target.transform.position;
		float fDiffLength = vDiff.magnitude;
		float fScale = Mathf.Clamp01(1.0f - fDiffLength / m_gcSwarmController.m_fAgentDistance);
		return vDiff * (fScale / fDiffLength);
	}

	void Start()
	{
		m_fPerlinSeed = Random.value * 10.0f;
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GAssert.Assert(null != m_gcRgdBdy, "Boid prefab setup without a Rigid Body!");
	}

	void Update()
	{
		if(TimerManager.IsPaused()) return;

		Vector3 vCurrPos = transform.position;
		Quaternion qCurrRot = m_qTrajectory;

		// Current velocity randomized with noise.
		float fVelScale = m_gcSwarmController.m_fAgentVelocity * Mathf.PerlinNoise(TimerManager.fGameTime, m_fPerlinSeed) * 2.0f;

		// Initializes the vectors.
		Vector3 vSep = Vector3.zero;
		Vector3 vAlign = m_gcSwarmController.transform.forward;
		Vector3 vCoh = m_gcSwarmController.transform.position;

		// Boid Count cannnot go below one!
		float fBoidCount = 1;

		// Accumulate from all nearby boids within distance to us...
		for (int i = 0; i < m_gcSwarmController.m_aSwarm.Length; ++i)
		{
			// Ignore self!
			if (m_gcSwarmController.m_aSwarm[i] == gameObject) continue;

			var go = m_gcSwarmController.m_aSwarm[i];
			if (null != go && (go.transform.position - transform.position).magnitude < m_gcSwarmController.m_fAgentDistance)
			{
				var t = m_gcSwarmController.m_aSwarm[i].transform;
				vSep += GetSeparationVector(t);
				vAlign += go.GetComponent<SwarmMovement>().m_qTrajectory * Vector3.forward;
				vCoh += t.position;
				++fBoidCount;
			}
		}

		// Calc the average to scalle the accumulated vectors by. fBoidCount must be 1.0f or more!
		float fAvgScaler = 1.0f / fBoidCount;
		vAlign *= fAvgScaler;
		vCoh *= fAvgScaler;
		vCoh = (vCoh - vCurrPos).normalized;

		// Calculate the wanted rotation from the vectors.
		Vector3 vTrajectory = vSep + vAlign * 0.3f + vCoh;
		Quaternion qRot = Quaternion.FromToRotation(Vector3.forward, vTrajectory.normalized);

		// Interpolate between current trajector and new trajectory...
		if (qRot != qCurrRot)
		{
			var ip = Mathf.Exp(-m_gcSwarmController.m_fAgentRotationSpeed * TimerManager.fGameDeltaTime);
			m_qTrajectory = Quaternion.Slerp(qRot, qCurrRot, ip);
		}

		// Move
		{
			Vector3 vNewPos = vCurrPos + (m_qTrajectory * Vector3.forward) * (fVelScale * TimerManager.fFixedDeltaTime);
			vNewPos.z = 0.0f;
			m_gcRgdBdy.MovePosition(vNewPos);
		}
	}
}
