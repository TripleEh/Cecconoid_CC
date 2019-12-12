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

//[RequireComponent(typeof(CanvasGroup))]
public class LerpCanvasGroupAlphaOverDuration : LerpSpriteAlphaOverDuration
{
	protected CanvasGroup m_gcCanvasGroup = null;

	protected override void Init()
	{
		m_gcCanvasGroup = GetComponent<CanvasGroup>();
		GAssert.Assert(null != m_gcCanvasGroup, "LerpCanvasGroupAlpha attached to a gameObjec that's missing a canvas group!" + gameObject.name);
		m_gcCanvasGroup.alpha = m_fStartAlpha;
		m_fEventTime = TimerManager.fGameTime;
	}
	// Update is called once per frame
	protected override void Update()
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
		switch (m_iEasingEnds)
		{
			case EEasingEnds._In: m_gcCanvasGroup.alpha = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseIn(fRatio, m_iEasingType)); break;
			case EEasingEnds._InOut: m_gcCanvasGroup.alpha = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseInOut(fRatio, m_iEasingType)); break;
			case EEasingEnds._Out: m_gcCanvasGroup.alpha = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseOut(fRatio, m_iEasingType)); break;
		}

		if (fRatio >= 1.0f) Destroy(this);
	}
}
