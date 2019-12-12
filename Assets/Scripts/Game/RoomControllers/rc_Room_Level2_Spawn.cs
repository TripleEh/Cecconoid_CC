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

public class rc_Room_Level2_Spawn : RoomControllerBase
{
	[Header("Spawn Effect")]
	[SerializeField] private GameObject m_goSpawnEffect = null;

	public override void OnRoomEnter()
	{
		base.OnRoomEnter();
		if(null != m_goSpawnEffect) m_goSpawnEffect.SetActive(true);
	}
}
