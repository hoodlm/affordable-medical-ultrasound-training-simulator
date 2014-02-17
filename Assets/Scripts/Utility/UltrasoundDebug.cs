using UnityEngine;
using System.Collections;

/** 
 *  Static class to house some useful miscellaneous debug functions.
 */
public static class UltrasoundDebug {

	/**
	 * 	Make an assertion.
	 *  If the assertion is true, continue execution normally.
	 * 	An error is logged if the assertion is false.
	 * 	@param assertion the assertion to test.
	 * 	@param failMessage a message to log if the assertion fails.
	 * 	@param caller The script that is making the assertion.
	 */
	public static void Assert(bool assertion, string failMessage, object caller) {
		Assert(assertion, failMessage, caller, false);
	}

	/**
	 * 	Make an assertion.
	 *  If the assertion is true, continue execution normally.
	 * 	An error is logged if the assertion is false.
	 * 	@param assertion the assertion to test.
	 * 	@param failMessage a message to log if the assertion fails.
	 * 	@param caller The script that is making the assertion.
	 * 	@param isFatal Whether to halt the program if the assertion is false.
	 */
	public static void Assert(bool assertion, string failMessage, object caller, bool isFatal) {
		if (!assertion) {
			Debug.LogError(string.Format ("{0}: {1}", caller.GetType(), failMessage));
			if (isFatal) {
				Debug.LogError("FATAL: Killing at the end of this frame.");
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
			}
		}
	}
}
