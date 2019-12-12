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


// This is a little gun with a rotating barrel that follows the player
//


public class TrackingEmplacement : GunBase
{
	[Header("Tracking Gun Properties")]
	[SerializeField] private GameObject m_goGun = null;
	[SerializeField] private GameObject m_goGunTurret = null;
	[SerializeField] private float m_fTrackingRotationDegrees = 80.0f;
	[SerializeField] private float m_fTurrentVertOffset = -0.03f;
	[SerializeField] private float m_fTurrentDistance = 0.1f;
	[SerializeField] private bool m_bInverted = false;
	[SerializeField] private bool m_bRandomiseInit = true;


	private GameObject m_goPlayerObject = null;
	private Vector3 m_vTurrentPositionOffset;



	// Initialise the gun, grab references to what's needed, and setup our position
	//
	override public void OnRoomEnter()
	{
		// Was the gun destroyed previously?
		{
			if (null == m_goGun)
			{
				SetCanFire(false);
				Destroy(m_goGunTurret);
				this.enabled = false;
				return;
			}
		}

		OnInit();

		// Cache the player
		m_goPlayerObject = GameInstance.Object.GetPlayerObject();
		GAssert.Assert(null != m_goPlayerObject, "Tracking gun unable to get player object from GameInstance!");

		// Setup the centre point for the turret and tell the gun
		{
			m_vTurrentPositionOffset = transform.position;
			m_vTurrentPositionOffset.y += m_fTurrentVertOffset;
			m_vGunPositionOffset.y = m_fTurrentVertOffset;
		}

		m_fBulletSpawnOffset = m_fTurrentDistance * 1.5f;
		if (m_bRandomiseInit) SetCanFire(true, Types.s_fTTL_RoomEntryActivityDelay + Random.Range(0f, 1.5f));
		else SetCanFire(true, Types.s_fTTL_RoomEntryActivityDelay);
	}



	// DeInit the gun and deactivate...
	//
	override public void OnRoomExit()
	{
		SetCanFire(false);
		OnDeInit();
	}



	override public void OnPlayerHasDied()
	{
		if (null == m_goGun) return;	// Gun has been destroyed
		SetCanFire(false);
		OnDeInit();
	}



	override 	public void OnPlayerRespawn()
	{
		if(null == m_goGun) return; 
		OnInit();
		if (m_bRandomiseInit) SetCanFire(true, Types.s_fTTL_RoomEntryActivityDelay + Random.Range(0f, 1.5f));
		else SetCanFire(true, Types.s_fTTL_RoomEntryActivityDelay);
	}



	// Track the player and update the barrel location. 
	//
	public void Update()
	{
		// If the gun isn't setup (Update can be called in a loaded level before the GameState has finished) then early out
		if (!m_bSetup) return;
		if (null == m_goPlayerObject) return;
		if (TimerManager.IsPaused()) return;


		// Has the Gun been destroyed?
		{
			if (null == m_goGun)
			{
				SetCanFire(false);
				Destroy(m_goGunTurret);
				OnDeInit();
				this.enabled = false;
				return;
			}
		}

		// Put all the positions on a plane
		Vector3 vPlayerPos = m_goPlayerObject.transform.position;
		vPlayerPos.z = m_vTurrentPositionOffset.z;

		// Get direction to the player and convert to degrees
		Vector3 vDir = (vPlayerPos - m_vTurrentPositionOffset).normalized;
		float fDeg = Mathf.Atan2(vDir.y, vDir.x) * Mathf.Rad2Deg;

		// If the player is in the opposing arc DO NOT update the firing angle, stay at our last position!
		if (fDeg < 0.0f && !m_bInverted) return;
		if (fDeg > 0.0f && m_bInverted) return;

		// Zero degrees is -> not ^
		if (!m_bInverted) fDeg = Mathf.Clamp(fDeg, 90.0f - m_fTrackingRotationDegrees, 90.0f + m_fTrackingRotationDegrees);
		else fDeg = fDeg = Mathf.Clamp(fDeg, -90 - m_fTrackingRotationDegrees, -90 + m_fTrackingRotationDegrees);

		// Make a new Traj from the degrees
		vDir = new Vector3(Mathf.Cos(fDeg * Mathf.Deg2Rad), Mathf.Sin(fDeg * Mathf.Deg2Rad), 0.0f);
		vDir.Normalize();

		// Place the turret
		// GNTODO: Clamp this to a pixel width
		m_goGunTurret.transform.position = m_vTurrentPositionOffset + (vDir * m_fTurrentDistance);

		// Tell the gun which direction to fire in!
		UpdateFireDirection(vDir);

		//Debug.DrawLine(m_vTurrentPositionOffset, m_vTurrentPositionOffset + (vDir * m_fTurrentDistance), Color.white);
	}
}
