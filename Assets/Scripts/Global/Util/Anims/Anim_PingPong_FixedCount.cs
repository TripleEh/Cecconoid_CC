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

// This is a very lazy hack, specifically for the full screen transition on Eugatron. 
// Plays a ping pong animation a set number of times, before disabling the gameObject

public class Anim_PingPong_FixedCount : Anim_PingPong
{
	// How many times to loop the ping pong before cutting out?
	[SerializeField] private int m_iLoopCount = 1;
	// Where are we, frame wise?
	protected int m_iAnimCount = 0;

	

	// Hide the base class start, and just set the frame count to zero
	public override void Start()
	{
		m_iAnimCount = 0;
	}
	


	// Belt and braces...
	private void OnEnable()
	{
		m_iAnimCount = 0;
	}
	  


	// Make sure we're setup correctly each time, rather than rely on the StartFunction...
	public override void StartAnimation()
	{
		m_iAnimCount = 0;
		m_iTimerHandle = 0;
		m_iDir = EAnimDirection._FORWARD;
		m_gcRenderer = GetComponent<SpriteRenderer>();
		GAssert.Assert(m_aAnimationFrames.Length > 0, "PingPongFixedCount not setup!" + gameObject.name);
	}
	
	

	// Different to the base class, we're NOT running on a timer here, we're using the
	// Update function from monobehaviour, going flat out, and then disabling the 
	// gameobject when we're done...
	private void FixedUpdate()
	{


		switch (m_iDir)
		{
			case EAnimDirection._FORWARD:
				{
					++m_iCurrentFrame;
					if (m_iCurrentFrame == m_aAnimationFrames.Length)
					{
						--m_iCurrentFrame;
						m_iDir = EAnimDirection._BACKWARD;
					}
				}
				break;
			case EAnimDirection._BACKWARD:
				{
					--m_iCurrentFrame;
					if (m_iCurrentFrame < 0)
					{
						m_iCurrentFrame = 0;
						m_iDir = EAnimDirection._FORWARD;
					}
				}
				break;
		}

		m_gcRenderer.sprite = m_aAnimationFrames[m_iCurrentFrame];
		
		++m_iAnimCount;
		if (m_iAnimCount >= (m_aAnimationFrames.Length * m_iLoopCount) * 2)
		{
			base.StopAnimation();
			gameObject.SetActive(false);
		}
	}
}
