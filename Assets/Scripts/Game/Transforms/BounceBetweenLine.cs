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

public class BounceBetweenLine : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] private Line m_LineComponent = null;
	[SerializeField] private float m_fDuration = 1.0f;
	[SerializeField] private EEasingType m_iEasing = EEasingType.Quartic;
	[Range(0,1)]
	[SerializeField] private float m_fTOffset = 0.0f;

	private float m_fOffset = 0;
	private float m_fEventTime;
	private Vector3 v1, v2;
	private bool m_bIsActive = false;



	public void OnRoomEnter()
	{
		GAssert.Assert(null != m_LineComponent, "BounceBetweenLine setup without a Line component!");
		v1 = m_LineComponent.GetStart();
		v2 = m_LineComponent.GetEnd();
		m_fEventTime = TimerManager.fGameTime;
		m_fOffset = m_fTOffset;
		m_bIsActive = true;
	}



	public void OnRoomExit()
	{
		m_bIsActive = false;
	}



	public void OnPlayerHasDied()
	{
		m_bIsActive = false;
	}



	public void OnPlayerRespawn()
	{
		m_bIsActive = true; ;
	}



	public void OnReset()
	{
		m_fOffset = m_fTOffset;
		m_fEventTime = TimerManager.fGameTime;
	}


	
	void Update()
	{
		if(!m_bIsActive) return;
		
		float fRatio = ((TimerManager.fGameTime - m_fEventTime) / m_fDuration) + m_fOffset;
		transform.position = Vector3.Lerp(v1, v2, Easing.EaseInOut(fRatio, m_iEasing));
		if (fRatio >= 1.0f)
		{
			m_fOffset = 0;
			Vector3 vTemp = v1;
			v1 = v2;
			v2 = vTemp;
			m_fEventTime = TimerManager.fGameTime;
		}
	}
}