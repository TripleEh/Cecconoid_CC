// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.

using UnityEngine;


// Sparks are based on their namesakes from Robotron. See the Enforcers for how these are used
//
public class BulletMovementSpark : BulletMovement
{
	private float m_fSpeedModOverTime = 0.0f;
	private Vector2 m_vTrajectoryAddOverTime = Vector2.zero;
	private CircleCollider2D m_gcCircCollider = null;


	// Params for when we're sliding and about to kill ourselves...
	private bool m_bCheckToDie;
	private float m_fDieSeconds;



	new void Awake()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		m_gcCircCollider = GetComponent<CircleCollider2D>();
		m_gcGameInstance = GameInstance.Object;
		GAssert.Assert(null != m_gcGameInstance, "Bullet, unable to get GameInstance!");
	}


	
	// We need a custom Gun for this bullet!
	//
	virtual public void SparkInitBullet(Vector3 vTraj, Vector3 vTrajMod, float fMovementSpeed, float fMovementSpeedMod, bool bPlayAudio = true)
	{
		m_fSpeedModOverTime = fMovementSpeedMod;
		m_vTrajectoryAddOverTime = vTrajMod;

		InitBullet(vTraj, fMovementSpeed, bPlayAudio);
	}



	// Move in the direction we're pointing
	//
	override public void FixedUpdate()
	{
		// Skip if we're paused
		if (TimerManager.IsPaused()) return;

		// Kill ourselves if we've been sliding along collision for too long...
		if (m_bCheckToDie && TimerManager.fGameTime > m_fDieSeconds) Destroy(gameObject);

		// Bounds check to the room we're in, kill if out of bounds
		if (!GameMode.BoundsCheckPosition(transform.position, m_gcCircCollider.radius)) DeInitBullet();

		// Update position
		if (m_bIsAlive)
		{
			m_vTrajectory += m_vTrajectoryAddOverTime * TimerManager.fGameDeltaTime;
			m_fMovementSpeed += m_fSpeedModOverTime * TimerManager.fGameDeltaTime;

			Vector2 vTraj = (m_vTrajectory * m_fMovementSpeed) * TimerManager.fFixedDeltaTime;
			Vector2 vPos = new Vector2(transform.position.x + vTraj.x, transform.position.y + vTraj.y);
			m_gcRgdBdy.MovePosition(vPos);
		}
	}



	// We only care about collisions with the player, bullet will persist until its velocity
	// gets close to zero...
	//
	override public void OnCollisionEnter2D(Collision2D collision)
	{
		if (!m_bIsAlive) return;

		if (collision.gameObject.CompareTag(Types.s_sTag_Player) || collision.gameObject.CompareTag(Types.s_sTag_PlayerBullets))
			DeInitBullet();
	}



	// If we've got a Stay event, it's most likely because we're sliding along a wall
	// We WANT to do this, but not forever. Set a timer to kill ourselves
	//
	public void OnCollisionStay2D(Collision2D collision)
	{
		// Already detected, so bail...
		if(m_bCheckToDie) return;

		if (!(collision.gameObject.CompareTag(Types.s_sTag_Player) || collision.gameObject.CompareTag(Types.s_sTag_PlayerBullets)))
		{
			m_bCheckToDie = true;
			m_fDieSeconds = TimerManager.fGameTime + Types.s_fDUR_SparkCollisionSlideDuration;
		}
	}
}
