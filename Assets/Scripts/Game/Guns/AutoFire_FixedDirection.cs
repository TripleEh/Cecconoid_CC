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
public class AutoFire_FixedDirection : GunBase, Types.IRoom_EnemyObject
{
	[Header("Auto Fire Gun Properties")]
	[SerializeField] private Vector2 m_vBulletTrajectory = Vector2.left;
	[SerializeField] private float m_fInitialFireDelay = Types.s_fTTL_RoomEntryActivityDelay;



	// When we fire, we ask to flick the animation directly!
	private Anim_FlickFrameOnRequest m_gcAnimator;



	override public void OnRoomEnter()
	{
		OnInit();
		m_gcAnimator = GetComponent<Anim_FlickFrameOnRequest>();
		GAssert.Assert(null != m_gcAnimator, "Unable to get flick frame animator!");
		m_vGunPositionOffset = Vector3.zero;
		UpdateFireDirection(m_vBulletTrajectory);
		SetCanFire(true, m_fInitialFireDelay);
	}



	override public void OnRoomExit()
	{
		SetCanFire(false);
		OnDeInit();
	}



	override public void OnPlayerRespawn()
	{
		OnInit();
		SetCanFire(true, m_fInitialFireDelay);
	}



	public override void Fire()
	{
		base.Fire();
		m_gcAnimator.FlickAnim();
	}
}
