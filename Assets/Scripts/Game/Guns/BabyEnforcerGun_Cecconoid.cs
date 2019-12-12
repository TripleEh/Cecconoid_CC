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


// Slight variation of the gun written for Eugatron, that 
// implements the Room_EnemyObject interface used to turn 
// objects on and off in Cecconoid...
//

[RequireComponent(typeof(TrackHitPoints))]
public class BabyEnforcerGun_Cecconoid : BabyEnforcerGun, Types.IRoom_EnemyObject
{
	private Vector3 m_vInitPosition = Vector3.zero;

	protected override void Awake()
	{
		// Do Nothing
	}

	protected override void Start()
	{
		// Do nothing
	}



	override public void OnRoomEnter()
	{
		// Validate!
		{
			m_gcGameInstance = GameInstance.Object;
			GAssert.Assert(null != m_gcGameInstance, "Unable to get GameInstance!");

			m_goPlayer = m_gcGameInstance.GetPlayerObject();
			GAssert.Assert(null != m_goPlayer, "Unable to get player!");

			BulletMovementSpark gc = m_BulletPrefab.GetComponent<BulletMovementSpark>();
			GAssert.Assert(null != gc, "Baby Enforcer Gun must have bullets of type BulletMovementSpark");

			m_gcHealth = GetComponent<TrackHitPoints>();
			GAssert.Assert(null != m_gcHealth, "Unable to get health!");
		}

		base.SetCanFire(true, Random.Range(0.5f, 3.5f));
		m_bSetup = true;
	}



	public override void OnRoomExit()
	{
		base.SetCanFire(false);
		base.OnRoomExit();
	}


	public override void OnPlayerHasDied()
	{
		base.SetCanFire(false);
	}



	public override void OnPlayerRespawn()
	{
		transform.position = m_vInitPosition;
		base.SetCanFire(true, Random.Range(0.5f, 3.5f));
	}



	private void OnDisable()
	{
		base.OnDeInit();
		if (m_iTimerHandle > 0) TimerManager.ClearTimerHandler(m_iTimerHandle, Fire);
	}
}
