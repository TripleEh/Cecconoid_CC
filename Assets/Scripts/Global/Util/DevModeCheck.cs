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
using UnityEngine.SceneManagement;


// If this is the only scene and we are actually IN the editor, then load the persistent scene
// so we have everything setup and configured correctly for play. 
//
// The persistent scene will be at index 0, as it's the first scene we would load in a release build. 
//
// This is a destructive scene load, so maintain this gameObject so the GameStateManager can pick it
// up and jump straight into gs_Game_In, without us needing to go through menus, etc. 
//
public class DevModeCheck : MonoBehaviour
{
	[HideInInspector]
	public string m_sSceneName = "";
	public bool m_bIsEugatron = false;
	
	void Awake()
	{
		if (Application.isEditor && SceneManager.sceneCount == 1)
		{
			Scene scene = SceneManager.GetActiveScene();
			m_sSceneName = scene.name;
			DontDestroyOnLoad(gameObject);
			SceneManager.UnloadSceneAsync(scene);
			SceneManager.LoadScene(0);
		}
		else Destroy(gameObject);
  }
}
