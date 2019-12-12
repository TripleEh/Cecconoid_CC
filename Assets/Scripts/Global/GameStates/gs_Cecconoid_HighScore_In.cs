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
using UnityEngine.UI;

public class gs_Cecconoid_HighScore_In : GameState
{
	// Player controller is deleted by the time we leave gs_Game, so we're cool to create a
	// new set of bindings here...
	private PlayerActions m_PlayerActionsBindings;

	// Reference to the UI / Canvas controller
	private HighScore_Euganoid m_gcHighScoreUI;

	// For the state machine...
	private Types.EHighScoreMenuState m_iState = Types.EHighScoreMenuState._IDLE_INACTIVE;

	// High score table will automatically time out and return us to the main menu...
	private float m_fEventTime = 0f;

	// The Text Field we need to update...
	private Text m_gcEntryInitial = null;

	// It's index
	private uint m_iEntry = 0;

	// Where we are in the allowable characters
	private int m_iAlphabetIndex;

	// What the user has entered...
	private string m_sFinalInitials = "   ";

	// Little hack to slow the speed we move through the available characters...
	private int m_iFrameCounter;



	private void Start()
	{
		// Re-Init the pad...
		m_PlayerActionsBindings = new PlayerActions();
		m_PlayerActionsBindings.UI_Back.ClearInputState();
		m_PlayerActionsBindings.UI_Confirm.ClearInputState();
		m_PlayerActionsBindings.UI_Pause.ClearInputState();


		m_gcHighScoreUI = GameInstance.Object.GetHighScoreCecconoid();
		GAssert.Assert(null != m_gcHighScoreUI, "Unable to get reference to the high score ui!");



		// Are we entering our initials?
		if (GameGlobals.s_bCEcconoidHighScoreEntryThisGo)
		{
			m_gcEntryInitial = m_gcHighScoreUI.GetTextEntry(m_iEntry);
			GAssert.Assert(null != m_gcEntryInitial, "gs_Eugatron_HighScore_In: Unable to get first text entry for initials");
			m_iState = Types.EHighScoreMenuState._ENTER_NEW_INITIAL1;
		}
		else
		{
			m_gcHighScoreUI.OnShowTable();
			m_iState = Types.EHighScoreMenuState._SHOW_TABLE;
			m_fEventTime = TimerManager.fGameTime + Types.s_fDUR_GameOverScreen;
		}
	}





	// Being cheeky again. Fixed Update locked at 60, and a frame counter can slow the update further...
	// Prevents movement through the alphabet going at the speed of light. 
	//

	private void FixedUpdate()
	{
		switch (m_iState)
		{
			// Do nothing...
			case Types.EHighScoreMenuState._IDLE_INACTIVE:
			case Types.EHighScoreMenuState._SHOW_TABLE: ShowTableUpdate(); break;

			// Read the stick...
			case Types.EHighScoreMenuState._ENTER_NEW_INITIAL1:
			case Types.EHighScoreMenuState._ENTER_NEW_INITIAL2:
			case Types.EHighScoreMenuState._ENTER_NEW_INITIAL3: EnterInitialsUpdate(); break;
		}
	}




	private void EnterInitialsUpdate()
	{

		// Check for the confirm button, to move onto the next initial or save and show table
		// This needs to run every tick....
		if (m_PlayerActionsBindings.UI_Confirm.WasPressed)
		{
			m_sFinalInitials.Insert((int)m_iEntry, Types.s_sHS_English.Substring(m_iAlphabetIndex, 1));

			++m_iEntry;
			if (m_iEntry < 3)
			{
				m_gcEntryInitial = m_gcHighScoreUI.GetTextEntry(m_iEntry);
				m_iAlphabetIndex = 0;
			}
			else
			{
				GameGlobals.SaveCecconoidScore(m_gcHighScoreUI.GetFinalString());
				m_gcHighScoreUI.OnShowTable();
				m_iState = Types.EHighScoreMenuState._SHOW_TABLE;
				m_fEventTime = TimerManager.fGameTime + Types.s_fDUR_GameOverScreen;
			}
		}


		// Now reduce update frequency, don't care about scrolling quickly
		{
			++m_iFrameCounter;
			if (m_iFrameCounter < 4) return;
			m_iFrameCounter = 0;
		}


		// Get the player's input
		Vector2 vMovementTrajectory = m_PlayerActionsBindings.GE_Move;

		// Deadzone it, same as in-game.
		if (vMovementTrajectory.magnitude < Types.s_fDeadZone_Movement) vMovementTrajectory = Vector2.zero;
		else vMovementTrajectory = vMovementTrajectory.normalized * ((vMovementTrajectory.magnitude - Types.s_fDeadZone_Movement) / (1.0f - Types.s_fDeadZone_Movement));

		// Move through the list of available letters
		if (vMovementTrajectory.x > Types.s_fDeadZone_Movement) ++m_iAlphabetIndex;
		else if (vMovementTrajectory.x < -Types.s_fDeadZone_Movement) --m_iAlphabetIndex;

		// Clamp (Roll round?)
		if (m_iAlphabetIndex < 0) m_iAlphabetIndex = 0; if (m_iAlphabetIndex > 26) m_iAlphabetIndex = 26;

		// Set the initial visibile on-screen
		m_gcEntryInitial.text = Types.s_sHS_English.Substring(m_iAlphabetIndex, 1);
	}




	// Timeout, or any button returns to main menu...
	//
	private void ShowTableUpdate()
	{
		if ((TimerManager.fGameTime > m_fEventTime
				|| m_PlayerActionsBindings.UI_Back.WasPressed
				|| m_PlayerActionsBindings.UI_Confirm.WasPressed
				|| m_PlayerActionsBindings.UI_Pause.WasPressed)
				&& m_gcGameStateManager.CanChangeState())
		{
			m_PlayerActionsBindings.UI_Back.ClearInputState();
			m_PlayerActionsBindings.UI_Confirm.ClearInputState();
			m_PlayerActionsBindings.UI_Pause.ClearInputState();
			m_gcGameStateManager.ChangeState(EGameStates._CECCONOID_HIGHSCORE_EXIT);
		}
	}
}
