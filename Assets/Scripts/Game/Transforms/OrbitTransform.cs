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

public class OrbitTransform : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fDistanceFromTransform = Types.s_fPixelSize * 48.0f;
	[SerializeField] private float m_fOrbitSpeed = 10;
	[SerializeField] private float m_fZLayer = Types.s_fPOS_PlayerOptionLayerZ;
	private float m_fAngle;
	private float m_fAngleMod = 10f;

	void Start()
	{
		m_fAngleMod = m_fOrbitSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 vPos = new Vector3(Mathf.Sin(m_fAngle) * m_fDistanceFromTransform, Mathf.Cos(m_fAngle) * m_fDistanceFromTransform, m_fZLayer);
		transform.localPosition = vPos;

		if (GameInstance.Object.GetPlayerController().GetMovementTrajectory().x > 0f) m_fAngleMod = m_fOrbitSpeed;
		else if (GameInstance.Object.GetPlayerController().GetMovementTrajectory().x < 0f) m_fAngleMod = -m_fOrbitSpeed;

		m_fAngle += m_fAngleMod * TimerManager.fGameDeltaTime;
	}
}
