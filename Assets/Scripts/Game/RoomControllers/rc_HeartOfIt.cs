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

using System.Collections;
using UnityEngine;

public class rc_HeartOfIt : RoomControllerBase
{
	[Header("Heart Objects")]
	[SerializeField] private GameObject m_goRoomObjects = null;
	[SerializeField] private GameObject m_goLaser = null;
	[SerializeField] private GameObject m_goScreenWhitener = null;
	[SerializeField] private GameObject m_goScreenDarkener = null;

	private void Awake()
	{
		GAssert.Assert(null != m_goRoomObjects, "HearOfItRoom not setup!");
		m_goRoomObjects.SetActive(false);
	}



	public override void OnRoomEnter()
	{
		m_sRoomName = this.gameObject.name;
#if GDEBUG
		GameInstance.Object.SetDebugHudRoomName(m_sRoomName);
#endif
		m_goRoomObjects.SetActive(true);

		Messenger.AddListener(Types.s_sGF_InduceSlowdown, InduceSlowDown);
	}



	private void OnDisable()
	{
		Messenger.RemoveListener(Types.s_sGF_InduceSlowdown, InduceSlowDown);
	}




	public void InduceSlowDown()
	{
		m_goLaser.SetActive(false);
		StartCoroutine(RunCompletionSequence());
	}



	public IEnumerator RunCompletionSequence()
	{
		// Unlock the achievement
		GameMode.AchievementUnlocked(Types.EAchievementIdentifier._CompletedCecconoid);

		bool bSlowingDown = true;
		do
		{
			if (TimerManager.fGameDeltaScale > 0.01f) TimerManager.SetGameTimeScale(TimerManager.fGameDeltaScale - 0.015f); else bSlowingDown = false;
			yield return new WaitForSeconds(0.1f);
		} while (bSlowingDown);
		yield return new WaitForSeconds(1f);

		Instantiate(m_goScreenWhitener, transform.position + new Vector3(0f, 0f, -0.76f), Quaternion.identity);
		yield return new WaitForSeconds(5f);


		CompletionSequenceGetter aLines = GameInstance.Object.GetCompletionSequence();
		GAssert.Assert(null != aLines, "Have the completion sequence lines been added to the game instance?");


		aLines.OnShowInstant();
		aLines.m_aTextLines[0].enabled = true;
		yield return new WaitForSeconds(3.5f);

		aLines.m_aTextLines[1].enabled = true;
		yield return new WaitForSeconds(3.5f);

		aLines.m_aTextLines[2].enabled = true;
		yield return new WaitForSeconds(3.5f);

		aLines.m_aTextLines[3].enabled = true;
		yield return new WaitForSeconds(2.5f);

		aLines.m_aTextLines[4].enabled = true;
		yield return new WaitForSeconds(3.5f);

		// Fade screen to black
		GameInstance.Object.HideHud();
		Instantiate(m_goScreenDarkener, transform.position + new Vector3(0f, 0f, -0.80f), Quaternion.identity);
		yield return new WaitForSeconds(2f);

		aLines.OnHideInstant();

		// Exit game...
		// Move camera off map, so there can be now glithces while we transiton
		//Vector3 vNewPos = transform.position + new Vector3(Types.s_fRoomBoundsX, 0f, 0f);
		//GameInstance.Object.GetGameCamera().WarpToPosition(ref vNewPos);
		Messenger.Invoke(Types.s_sGF_BeginExitGame);
	}
}
