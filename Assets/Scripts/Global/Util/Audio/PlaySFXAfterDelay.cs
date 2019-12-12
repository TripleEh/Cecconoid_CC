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

public class PlaySFXAfterDelay : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fDelaySeconds = 0.5f;
	[SerializeField] private AudioSource m_gcAudioSource = null;

	private float m_fEventTime = 0f;

	// Start is called before the first frame update
	void Start()
	{
		GAssert.Assert(null != m_gcAudioSource);
		m_fEventTime = TimerManager.fGameTime + m_fDelaySeconds;
	}

	// Update is called once per frame
	void Update()
	{
		if(m_fEventTime < 0) return;
		if(TimerManager.fGameTime >= m_fEventTime && !m_gcAudioSource.isPlaying)
		{
			m_gcAudioSource.Play();
			TimerManager.AddTimer(m_gcAudioSource.clip.length, DestroyComponent);
		}
	}

	void DestroyComponent()
	{
		Destroy(this);
	}
}
