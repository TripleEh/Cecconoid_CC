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

using System;
using System.Collections.Generic;
using UnityEngine;

// This class wraps Unity's Time class to avoid some of the traps, and provides the following:
//
// 1. TimerCallbacks:	a method to add Delegates that will be called at a future point in time. 
// 2. GameDeltaTime: a specific DeltaTime for objects in the GameWorld
// 3. UIDeltaTime: a specific DeltaTime for UI and HUD elements
//
// You can replace Time.deltaTime with TimerManger.fGameDeltaTime / TimerManager.fUIDeltaTime 
//
static public class TimerManager
{
	// Internal Dictionary of Timer Callbacks
	private static Dictionary<UInt64, Delegate> m_aEventTable = new Dictionary<UInt64, Delegate>();

	// Internal DeltaTime trackers
	private static float m_fGameDeltaTime = 0.0f;
	private static float m_fFixedDeltaTime = Types.s_fFixedDeltaTimeUpdate;
	private static float m_fUIDeltaTime = 0.0f;
	private static float m_fGameDeltaScaler = 1.0f;
	private static float m_fUIDeltaScaler = 1.0f;
	private static float m_fGameTime = 0.0f;
	private static float m_fUITime = 0.0f;


	// Public Getters
	public static float fGameDeltaTime { get { return m_fGameDeltaTime; } set{} } 
	public static float fUIDeltaTime { get { return m_fUIDeltaTime; } set{} }
	public static float fGameDeltaScale { get { return m_fGameDeltaScaler;  } set {} }
	public static float fFixedDeltaTime {  get {  return m_fFixedDeltaTime; } set {} }
	
	public static float fGameTime { get { return m_fGameTime; } set {} }
	public static float fUIGameTime { get { return m_fUITime; } set { } }



	// We can scale the update speed to fake slow-down, but 
	// this doesn't affect the physics update frequency, or 
	// the velocities of objects in the simulation...
	//
	public static void SetGameTimeScale(float fScale)
	{
		m_fGameDeltaScaler= Mathf.Clamp(fScale, 0.0f, 1.0f);
	}



	// As above, but for UI specific timers...
	//
	public static void SetUITimeScale(float fScale)
	{
		m_fUIDeltaScaler= Mathf.Clamp(fScale, 0.0f, 1.0f);
	}

	
	
  public static void SetDefaults(float fGS, float fUI)
	{
		// The physics update time is never changed! Physics engine 
		// should always be running at 1/60...
		//
		// GNTODO: m_fFixedDeltaTime could be zero'd and would be 
		// nicer than everything checking this class for IsPaused()!
		// Is there anything that's running under impulse that
		// would need to be managed differently?
		Time.fixedDeltaTime = Types.s_fFixedDeltaTimeUpdate;

		SetGameTimeScale(fGS);
		SetUITimeScale(fUI);
	}



	public static bool IsPaused()
	{
		return (m_fGameDeltaScaler < 0.001f);
	}

	public static void PauseGame()
	{
		//m_fFixedDeltaTime = 0;
		m_fGameDeltaScaler = 0f;
	}


	public static void UnPauseGame()
	{
		//m_fFixedDeltaTime = Types.s_fFixedDeltaTimeUpdate;
		m_fGameDeltaScaler = 1f;
	}


	// Adds a timer to the internal list. 
	//
	// Parameters:
	// 	<fTime> the number of SECONDS you want the timer to last.
	//	<handler> the function to call...
	//
	// Returns: 
	//	Time of Event, as UInt64 (ulong), to be retained by the callback. 
	//	If a callback wants to remove itself from the timer list (early) it can pass this as a 
	//	param to ClearTimerHandle...
	//
	public static UInt64 AddTimer(float fTTLSeconds, Callback handler)
	{
		// Convert to Int so comparisions are safe...
		UInt64 iEventTime = (UInt64)((m_fGameTime + fTTLSeconds) * 1000.0f);

		lock (m_aEventTable)
		{
			// Create an entry for this event type if it doesn't already exist.
			if (!m_aEventTable.ContainsKey(iEventTime)) m_aEventTable.Add(iEventTime, null);

			// Add the callback delegate
			m_aEventTable[iEventTime] = (Callback)m_aEventTable[iEventTime] + handler;
		}
		return iEventTime;
	}


	
	
	// Called by the GameInstance once per tick, to update internal state and make sure 
	// all callbacks are processed before DefaultTime. GameInstance's priority is set to 
	// run Update before normal time, so this should be set before anything uses it. 
	//
	public static void Update()
	{
		// Handle our DeltaTimes
		{
			m_fGameDeltaTime = Time.deltaTime * m_fGameDeltaScaler;
			m_fUIDeltaTime = Time.deltaTime * m_fUIDeltaScaler;
			m_fGameTime += m_fGameDeltaTime;
			m_fUITime += m_fUIDeltaTime;
		}

		// Process the timer callbacks
		lock (m_aEventTable)
		{
			Delegate d;
			UInt64 iTime = (UInt64)(m_fGameTime * 1000.0f);

			// Get key collection from dictionary into a list to loop through
			// We need to do this as we are going to modify the Dictionary 
			// as we loop through it, which will cause an exception
			List<UInt64> aKeys = new List<UInt64>(m_aEventTable.Keys);

			// For all times in the past, invoke ALL callbacks attached and
			// then remove them from the internal list...
			// Because of the Try/Catch errors in delegates won't appear in location. Need to be aware of that...
			foreach (UInt64 iKey in aKeys)
			{
				if (iKey <= iTime)
				{
					if (m_aEventTable.TryGetValue(iKey, out d))
					{
						try
						{
							Invoke(iKey);
						}
						catch
						{
							Debug.LogWarning("Error encountered in Timer Delegate. Stick a breakpoint here to catch the offending object");
						}
					}

					ClearTimer(iKey);
				}
			}
		}
	}



	// Remove a single callback from the internal list
	//
	public static void ClearTimerHandler(UInt64 iEventTime, Callback handler)
	{
		lock (m_aEventTable)
		{
			if (m_aEventTable.ContainsKey(iEventTime))
			{
				m_aEventTable[iEventTime] = (Callback)m_aEventTable[iEventTime] - handler;
				if (m_aEventTable[iEventTime] == null) m_aEventTable.Remove(iEventTime);
			}
		}
	}



	// Should only be called by ProcessTimers, to remove all timers in the past...
	//
	private static void ClearTimer(UInt64 iEventTime)
	{
		lock (m_aEventTable)
		{
			if (m_aEventTable.ContainsKey(iEventTime))
			{
				m_aEventTable[iEventTime] = null;
				m_aEventTable.Remove(iEventTime);
			}
		}
	}


	
	// Only called by Update() when a timer event has passed...
	//
	private static void Invoke(UInt64 eventTime)
	{
		Delegate d;

		// Invoke the delegate only if the event type is in the dictionary.
		if (m_aEventTable.TryGetValue(eventTime, out d))
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback c = (Callback)d;

			// Invoke the delegate if it's not null.
			if (null != c && !c.Equals(null)) c();
		}
		else Debug.LogError("Invoke failed to find eventTime: " + eventTime);
	}



	// GameStateManager will call this when changing state
	//
	public static void ClearAll()
	{
		lock (m_aEventTable) { m_aEventTable.Clear(); }
	}



	// DEBUG ONLY!
	//
	public static void ListAll()
	{
		lock (m_aEventTable)
		{
			foreach (Int64 key in m_aEventTable.Keys) Debug.Log("Key: " + key);
		}
	}


}
