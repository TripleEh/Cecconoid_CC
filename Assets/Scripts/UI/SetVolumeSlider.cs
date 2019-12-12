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

[RequireComponent(typeof(Slider))]
public class SetVolumeSlider : MonoBehaviour
{
	public enum ESliderID
	{
		_MASTER,
		_MUSIC,
		_SFX,
	}

	[Header("Slider Type")]
	[SerializeField] private ESliderID m_iSliderID = ESliderID._MASTER;

	private Slider m_gcSlider = null;



	void Awake()
	{
		m_gcSlider = GetComponent<Slider>();
		GAssert.Assert(null != m_gcSlider, "SetVolumeSlider on gameObject without a slider component! " + gameObject.name);
	}



	void OnEnable()
	{
		switch(m_iSliderID)
		{
			case ESliderID._MASTER: m_gcSlider.value = GameGlobals.s_fUI_SliderMaster; break;
			case ESliderID._MUSIC: m_gcSlider.value = GameGlobals.s_fUI_SliderMusic; break;
			case ESliderID._SFX: m_gcSlider.value = GameGlobals.s_fUI_SliderSFX; break;
		}
	}
}
