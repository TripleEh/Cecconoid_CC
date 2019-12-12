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

public class RoomControllerBase : MonoBehaviour
{
	[SerializeField] protected GameObject[]  m_aRoomObjects;
	[SerializeField] protected Doorway[] m_aDoors = null;

	protected string m_sRoomName = "Base Class";



	// For rooms that want to lock the player in once they enter...
	//
	public virtual void OpenAllDoors()
	{
		foreach(Doorway gc in m_aDoors)
			if(null != gc) gc.Open();
	}



	public virtual void CloseAllDoors()
	{
		foreach (Doorway gc in m_aDoors)
			if (null != gc) gc.Close();
	}



	public virtual void OpenAllDoorsInstant()
	{
		foreach (Doorway gc in m_aDoors)
			if (null != gc) gc.OpenInstant();
	}



	public virtual void CloseAllDoorsInstant()
	{
		foreach (Doorway gc in m_aDoors)
			if (null != gc) gc.CloseInstant();
	}



	// By default, we want to "turn on" all the interactive objects in this 
	// room when the player enters. Bear in mind, specific rooms can override
	// this behaviour, so it's not a requirement. 
	// 
	// We don't care which component types need to be active, we just look 
	// for those that implement the correct interface...
	//
	public virtual void OnRoomEnter()
	{
		m_sRoomName = this.gameObject.name;
#if GDEBUG
		GameInstance.Object.SetDebugHudRoomName(m_sRoomName);
#endif
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;

			var io = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach(var gc in io) gc.OnRoomEnter();
		}
	}



	public virtual void FinaliseRoomEntry() { } 

	

	// When exiting we're not destroying any of the objects, just letting
	// them deactivate themselves. 
	//
	public virtual void OnRoomExit()
	{
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;
			
			var aComponents = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach(var gc in aComponents)
				gc.OnRoomExit();
		}
	}



	public virtual void FinaliseRoomExit() { }




	// Go through every object this room controller tracks and tell them 
	// that the player has died. This function is called from the GameMode
	//
	public virtual void OnPlayerHasDied(uint iPlayerLives)
	{
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;

			var eo = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach (var gc in eo) gc.OnPlayerHasDied();
		}
	}



	// As above, but for things that need to reset...
	// Also called from the GameMode
	//
	public virtual void BeginPlayerRespawn()
	{
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;

			var eo = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach(var gc in eo) gc.OnReset();
		}
	}



	// The player is alive, everything should be able to proceed... 
	// Also called from the GameMode
	//
	public virtual void EndPlayerRespawn()
	{
		// Enemies and static object need to know that the player respawn is complete...
		foreach (GameObject go in m_aRoomObjects)
		{
			if (null == go) continue;

			var eo = go.GetComponents<Types.IRoom_EnemyObject>();
			foreach(var gc in eo) gc.OnPlayerRespawn();
		}
	}



	public virtual void OnGameOver()
	{

	}
}
