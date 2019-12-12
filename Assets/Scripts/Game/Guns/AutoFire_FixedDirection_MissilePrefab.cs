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

[RequireComponent(typeof(Anim_FlickFrameOnRequest))]
public class AutoFire_FixedDirection_MissilePrefab : GunBase
{

	[Header("Auto Fire Gun Properties")]
	[SerializeField] private Vector3 m_vSpawnPosition = Vector2.zero;
	[Range(0.16f, 100)]
	[SerializeField] private float m_fInitialFireDelay = Types.s_fTTL_RoomEntryActivityDelay;		// This MUST be greater than 0!
	[SerializeField] private uint m_iAmmunition = 15;

	private Anim_FlickFrameOnRequest m_gcAnimator;



	override public void OnRoomEnter()
	{
		OnInit();
		SetCanFire(true, m_fInitialFireDelay);
		m_gcAnimator = GetComponent<Anim_FlickFrameOnRequest>();
		GAssert.Assert(null != m_gcAnimator, "Unable to get flick frame animator!");
	}



	override public void OnRoomExit()
	{
		SetCanFire(false);
		OnDeInit();
	}



	public override void Fire()
	{
		if (m_bCanFire && m_iAmmunition > 0)
		{
			// Get the fire direction so spawning is at the correct initial rotation
			Vector3 vTraj = transform.position - (transform.position + m_vSpawnPosition);
			float fAngle = (Mathf.Atan2(vTraj.y, vTraj.x) * Mathf.Rad2Deg) + 90f;

			// Spawn the Missile
			Instantiate(m_BulletPrefab, (transform.position + m_vSpawnPosition), Quaternion.AngleAxis(fAngle, Vector3.forward));
			GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_MISSLE_TAKEOFF);
			--m_iAmmunition;

			// Set a timer for the next shot...
			m_iTimerHandle = TimerManager.AddTimer(m_fFiringPauseSeconds, Fire);

			// Null check belt and braces, Fire can be called before OnRoomEnter if m_fInitialFireDelay is too small
			if(null != m_gcAnimator ) m_gcAnimator.FlickAnim();
		}
	}



	override public void OnPlayerHasDied() { SetCanFire(false); }
	override public void OnPlayerRespawn() { SetCanFire(true, m_fInitialFireDelay); }



	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position + m_vSpawnPosition, new Vector2(Types.s_fPixelSize * 4, Types.s_fPixelSize * 4));
	}
}

