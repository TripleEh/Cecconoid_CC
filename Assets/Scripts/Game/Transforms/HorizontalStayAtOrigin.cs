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

public class HorizontalStayAtOrigin : MonoBehaviour
{
	[SerializeField] private bool m_bTrackParent = true;
	[SerializeField] private float m_fZOffset = 0.1f;

	private void LateUpdate()
	{
		Vector3 vPos = Vector3.zero;

		if (m_bTrackParent)
			vPos = transform.parent.position;
		else
			GAssert.Assert(false, "GNTODO"); 

		transform.position = new Vector3(GameMode.GetRoomOrigin().x, vPos.y, vPos.z + m_fZOffset); 
	}
}
