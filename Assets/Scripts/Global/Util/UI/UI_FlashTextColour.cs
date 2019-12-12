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



[RequireComponent(typeof(Text))]
public class UI_FlashTextColour : MonoBehaviour
{
	public bool m_bIsEnabled = false;

	private Text m_gcText = null;
	private bool m_bPing = false;

	void Start()
	{
		m_gcText = GetComponent<Text>();
	}

	void Update()
	{
		if(!m_bIsEnabled) return;

		if (m_bPing) m_gcText.color = new Color(1f, 0f,0f,1f);
		else m_gcText.color = new Color(1f, 1f, 1f, 1f);

		m_bPing ^= true;
	}
}
