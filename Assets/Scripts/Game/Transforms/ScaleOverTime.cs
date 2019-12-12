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
public class ScaleOverTime : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMaxScale = 45f;
	[SerializeField] private float m_fScaleDuration = 3f;
	[SerializeField] private EEasingType m_iType = EEasingType.Quintic;

	private Vector3 m_vScaleStart = Vector3.zero;
	private Vector3 m_vScaleEnd = Vector3.zero;
	private float m_fEventTime = 0;

	// Start is called before the first frame update
	void Start()
	{
		m_vScaleStart = transform.localScale;
		m_vScaleEnd = new Vector3(m_fMaxScale, m_fMaxScale, 1f);
		m_fEventTime = TimerManager.fGameTime;
	}

	// Update is called once per frame
	void Update()
	{
		float fRatio = (TimerManager.fGameTime - m_fEventTime) / m_fScaleDuration;
		transform.localScale = Vector3.Lerp(m_vScaleStart, m_vScaleEnd, Easing.EaseInOut(fRatio, m_iType));
	}
}
