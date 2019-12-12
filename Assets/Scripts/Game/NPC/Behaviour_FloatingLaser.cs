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

// These are basically collision rectangles that float in the
// arena and turn "on" and "off" on a timer. Collision will 
// kill the player if it toches them, but it HASN'T been 
// setup to stop every enemy. On purpose ;D
//


[RequireComponent(typeof(SpriteRenderer))]
public class Behaviour_FloatingLaser : EnemyObjectBase
{
	[Header("SETUP IN EDITOR")]
	[SerializeField] private GameObject m_goLaser = null;
	[SerializeField] private Material m_WarmUpMat = null;
	[SerializeField] private Material m_ActiveMat = null;
	[SerializeField] private Material m_InactiveMat = null;

	[Header("Tweakables")]
	[SerializeField] private float m_fLaserInactiveTime = 8.0f;
	[SerializeField] private float m_fLaserActiveTime = 3.0f;

	private float m_fNextEventTime = 0f;
	private GameAudioManager m_gcAudioManager = null;


	private enum ELaserState
	{
		_IDLE_OFF,
		_WARM_UP,
		_IDLE_ON,
	}

	private ELaserState m_iState = ELaserState._IDLE_OFF;


	public void Start()
	{
		if (!m_bEugatronSpecific)
		{
			m_goLaser.SetActive(false);
			m_gcRenderer.material = m_InactiveMat;
		}
	}

	private void OnDisable()
	{
		base.OnRoomExit();
	}



	public override void OnRoomEnter()
	{
		base.OnRoomEnter();

		// Verify...
		{
			m_gcAudioManager = GameInstance.Object.GetAudioManager();
			GAssert.Assert(null != m_gcAudioManager, "Unable to get audio manager");

			m_gcRenderer = GetComponent<SpriteRenderer>();
			GAssert.Assert(null != m_gcRenderer, "Unable to get sprite renderer!");

			GAssert.Assert(null != m_goLaser, "Floating Laser properties not setup in editor!");
			GAssert.Assert(null != m_WarmUpMat, "Floating Laser properties not setup in editor!");
			GAssert.Assert(null != m_InactiveMat, "Floating Laser properties not setup in editor!");
			GAssert.Assert(null != m_ActiveMat, "Floating Laser properties not setup in editor!");
		}


		// Init
		LaserOff(false);
		m_bBehaviourCanUpdate = true;
		m_iState = ELaserState._IDLE_OFF;
		m_gcRenderer.material = m_InactiveMat;
		m_fNextEventTime = TimerManager.fGameTime + (m_fLaserInactiveTime - Types.s_fDUR_FloatingLaserWarmUp);

		// Warn Players that this level contains lasers...
		m_gcAudioManager.PlayAudio(EGameSFX._SFX_FLOATING_LASER_LEVEL_WARNING);

	}



	// Turn off instant, and stop tracking event times
	//
	public override void OnPlayerHasDied()
	{
		base.OnPlayerHasDied();

		m_bBehaviourCanUpdate = false;
		m_fNextEventTime = -1;
		LaserOff();
	}



	// Go back to default init state, with a fresh timer
	//
	public override void OnPlayerRespawn()
	{
		base.OnPlayerRespawn();

		LaserOff();
		m_bBehaviourCanUpdate = true;
		m_fNextEventTime = TimerManager.fGameTime + (m_fLaserInactiveTime - Types.s_fDUR_FloatingLaserWarmUp);
		m_gcRenderer.material = m_InactiveMat;
		m_iState = ELaserState._IDLE_OFF;
	}



	// Turn off immediate
	//
	private void LaserOff(bool bPlayAudio = true)
	{
		if (bPlayAudio) m_gcAudioManager.PlayAudioAtLocation(transform.position, EGameSFX._SFX_NPC_LASERFIRE1);
		m_goLaser.SetActive(false);
		m_gcRenderer.material = m_InactiveMat;
		m_fNextEventTime = TimerManager.fGameTime + m_fLaserInactiveTime;
	}



	// Turn On Immediate
	//
	private void LaserOn()
	{
		m_gcAudioManager.PlayAudioAtLocation(transform.position, EGameSFX._SFX_FLOATING_LASER_BEAM);
		m_goLaser.SetActive(true);
		m_gcRenderer.material = m_ActiveMat;
		m_fNextEventTime = TimerManager.fGameTime + m_fLaserActiveTime;
	}



	// Simple 3-state state machine to track what the laser is doing
	//
	private void Update()
	{
		if (!m_bBehaviourCanUpdate) return;
		if (TimerManager.fGameTime < m_fNextEventTime) return;

		switch (m_iState)
		{
			case ELaserState._IDLE_OFF:
				m_gcAudioManager.PlayAudioAtLocation(transform.position, EGameSFX._SFX_FLOATING_LASER_WARMUP);
				m_fNextEventTime = TimerManager.fGameTime + 0.75f;
				m_gcRenderer.material = m_WarmUpMat;
				m_iState = ELaserState._WARM_UP;
				break;

			case ELaserState._WARM_UP:
				LaserOn();
				m_fNextEventTime = TimerManager.fGameTime + m_fLaserActiveTime;
				m_iState = ELaserState._IDLE_ON;
				break;

			case ELaserState._IDLE_ON:
				LaserOff();
				m_fNextEventTime = TimerManager.fGameTime + (m_fLaserInactiveTime - Types.s_fDUR_FloatingLaserWarmUp);
				m_iState = ELaserState._IDLE_OFF;
				break;
		}
	}
}
