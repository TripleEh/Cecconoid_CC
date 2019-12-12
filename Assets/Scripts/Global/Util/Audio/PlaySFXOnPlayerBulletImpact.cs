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

public class PlaySFXOnPlayerBulletImpact : MonoBehaviour
{
	[SerializeField] private EGameSFX m_iSFX = EGameSFX._SFX_NPC_BOUNCE_LOW;

	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.CompareTag(Types.s_sTag_PlayerBullets)) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX);
	}
}
