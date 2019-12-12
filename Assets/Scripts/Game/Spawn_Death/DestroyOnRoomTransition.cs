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

// Clean up component, removes anything it's attached to
// when the player moves out of the active room and into 
// the next one.
//

public class DestroyOnRoomTransition : MonoBehaviour
{
	void Start()
	{
		Messenger.AddListener(Types.s_sGF_BeginRoomTransition, OnRoomTransition);
	}

	void OnDisable()
	{
		Messenger.RemoveListener(Types.s_sGF_BeginRoomTransition, OnRoomTransition);
	}

	public void OnRoomTransition()
	{
		Destroy(gameObject);
	}
}
