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

public class AddScore : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private uint m_iPoints=0;

	public bool AddScoreToPlayer()
	{
			PlayerState gcState = GameInstance.Object.GetPlayerState();
			if(null != gcState)	return gcState.AddScore(m_iPoints); else return false;
	}
}
