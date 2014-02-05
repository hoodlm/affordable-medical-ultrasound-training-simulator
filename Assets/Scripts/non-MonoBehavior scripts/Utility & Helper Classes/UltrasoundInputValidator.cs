using UnityEngine;
using System.Collections;

/** 
 *  Static class to house some useful miscellaneous validation functions.
 */
public static class UltrasoundInputValidator {
    
    /**
     * Verify that an object is non-null.
     * 
     * @param o The allegedly-null object to test.
     * @throw ArgumentNotNullException
     * @return the object, if it is not null
     */
    public static object CheckNotNull(object o) {
        if (null == o) {
            throw new System.ArgumentNullException();
        } else {
            return o;
        }
    }
    
    /**
     * Verify that an object is non-null, with a custom error string.
     * 
     * @param o The allegedly-null object to test.
     * @param errorString A custom error string to include in the exception.
     * @throw ArgumentNotNullException
     * @return the object, if it is not null
     */
    public static object CheckNotNull(object o, string errorString) {
        if (null == o) {
            throw new System.ArgumentNullException(errorString);
        } else {
            return o;
        }
    }
}
