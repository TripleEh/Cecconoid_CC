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

public class rc_R0C5_FirstBrains : RoomControllerBase
{
	[SerializeField] private Doorway m_gcDoorLeft = null;



	public override void OnRoomEnter()
	{
		base.OnRoomEnter();
		if (!GameGlobals.TestGameEvent(Types.s_iGE_FirstBrains) && !m_gcDoorLeft.GetIsClosed())
			TimerManager.AddTimer(Types.s_fDUR_DoorCloseDuration, CloseDoorsFirstEntry);
	}



	void CloseDoorsFirstEntry()
	{
		m_gcDoorLeft.Close();
	}



	void Update()
	{
		if(!GameGlobals.TestGameEvent(Types.s_iGE_FirstBrains))
		{
			// If we don't have objects...
			bool bHaveObjects = false;
			foreach (GameObject go in m_aRoomObjects)
			{
				if (null != go) bHaveObjects = true;
			}

			// ...Open the door and set the event.
			if (!bHaveObjects)
			{
				m_gcDoorLeft.Open();
				GameGlobals.SetGameEvent(Types.s_iGE_FirstBrains);
			}
		}
	}
}
