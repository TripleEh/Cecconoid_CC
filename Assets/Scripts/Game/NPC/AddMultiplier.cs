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



public class AddMultiplier : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private uint m_iMultiplier = 1;

	public bool AddMultiplierToPlayer()
	{
		PlayerState gcState = GameInstance.Object.GetPlayerState();
		if (null != gcState)
		{
			gcState.AddMultiplier(m_iMultiplier);
			return gcState.AddScore(1); // Well, even multipliers should give you points!
		}
		else return false;
	}
}
