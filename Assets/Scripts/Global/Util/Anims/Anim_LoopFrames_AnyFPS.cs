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

public class Anim_LoopFrames_AnyFPS : Anim_LoopFrames
{

	override public void Start()
	{
		GAssert.Assert(m_aAnimationFrames.Length != 0, "Anim_Loop has no frames!");
		m_iTimerHandle = 0;
		m_gcRenderer = GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		++m_iCurrentFrame;
		if (m_iCurrentFrame == m_aAnimationFrames.Length) m_iCurrentFrame = 0;

		m_gcRenderer.sprite = m_aAnimationFrames[m_iCurrentFrame];
	}
}
