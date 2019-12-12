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
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_SetDevModeButtonClickParams : MonoBehaviour
{

	// Public to allow debugging in the inspector...
	//
	public Vector3 m_vPos = Vector3.zero;
	public GameObject m_goRoomController = null;

	
	public void SetParams(Vector3 vPos, GameObject go, UI_DevMenu_ButtonResponders responder)
	{
		m_goRoomController = go;
		m_vPos = vPos;
		Button gcButt = GetComponent<Button>();
		gcButt.onClick.AddListener(delegate { responder.RoomJumpButtonResponder(m_vPos, ref m_goRoomController); });
	}
}
