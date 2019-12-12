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

public class CircularShotGun : GunBase
{
	[Header("Circular Shot Properties")]
	[Range(1,32)]
	[SerializeField] private int m_iBulletsPerShot = 8;
	public override void OnRoomEnter()
	{
		OnInit();
		m_bCanFire = true;
	}


	public override void OnPlayerHasDied()
	{
		m_bCanFire = false;
	}


	public override void OnPlayerRespawn()
	{
		m_bCanFire = true;
	}


	public override void Fire()
	{
		if(m_bCanFire)
		{
			float fAngleStep = (360/m_iBulletsPerShot) * Mathf.Deg2Rad;
			for(int i = 0; i < m_iBulletsPerShot; ++i)
			{
				m_vFireDirection = new Vector3(Mathf.Sin(i*fAngleStep), Mathf.Cos(i*fAngleStep), 0f);
				// Bullet spawn position is gun's position, plus a definable distance, offset in the direction of fire...

				Vector3 vSpawnPos = (transform.position + m_vGunPositionOffset) + (new Vector3(m_vFireDirection.x, m_vFireDirection.y, 0.0f) * m_fBulletSpawnOffset);

				// Spawn the bullet
				{
					if (m_aBulletPool[m_iPoolIndex].activeSelf) Debug.LogError(gameObject.name + ": Bullet Pool overrun!");
					m_aBulletPool[m_iPoolIndex].SetActive(true);
					m_aBulletPool[m_iPoolIndex].transform.position = vSpawnPos;
					m_aBulletPool[m_iPoolIndex].GetComponent<BulletMovement>().InitBullet(m_vFireDirection, m_fBulletSpeed);
					++m_iPoolIndex;
					if (m_iPoolIndex >= m_iBulletPoolSize) m_iPoolIndex = 0;
				}
			}
		}
	}
}
