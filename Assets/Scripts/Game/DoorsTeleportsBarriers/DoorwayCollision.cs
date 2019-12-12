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


// Doorway collision is the sprite of the door and the box collider around it. 
// These animate when opening / closing, but can be forced open or shut
// instantly as part of the room transition. 
//
// In the editor the only real change you ever need to make is the size 
// of the collider... But as there's only one door sprite...
//

public class DoorwayCollision : MonoBehaviour
{
	private Types.EDoorwayCollisionState m_iState = Types.EDoorwayCollisionState._IDLE_OPEN;
	private Types.EDirection m_iDir = Types.EDirection._VERTICAL;
	private float m_fEventTime = 0f;
	private Vector3 m_vClosedPosition;
	private Vector3 m_vOpenPosition;



	public void OpenInstant()
	{
		transform.position = m_vOpenPosition;
	}



	public void CloseInstant()
	{
		transform.position = m_vClosedPosition;
	}



	public void Open()
	{
		m_fEventTime = TimerManager.fGameTime;
		m_iState = Types.EDoorwayCollisionState._OPENING;
		GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_DOOR_SLIDE_UP);
	}



	public void Close()
	{
		m_fEventTime = TimerManager.fGameTime;
		m_iState = Types.EDoorwayCollisionState._CLOSING;
		GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_DOOR_SLIDE_DOWN);
	}



	public void Init(Types.EDirection iDir)
	{
		m_iDir = iDir;

		m_vClosedPosition = transform.position;

		if (m_iDir == Types.EDirection._HORIZONTAL) m_vOpenPosition = m_vClosedPosition + new Vector3(0f, 0.48f, 0.01f);
		else if (m_iDir == Types.EDirection._VERTICAL) m_vOpenPosition = m_vClosedPosition + new Vector3(0.48f, 0.0f, 0.01f);

		OpenInstant();
	}



	public void OnRoomExit()
	{

	}


	public Types.EDoorwayCollisionState GetDoorwayCollisionState()
	{
		return m_iState;
	}



	void Update()
	{
		float fRatio;
		switch (m_iState)
		{
			// Case fall-through does work in C#
			case Types.EDoorwayCollisionState._IDLE_OPEN:
			case Types.EDoorwayCollisionState._IDLE_CLOSED: break;

			case Types.EDoorwayCollisionState._OPENING:
				fRatio = (TimerManager.fGameTime - m_fEventTime) / Types.s_fDUR_DoorCloseDuration;
				transform.position = Vector3.Lerp(m_vClosedPosition, m_vOpenPosition, Easing.EaseOut(fRatio, EEasingType.Quartic));
				if (fRatio >= 1.0f)
				{
					m_iState = Types.EDoorwayCollisionState._IDLE_OPEN;
					GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_DOOR_SHUT);
				}
				break;

			case Types.EDoorwayCollisionState._CLOSING:
				fRatio = (TimerManager.fGameTime - m_fEventTime) / Types.s_fDUR_DoorCloseDuration;
				transform.position = Vector3.Lerp(m_vOpenPosition, m_vClosedPosition, Easing.EaseOut(fRatio, EEasingType.Quartic));
				if (fRatio >= 1.0f)
				{
					m_iState = Types.EDoorwayCollisionState._IDLE_CLOSED;
					GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, EGameSFX._SFX_DOOR_SHUT);
				}
				break;
		}
	}
}
