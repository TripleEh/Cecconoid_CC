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


public class DevMenu : MonoBehaviour
{
	[Header("UI Objects - Setup In Editor")]
	[SerializeField] private GameObject m_goEventSystem = null;
	[SerializeField] private GameObject m_goCanvas = null;
	[SerializeField] private GameObject m_gcScrollListContent = null;
	[SerializeField] private GameObject m_goScrollListButton = null;

	private float m_fScrollListOffset = -12f;


	public void Start()
	{
		OnHide();
	}

	public void SetDefaults()
	{
#if GDEBUG
		Messenger.AddListener(Types.s_sMenu_DevMenuShow, OnShow);
		Messenger.AddListener(Types.s_sMenu_DevMenuHide, OnHide);
#endif
		OnHide();
	}

	private void OnDisable()
	{
#if GDEBUG
		Messenger.RemoveListener(Types.s_sMenu_DevMenuShow, OnShow);
		Messenger.RemoveListener(Types.s_sMenu_DevMenuHide, OnHide);
#endif
	}

	public void OnShow()
	{
#if GDEBUG
		m_goEventSystem.SetActive(true);
		m_goCanvas.SetActive(true);
#endif
	}


	public void OnHide()
	{
		m_goEventSystem.SetActive(false);
		m_goCanvas.SetActive(false);
	}


	public void DepopulateRoomList()
	{
		for(int i = 0; i < m_gcScrollListContent.transform.childCount; ++i)
		{
			Transform t = m_gcScrollListContent.transform.GetChild(i);
			Destroy(t.gameObject);
		}
		m_fScrollListOffset = -12f;
	}




	public void PopulateRoomList()
	{
		UI_DevMenu_ButtonResponders gcResponder = GetComponent<UI_DevMenu_ButtonResponders>();
		GAssert.Assert(null != gcResponder, "Dev Menu not attached to the same object as the button responder!");

		GameObject[] aGO = GameObject.FindGameObjectsWithTag(Types.s_sTAG_Doorway);

		foreach (GameObject go in aGO)
		{
			Types.SDevMenuDoorDetails deets = go.GetComponent<Doorway>().GetDoorwayDetails();

			if (null != deets._goRoom1)
			{
				GameObject goButton1 = Instantiate(m_goScrollListButton, m_gcScrollListContent.transform) as GameObject;
				GAssert.Assert(null != goButton1, "Unable to add button to scroll list!");
				goButton1.transform.localPosition = new Vector3(0f, m_fScrollListOffset, 0f);
				m_fScrollListOffset -= 20f;

				Text gcText1 = goButton1.GetComponentInChildren<Text>();
				gcText1.text = deets._goRoom1.name.ToString();

				UI_SetDevModeButtonClickParams gcParams = goButton1.GetComponent<UI_SetDevModeButtonClickParams>();
				if (null != gcParams) gcParams.SetParams(deets._vPos1, deets._goRoom1, gcResponder);
			}


			if (null != deets._goRoom2)
			{
				GameObject goButton2 = Instantiate(m_goScrollListButton, m_gcScrollListContent.transform) as GameObject;
				GAssert.Assert(null != goButton2, "Unable to add button to scroll list!");
				goButton2.transform.localPosition = new Vector3(0f, m_fScrollListOffset, 0f);
				m_fScrollListOffset -= 20f;

				Text gcText2 = goButton2.GetComponentInChildren<Text>();
				gcText2.text = deets._goRoom2.name.ToString();

				UI_SetDevModeButtonClickParams gcParams = goButton2.GetComponent<UI_SetDevModeButtonClickParams>();
				if (null != gcParams) gcParams.SetParams(deets._vPos2, deets._goRoom2, gcResponder);
			}
		}
	}
}
