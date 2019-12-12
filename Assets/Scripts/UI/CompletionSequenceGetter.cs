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


// The text contained in this class is turned on by the final room 
// controller. All that's needed here is to make sure the canvas is 
// in the right state and text always defaults to off...
//

public class CompletionSequenceGetter : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private Canvas m_gcCanvas = null;
	public Text[] m_aTextLines;


	private void Start()
	{
		GAssert.Assert(null != m_gcCanvas, "Canvas not assigned in editor!");
		SetDefaults();
	}



	public void SetDefaults()
	{
		OnHideInstant();
	}



	public void OnHideInstant()
	{
		foreach(Text line in m_aTextLines) line.enabled = false;
		m_gcCanvas.enabled = false;	
	}



	public void OnShowInstant()
	{
		foreach (Text line in m_aTextLines) line.enabled = false;
		m_gcCanvas.enabled = true;
	}
}
