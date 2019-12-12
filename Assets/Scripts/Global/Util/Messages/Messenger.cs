// Messenger.cs v0.1 (20090925) by Rod Hyde (badlydrawnrod).
//
// This is a C# messenger (notification center) for Unity. It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other.

// AVOID: Message callbacks shouldn't be nested. Ie: don't invoke a message that
// then runs code that invokes more messages!
//
using UnityEngine;
using System;
using System.Collections.Generic;


/**
 * A messenger for events that have no parameters.
 */
static public class Messenger
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static public void ClearAll()
	{
		lock (eventTable)
			eventTable.Clear();
	}



	static public void ListAll()
	{
		lock (eventTable)
		{
			foreach (string key in eventTable.Keys) Debug.Log("Key: " + key);
		}
	}



	static public void AddListener(string eventType, Callback handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Create an entry for this event type if it doesn't already exist.
			if (!eventTable.ContainsKey(eventType))
				eventTable.Add(eventType, null);

			// Add the handler to the event.
			eventTable[eventType] = (Callback)eventTable[eventType] + handler;
		}
	}



	static public void RemoveListener(string eventType, Callback handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			if (eventTable.ContainsKey(eventType))
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if (eventTable[eventType] == null) eventTable.Remove(eventType);
			}
		}
	}



	static public void Invoke(string eventType)
	{
		Delegate d;
		lock (eventTable)
		{
			// Invoke the delegate only if the event type is in the dictionary.
			if (eventTable.TryGetValue(eventType, out d))
			{
				// Take a local copy to prevent a race condition if another thread
				// were to unsubscribe from this event.
				Callback callback = (Callback)d;

				// Invoke the delegate if it's not null.
				if (callback != null)
				{
					try
					{
						callback();
					}
					catch
					{
#if GDEBUG
						// Nothing bad happens if the invoke is called on an object that's
						// being destroyed, and the event is culled during the Update anyway
						// but it may indicate that something has forgotten to remove itself
						// from the Listener list...
						//
						// Alternatively you may have a message invoke that is calling code
						// that invokes more messages. Nested callbacks like this will not
						// work correctly. 
						Debug.LogWarning("Message Invoke Called on destroyed object, or nested inside another Invoke...");
#endif
					}
				}
			}
		}
	}
}




/**
 * A messenger for events that have one parameter of type T.
 */
static public class Messenger<T>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static public void AddListener(string eventType, Callback<T> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Create an entry for this event type if it doesn't already exist.
			if (!eventTable.ContainsKey(eventType))
			{
				eventTable.Add(eventType, null);
			}
			else
			{
				Debug.LogError("Event Table already contains: " + eventType);
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
		}
	}



	static public void RemoveListener(string eventType, Callback<T> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Only take action if this event type exists.
			if (eventTable.ContainsKey(eventType))
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if (eventTable[eventType] == null)
				{
					eventTable.Remove(eventType);
				}
			}
		}
	}



	static public void Invoke(string eventType, T arg1)
	{
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if (eventTable.TryGetValue(eventType, out d))
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T> callback = (Callback<T>)d;

			// Invoke the delegate if it's not null.
			if (callback != null)
			{
				try
				{
					callback(arg1);
				}
				catch
				{

				}
			}
		}
		else
		{
			Debug.LogError("Invoke failed to find eventType: " + eventType);
		}
	}
}




/**
 * A messenger for events that have two parameters of types T and U.
 */
static public class Messenger<T, U>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static public void AddListener(string eventType, Callback<T, U> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Create an entry for this event type if it doesn't already exist.
			if (!eventTable.ContainsKey(eventType))
			{
				eventTable.Add(eventType, null);
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
		}
	}



	static public void RemoveListener(string eventType, Callback<T, U> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Only take action if this event type exists.
			if (eventTable.ContainsKey(eventType))
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if (eventTable[eventType] == null)
				{
					eventTable.Remove(eventType);
				}
			}
		}
	}



	static public void Invoke(string eventType, T arg1, U arg2)
	{
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if (eventTable.TryGetValue(eventType, out d))
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T, U> callback = (Callback<T, U>)d;

			// Invoke the delegate if it's not null.
			if (callback != null)
			{
				try
				{
					callback(arg1, arg2);
				}
				catch
				{

				}
			}
		}
	}
}