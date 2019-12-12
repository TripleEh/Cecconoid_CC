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

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerGun))]
[RequireComponent(typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{

	// For our State Machine...
	enum EPlayerControllerState
	{
		_PLAYER_ACTIVE,
		_TRANSITIONING_ROOMS,
	}


	// Reference to the PlayerActions defined with InControl
	private PlayerActions m_PlayerActionsBindings;

	// Directions for the twin sticks
	private Vector2 m_vMovementTrajectory;
	private Vector2 m_vFireTrajectory;

	// Player Ship components
	private Rigidbody2D m_gcRgdBdy;
	private CircleCollider2D m_gcCollision;

	// We need to check against the player's state when making decision
	private PlayerState m_gcPlayerState;

	// For our state machine
	private EPlayerControllerState m_iControllerState = EPlayerControllerState._PLAYER_ACTIVE;

	private float m_fTransitionEventTime = 0.0f;

	private PlayerSpriteDirectionUpdater m_gcSpriteUpdater = null;




	// Grab all the components we need control of. 
	//
	void Awake()
	{
		Debug.Log("Player Controller Awake...");
		m_PlayerActionsBindings = new PlayerActions();

		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GAssert.Assert(null != m_gcRgdBdy, "Player prefab is missing a rigid body!");
		m_gcPlayerState = GetComponent<PlayerState>();
		GAssert.Assert(null != m_gcPlayerState, "Player prefab is missing the PlayerState!");
		m_gcCollision = GetComponent<CircleCollider2D>();
		GAssert.Assert(null != m_gcCollision, "Player prefab is missing the colision!");
		m_gcSpriteUpdater = GetComponent<PlayerSpriteDirectionUpdater>();
		GAssert.Assert(null != m_gcSpriteUpdater, "Unable to get Sprite Updater!");
	}



	// Setup the params for the lerp between rooms. 
	// Collision must be disabled, as it's possible to be scraping a polygon 
	// collider when the doorway trigger is touched. It'll possibly look a 
	// little odd, but better to teleport through something solid than be
	// stuck off-screen!
	//
	public void BeginRoomTransition()
	{
		m_fTransitionEventTime = TimerManager.fGameTime;
		m_iControllerState = EPlayerControllerState._TRANSITIONING_ROOMS;
		m_gcCollision.enabled = false;
	}



	public void BeginTeleport()
	{
		m_iControllerState = EPlayerControllerState._TRANSITIONING_ROOMS;
		m_gcCollision.enabled = false;
	}



	public void EndRoomTransition()
	{
		// Do we do anything?
	}



	public void EndTeleport()
	{
		m_iControllerState = EPlayerControllerState._PLAYER_ACTIVE;
		m_gcCollision.enabled = true;
	}


	public void Update()
	{
		// Clear Input
		{
			m_vMovementTrajectory = Vector3.zero;
			m_vFireTrajectory = Vector3.zero;
		}

		// Handle Game Pause
		{
			if (m_PlayerActionsBindings.UI_Pause.IsPressed) GameMode.PauseGame();
			if (m_PlayerActionsBindings.UI_Back.IsPressed) GameMode.UnPauseGame();
		}

		// Handle the developer menu
		{
			if (Application.isEditor && m_PlayerActionsBindings.UI_DevMenu.WasPressed) GameMode.ToggleDevMenu();
		}
	}



	// Do the correct update for the current state.
	//
	private void FixedUpdate()
	{
		if (TimerManager.IsPaused()) return;

		switch (m_iControllerState)
		{
			case EPlayerControllerState._PLAYER_ACTIVE: PlayerUpdate(); break;
			case EPlayerControllerState._TRANSITIONING_ROOMS: TransitionUpdate(); break;
		}
	}



	// Lerp from room to room. When finished, snap to the final position and
	// re-enable the collision.
	//
	private void TransitionUpdate()
	{
		float fRatio = (TimerManager.fGameTime - m_fTransitionEventTime) / Types.s_fCAM_RoomTransitionDuration;
		m_gcRgdBdy.MovePosition(Vector3.Lerp(GameGlobals.s_vRoomTransitionFrom, GameGlobals.s_vRoomTransitionTo, Easing.EaseInOut(fRatio, EEasingType.Quintic)));

		if (fRatio >= 1.0f)
		{
			m_iControllerState = EPlayerControllerState._PLAYER_ACTIVE;
			m_fTransitionEventTime = 0.0f;
			transform.position = GameGlobals.s_vRoomTransitionTo;
			m_gcCollision.enabled = true;
		}
	}



	// Update the controls
	//
	private void PlayerUpdate()
	{
		if (m_gcPlayerState.GetPlayerCanMove())
		{
			// Get Input
			{
				m_vMovementTrajectory = m_PlayerActionsBindings.GE_Move;
				m_vMovementTrajectory.Normalize();
				m_vFireTrajectory = m_PlayerActionsBindings.GE_Fire;
				m_vFireTrajectory.Normalize();
			}

			// Do the Deadzone
			{
				if (m_vMovementTrajectory.magnitude < Types.s_fDeadZone_Movement) m_vMovementTrajectory = Vector2.zero;
				else m_vMovementTrajectory = m_vMovementTrajectory * ((m_vMovementTrajectory.magnitude - Types.s_fDeadZone_Movement) / (1.0f - Types.s_fDeadZone_Movement));

				if (m_vFireTrajectory.magnitude < Types.s_fDeadZone_Firing) m_vFireTrajectory = Vector2.zero;
				else m_vFireTrajectory = m_vFireTrajectory * ((m_vFireTrajectory.magnitude - Types.s_fDeadZone_Firing) / (1.0f - Types.s_fDeadZone_Firing));
			}

			// Move the object
			Vector3 vTraj = (new Vector3(m_vMovementTrajectory.x, m_vMovementTrajectory.y, 0.0f) * m_gcPlayerState.GetPlayerMovementSpeed()) * TimerManager.fFixedDeltaTime;
			m_gcRgdBdy.MovePosition(transform.position + vTraj);
		}

		// Pass fire to the equipped weapon
		if (m_vFireTrajectory.magnitude > 0.0f) m_gcPlayerState.UpdateEquippedWeapon(m_vFireTrajectory, true);
		else m_gcPlayerState.UpdateEquippedWeapon(Vector2.zero, false);

		// Pass direction on to the sprite updater
		if (null != m_gcSpriteUpdater) m_gcSpriteUpdater.SetDirection(m_vMovementTrajectory);
	}



	// Some of the enemy bullets use the player's trajectory to 'guess' where the player
	// will be in a few frames time...
	//
	public Vector2 GetMovementTrajectory()
	{
		return m_vMovementTrajectory;
	}



	public void MovePlayerInstant(Vector3 vPos)
	{
		transform.position = vPos;
	}



	public bool IsAnyInput()
	{
		if (m_PlayerActionsBindings.UI_Back.HasChanged
			|| m_PlayerActionsBindings.UI_Confirm.HasChanged
			|| m_PlayerActionsBindings.UI_Pause.HasChanged
			|| Input.anyKeyDown) return true;
		else return false;
	}
}
