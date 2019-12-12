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

// These are laser beams that scan from side to side, and detect collision 
// to modify the length of the laser beam dynamically. 
// Uses a line renderer for the effect and raycasts for the checks.
//

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class RotatingLaserBeam : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Timer Settings")]
	[SerializeField] private bool m_bHasTimer = true;
	[SerializeField] private bool m_bHasAudio = true;
	[SerializeField] private float m_fStartOffsetSeconds = 0.0f;
	[Range(0f, 1f)]
	[SerializeField] private float m_fArcTrackingOffset = 0.0f;
	[SerializeField] private float m_fTimeOff = 1.0f;

	[Header("Tracking Settings")]
	[SerializeField] private bool m_bPingPong = true;
	[SerializeField] private float m_fRotationArcDegrees = 90.0f;
	[SerializeField] private float m_fArcTrackingDuration = 6.0f;
	[SerializeField] private Types.EDirection m_iDir = Types.EDirection._VERTICAL_U2D;
	[SerializeField] private EEasingType m_iEasingType = EEasingType.Quartic;
	[SerializeField] private float m_fColliderRayCastOffset = 0.08f;

	[Header("Effects")]
	[SerializeField] private GameObject m_goEndPointParticlePrefab = null;

	private LineRenderer m_gcLineRenderer;
	private Vector3[] m_aPositions;
	private AudioSource m_gcAudio;
	private GameObject m_goEndPointParticles;

	private bool m_bIsOn = false;
	private bool m_bIsForward = true;
	private bool m_bIsWaiting = false;
	private ulong m_iTimerHandle = 0;
	private float m_fEventTime = 0;
	private float m_fTrackingAngle = 0.0f;
	private float m_fRatioOffset = 0.0f;



	void Awake()
	{
		m_gcLineRenderer = GetComponent<LineRenderer>();
		GAssert.Assert(null != m_gcLineRenderer, "RotatingLaserBeam: unable to find line renderer!");
		m_aPositions = new Vector3[2];
		m_aPositions[0] = transform.position + new Vector3(0.0f, 0.0f, 0.1f);
		m_aPositions[1] = transform.position + new Vector3(0.0f, 0.0f, 0.1f);
		m_gcLineRenderer.SetPositions(m_aPositions);
		m_gcLineRenderer.startWidth = Types.s_fPixelSize;
		m_gcLineRenderer.endWidth = Types.s_fPixelSize;

		if (m_bHasAudio)
		{
			m_gcAudio = GetComponent<AudioSource>();
			GAssert.Assert(null != m_gcAudio, "RotatingLaserBeam: setup without an audio component!");
			m_gcAudio.Stop();
		}
	}



	public void OnRoomEnter()
	{
		if (m_bIsOn) return;

		// If we want the lasers offset by a certain percentage (to avoid chorus lining)
		// then ArcTrackingOffsetPercent is the lever. See TurnOn()

		// NOTE: Use StartOffsetSeconds if you want to de-sync the timings, this DOESN'T
		// affect the timer, only the position in the arc
		m_fRatioOffset = m_fArcTrackingDuration * m_fArcTrackingOffset;

		// GNTODO: Race conditon here if the player can exit the room before StartOffset expires
		// RoomControllerBase should store Room Entry time so we can compare durations and
		// clear offset timers without extra flags in each class...
		//
		// Or, just clear the timer and accept it might not always exist... (<-- Do this!)
		if (m_fStartOffsetSeconds > 0.0f)
		{
			m_bIsWaiting = true;
			m_iTimerHandle = TimerManager.AddTimer(m_fStartOffsetSeconds, TurnOn);
		}
		else TurnOn();
	}



	public void OnRoomExit()
	{
		if (m_bHasTimer && m_iTimerHandle > 0) CleanRemoveTimer();
		TurnOffInstant();
		SetDefaults();
	}



	protected void CleanRemoveTimer()
	{
		if (m_bIsWaiting) TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOn);
		else if (m_bIsOn) TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOff);
		else TimerManager.ClearTimerHandler(m_iTimerHandle, TurnOn);
	}



	protected void SetDefaults()
	{
		m_fRatioOffset = m_fArcTrackingDuration * m_fArcTrackingOffset;
		m_bIsOn = false;
		m_bIsForward = true;
		m_bIsWaiting = false;
		m_iTimerHandle = 0;
		m_fEventTime = 0;
		m_fTrackingAngle = 0.0f;
		m_fRatioOffset = 0.0f;
	}



	public void OnPlayerHasDied()
	{
		if (m_bHasTimer && m_iTimerHandle > 0) CleanRemoveTimer();
		TurnOffInstant();

	}



	public void OnPlayerRespawn()
	{
		m_fRatioOffset = m_fArcTrackingDuration * m_fArcTrackingOffset;
		if (m_fStartOffsetSeconds > 0.0f)
		{
			m_bIsWaiting = true;
			m_iTimerHandle = TimerManager.AddTimer(m_fStartOffsetSeconds, TurnOn);
		}
		else TurnOn();
	}



	public void OnReset()
	{
		SetDefaults();
	}



	public void TurnOn()
	{
		m_bIsOn = true;
		m_bIsWaiting = false;

		// By pushing event time backwards we're offsetting the initial 
		// position of the laser in it's arc... Only need to do this 
		// the first time we turn the laser on...
		m_fEventTime = TimerManager.fGameTime - m_fRatioOffset;
		m_fRatioOffset = 0;

		if (null != m_goEndPointParticlePrefab)
		{
			m_goEndPointParticles = Instantiate(m_goEndPointParticlePrefab, transform.position, Quaternion.identity);
			m_goEndPointParticles.GetComponent<ParticleSystem>().Play();
		}

		if (m_bHasAudio) m_gcAudio.Play();

		// Timer lasts for a couple of frames too long, to ensure the PingPong (m_bIsForward) gets flipped in Update()
		if (m_bHasTimer) m_iTimerHandle = TimerManager.AddTimer(m_fArcTrackingDuration + (TimerManager.fGameDeltaTime * 2), TurnOff);
	}



	public void TurnOffInstant()
	{
		m_aPositions[0] = transform.position + new Vector3(0.0f, 0.0f, 0.1f);
		m_aPositions[1] = transform.position + new Vector3(0.0f, 0.0f, 0.1f);
		m_gcLineRenderer.SetPositions(m_aPositions);

		if (m_bHasAudio) m_gcAudio.Stop();

		if (null != m_goEndPointParticles) Destroy(m_goEndPointParticles);

		m_bIsOn = false;
		;
	}



	public void TurnOff()
	{
		TurnOffInstant();
		if (m_bHasTimer) m_iTimerHandle = TimerManager.AddTimer(m_fTimeOff, TurnOn);
	}



	public void Update()
	{
		if (!m_bIsOn) return;

		float fDir, fRatio;
		Vector3 vDir = Vector3.zero;

		
		// Process the Lerp
		{
			fRatio = Mathf.Abs((TimerManager.fGameTime - m_fEventTime) / m_fArcTrackingDuration);

			// Handle ping pong
			if (!m_bIsForward) fDir = 1.0f - fRatio; else fDir = fRatio;

			if (fRatio >= 1.0f)
			{
				m_fEventTime = TimerManager.fGameTime;
				if (m_bPingPong) m_bIsForward = !m_bIsForward;
			}
		}


		// Calc the tracking angle
		{
			m_fTrackingAngle = -(m_fRotationArcDegrees / 2.0f) + (m_fRotationArcDegrees * Easing.EaseInOut(fDir, m_iEasingType));

			switch (m_iDir)
			{
				case Types.EDirection._VERTICAL_U2D: vDir = Quaternion.AngleAxis(m_fTrackingAngle, transform.forward) * Vector2.down; break;
				case Types.EDirection._VERTICAL_D2U: vDir = Quaternion.AngleAxis(m_fTrackingAngle, transform.forward) * Vector2.up; break;
				case Types.EDirection._HORIZONTAL_L2R: vDir = Quaternion.AngleAxis(m_fTrackingAngle, transform.forward) * Vector2.right; break;
				case Types.EDirection._HORIZONTAL_R2L: vDir = Quaternion.AngleAxis(m_fTrackingAngle, transform.forward) * Vector2.left; break;
			}
		}


		// Find the end point for the laser
		RaycastHit2D Hit = Physics2D.Raycast(transform.position + (vDir * m_fColliderRayCastOffset), vDir * Types.s_fRoomWidth);
		Debug.DrawRay(transform.position + (vDir * m_fColliderRayCastOffset), vDir * Types.s_fRoomWidth);


		// Set the positions of the line renderer... (Start point is pushed behind the gun's sprite)
		{
			m_aPositions[0] = transform.position + new Vector3(0.0f, 0.0f, 0.1f);

			if (null != Hit.collider) m_aPositions[1] = Hit.point;
			else m_aPositions[1] = transform.position + (vDir * Types.s_fRoomWidth);

			m_gcLineRenderer.SetPositions(m_aPositions);
		}

		
		// Did we hit the player?
		{
			if (null != Hit.collider && Hit.collider.gameObject.CompareTag(Types.s_sTag_Player))
			{
				PlayerHitDetection gc = Hit.transform.gameObject.GetComponent<PlayerHitDetection>();
				if (null != gc) gc.DoImpact();
			}
		}

		
		// Place the particle system...
		if (null != m_goEndPointParticles) m_goEndPointParticles.transform.position = m_aPositions[1];
	}
}
