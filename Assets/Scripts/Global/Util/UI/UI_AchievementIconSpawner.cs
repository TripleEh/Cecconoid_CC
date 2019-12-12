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

public class UI_AchievementIconSpawner : MonoBehaviour
{
	[Header("Assets")]
	[SerializeField] private GameObject m_goAchievementPrefab = null;

	void Start()
	{
		GAssert.Assert(null != m_goAchievementPrefab, "Canvas_Achievements: No Achievement Icon Prefab set!");
	}

	public void SpawnAchievement(ref Types.SAchievementInfo info)
	{
		GameObject go = Instantiate(m_goAchievementPrefab, transform, false);

		UI_AchievementIconSetup gc = go.GetComponent<UI_AchievementIconSetup>();
		GAssert.Assert(null != gc, "Unable to setup Achievement Icon Info!");
		gc.SetInfo(ref info);

		LerpToPositionOverDuration gc2 = go.GetComponent<LerpToPositionOverDuration>();
		GAssert.Assert(null != gc2, "Unable to get the lerp component on the achievement icon!");
		gc2.SetupParams(go.transform.position, go.transform.position + new Vector3(0f, 39f, 0f), 1.8f);
	}
}
