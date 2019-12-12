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
using System.Runtime.CompilerServices;
using System.IO;

// WTF do Unity Assertions even do...
// Assert that'll just treat everything as an Error and stop 
// the game in editor until it's fixed...As God intended.
//
public static class GAssert
{
	public static void Assert(bool bTest, string sMessage = null, [CallerLineNumber] int iLine = 0, [CallerMemberName] string sMethod = null, [CallerFilePath] string sFile = null)
	{
		#if UNITY_EDITOR
		if(!bTest)
		{
			Debug.LogError("ASSERTION FAILED: [" + Path.GetFileName(sFile) + ": " + sMethod + "() Line no.: " + iLine.ToString() + "] " + sMessage);
			UnityEditor.EditorApplication.isPlaying = false;
		}
		#endif
	}
	// GNTODO: System Breakpoint, to halt debug builds on the line
	// GNTODO: AssertNotNull
	// GNTODO: Application.Quit for asserts in release / debug builds out of editor?
}
