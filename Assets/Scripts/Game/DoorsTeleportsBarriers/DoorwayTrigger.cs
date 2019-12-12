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

[RequireComponent(typeof(BoxCollider2D))]
public class DoorwayTrigger : MonoBehaviour
{
	[SerializeField] private Types.SDoorwaySpawn m_Params = new Types.SDoorwaySpawn();

	private BoxCollider2D m_gcTrigger;



	void Awake()
	{
		m_gcTrigger = GetComponent<BoxCollider2D>();
		GAssert.Assert(null != m_gcTrigger, "DoorwayTrigger missing trigger area!");
	}



	// Return the correct spawn offset. 
	// Called by the opposing door. 
	//
	public Vector3 GetSpawnOffset()
	{
		Vector3 vOffset = Vector3.zero;
		switch (m_Params._iDir)
		{
			case Types.EDirection._HORIZONTAL_L2R:
			case Types.EDirection._HORIZONTAL_R2L: vOffset.x = m_Params._fSpawnOffset; vOffset.y = 0.0f; break;
			case Types.EDirection._VERTICAL_D2U:
			case Types.EDirection._VERTICAL_U2D: vOffset.y = m_Params._fSpawnOffset; vOffset.x = 0.0f; break;
		}
		return vOffset;
	}



	// Called by the opposing door
	//
	public GameObject GetNextRoom()
	{
		return m_Params._goRoom;
	}



	// Begins the room transition process...
	//
	private void OnTriggerEnter2D(Collider2D other)
	{
		// ...but only if the player triggered!
		if (!other.CompareTag("Player")) return;

		// Get the two transition positions (where we're coming from and where we're going to)
		{
			GAssert.Assert(null != m_Params._goOpposingSpawn, "Door trigger entered, but opposing spawn GameObject has not been set");
			DoorwayTrigger gcOpposing = m_Params._goOpposingSpawn.GetComponent<DoorwayTrigger>();
			GAssert.Assert(null != gcOpposing, "Unable to get opposing doorway params!");

			// Set the lerp to position to be the spawn point set in the opposing doorway trigger (t'other side of the doorway)
			// But keep the player's horiz/vertial offset where it is
			{
				GameGlobals.s_vRoomTransitionTo = m_Params._goOpposingSpawn.transform.position + gcOpposing.GetSpawnOffset();
				if (m_Params._iDir == Types.EDirection._HORIZONTAL_L2R || m_Params._iDir == Types.EDirection._HORIZONTAL_R2L)
					GameGlobals.s_vRoomTransitionTo.y = GameInstance.Object.GetPlayerPosition().y;
				else
					GameGlobals.s_vRoomTransitionTo.x = GameInstance.Object.GetPlayerPosition().x;
			}

			// Set the position to lerp from to be the position the player is at when they hit the trigger
			GameGlobals.s_vRoomTransitionFrom = other.transform.position;

			// Get the next room, this may or may not have a room controller attached but we don't need to care here...
			GameObject goNextRoom = gcOpposing.GetNextRoom();

			// And start the room transition. 
			GameMode.BeginRoomTransition(ref m_Params._goRoom, ref goNextRoom);
		}

		// Disable the opposing trigger (t'other side of the doorway) and set a timer to re-enable it
		{
			m_gcTrigger.enabled = false;
			if (null != m_Params._goOpposingSpawn) m_Params._goOpposingSpawn.SetActive(false);
			TimerManager.AddTimer(Types.s_fCAM_RoomTransitionDuration, EnableDoorway);
		}
	}



	// Doorways consist of two triggers, both of which should be disabled as the player transitions between rooms
	// When the transition is complete we can re-enable them...
	void EnableDoorway()
	{
		m_gcTrigger.enabled = true;
		if (null != m_Params._goOpposingSpawn) m_Params._goOpposingSpawn.SetActive(true);
	}



	public Types.SDoorwaySpawn GetParams()
	{
		return m_Params;
	}




	// DEBUG ONLY! Just to help visualise the doorway in the editor...
	void OnDrawGizmos()
	{
		switch (m_Params._iDir)
		{
			// Yay fall-through again!
			case Types.EDirection._HORIZONTAL_L2R:
			case Types.EDirection._HORIZONTAL_R2L: Gizmos.DrawWireCube(transform.position + new Vector3(m_Params._fSpawnOffset, 0.0f, 0.0f), Vector3.one * 0.05f); break;
			case Types.EDirection._VERTICAL_D2U:
			case Types.EDirection._VERTICAL_U2D: Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, m_Params._fSpawnOffset, 0.0f), Vector3.one * 0.05f); break;
			default: break;
		}
	}
}
