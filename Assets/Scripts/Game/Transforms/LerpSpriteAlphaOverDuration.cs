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

//[RequireComponent(typeof(SpriteRenderer))]
public class LerpSpriteAlphaOverDuration : MonoBehaviour
{
	[SerializeField] protected float m_fSecondsDelayBeforeStarting = 0.0f;
	[SerializeField] protected float m_fStartAlpha = 0.0f;
	[SerializeField] protected float m_fEndAlpha = 1.0f;
	[SerializeField] protected float m_fDurationInSeconds = 1.5f;
	[SerializeField] protected EEasingType m_iEasingType = EEasingType.Quartic;
	[SerializeField] protected EEasingEnds m_iEasingEnds = EEasingEnds._InOut;

	protected SpriteRenderer m_gcRenderer = null;
	protected Color m_vColour;
	protected float m_fEventTime;
	protected bool m_bIsUpdating = true;



	protected virtual void Init()
	{
		// Set the initial alpha value
		m_gcRenderer = GetComponent<SpriteRenderer>();
		GAssert.Assert(null != m_gcRenderer, "LerpSpriteAlphaOverDuration attached to a gameobject that's not got a sprite renderer:" + gameObject.name);
		m_vColour = m_gcRenderer.color;
		m_vColour.a = m_fStartAlpha;
		m_gcRenderer.color = m_vColour;
		m_fEventTime = TimerManager.fGameTime;
	}



	protected virtual void Start()
	{
		m_fEventTime = TimerManager.fGameTime;

		// Optionally setup for a pause in processing
		if (m_fSecondsDelayBeforeStarting > 0.0f)	m_bIsUpdating = false;
		else Init();	
	}



	protected virtual void Update()
	{
		// If there's a pause defined, wait before doing the actual lerp...
		if (!m_bIsUpdating)
		{
			if (TimerManager.fGameTime - m_fEventTime > m_fSecondsDelayBeforeStarting)
			{
				m_bIsUpdating = true;
				Init();
			}
			else return;
		}


		float fRatio = (TimerManager.fGameTime - m_fEventTime) / m_fDurationInSeconds;
		switch(m_iEasingEnds)
		{
			case EEasingEnds._In: 		m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseIn(fRatio, m_iEasingType)); break;
			case EEasingEnds._InOut:	m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseInOut(fRatio, m_iEasingType)); break;
			case EEasingEnds._Out: 		m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseOut(fRatio, m_iEasingType)); break;
		}

		m_gcRenderer.color = m_vColour;

		if(fRatio >= 1.0f) Destroy(this); 
	}
}
