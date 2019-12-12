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


// Asks the camera to add additional amount to the shake velocity
// All amounts are predefined and can be tweaked in Types.cs
//

public class AddCameraShake : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private Types.ECamShakeAmount m_iAmount = Types.ECamShakeAmount._SMALL_ENEMY;

	public void AddShakeToCamera()
	{
		GameCamera gcCam = GameInstance.Object.GetGameCamera();
		GAssert.Assert(null != gcCam, "Unable to get camera object from Game Instance!");

		float fAmount = 0.0f;
		switch (m_iAmount)
		{
			case Types.ECamShakeAmount._SMALL_ENEMY: fAmount = Types.s_fCAM_SmallEnemyShakeAmount; break;
			case Types.ECamShakeAmount._MID_ENEMY: fAmount = Types.s_fCAM_MedEnemyShakeAmount; break;
			case Types.ECamShakeAmount._LARGE_ENEMY: fAmount = Types.s_fCAM_LargeEnemyShakeAmount; break;
			case Types.ECamShakeAmount._PROJECTILE: fAmount = Types.s_fCAM_ProjectileShakeAmount; break;
			case Types.ECamShakeAmount._EMPLACEMENT: fAmount = Types.s_fCAM_EmplacementShakeAmount; break;
		}
		gcCam.AddShake(fAmount);
	}
}
