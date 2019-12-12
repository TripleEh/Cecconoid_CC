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


// GNTODO:
// 
// This class runs all the time, which means placed objects in the world (rather than spawned stuff)
// will be animating even though it's not on screen. 
//
// This should probably be converted to follow the OnRoomEnter style from the Enemy Interface.
// Or knock up a second version for editor placed objects that does this, and leave this class
// for spawned enemies only. 
//
// Check how many active instances there are in the level!

[RequireComponent(typeof(SpriteRenderer))]
public class Anim_LoopFrames : MonoBehaviour
{
	[SerializeField] protected Sprite[] m_aAnimationFrames = null;
	[SerializeField] protected float m_fFPS;
	[SerializeField] protected bool m_bAutoStart = false;

	protected ulong m_iTimerHandle = 0;
	protected int m_iCurrentFrame;
	protected SpriteRenderer m_gcRenderer;
	protected float m_fDeltaFPS = 0f;


	public virtual void Start()
	{
		GAssert.Assert(m_aAnimationFrames.Length != 0, "Anim_Loop has no frames!");
		m_fDeltaFPS = 1.0f / m_fFPS;
		m_iTimerHandle = 0;
		m_gcRenderer = GetComponent<SpriteRenderer>();

		if (m_bAutoStart) StartAnimation();
	}



	private void OnDisable()
	{
		StopAnimation();
	}


	private void OnDestroy()
	{
		StopAnimation();
	}


	public virtual void StartAnimation()
	{
		if (m_iTimerHandle == 0) UpdateAnimFrame();
	}



	public virtual void StopAnimation()
	{
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, UpdateAnimFrame);
		m_iTimerHandle = 0;
	}



	protected virtual void UpdateAnimFrame()
	{
		++m_iCurrentFrame;
		if (m_iCurrentFrame == m_aAnimationFrames.Length) m_iCurrentFrame = 0;

		m_gcRenderer.sprite = m_aAnimationFrames[m_iCurrentFrame];
		m_iTimerHandle = TimerManager.AddTimer(m_fDeltaFPS, UpdateAnimFrame);
	}


	public void SetAnimFrames(Sprite[] aAnimFrames)
	{
		m_aAnimationFrames = aAnimFrames;
	}
}
