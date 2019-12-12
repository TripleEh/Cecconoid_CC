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
public class Anim_FlickFrameOnRequest : MonoBehaviour
{
	[Header("Timing")]
	[SerializeField] private float m_fFlickDuration = 0.16f;

	[Header("Anim Frames")]
	[SerializeField] private Sprite m_gcFrame1 = null;
	[SerializeField] private Sprite m_gcFrame2 = null;

	private ulong m_iTimerHandle = 0;
	private SpriteRenderer m_gcRenderer = null;
	private Types.EFlickFrameState m_iState = Types.EFlickFrameState._FLICK;


	public void Awake()
	{
		m_gcRenderer = GetComponent<SpriteRenderer>();
		GAssert.Assert(null != m_gcRenderer, "No Sprite Renderer present on this GameObject: " + gameObject.name);
		GAssert.Assert(null != m_gcFrame1, "Component missing animation frames!");
		GAssert.Assert(null != m_gcFrame2, "Component missing animation frames!");

		m_iState = Types.EFlickFrameState._FLICK;
	}


	private void OnDisable()
	{
		if(m_iState == Types.EFlickFrameState._REVERT) TimerManager.ClearTimerHandler(m_iTimerHandle, FlickAnim);
		m_iState = Types.EFlickFrameState._FLICK;
	}



	// Alternate between two frames
	// There's an obvious assumption here that the timer will expire before
	// the controlling component triggers this again...
	//
	public void FlickAnim()
	{
		switch (m_iState)
		{
			case Types.EFlickFrameState._FLICK:
				{
					m_gcRenderer.sprite = m_gcFrame2;
					m_iTimerHandle = TimerManager.AddTimer(m_fFlickDuration, FlickAnim);
					m_iState = Types.EFlickFrameState._REVERT;
				}
				break;

			case Types.EFlickFrameState._REVERT:
				{
					m_gcRenderer.sprite = m_gcFrame1;
					m_iState = Types.EFlickFrameState._FLICK;
				}
				break;
		}
	}
}

