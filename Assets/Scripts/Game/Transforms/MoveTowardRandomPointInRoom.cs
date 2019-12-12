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

[RequireComponent(typeof(Rigidbody2D))]
public class MoveTowardRandomPointInRoom : MonoBehaviour
{
	[Header("Tweakables")]
	// Movement speed for the object
	[SerializeField] protected float m_fMovementSpeed = 0.15f;
	// Scale of the unit sphere we use to find a random point
	[SerializeField] protected float m_fRandomPointDistance = 1.0f;

	// Movement direction
	protected Vector3 m_vTraj = Vector3.zero;
	// Rigid Body for collision detection
	protected Rigidbody2D m_gcRgdBdy = null;
	// Handle to the timer we use to make the next decision
	protected ulong m_iTimerHandle;
	// Flag to track if we're actually moving this tick...
	// Set to false initially, for the derived classes which skip Start()
	protected bool m_bIsActive { get; set; } = false;



	// Setup
	//
	virtual public void Start()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GAssert.Assert(null != m_gcRgdBdy, "Unable to get rigidbody!" );
		m_bIsActive = true;
		GetRandomPoint();
	}


	// Remove the current timer on this object
	//
	protected void OnDisable()
	{
		m_bIsActive = false;
		TimerManager.ClearTimerHandler(m_iTimerHandle, GetRandomPoint);
	}


	// Enemy Behaviours can pause movement if they so wish
	//
	public void SetIsUpdating( bool bState )
	{
		m_bIsActive = bState;
	}


	// Gets a random point within a unit sphere (flattened to a circle), that's scaled
	// by m_fRandomPointDistance. 
	// If the point lies outside the room bounds, it's changed to be an offset from 
	// the room's origin. This is enough to prevent bunching along the walls over time
	//
	public void GetRandomPoint()
	{
		if(!m_bIsActive) return;

		Vector3 vPoint = transform.position + (Random.insideUnitSphere * m_fRandomPointDistance);
		vPoint.z = transform.position.z;

		// Flip points out of bounds to somewhere on the other side of the arena
		if (!GameMode.BoundsCheckPosition(vPoint))
		{
			Vector3 vOrigin = GameMode.GetRoomOrigin();
			if (vPoint.x > vOrigin.x + Types.s_fRoomBoundsX) vPoint.x = vOrigin.x - vPoint.x;
			if (vPoint.x < vOrigin.x - Types.s_fRoomBoundsX) vPoint.x = vOrigin.x + vPoint.x;
			if (vPoint.y > vOrigin.y + Types.s_fRoomBoundsY) vPoint.y = vOrigin.y - vPoint.y;
			if (vPoint.y < vOrigin.y - Types.s_fRoomBoundsY) vPoint.y = vOrigin.y + vPoint.y;
		}

		m_vTraj = (vPoint - transform.position).normalized;

		// Change direction randomly
		m_iTimerHandle = TimerManager.AddTimer(Random.Range( 1.5f, 7f ), GetRandomPoint);
	}


	// Move
	//
	public virtual void FixedUpdate()
	{
		if (!m_bIsActive) return;
		if (TimerManager.IsPaused()) return;
		if (null != GameInstance.Object.GetPlayerState() && !GameInstance.Object.GetPlayerState().PlayerIsAlive()) return;

		m_gcRgdBdy.MovePosition(transform.position + ((m_vTraj * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
	}


	// Collisions should just force the selection of a new point to move
	// toward, regardless of how many events this triggers. Clears any 
	// existing timers...
	//
	public void OnCollisionEnter2D(Collision2D collision)
	{
		TimerManager.ClearTimerHandler(m_iTimerHandle, GetRandomPoint);
		if(!collision.gameObject.CompareTag(Types.s_sTag_PlayerBullets))
			GetRandomPoint();
	}
}
