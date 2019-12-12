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

[RequireComponent(typeof(AudioSource))]
public class PlaySFXOnAnimationEvent : MonoBehaviour
{
	private AudioSource m_gcAudio = null;

	private void Awake()
	{
		m_gcAudio = GetComponent<AudioSource>();
		GAssert.Assert(null != m_gcAudio, "PlaySFXOnAnimationEvent on gameObject without an Audio Source component: " + gameObject.name);
	}

	public void OnAnimationEvent()
	{
		m_gcAudio.Play();
	}
}
