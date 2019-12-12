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

public class rc_R0C7_FirstSpawners : RoomControllerBase 
{
	[Header("Setup in Editor!")]
	[SerializeField] private Doorway m_aDoorRight = null;
	

	public override void OnRoomEnter()
	{
		if(GameGlobals.TestGameEvent(Types.s_iGE_FirstSpawners)) return;

		base.OnRoomEnter();
		TimerManager.AddTimer(Types.s_fCAM_RoomTransitionDuration, CloseDoor);
	}



	public void CloseDoor()
	{
		GAssert.Assert(null != m_aDoorRight, "Room's door is not setup in the editor!");
		m_aDoorRight.Close();
	}



	public void Update()
	{
		if (GameGlobals.TestGameEvent(Types.s_iGE_FirstSpawners)) return;

		// If we don't have objects...
		bool bHaveObjects = false;
		foreach(GameObject go in m_aRoomObjects)
		{
			if(null != go) bHaveObjects = true;
		}

		// Open the door and set the event...
		if(!bHaveObjects)
		{
			m_aDoorRight.Open();
			GameGlobals.SetGameEvent(Types.s_iGE_FirstSpawners);
		}
	}
}
