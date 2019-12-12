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

[RequireComponent(typeof(GunBase))]
public class FireGunOnPlayerBulletImpact : MonoBehaviour
{
	private GunBase m_gcGun = null;

	private void Awake()
	{
		m_gcGun = GetComponent<GunBase>();
		GAssert.Assert(null != m_gcGun, "FireGunOnPlayerBulletImpact on an object with no gun! " + gameObject.name);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(Types.s_sTag_PlayerBullets) && null != m_gcGun) m_gcGun.Fire();
	}
}