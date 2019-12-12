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


// Doorways aren't brilliantly designed, as the params that do most of the 
// work are in children of the main object. 
// 
// See also: DoorwayCollision and DoorwayTrigger for the classes that do 
// most of the work...
//


public class Doorway : MonoBehaviour
{
	[Header("Door Params - See children for room links")]
	[SerializeField] private bool m_bNoCollision = false;
	[SerializeField] private Types.SDoorwayColliders m_Params = new Types.SDoorwayColliders();
	[SerializeField] private Types.EDirection m_iDoorOrientation = Types.EDirection._VERTICAL;

	public void Start()
	{
		if (m_bNoCollision) { m_Params._gcDoorL.gameObject.SetActive(false); m_Params._gcDoorR.gameObject.SetActive(false); };
		if (null != m_Params._gcDoorL) m_Params._gcDoorL.Init(m_iDoorOrientation);
		if (null != m_Params._gcDoorR) m_Params._gcDoorR.Init(m_iDoorOrientation);
	}



	public void CloseInstant()
	{
		GAssert.Assert(m_bNoCollision == false, "Doorway set to no collision, but code is asking it to open/close. Change collision toggle in editor!");
		if (null != m_Params._gcDoorL) m_Params._gcDoorL.CloseInstant();
		if (null != m_Params._gcDoorR) m_Params._gcDoorR.CloseInstant();
		SetTriggers(false);
	}



	public void OpenInstant()
	{
		GAssert.Assert(m_bNoCollision == false, "Doorway set to no collision, but code is asking it to open/close. Change collision toggle in editor!");
		if (null != m_Params._gcDoorL) m_Params._gcDoorL.OpenInstant();
		if (null != m_Params._gcDoorR) m_Params._gcDoorR.OpenInstant();
		SetTriggers(true);
	}



	public void Open()
	{
		GAssert.Assert(m_bNoCollision == false, "Doorway set to no collision, but code is asking it to open/close. Change collision toggle in editor!");
		if (null != m_Params._gcDoorL) m_Params._gcDoorL.Open();
		if (null != m_Params._gcDoorR) m_Params._gcDoorR.Open();
		SetTriggers(true);
	}



	public void Close()
	{
		GAssert.Assert(m_bNoCollision == false, "Doorway set to no collision, but code is asking it to open/close. Change collision toggle in editor!");
		if (null != m_Params._gcDoorL) m_Params._gcDoorL.Close();
		if (null != m_Params._gcDoorR) m_Params._gcDoorR.Close();
		SetTriggers(false);
	}



	private void SetTriggers(bool bState)
	{
		if (null != m_Params._gcCollider_L) m_Params._gcCollider_L.enabled = bState;
		if (null != m_Params._gcCollider_R) m_Params._gcCollider_R.enabled = bState;
		if (null != m_Params._gcCollider_U) m_Params._gcCollider_U.enabled = bState;
		if (null != m_Params._gcCollider_D) m_Params._gcCollider_D.enabled = bState;
	}



	// Show where the respawn locations are for the doors...
	//
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (null != m_Params._gcCollider_L) Gizmos.DrawWireCube(transform.position + new Vector3(m_Params._gcCollider_L.offset.x, m_Params._gcCollider_L.offset.y, 0.0f), m_Params._gcCollider_L.size);
		if (null != m_Params._gcCollider_R) Gizmos.DrawWireCube(transform.position + new Vector3(m_Params._gcCollider_R.offset.x, m_Params._gcCollider_R.offset.y, 0.0f), m_Params._gcCollider_R.size);
		if (null != m_Params._gcCollider_U) Gizmos.DrawWireCube(transform.position + new Vector3(m_Params._gcCollider_U.offset.x, m_Params._gcCollider_U.offset.y, 0.0f), m_Params._gcCollider_U.size);
		if (null != m_Params._gcCollider_D) Gizmos.DrawWireCube(transform.position + new Vector3(m_Params._gcCollider_D.offset.x, m_Params._gcCollider_D.offset.y, 0.0f), m_Params._gcCollider_D.size);
	}



	public bool GetIsClosed()
	{
		return m_Params._gcDoorL.GetDoorwayCollisionState() == Types.EDoorwayCollisionState._IDLE_CLOSED;
	}


	// This is used by the DevMenu, which puts all doors into a scrolling 
	// list so you can jump between rooms quickly in DEBUG builds...
	//
	public Types.SDevMenuDoorDetails GetDoorwayDetails()
	{
		Types.SDevMenuDoorDetails ret = new Types.SDevMenuDoorDetails();

		DoorwayTrigger gcDoor1 = null;
		DoorwayTrigger gcDoor2 = null;
		Types.SDoorwaySpawn gcDets1;
		Types.SDoorwaySpawn gcDets2;

		if (m_iDoorOrientation == Types.EDirection._HORIZONTAL)
		{
			gcDoor1 = m_Params._gcCollider_L.gameObject.GetComponent<DoorwayTrigger>();
			GAssert.Assert(null != gcDoor1, "Door trigger set without component?!");
			gcDets1 = gcDoor1.GetParams();

			gcDoor2 = m_Params._gcCollider_L.gameObject.GetComponent<DoorwayTrigger>();
			GAssert.Assert(null != gcDoor1, "Door trigger set without component?!");
			gcDets2 = gcDoor2.GetParams();


			ret._vPos1 = m_Params._gcCollider_L.transform.position + new Vector3(gcDets1._fSpawnOffset, 0f, 0f);
			ret._vPos2 = m_Params._gcCollider_R.transform.position + new Vector3(gcDets2._fSpawnOffset, 0f, 0f);

			ret._goRoom1 = gcDets1._goRoom;
			ret._goRoom2 = gcDets2._goRoom;
		}
		else
		{
			gcDoor1 = m_Params._gcCollider_U.gameObject.GetComponent<DoorwayTrigger>();
			GAssert.Assert(null != gcDoor1, "Door trigger set without component?!");
			gcDets1 = gcDoor1.GetParams();

			gcDoor2 = m_Params._gcCollider_D.gameObject.GetComponent<DoorwayTrigger>();
			GAssert.Assert(null != gcDoor1, "Door trigger set without component?!");
			gcDets2 = gcDoor2.GetParams();


			ret._vPos1 = m_Params._gcCollider_U.transform.position + new Vector3(0f, gcDets1._fSpawnOffset, 0f);
			ret._vPos2 = m_Params._gcCollider_D.transform.position + new Vector3(0f, gcDets2._fSpawnOffset, 0f);

			ret._goRoom1 = gcDets1._goRoom;
			ret._goRoom2 = gcDets2._goRoom;
		}

		return ret;
	}
}
