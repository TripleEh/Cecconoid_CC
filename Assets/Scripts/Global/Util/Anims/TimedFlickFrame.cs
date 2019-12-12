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



// Using Unity's Animation system is vast overkill for certain flick book anims. 
// This class handles a 2 frame animation, that's timed, for use on things 
// like guns.
//
[RequireComponent(typeof(SpriteRenderer))]
public class TimedFlickFrame : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Timing")]
	[SerializeField] private float m_fInitialDelay = Types.s_fTTL_RoomEntryActivityDelay;
	[SerializeField] private float m_fFlickFrameDelay = 1.0f;
	[SerializeField] private float m_fFlickDuration = 0.16f;
	
	[Header("Anim Frames")]
	[SerializeField] private Sprite m_gcFrame1= null;
	[SerializeField] private Sprite m_gcFrame2 = null;

	private bool m_bIsAnimating = false;
	private ulong m_iTimerHandle = 0;
	private SpriteRenderer m_gcRenderer = null;
	private Types.EFlickFrameState m_iState = Types.EFlickFrameState._FLICK;

	// Class shouldn't be active until the object it's attached to is
	// Use the same Enter/Exit pattern to ensure any timings we need will match. 
	//
	public void OnRoomEnter()
	{
		m_gcRenderer = GetComponent<SpriteRenderer>();
		GAssert.Assert(null != m_gcRenderer, "No Sprite Renderer present on this GameObject: " + gameObject.name	);
		GAssert.Assert(null != m_gcFrame1, "Component missing animation frames!");
		GAssert.Assert(null != m_gcFrame2, "Component missing animation frames!");
		
		m_bIsAnimating = false;
		m_iState = Types.EFlickFrameState._FLICK;
		SetCanAnimate(true, m_fInitialDelay);
	}



	public void OnRoomExit()
	{
		SetCanAnimate(false);
	}



	public void OnPlayerHasDied()
	{
		SetCanAnimate(false);
	}



	public void OnPlayerRespawn()
	{
		SetCanAnimate(true);
	}



	public void OnReset()
	{

	}



	private void OnDestroy()
	{
		m_bIsAnimating = false;
		m_iState = Types.EFlickFrameState._FLICK;
		SetCanAnimate(false);
	}



	private void OnDisable()
	{
		m_bIsAnimating = false;
		m_iState = Types.EFlickFrameState._FLICK;
		SetCanAnimate(false);
	}



	// If we're already animating, clear existing timers, otherwise create a new one
	//
	protected void SetCanAnimate(bool bState, float fDelayStart = -1.0f)
	{
		if (bState)
		{
			if (!m_bIsAnimating)
			{
				m_bIsAnimating = true;
				if(fDelayStart > 0.0f) m_iTimerHandle = TimerManager.AddTimer(fDelayStart, FlickAnim);
				else FlickAnim();
			}
		}
		else
		{
			TimerManager.ClearTimerHandler(m_iTimerHandle, FlickAnim);
			m_bIsAnimating = false;
		}
	}



	// Alternate between two frames
	//
	protected void FlickAnim()
	{
		if (!m_bIsAnimating) return;
		
		switch (m_iState)
		{
			case Types.EFlickFrameState._FLICK: {
				m_gcRenderer.sprite = m_gcFrame2;
				m_iTimerHandle = TimerManager.AddTimer(m_fFlickDuration, FlickAnim);
				m_iState = Types.EFlickFrameState._REVERT;
			} break;
			
			case Types.EFlickFrameState._REVERT: {
				m_gcRenderer.sprite = m_gcFrame1;
				m_iTimerHandle = TimerManager.AddTimer(m_fFlickFrameDelay - m_fFlickDuration, FlickAnim);
				m_iState = Types.EFlickFrameState._FLICK;
			} break;
		}
	}
}


