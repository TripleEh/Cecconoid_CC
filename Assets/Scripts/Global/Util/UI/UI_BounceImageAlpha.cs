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

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class UI_BounceImageAlpha : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fSine1Speed = 1.0f;
	[SerializeField] private float m_fSine2Speed = 0.5f;
	[SerializeField] private float m_fMaxAlpha = 0.17f;

	private UnityEngine.UI.Image m_gcImage = null;
	private Color m_vColour = Color.black;
	private float m_fAngle1 = 0;
	private float m_fAngle2 = 0;



	void Start()
	{
		m_gcImage = GetComponent<UnityEngine.UI.Image>();
		GAssert.Assert(null != m_gcImage, "Unable to get UI image component!");
		m_vColour = m_gcImage.color;
	}



	void Update()
	{
		m_fAngle1 += m_fSine1Speed * TimerManager.fUIDeltaTime;
		m_fAngle2 += m_fSine2Speed * TimerManager.fUIDeltaTime;

		m_vColour.a = ((Mathf.Sin(m_fAngle1) + Mathf.Sin(m_fAngle1)) / 2.0f) * m_fMaxAlpha; 

		m_gcImage.color = m_vColour;
	}
}
