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

public class GameOverScreen : MonoBehaviour
{
	[Header("SETUP IN EDITOR")]
	[SerializeField] private GameObject m_gcCanvas = null;
	[SerializeField] private GameObject m_goNewHigh = null;
	[SerializeField] private GameObject m_goNoHigh = null;


	private void Awake()
	{
		GAssert.Assert(null != m_gcCanvas, "GameOver screen setup without canvas group reference");
	}

	private void Start()
	{
		OnHideInstant();
	}

	public void SetDefaults()
	{
		OnHideInstant();
	}

	public void OnShowInstant(bool bIsCecconoid)
	{
		if (GameGlobals.s_bCEcconoidHighScoreEntryThisGo || GameGlobals.s_bEugatronHighScoreEntryThisGo) m_goNewHigh.SetActive(true); else m_goNoHigh.SetActive(true);
		m_gcCanvas.SetActive(true);
	}

	public void OnHideInstant()
	{
		m_gcCanvas.SetActive(false);
		m_goNewHigh.SetActive(false);
		m_goNoHigh.SetActive(false);
	}
}
