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

public class AddPowerup : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private Types.EPowerUp m_iPowerUp = Types.EPowerUp._SHOT_DOUBLE;

	public bool AddPowerupToPlayer()
	{
		PlayerState gcState = GameInstance.Object.GetPlayerState();
		if(null != gcState) return gcState.AddPowerUp(m_iPowerUp); else return false;
	}
}
