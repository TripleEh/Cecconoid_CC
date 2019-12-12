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

// We only have one game event atm, and that's completing the game!
// But this uses the same process as AddScore/AddMultiplier
//

public class AddGameEvent : MonoBehaviour
{
	enum EGameEvent
	{
		_EXIT_KEY,
	};

	[Header("Tweakables")]
	[SerializeField] private EGameEvent m_iEvent = EGameEvent._EXIT_KEY;

	public bool AddGameEventForPlayer()
	{
		switch(m_iEvent) { case EGameEvent._EXIT_KEY: GameGlobals.SetGameEvent(Types.s_iGE_ExitKey); break; }
		return true;
	}
}
