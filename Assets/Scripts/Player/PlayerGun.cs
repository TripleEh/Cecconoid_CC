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

public class PlayerGun : GunBase
{
	// We track number of bullets fired, as they decrement the player's
	// current score multiplier value :)
	protected int m_iBulletCount;	
	protected PlayerState m_gcPlayerState;



	// Gun, and bullet pool, can be created immediately for the player...
	//
	protected	override void Awake()
	{
		OnInit();
	}

	
	
	void Start()
	{
		m_gcPlayerState = GameInstance.Object.GetPlayerState();
	}
	
	
	
	public override void Fire()
	{
		if(m_bCanFire)
		{
			++m_iBulletCount;
			if(m_iBulletCount > Types.s_iPLAYER_BulletPerMultiplier) 
			{
				m_gcPlayerState.DecMultiplier(1);
				m_iBulletCount = 0;
			}
		}
		base.Fire();
	}

	
	
	// Quantise the direction to 8way
	//
	public override void UpdateFireDirection(Vector2 vDir)
	{
		base.UpdateFireDirection(vDir);
		m_vFireDirection.x = Mathf.Round(m_vFireDirection.x);
		m_vFireDirection.y = Mathf.Round(m_vFireDirection.y);
	}
}
