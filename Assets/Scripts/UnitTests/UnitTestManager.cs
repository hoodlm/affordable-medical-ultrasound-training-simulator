using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System;

/// Abstract class for individual unit tests to inherit from.
public abstract class UnitTest {
	/**
		 *	Execute a unit test.
		 *	@return A string describing the outcome of the test.
		 */
	public abstract string ExecuteTest();
}

public class UnitTestManager : MonoBehaviour {

	private IList<UnitTest> tests;
	private int currentTestIndex;
	private StringBuilder testResultString;
	private bool testsRunning = false;
	private GUIStyle style;

	// Use this for initialization
	void Start () {
		testResultString = new StringBuilder("Waiting to start tests...");
		tests = new List<UnitTest>();

		// Add tests here
		tests.Add(new UTTransposeTest());
		tests.Add(new UTCopyMonochromeBitmapTest());

		// Set up the GUIStyle
		style = new GUIStyle();
		style.fontSize = 10;

		StartTests();
	}
	
	// Update is called once per frame
	void Update () {
		if (testsRunning) {
			if (currentTestIndex < tests.Count) {
				testResultString.AppendLine(tests[currentTestIndex++].ExecuteTest());
			} else {
				FinishTests();
			}
		}
	}

	/// Render GUI
	void OnGUI () {
		Rect displayArea = new Rect(0f, 0f, Screen.width, Screen.height);
		GUI.Box (displayArea, testResultString.ToString(), style);
	}

	private void StartTests () {
		OnionLogger.globalLog.PushInfoLayer("Running unit tests");
		testResultString = new StringBuilder();
		
		string timestamp = 
			string.Format("Starting unit tests on {0,4:D4}-{1,2:D2}-{2,2:D2} {3,2:D2}:{4,2:D2}:{5,2:D2}",
			              DateTime.Now.Year,
			              DateTime.Now.Month,
			              DateTime.Now.Day,
			              DateTime.Now.Hour,
			              DateTime.Now.Minute,
			              DateTime.Now.Second);
		
		testResultString.AppendLine(timestamp);

		currentTestIndex = 0;
		testsRunning = true;
	}

	private void FinishTests () {
		string timestamp = 
			string.Format("Finished unit tests on {0,4:D4}-{1,2:D2}-{2,2:D2} {3,2:D2}:{4,2:D2}:{5,2:D2}",
			              DateTime.Now.Year,
			              DateTime.Now.Month,
			              DateTime.Now.Day,
			              DateTime.Now.Hour,
			              DateTime.Now.Minute,
			              DateTime.Now.Second);
		
		testResultString.AppendLine(timestamp);
		testsRunning = false;
		OnionLogger.globalLog.PopInfoLayer();
	}
}
