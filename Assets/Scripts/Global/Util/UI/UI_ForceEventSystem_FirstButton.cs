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

public class UI_ForceEventSystem_FirstButton : MonoBehaviour
{
	[Header("Params - SETUP IN EDITOR!")]
	[SerializeField] private GameObject m_goButton = null;



	public void OnEnable()
	{
		ForceNewButton(m_goButton);
	}



	// Event System needs to force first selectable because Unity Derp. 
	// GNTODO: This will trigger any audio attached to the button... 
	//
	public void ForceNewButton(GameObject goButton)
	{
		UnityEngine.EventSystems.EventSystem es = GetComponent<UnityEngine.EventSystems.EventSystem>();
		if (es.currentSelectedGameObject != null)
		{
			var previous = es.currentSelectedGameObject.GetComponent<UnityEngine.UI.Selectable>();
			if (previous != null)
			{
				previous.OnDeselect(null);
				es.SetSelectedGameObject(null);
			}
		}
		// Select button and play its selection transition:
		es.SetSelectedGameObject(goButton);
		var current = es.currentSelectedGameObject.GetComponent<UnityEngine.UI.Selectable>();
		current.OnSelect(null);
	}
}
