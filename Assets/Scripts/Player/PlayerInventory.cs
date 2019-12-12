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

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	// Bitfield for the inventory...
	private ulong m_iInventoryItems;


	// Player Inventory Objects prefabs...
	[Header("Weapons - SETUP IN EDITOR!")]
	[SerializeField] private GunBase m_gcWeapon_ShotSingle = null;
	[SerializeField] private GunBase m_gcWeapon_ShotDouble = null;
	[SerializeField] private GunBase m_gcWeapon_ShotTriple = null;
	[SerializeField] private GunBase m_gcWeapon_LaserSingle = null;
	[SerializeField] private GunBase m_gcWeapon_LaserDouble = null;
	[SerializeField] private GunBase m_gcWeapon_LaserTriple = null;
	[SerializeField] private GameObject m_goOption = null;

	[Header("Dropped Weapons - SETUP IN EDITOR!")]
	[SerializeField] private GameObject m_gcPowerup_ShotDouble = null;
	[SerializeField] private GameObject m_gcPowerup_ShotTriple = null;
	[SerializeField] private GameObject m_gcPowerup_LaserSingle = null;
	[SerializeField] private GameObject m_gcPowerup_LaserDouble = null;
	[SerializeField] private GameObject m_gcPowerup_LaserTriple = null;
	[SerializeField] private GameObject m_gcPowerup_Speed2 = null;
	[SerializeField] private GameObject m_gcPowerup_Speed1 = null;
	[SerializeField] private GameObject m_goPowerup_Option = null;
	[SerializeField] private float m_fSpawnRadius = 0.4f;

	[Header("Exit Key Prefab - SETUP IN EDITOR")]
	[SerializeField] private GameObject m_goExitKeyPrefab = null;
	private GameObject m_goSpawnedExitKey;


	// Sets iItem bit to high in the inventory bitfield
	//
	public void SetInventoryItem(ulong iItem)
	{
		m_iInventoryItems |= iItem;
	}



	// Checks the bitfield to see if a given bit is high
	//
	public bool HasInventoryItem(ulong iItem)
	{
		return (bool)((m_iInventoryItems & iItem) != 0);
	}



	// Clears iItem bit 
	// 
	public void ClearInventoryItem(ulong iItem)
	{
		m_iInventoryItems &= ~(iItem);
	}



	// m_goOption is actually a reference to the object attached to the player ship :)
	// Clears the inventory bitfield, and turns off the Option powerup
	//
	public void SetDefaults()
	{
		m_iInventoryItems = 0x0;
		m_goOption.SetActive(false);


		m_gcWeapon_ShotSingle.SetCanFire(false);
		m_gcWeapon_ShotDouble.SetCanFire(false);
		m_gcWeapon_ShotTriple.SetCanFire(false);
		m_gcWeapon_LaserSingle.SetCanFire(false);
		m_gcWeapon_LaserDouble.SetCanFire(false);
		m_gcWeapon_LaserTriple.SetCanFire(false);
	}



	// Strictly speaking this is a debug only function...
	// Gives all inventory items to the player.
	//
	public void AddAll()
	{
		m_iInventoryItems = Types.s_iINV_ALL;
		m_goOption.SetActive(true);
	}



	// Return the highest equipped weapon...
	//
	public GunBase GetEquippedWeapon()
	{
		if(HasInventoryItem(Types.s_iINV_Laser_Triple)) return m_gcWeapon_LaserTriple;
		if(HasInventoryItem(Types.s_iINV_Laser_Double)) return m_gcWeapon_LaserDouble;
		if(HasInventoryItem(Types.s_iINV_Laser_Single)) return m_gcWeapon_LaserSingle;
		if(HasInventoryItem(Types.s_iINV_Shot_Triple)) return m_gcWeapon_ShotTriple;
		if(HasInventoryItem(Types.s_iINV_Shot_Double)) return m_gcWeapon_ShotDouble;
		return m_gcWeapon_ShotSingle;
	}



	public uint GetSpeedMultiplier()
	{
		if(HasInventoryItem(Types.s_iINV_Speed2)) return 2;
		else if (HasInventoryItem(Types.s_iINV_Speed1)) return 1;
		return 0;
	}



	public GameObject GetPlayerOption()
	{
		return m_goOption;
	}



	public void DropInventory()
	{
		Vector2 vPos = transform.position;


		// Only drop weapons in Cecconoid and the cheat mode isn't enabled...
		if(GameInstance.Object.m_bIsCecconoid && GameInstance.Object.GetPlayerState().m_bCanDrop)
		{
			// Count how many items we have...
			ulong iV = m_iInventoryItems;
			ulong iC;
			for (iC = 0; iV > 0; iC++)
				iV &= iV - 1;

			// Store what we want to respawn in here...
			List<GameObject> aSpawnList = new List<GameObject>();

			// Collate the things we have...
			if (HasInventoryItem(Types.s_iINV_Laser_Triple)) aSpawnList.Add(m_gcPowerup_LaserTriple);
			if (HasInventoryItem(Types.s_iINV_Laser_Double)) aSpawnList.Add(m_gcPowerup_LaserDouble);
			if (HasInventoryItem(Types.s_iINV_Laser_Single)) aSpawnList.Add(m_gcPowerup_LaserSingle);
			if (HasInventoryItem(Types.s_iINV_Shot_Triple)) aSpawnList.Add(m_gcPowerup_ShotTriple);
			if (HasInventoryItem(Types.s_iINV_Shot_Double)) aSpawnList.Add(m_gcPowerup_ShotDouble);
			if (HasInventoryItem(Types.s_iINV_Speed2)) aSpawnList.Add(m_gcPowerup_Speed2);
			if (HasInventoryItem(Types.s_iINV_Speed1)) aSpawnList.Add(m_gcPowerup_Speed1);
			if (HasInventoryItem(Types.s_iINV_Option)) aSpawnList.Add(m_goPowerup_Option);

			if(iC > 1)
			{
				// If we're holding more than one power up, subtract the count by 1 so you
				// lose something when you die...
				--iC;

				// Shuffle the list
				int iN = aSpawnList.Count;
				while (iN > 1)
				{
					iN--;
					int k = Random.Range(0, iN);
					GameObject go = aSpawnList[k];
					aSpawnList[k] = aSpawnList[iN];
					aSpawnList[iN] = go;
				}

				// Spawn iC items...
				for(ulong i = 0; i < iC; ++i)
					Instantiate(aSpawnList[(int)i], vPos + Random.insideUnitCircle * m_fSpawnRadius, Quaternion.identity);
			}
			else if( iC > 0)
				// We only had one power-up, so spawn it so the player doesn't feel hard done by
				// AREN'T I NICE TO YOU
				Instantiate(aSpawnList[0], vPos + Random.insideUnitCircle * m_fSpawnRadius, Quaternion.identity);
		}
		
		SetDefaults();
	}



	public void SpawnExitKey()
	{
		GAssert.Assert(null != m_goExitKeyPrefab, "Exit Key Prefab has not been setup in the editor!");
		m_goSpawnedExitKey = Instantiate(m_goExitKeyPrefab, transform.position, Quaternion.identity);

		// Unlock the achievement
		GameMode.AchievementUnlocked(Types.EAchievementIdentifier._GotTheKey);
	}



	public bool IsFullPower()
	{
		return (HasInventoryItem(Types.s_iINV_Laser_Triple) && HasInventoryItem(Types.s_iINV_Option) && HasInventoryItem(Types.s_iINV_Speed2));
	}


	public void DespawnExitKey()
	{
		Destroy(m_goSpawnedExitKey);
	}
}
