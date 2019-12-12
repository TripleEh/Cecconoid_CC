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


// So this was originally a class that displayed the 
// correct sprite for each of the 8-Directions the 
// player can move in... 
//
// But it looked rubbish, so now it's just left & right

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteDirectionUpdater : MonoBehaviour
{
	// Sprites in the t-page
	[Header("Tweakables -- STEUP IN EDITOR")]
	[SerializeField] private Sprite m_gcE = null; 
	[SerializeField] private Sprite m_gcW = null; 

	// The two particle systems place on the player ship object
	[SerializeField] private ParticleSystem m_gcTrailLeft = null;
	[SerializeField] private ParticleSystem m_gcTrailRight = null;

	// This object's renderer
	private SpriteRenderer m_gcSpriteRenderer = null;
	// Current Direction...
	private Vector3 m_vTrajectory = Vector3.zero;


	// Get what we need and verify
	//
	void Start()
	{
		GAssert.Assert(null != m_gcE, "Sprites not setup!");
		GAssert.Assert(null != m_gcW, "Sprites not setup!");

		m_gcSpriteRenderer = GetComponent<SpriteRenderer>();
		GAssert.Assert(null != m_gcSpriteRenderer, "Unable to get SpriteRenderer!");

		m_gcTrailRight.Pause();
	}



	public void SetDirection(Vector3 vTraj)
	{
		m_vTrajectory = vTraj;
	}



	// Check direction, switch sprite, and flip-flop the particle
	// systems...
	//
	void Update()
	{
		if(m_vTrajectory.magnitude < Types.s_fDeadZone_Movement) return;

		if (m_vTrajectory.x > 0f) {
			m_gcSpriteRenderer.sprite = m_gcE; 
			if(!m_gcTrailLeft.isPlaying) m_gcTrailLeft.Play();
			m_gcTrailRight.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			return;
		}

		if(m_vTrajectory.x < 0f ) {
			m_gcSpriteRenderer.sprite = m_gcW;
			m_gcTrailLeft.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			if (!m_gcTrailRight.isPlaying) m_gcTrailRight.Play();
			return;
		}
	}
}
