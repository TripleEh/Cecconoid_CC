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

public class LerpToPositionOverDuration : MonoBehaviour
{
	[SerializeField] private float m_fSecondsDelayBeforeStarting = 0.0f;
	[SerializeField] private Vector3 m_vFinalPosition = Vector3.zero;
	[SerializeField] private float m_fMovementDurationSeconds = 1.5f;
	[SerializeField] private EEasingType m_iEasingType = EEasingType.Quartic;
	[SerializeField] private EEasingEnds m_iEasingEnds = EEasingEnds._InOut;

	private Vector3 m_vStartPos;
	private float m_fEventTime;
	private bool m_bIsUpdating = true;

  private bool m_bProceduralSetup  = false;


  void Start()
	{
		// Don't run Start if we were setup the frame of creation
		if(m_bProceduralSetup) return;

    m_fEventTime = TimerManager.fGameTime;

		// Optionally setup for a pause in processing
		if (m_fSecondsDelayBeforeStarting > 0.0f)
			m_bIsUpdating = false;
		else
			m_vStartPos = transform.position;
	}


	// If this is being setup by a controller outside of the class, then it's probably going to be called
	// before Start/Awake -- ie: immedaitely on instantiation. 
	//
	public void SetupParams(Vector3 vStartPos, Vector3 vOffset, float fPauseAmount)
	{
    m_bProceduralSetup = true;
    m_fSecondsDelayBeforeStarting = fPauseAmount;
    m_vFinalPosition = vOffset;
    m_vStartPos = vStartPos;

    m_fEventTime = TimerManager.fGameTime;
    if (m_fSecondsDelayBeforeStarting > 0.0f) m_bIsUpdating = false;
  }


	void Update()
	{
		// If there's a pause defined, wait before doing the actual lerp...
		if (!m_bIsUpdating)
		{
			if (TimerManager.fGameTime - m_fEventTime > m_fSecondsDelayBeforeStarting)
			{
				m_bIsUpdating = true;
				m_vStartPos = transform.position;
				m_fEventTime = TimerManager.fGameTime;
			}
			else return;
		}

		float fRatio = (TimerManager.fGameTime - m_fEventTime) / m_fMovementDurationSeconds;

		switch (m_iEasingEnds)
		{
			case EEasingEnds._In: transform.position = Vector3.Lerp(m_vStartPos, m_vFinalPosition, Easing.EaseIn(fRatio, m_iEasingType)); break;
			case EEasingEnds._InOut: transform.position = Vector3.Lerp(m_vStartPos, m_vFinalPosition, Easing.EaseInOut(fRatio, m_iEasingType)); break;
			case EEasingEnds._Out: transform.position = Vector3.Lerp(m_vStartPos, m_vFinalPosition, Easing.EaseOut(fRatio, m_iEasingType)); break;
		}

		if (fRatio >= 1.0f) Destroy(this);
	}
}
