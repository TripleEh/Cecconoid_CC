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

// Base class for all Enemy Objects (things that move and attack 
// the player, generally!)
//

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyObjectBase : MonoBehaviour, Types.IRoom_EnemyObject
{
	[Header("Gamemode")]
	[SerializeField] protected bool m_bEugatronSpecific = false;

	// We'll be hiding the sprite when we reset positions...
	protected SpriteRenderer m_gcRenderer = null;
	// Record our initial position, so when the player dies we can reset to where we spawned...
	protected Vector3 m_vInitPosition = Vector3.zero;
	// This will flip based on player being alive or not...
	protected bool m_bBehaviourCanUpdate = false;



	void Awake()
	{
		m_gcRenderer = GetComponent<SpriteRenderer>();
	}




	void OnDisable()
	{
		OnRoomExit();
	}




	// Record the initial position, and setup callback if we're in Eugatron
	//
	virtual public void OnRoomEnter()
	{
		m_vInitPosition = transform.position;
		if(m_bEugatronSpecific) Messenger.AddListener(Types.s_sEUGE_ScreenTransitionFull, OnPlayerRespawn);
	}



	// Clean up the callback
	//
	virtual public void OnRoomExit()
	{
		if (m_bEugatronSpecific) Messenger.RemoveListener(Types.s_sEUGE_ScreenTransitionFull, OnPlayerRespawn);
	}



	virtual public void OnPlayerHasDied()
	{
		m_bBehaviourCanUpdate = false;

		// Explode...
		TrackHitPoints gc = GetComponent<TrackHitPoints>();
		if(null != gc) gc.PlayDeathEffects();

		// Hide...
		m_gcRenderer.enabled = false;
	}



	virtual public void OnPlayerRespawn()
	{
		m_bBehaviourCanUpdate = true;
		m_gcRenderer.enabled = true;
	}



	virtual public void OnReset()
	{
		// GNTODO: If a rigid body is attached, use that instead...
		transform.position = m_vInitPosition;
	}



	// So the room controller can spawn a warning in our respawn pos...
	//
	virtual public Vector3 GetInitPosition()
	{
		return m_vInitPosition;
	}
}
