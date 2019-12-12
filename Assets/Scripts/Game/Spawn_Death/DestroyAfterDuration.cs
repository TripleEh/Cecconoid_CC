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

public class DestroyAfterDuration : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private float m_fTimeToLive = 1.0f;
	[SerializeField] private Material m_gcWarningMaterial = null;
	[SerializeField] private EGameSFX m_iSFX = EGameSFX._SFX_PLAYER_POWERUP_MISSED;
	[SerializeField] private bool m_bPlaySFXWhenTimingOut = false;

	private float m_fEventTime = 0.0f;
	private Types.EWarnState m_iState = Types.EWarnState._IDLE;



	void Start()
	{
		m_fEventTime = TimerManager.fGameTime;
	}




	public void Update()
	{
		if (m_iState == Types.EWarnState._IDLE)
		{
			if (TimerManager.fGameTime - m_fEventTime > m_fTimeToLive / 2.0f)
			{
				// Change material if that's setup...
				SpriteRenderer gcRenderer = GetComponent<SpriteRenderer>();
				if (null != m_gcWarningMaterial && null != gcRenderer) gcRenderer.material = m_gcWarningMaterial;

				// Change state...
				m_iState = Types.EWarnState._WARNINGPLAYER;
				return;
			}
		}
		else if (TimerManager.fGameTime - m_fEventTime >= m_fTimeToLive)
		{
			if (m_bPlaySFXWhenTimingOut) GameInstance.Object.GetAudioManager().PlayAudioAtLocation(transform.position, m_iSFX);
			Destroy(gameObject);
		}
	}
}
