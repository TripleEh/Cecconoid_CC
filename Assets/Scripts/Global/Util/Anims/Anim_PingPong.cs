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

public enum EAnimDirection
{
	_FORWARD,
	_BACKWARD,
};

[RequireComponent(typeof(SpriteRenderer))]
public class Anim_PingPong : MonoBehaviour
{
	[SerializeField] protected Sprite[] m_aAnimationFrames = null;
	[SerializeField] protected float m_fFPS;
	[SerializeField] protected bool m_bAutoStart = false;

	protected EAnimDirection m_iDir = EAnimDirection._FORWARD;
	protected ulong m_iTimerHandle = 0;
	protected int m_iCurrentFrame;
	protected SpriteRenderer m_gcRenderer;
	protected float m_fDeltaFPS = 0f;


	public virtual void Start()
	{
		GAssert.Assert(m_aAnimationFrames.Length != 0, "PingPong has no frames!");
		m_fDeltaFPS = 1.0f / m_fFPS;
		m_iTimerHandle = 0;
		m_iDir = EAnimDirection._FORWARD;
		m_gcRenderer = GetComponent<SpriteRenderer>();

		if (m_bAutoStart) StartAnimation();
	}

	private void OnDestroy()
	{
		StopAnimation();
	}

	private void OnDisable()
	{
		StopAnimation();
	}



	public virtual void StartAnimation()
	{
		if (m_iTimerHandle == 0) UpdateAnimFrame();
	}



	public virtual void StopAnimation()
	{
		TimerManager.ClearTimerHandler(m_iTimerHandle, UpdateAnimFrame);
		m_iTimerHandle = 0;
	}



	protected virtual void UpdateAnimFrame()
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
		m_iTimerHandle = TimerManager.AddTimer(m_fDeltaFPS, UpdateAnimFrame);
	}


	public void SetAnimFrames(Sprite[] aAnimFrames)
	{
		m_aAnimationFrames = aAnimFrames;
	}
}
