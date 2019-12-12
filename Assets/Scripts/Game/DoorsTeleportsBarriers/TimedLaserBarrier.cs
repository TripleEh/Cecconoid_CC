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


// Pretty simple, uses the timer manager to ping-pong collision and effects
// so the barrier appears on/off in-game
//

public class TimedLaserBarrier : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Timer Settings")]
	[SerializeField] private float m_fTimeStartOffset = -1.0f;
	[SerializeField] private float m_fTimeOn = 1.0f;
	[SerializeField] private float m_fTimeOff = 1.0f;

	private ParticleSystem m_gcLaserParticles;
	private BoxCollider2D m_gcCollision;
	private AudioSource m_gcAudio;

	private ulong m_iTimerHandle;
	private bool m_bIsOn = false;
	private bool m_bIsWaiting = false;
	private bool m_bIsDeactivated = false;



	void Awake()
	{
		m_gcLaserParticles = GetComponentInChildren<ParticleSystem>();
		GAssert.Assert(null != m_gcLaserParticles, "TimedLaserBarrier: unable to find particle system!");
		m_gcCollision = GetComponent<BoxCollider2D>();
		GAssert.Assert(null != m_gcCollision, "TimedLaserBarrier: unable to find collision component!");
		m_gcAudio = GetComponent<AudioSource>();
		GAssert.Assert(null != m_gcAudio, "TimedLaserBarrier: Unable to find audio clip!");

		TurnOffInstant();
	}



	public void OnReset() { }
	public void OnPlayerHasDied() { if(!m_bIsDeactivated) ClearTimersTurnOffInstant(); }
	public void OnPlayerRespawn() { if(!m_bIsDeactivated) OnRoomEnter(); }



	public void OnRoomEnter()
	{
		if(m_fTimeStartOffset > 0.0f)
		{
			m_bIsWaiting = true;
			m_iTimerHandle = TimerManager.AddTimer(m_fTimeStartOffset, TurnOn);
		}
		else TurnOn();
	}



	public void OnRoomExit()
	{
		if(m_iTimerHandle == 0) return;
		ClearTimersTurnOffInstant();
	}



	public void ClearTimersTurnOffInstant()
	{
		if(m_bIsWaiting) TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOn);
		else if(m_bIsOn) TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOff);
		else TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOn);

		// Order is important, this function changes the IsOn bool :)
		TurnOffInstant();
	}



	public void TurnOn()
	{
		m_bIsOn = true;
		m_bIsWaiting = false;
		m_gcLaserParticles.Play();
		m_gcAudio.Play();
		m_gcCollision.enabled = true;
		m_iTimerHandle = TimerManager.AddTimer(m_fTimeOn, TurnOff);
	}



	public void TurnOffInstant(bool bDeactivate = false)
	{
		m_bIsOn = false;
		m_gcLaserParticles.Stop();
		m_gcAudio.Stop();
		m_gcCollision.enabled = false;
		m_bIsDeactivated = bDeactivate;
	}



	public void TurnOff()
	{
		TurnOffInstant();
		m_iTimerHandle = TimerManager.AddTimer(m_fTimeOff, TurnOn);
	}



	public void Deactivate()
	{
		m_bIsDeactivated = true;
	}
}
