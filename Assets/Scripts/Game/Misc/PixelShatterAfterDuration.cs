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

[RequireComponent(typeof(SpriteRenderer))]
public class PixelShatterAfterDuration : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] private GameObject m_goPixelShatterPrefab = null;
	[SerializeField] private Types.EPixelShatterLife m_iPixelShatterTTL = Types.EPixelShatterLife._MEDIUM;
	[SerializeField] private float m_fShatterDelay = 1.0f;
	[SerializeField] private float m_fShatterVelocity = 2.0f;

	private Sprite m_gcSprite;
	private float m_fEventTime;

	// Start is called before the first frame update
	void Start()
	{
		GAssert.Assert(null != m_goPixelShatterPrefab, "No Prefab particle effect assigned... ");
		m_gcSprite = GetComponent<SpriteRenderer>().sprite;
		GAssert.Assert(null != m_gcSprite, "Sprite Renderer has no sprite assigned!");

		m_fEventTime = TimerManager.fGameTime + m_fShatterDelay;
	}

	// Update is called once per frame
	void Update()
	{
		if(TimerManager.fGameTime > m_fEventTime)
		{
			SpriteToParticleSystem.ExplodeSprite(transform.position, m_fShatterVelocity, m_goPixelShatterPrefab, m_gcSprite, m_iPixelShatterTTL);
			GetComponent<SpriteRenderer>().enabled = false;
			Destroy(gameObject);
		}
	}
}
