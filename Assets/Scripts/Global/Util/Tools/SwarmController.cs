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

public class SwarmController : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Swarm Properties")]
	[SerializeField] private GameObject m_goAgentPrefab = null;
	[SerializeField] private float m_fSpawnCount = 9.0f;
	[SerializeField] private float m_fSpawnRadius = 0.4f;

	public float m_fAgentVelocity;
	public float m_fAgentRotationSpeed;
	public float m_fAgentDistance;
	public GameObject[] m_aSwarm;

	[Header("Controller Behaviour")]
	[SerializeField] private bool m_bPulseAgentDistance = false;
	[SerializeField] private float m_fDistancePulseSize = 0.15f;
	[SerializeField] private float m_fDistancePulseSpeed = 5.0f;
	[SerializeField] private bool m_bPulseAgentVelocity = false;
	[SerializeField] private float m_fVelocityPulseSize = 0.2f;
	[SerializeField] private float m_fVelocityPulseSpeed = 8.0f;

	private bool m_bIsActive = false;
	private float m_fDistancePulseOrigin;
	private float m_fVelocityPulseOrigin;



	void Awake()
	{
		m_fDistancePulseOrigin = m_fAgentDistance;
		m_fVelocityPulseOrigin = m_fAgentVelocity;
	}



	public void OnRoomEnter()
	{
		m_aSwarm = new GameObject[(int)m_fSpawnCount];
		

		for (int i = 0; i < m_fSpawnCount; ++i)
		{
			if (null == m_goAgentPrefab) Debug.LogError("Unable to spawn, no prefab!");

			Vector3 vPos = transform.position + Random.insideUnitSphere * m_fSpawnRadius;

			GameObject go = Instantiate(m_goAgentPrefab, vPos, Quaternion.identity);
			go.GetComponent<SwarmMovement>().m_gcSwarmController = this;
			go.GetComponent<SwarmMovement>().m_qTrajectory = Quaternion.Slerp(transform.rotation, Random.rotation, 0.8f);
			m_aSwarm[i] = go;
		}

		m_bIsActive = true;
	}



	public void OnRoomExit()
	{
		for (int i = 0; i < m_fSpawnCount; ++i)
			if (null != m_aSwarm[i]) Destroy(m_aSwarm[i]);

		m_bIsActive = false;
	}



	public void OnPlayerHasDied()
	{
		m_bIsActive = false;
	}



	public void OnPlayerRespawn()
	{
		m_bIsActive = true;
	}



	public void OnReset()
	{

	}



	public uint GetBlobCount()
	{
		uint iRet = 0;
		foreach (GameObject go in m_aSwarm)
			if (null != go) ++iRet;

		return iRet;
	}



	public void Update()
	{
		if (!m_bIsActive) return;
		if (m_bPulseAgentDistance) m_fAgentDistance = m_fDistancePulseOrigin + Mathf.Sin(TimerManager.fGameTime * m_fDistancePulseSpeed) * m_fDistancePulseSize;
		if (m_bPulseAgentVelocity) m_fAgentVelocity = m_fVelocityPulseOrigin + Mathf.Sin(TimerManager.fGameTime * m_fVelocityPulseSpeed) * m_fVelocityPulseSize;
	}
}
