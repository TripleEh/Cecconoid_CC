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

public class SineBounce : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]
	[SerializeField] private float m_fMoveAmount = Types.s_fPixelSize * 24f;
	[SerializeField] private float m_fSineSpeed = 1.0f;
	[SerializeField] private Vector3 m_vMovementVector = new Vector3(0f, 1f, 0f);

	private Vector3 m_vOrigin;
	private float m_fAngle;
	private bool m_bIsActive = false;

	public void OnPlayerRespawn() {}
	public void OnPlayerHasDied() {}


	public void OnRoomEnter()	{
		m_bIsActive = true;
	}



	public void OnRoomExit() {
		m_bIsActive = false;
	}



	public void OnReset() { }



	void Start() {
		m_vMovementVector = m_vMovementVector.normalized;
		m_vOrigin = transform.position;
	}



	// Update is called once per frame
	void Update()
	{
		if(m_bIsActive)
		{
			transform.position = m_vOrigin + (m_vMovementVector * Mathf.Sin(m_fAngle)) * m_fMoveAmount;
			m_fAngle += m_fSineSpeed * TimerManager.fGameDeltaTime;
		}
	}
}
