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

// This class handles One time use, Flip/Flop switches in the world!
//
// This class also looks a tad over-engineered, and that's because it is
//
// My original thoughts were to have more types of switches in the world
// including ones that timed out (reverted to init-state) and multiple 
// switches chained together. 
//
// Didn't use any of that stuff, so this could have been done in a much 
// simpler way in the end. Ho hum. This is why you build what you need & 
// not what you think you might need... :D
//

[RequireComponent(typeof(SpriteRenderer))]
public class Switch_OneTime : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Tweakables")]

	// Material to switch to when the we're finally inactive...
	[SerializeField] private Material m_iM_Inactive = null;

	// List of objects that will respond to this switch.
	// Easier to be an array of gameObject, as the Interface won't serialise in the editor without more code...
	[SerializeField] private GameObject[] m_aResponders = null;

	// Current switch state
	private Types.ESWitchState m_iState = Types.ESWitchState._IDLE_INROOM_ACTIVE_FLIP;



	// Re-enable the switch when the player has respawned
	//
	public void OnPlayerRespawn()
	{
		switch (m_iState)
		{
			case Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLIP: m_iState = Types.ESWitchState._IDLE_INROOM_ACTIVE_FLIP; break;
			case Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLOP: m_iState = Types.ESWitchState._IDLE_INROOM_ACTIVE_FLOP; break;
		}

	}



	// If the player has died, disable the switch...
	//
	public void OnPlayerHasDied()
	{
		switch (m_iState)
		{
			case Types.ESWitchState._IDLE_INROOM_ACTIVE_FLIP: m_iState = Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLIP; break;
			case Types.ESWitchState._IDLE_INROOM_ACTIVE_FLOP: m_iState = Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLOP; break;
		}
	}

	public void OnReset() { }




	// On room enter, revert to the proper active states
	//
	public void OnRoomEnter()
	{

		// GNTODO: Is it likely for switch responders to be destroyed?
		//
		GAssert.Assert(m_aResponders.Length > 0, "Switch setup with no switch responders!");

		switch(m_iState)
		{
			case Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLIP: m_iState = Types.ESWitchState._IDLE_INROOM_ACTIVE_FLIP; break;
			case Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLOP: m_iState = Types.ESWitchState._IDLE_INROOM_ACTIVE_FLOP; break;
		}
	}



	// On room exit, swap state to blockers, so any object alive during room transition
	// can't accidentally affect state...
	//
	public void OnRoomExit()
	{
		switch (m_iState)
		{
			case Types.ESWitchState._IDLE_INROOM_ACTIVE_FLIP: m_iState = Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLIP; break;
			case Types.ESWitchState._IDLE_INROOM_ACTIVE_FLOP: m_iState = Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLOP; break;
		}
	}



	// Only react to player bullets, and even then, only if we're in the active room.
	// (The latter shouldn't ever happen, but just in case...)
	//
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if( !collision.gameObject.CompareTag(Types.s_sTag_PlayerBullets) 
			|| m_iState == Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLIP 
			|| m_iState == Types.ESWitchState._IDLE_EXROOM_ACTIVE_FLOP
			|| m_iState == Types.ESWitchState._IDLE_INACTIVE ) return;


		// Switch material
		SpriteRenderer sr  = GetComponent<SpriteRenderer>();
		GAssert.Assert(null != sr, "Switch: No sprite renderer attached to this object!");
		sr.material = m_iM_Inactive;


		// Trigger all the objects that care about this switch. 
		foreach(GameObject go in m_aResponders)
		{
			var aResponders =  go.GetComponents<Types.IRoom_SwitchResponder>();
			foreach(var gc in aResponders) gc.OnSwitchFlip();
		}

		// Trigger some audio
		GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_SWITCH_TOGGLED);

		// One time switches just deactivate. No flip flops here...
		m_iState = Types.ESWitchState._IDLE_INACTIVE;
		this.enabled = false;
	}
}
