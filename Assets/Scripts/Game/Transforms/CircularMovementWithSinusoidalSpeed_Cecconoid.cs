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


// This is basically the same class as CircularMovementWithSinusoidalSpeed, which 
// was written for Eugatron. 
//
// Slightly re-arranging how this works to fit into the Cecconoid EnemyObject Interface
// which isn't brilliant, but it's a quick fix ;) 
//
public class CircularMovementWithSinusoidalSpeed_Cecconoid : CircularMovementWithSinusoidalSpeed, Types.IRoom_EnemyObject
{
	protected bool m_bIsActive = false;
	protected Vector3 m_vInitposition = Vector3.zero;


	// Save where we came from...
	//
	public override void Start()
	{
		m_vInitposition = transform.position;
	}



	// Set a random direction
	//
	public void OnRoomEnter()
	{
		// Validate
		{
			m_gcRgdBdy = GetComponent<Rigidbody2D>();
			GAssert.Assert(null != m_gcRgdBdy, "OrbitRoomOrigin: setup with no rigidbody...");
			m_gcPlayerState = GameInstance.Object.GetPlayerState();
			GAssert.Assert(null != m_gcPlayerState, "Unable to get Game Instance");
		}

		// Randomise the angle offsets
		m_fOrbitAngle = Random.Range(0f, 359f) * Mathf.Deg2Rad;
		m_fSpeedModAngle = Random.Range(0f, 359f) * Mathf.Deg2Rad;

		// Generate the initial trajectory
		m_vTrajectory = new Vector3(Mathf.Sin(m_fOrbitAngle), Mathf.Cos(m_fOrbitAngle), 0.0f); ;
		m_vInitposition = transform.position;
		m_bIsActive = true;

		OnReset();
	}



	public void OnRoomExit()
	{
		m_bIsActive = false;
	}



	public void OnPlayerHasDied()
	{
		m_bIsActive = false;
	}



	public void OnPlayerRespawn()
	{
		m_bIsActive = true;
	}


	public void OnReset()
	{
		transform.position = m_vInitposition;
	}


	public override void FixedUpdate()
	{
		if(m_bIsActive) base.FixedUpdate(); ;
	}
}
