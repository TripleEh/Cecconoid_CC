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
public class MoveTowardPlayerErratic : MonoBehaviour
{
	[Header("Tweakables")]
	[SerializeField] protected float m_fMinDecisionDuration = 0.05f;
	[SerializeField] protected float m_fMaxDecisionDuration = 0.4f;
	[SerializeField] protected float m_fMovementSpeed = 0.95f;

	protected float m_fNextEventTime = 0f;
	protected Rigidbody2D m_gcRgdBdy = null;
	protected Vector3 m_vTraj;



	private void Start()
	{
		m_gcRgdBdy = GetComponent<Rigidbody2D>();
		GetDirToPlayer();
	}



	protected void GetDirToPlayer()
	{
		// Get the rounded, 8-Way vector to the player
		Vector3 vTraj = (GameInstance.Object.GetPlayerPosition() - transform.position).normalized;
		vTraj.x = Mathf.Round(vTraj.x);
		vTraj.y = Mathf.Round(vTraj.y);

		// Convert to Deg
		float fDegs = Mathf.Atan2(vTraj.x, vTraj.y) * Mathf.Rad2Deg;

		// Add / Sub 45deg from it 
		float fNoise = Random.Range(0f, 99f);
		if (fNoise > 60) fDegs += 45f; else if (fNoise < 40) fDegs -= 45f;

		// That's our new traj
		m_vTraj = new Vector3(Mathf.Sin(fDegs * Mathf.Deg2Rad), Mathf.Cos(fDegs * Mathf.Deg2Rad), 0);

		// Randomise next decision point
		m_fNextEventTime = TimerManager.fGameTime + Random.Range(m_fMinDecisionDuration, m_fMaxDecisionDuration);
	}



	private void FixedUpdate()
	{
		if (TimerManager.IsPaused()) return;
		if (!GameMode.PlayerIsAlive()) return;
		if (TimerManager.fGameTime > m_fNextEventTime) GetDirToPlayer();

		m_gcRgdBdy.MovePosition(transform.position + ((m_vTraj * m_fMovementSpeed) * TimerManager.fFixedDeltaTime));
	}
}
