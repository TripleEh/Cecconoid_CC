﻿// Cecconoid by Triple Eh? Ltd -- 2019
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

public class PlayerGun_DoubleShot : PlayerGun
{
	[SerializeField] private float m_fBulletSpread = Types.s_fPixelSize * 4f;



	public override void Fire()
	{
		if (m_bCanFire)
		{
			m_iBulletCount += 2;
			if (m_iBulletCount > Types.s_iPLAYER_BulletPerMultiplier)
			{
				m_gcPlayerState.DecMultiplier(1);
				m_iBulletCount = 0;
			}

			Quaternion qRot = Quaternion.LookRotation( m_vFireDirection, Vector3.right );

			// Spawn the bullets
			{
				Vector3 vPos = transform.position + (new Vector3(m_vFireDirection.x, m_vFireDirection.y, 0.0f) * m_fBulletSpawnOffset);

				if (m_aBulletPool[m_iPoolIndex].activeSelf) Debug.LogError(gameObject.name + ": Bullet Pool overrun!");
				m_aBulletPool[m_iPoolIndex].SetActive(true);
				m_aBulletPool[m_iPoolIndex].transform.position = vPos + (qRot * new Vector3(0f, -1, 0f)) * m_fBulletSpread;
				m_aBulletPool[m_iPoolIndex].GetComponent<BulletMovement>().InitBullet(m_vFireDirection, m_fBulletSpeed);
				++m_iPoolIndex;
				if (m_iPoolIndex >= m_iBulletPoolSize) m_iPoolIndex = 0;

				if (m_aBulletPool[m_iPoolIndex].activeSelf) Debug.LogError(gameObject.name + ": Bullet Pool overrun!");
				m_aBulletPool[m_iPoolIndex].SetActive(true);
				m_aBulletPool[m_iPoolIndex].transform.position = vPos + (qRot * new Vector3(0f, 1, 0f)) * m_fBulletSpread;
				m_aBulletPool[m_iPoolIndex].GetComponent<BulletMovement>().InitBullet(m_vFireDirection, m_fBulletSpeed);
				++m_iPoolIndex;
				if (m_iPoolIndex >= m_iBulletPoolSize) m_iPoolIndex = 0;
			}

			// Set a timer for the next shot...
			m_iTimerHandle = TimerManager.AddTimer(m_fFiringPauseSeconds, Fire);
		}
	}
}
