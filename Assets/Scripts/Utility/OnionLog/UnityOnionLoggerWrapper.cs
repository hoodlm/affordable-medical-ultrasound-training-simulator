using UnityEngine;
using System.Collections;

/**
 *	An example wrapper script for use in Unity3D.
 *
 *	This wrapper handles the initialization and closure of the OnionLogger.globalLog singleton.
 *
 *	As long as this script is present on a GameObject at the start of a Scene,
 *	then you can safely use OnionLogger.globalLog from any other script.
 */
public class UnityOnionLoggerWrapper : MonoBehaviour {

	/// Set the level of this log. Messages that are lower than the logging level are suppressed.
	public OnionLogger.LoggingLevels LogLevel = OnionLogger.LoggingLevels.INFO;

	/// Sets up a logger to write to a Logs directory in the project folder.
	void Awake ()
	{
		// Make sure the directory exists.
		string logDirectory = "./Logs";
		System.IO.Directory.CreateDirectory(logDirectory);

		// We'll use the default log filename,
		// which uses the format "YYYY-MM-DD.HH.MM.SS.txt".
		string filepath = logDirectory + "/" + OnionLogger.DefaultLogFilename();

		// Set the level for this logger from the inspector setting.
		OnionLogger.LoggingLevels logLevel = this.LogLevel;

		// Configure the appearance of the logs. 
		char indentChar = ' ';
		uint indentSize = 2;

		OnionLogger.globalLog = new OnionLogger(filepath, indentChar, indentSize, logLevel);
		Debug.Log("Started OnionLog at "+filepath);
		OnionLogger.globalLog.PushInfoLayer("Scene " + Application.loadedLevelName);
	}

	/// OnDestroy is called if the object holding the script is removed from the scene or if the scene is closed.
	/// This will ensure that any remaining messages in the buffer are written to the file.
	void OnDestroy () {
		OnionLogger.globalLog.FlushFrequencyInMilliseconds = -1;
		OnionLogger.globalLog.PopInfoLayer();
		OnionLogger.globalLog = null;
	}
}
