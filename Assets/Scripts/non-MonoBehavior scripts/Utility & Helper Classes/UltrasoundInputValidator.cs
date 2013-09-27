using UnityEngine;
using System.Collections;

/// <summary>
/// Some miscellaneous validation functions.
/// </summary>
public static class UltrasoundInputValidator {
	
	public static object CheckNotNull(object o) {
		if (null == o) {
			throw new System.ArgumentNullException();
		} else {
			return o;
		}
	}
	
	public static object CheckNotNull(object o, string errorString) {
		if (null == o) {
			throw new System.ArgumentNullException(errorString);
		} else {
			return o;
		}
	}
}
