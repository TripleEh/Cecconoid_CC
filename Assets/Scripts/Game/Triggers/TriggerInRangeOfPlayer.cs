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


[RequireComponent(typeof(SpriteRenderer))]
public class TriggerInRangeOfPlayer : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Trigger Settings")]
	[SerializeField] protected bool m_bRespectColliders = false;
	[SerializeField] protected float m_fTriggerDistance = 0.5f;
	[SerializeField] protected Types.EPlayerTriggerArea m_iTriggerZone = Types.EPlayerTriggerArea._ABOVE;
	[SerializeField] protected Material m_gcActiveMaterial = null;

	protected Material m_gcInactiveMaterial = null;
	protected GameObject m_goPlayerObject = null;
	protected bool m_bIsActive = false;
	protected bool m_bPlayerInRange = false;



	void Awake()
	{
		m_gcInactiveMaterial = GetComponent<SpriteRenderer>().material;
		GAssert.Assert(null != m_gcInactiveMaterial, "TriggerInRangeOfPlayer, unable to get the sprite renderer?");
	}



	virtual public void OnRoomEnter()
	{
		m_bIsActive = true;
		m_goPlayerObject = GameInstance.Object.GetPlayerObject();
	}



	virtual public void OnRoomExit()
	{
		m_bIsActive = false;
	}



	virtual public void OnPlayerHasDied()
	{
		m_bIsActive = false;
		GetComponent<SpriteRenderer>().material = m_gcInactiveMaterial;
	}



	virtual public void OnPlayerRespawn()
	{
		m_bIsActive = true;
	}



	virtual public void OnReset()
	{

	}



	protected void Update()
	{
		// Default to not in range
		m_bPlayerInRange = false;

		// Quick checks to see if we should bother...
		if (!m_bIsActive) return;
		if(null == m_goPlayerObject) return;
		if(TimerManager.IsPaused()) return;

		// Distance checks..
		Vector2 vPos = m_goPlayerObject.transform.position;
		float fHDist = Mathf.Abs(transform.position.x - vPos.x);
		//float fVDist = Mathf.Abs(transform.position.y - vPos.y);

		switch (m_iTriggerZone)
		{
			case Types.EPlayerTriggerArea._ABOVE: if (vPos.y >= transform.position.y && fHDist < m_fTriggerDistance) m_bPlayerInRange = true; break;
			case Types.EPlayerTriggerArea._BELOW: if (vPos.y <= transform.position.y && fHDist < m_fTriggerDistance) m_bPlayerInRange = true; break;
			case Types.EPlayerTriggerArea._ANY: if (fHDist < m_fTriggerDistance) m_bPlayerInRange = true; break;
		}
	}
}