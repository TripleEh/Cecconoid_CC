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

public class SimpleRotate : MonoBehaviour
{
	public float m_fRotateSpeed;
	private float m_fRotateAngle;

	void Update()
	{
		transform.localRotation *= Quaternion.AngleAxis(m_fRotateSpeed * TimerManager.fFixedDeltaTime, Vector3.forward);
	}
}
