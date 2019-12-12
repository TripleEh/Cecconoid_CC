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

public class TriggerExitLevelOne : MonoBehaviour
{
	[Header("Effect")]
	[SerializeField] private GameObject m_goSpawnEffectPrefab = null;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag(Types.s_sTag_Player) && GameGlobals.TestGameEvent(Types.s_iGE_Robotron4))
		{
			PlayerState gcPS = GameInstance.Object.GetPlayerState();
			GAssert.Assert(null != gcPS, "Unable to get the player state!");
			gcPS.LockPlayer();
			gcPS.DespawnExitKey();

			GAssert.Assert(null != m_goSpawnEffectPrefab, "Spawn effect prefab not set in editor!");
			Instantiate(m_goSpawnEffectPrefab, collision.transform.position + new Vector3(0f,0f,-1.5f), Quaternion.identity);

			TimerManager.AddTimer(1.5f, BeginWarpToLevel2);
		}
	}


	public void BeginWarpToLevel2()
	{
		GameMode.WarpPlayerToLevel2();
	}
}
