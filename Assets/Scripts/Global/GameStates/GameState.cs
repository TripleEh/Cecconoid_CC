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

// Game States control the overall flow of the application
// This is the base class they all derive from
//

public class GameState : MonoBehaviour
{
	protected string m_sStateName = "BaseClass";
	protected GameInstance m_gcGameInstance;
	protected GameStateManager m_gcGameStateManager;
	protected UnityEngine.SceneManagement.Scene m_LoadedScene;

	virtual public void Awake()
	{
		Debug.Log("GameState: " + m_sStateName + "\n");

		m_gcGameStateManager = (GameStateManager)GameObject.Find("GameInstance").GetComponent<GameStateManager>();
		GAssert.Assert(null!=m_gcGameStateManager, "Unable to get GameStateManager");

		m_gcGameInstance = GameInstance.Object;
		GAssert.Assert(null != m_gcGameInstance, "Unable to get GameInstance");
	}

}
