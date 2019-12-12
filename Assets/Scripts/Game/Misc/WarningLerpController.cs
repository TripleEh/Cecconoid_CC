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

public class WarningLerpController : MonoBehaviour
{
  [Header("Tweakables")]
  [SerializeField] private LerpToPositionOverDuration m_gcLerpIn = null;
  [SerializeField] private LerpToPositionOverDuration m_gcLerpOut = null;
  [SerializeField] private bool m_bFromLeft = true;

  void Awake()
  {
    Vector3 vPos = transform.position;
    if(m_bFromLeft)
    {
      m_gcLerpIn.SetupParams(transform.position - new Vector3(Types.s_fRoomWidth, 0f, 0f), transform.position, Types.s_fDUR_RoomEntryDoorCloseDelay);
      m_gcLerpOut.SetupParams(transform.position, transform.position + new Vector3(Types.s_fRoomWidth, 0f, 0f), Types.s_fDUR_RoomEntryDoorCloseDelay + 0.75f);
      transform.position = transform.position - new Vector3(Types.s_fRoomWidth, 0f, 0f);
    }
    else
    {
      m_gcLerpIn.SetupParams(transform.position + new Vector3(Types.s_fRoomWidth, 0f, 0f), transform.position, Types.s_fDUR_RoomEntryDoorCloseDelay);
      m_gcLerpOut.SetupParams(transform.position, transform.position - new Vector3(Types.s_fRoomWidth, 0f, 0f), Types.s_fDUR_RoomEntryDoorCloseDelay + 0.75f);
      transform.position = transform.position + new Vector3(Types.s_fRoomWidth, 0f, 0f);
    }
    TimerManager.AddTimer(Types.s_fDUR_RoomEntryDoorCloseDelay + 1f, DestroyThis);
  }

  void DestroyThis()
  {
    Destroy(gameObject);
  }
}
