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
public class LerpSpriteAlphaOverDuration_EngineTime : LerpSpriteAlphaOverDuration
{
	protected override void Start()
	{
		base.Start();
		m_fEventTime = Time.time;
	}



	protected override void Update()
	{
		// If there's a pause defined, wait before doing the actual lerp...
		if (!m_bIsUpdating)
		{
			if (Time.time - m_fEventTime > m_fSecondsDelayBeforeStarting)
			{
				m_bIsUpdating = true;
				Init();
			}
			else
				return;
		}


		float fRatio = (Time.time - m_fEventTime) / m_fDurationInSeconds;
		switch (m_iEasingEnds)
		{
			case EEasingEnds._In: m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseIn(fRatio, m_iEasingType)); break;
			case EEasingEnds._InOut: m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseInOut(fRatio, m_iEasingType)); break;
			case EEasingEnds._Out: m_vColour.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, Easing.EaseOut(fRatio, m_iEasingType)); break;
		}

		m_gcRenderer.color = m_vColour;

		if (fRatio >= 1.0f) Destroy(this);
	}
}
