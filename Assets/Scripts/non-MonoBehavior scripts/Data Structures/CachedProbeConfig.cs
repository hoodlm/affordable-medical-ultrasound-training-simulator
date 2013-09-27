using UnityEngine;
using System.Collections;

public class CachedProbeConfig {
	
	private ProbeConfiguration config;
	private float cachedMinDistance;
	private float cachedMaxDistance;
	private int cachedPointsPerScanline;
	private float cachedArcSize;
	private int cachedScanlineCount;
	private float cachedNoiseLevel;
	
	public CachedProbeConfig() {
		config = null;
	}
	
	public CachedProbeConfig(ProbeConfiguration c) {
		if (null != c) {
			this.config = c; // Deep copy
			cachedMinDistance = c.GetMinDistance();
			cachedMaxDistance = c.GetMaxDistance();
			cachedPointsPerScanline = c.GetPointCount();
			cachedArcSize = c.GetArcSize();
			cachedScanlineCount = c.GetScanlineCount();
			cachedNoiseLevel = c.GetNoiseLevel();
		}
	}
	
	public bool isDirty() {
		if (config == null) {
			return true;
		} else {
			return (!config.GetMinDistance().Equals(cachedMinDistance)
				 || !config.GetMaxDistance().Equals(cachedMaxDistance)
				 || !config.GetArcSize().Equals(cachedArcSize)
				 || !config.GetPointCount().Equals(cachedPointsPerScanline)
				 || !config.GetScanlineCount().Equals(cachedScanlineCount)
				 || !config.GetNoiseLevel().Equals(cachedNoiseLevel));
		}
	}
}
