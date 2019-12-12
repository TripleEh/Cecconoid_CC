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

public class KillAfterSeconds : MonoBehaviour
{
	[SerializeField] private float m_fTTL_Seconds = 1.0f;
	private ulong m_iTimerHandle;

	void Start()
	{
		m_iTimerHandle = TimerManager.AddTimer(m_fTTL_Seconds, KillObject);
	}

	void OnDisable()
	{
		TimerManager.ClearTimerHandler(m_iTimerHandle, KillObject);
	}

	void OnDestroy()
	{
		TimerManager.ClearTimerHandler(m_iTimerHandle, KillObject);
	}

	public void KillObject()
	{
		Destroy(gameObject);
	}
}
