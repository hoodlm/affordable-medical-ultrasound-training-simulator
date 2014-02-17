using UnityEngine;
using System;

/** 
 *  A single point in space.
 */
public class UltrasoundPoint : IComparable {
	private readonly Vector3 worldSpaceLocation;
    private readonly Vector2 projectedLocation;
    private float brightness;
    
    /**
     *  Create a new UltrasoundPoint.
     *  @param worldSpaceLocation The location of the point in 3D world space.
     *  @param projectedLocation Within a scanning plane, the 2D position of the point.
     */
    public UltrasoundPoint (Vector3 worldSpaceLocation, Vector2 projectedLocation)
    {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(projectedLocation.y > 0f, 
		                       "y coordinate of projectedLocation should be positive.",
		                       this);
#endif
        this.worldSpaceLocation = worldSpaceLocation;
        this.projectedLocation = projectedLocation;
        brightness = 0f;
    }
    
    /**
     *  Return the 3D position of this UltrasoundPoint.
     *  @return worldSpaceLocation The location of the point in 3D world space.
     */
    public Vector3 GetWorldSpaceLocation ()
    {
        return this.worldSpaceLocation;
    }

    /**
     *  Get the 2D projected position of this UltrasoundPoint within the scanning plane.
     *  @return Within the scanning plane, the 2D position of the point.
     */
    public Vector2 GetProjectedLocation ()
    {
        return this.projectedLocation;
    }
     
    /**
     *  Get the brightness (echogenicity) of this point.
     *  @return Brightness between 0 and 1, inclusive.
     */
    public float GetBrightness ()
    {
        return this.brightness;
    }
     
    /**
     *  Change the brightness (echogenicity) of this point.
     *  @param brightness - clamped between 0 and 1.
     */
    public void SetBrightness (float brightness)
    {
       this.brightness = Mathf.Clamp(brightness, 0f, 1f);
    }
    
    /**
     *  String representation of an UltrasoundPoint.
     */
    public override string ToString ()
    {
        return string.Format ("[UltrasoundPoint] 3D: {0} Proj: {1} Brightness: {2}",
        					  worldSpaceLocation, projectedLocation, brightness);
    }
    
    /**
     *  UltrasoundPoints are considered equal if they have the same ProjectedLocation and WorldSpaceLocation.
     *  Brightness is not included in the equality comparison.
     *
     *  @param obj The object to compare to this UltrasoundPoint.
     */
    public override bool Equals (object obj)
    {
        if (this == obj) {
            return true;
        }
        
        if (!(obj is UltrasoundPoint)) {
            return false;
        }
        
        UltrasoundPoint other = (UltrasoundPoint)obj;
        return (other.GetProjectedLocation().Equals(this.GetProjectedLocation()) &&
                other.GetWorldSpaceLocation().Equals(this.GetWorldSpaceLocation()));
    }

    /**
     *  UltrasoundPoints' hashcodes are generated from ProjectedLocation and WorldSpaceLocation.
     *  Brightness is not included in the hashcode generation.
     *
     *  @return A hash.
     */
    public override int GetHashCode ()
    {
        int prime = 31;
        int hash = 1;
        
        hash = prime * hash + this.projectedLocation.GetHashCode();
        hash = prime * hash + this.worldSpaceLocation.GetHashCode();
        
        return hash;
    }

    /**
     *  Comparison is solely calculated based on distance in the projection plane.
     *
     *  @param obj Another Ultrasound point to compare to.
     *  @return A factor proportional in distance between the origin of the two UltrasoundPoints.
     */
    public int CompareTo (object obj)
    {
        if (this.Equals(obj)) {
            return 0;
        }
        
        if (!(obj is UltrasoundPoint)) {
            throw new ArgumentException("Can only compare UltrasoundPoints to other UltrasoundPoints!");
        }
        
        UltrasoundPoint other = (UltrasoundPoint)obj;
        float difference = this.GetProjectedLocation().magnitude - other.GetProjectedLocation().magnitude;
        
        /* CompareTo must return an integer. If we just rounded difference directly, we would lose too much
         * precision. Therefore, we multiply it by a large scalar before casting to an integer.
         */
        return (int)(10000f * difference);
    }
}
