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

public class BounceOnBezierCurve : MonoBehaviour, Types.IRoom_EnemyObject
{
	[SerializeField] private BezierCurve m_BezierCurveComponent = null;
	[SerializeField] private float m_fDuration = 1.0f;
	[SerializeField] private EEasingType m_iEasing = EEasingType.Quartic;

	private float m_fEventTime;
	private bool m_bIsActive = false;
	private bool m_bIsForward = true;

	public void OnRoomEnter()
	{
		GAssert.Assert(null != m_BezierCurveComponent, "BounceOnBezierCurve setup without a Bezier component!");
		m_fEventTime = TimerManager.fGameTime;
		m_bIsActive = true;
	}


	public void OnRoomExit()
	{
		m_bIsActive = false;
	}


	public void OnPlayerHasDied() { m_bIsActive = false; }
	public void OnPlayerRespawn() { m_bIsActive = true; }
	public void OnReset() { OnRoomEnter(); }
	

	void Update()
	{
		if (!m_bIsActive) return;

		// Where should we be, in time, along the path?
		float fRatio = (TimerManager.fGameTime - m_fEventTime) / m_fDuration;
		float fDir;
		if (!m_bIsForward) fDir = 1.0f - fRatio;
		else fDir = fRatio;
	
		// If we've gone off the end, check now and flip for the next update 
		if (fRatio >= 1.0f)
		{
			m_fEventTime = TimerManager.fGameTime;
			m_bIsForward = !m_bIsForward;
		}

		// Set the transform of this object to where we should be...
		transform.position = m_BezierCurveComponent.GetPoint(Easing.EaseInOut(fDir, m_iEasing));
	}
}