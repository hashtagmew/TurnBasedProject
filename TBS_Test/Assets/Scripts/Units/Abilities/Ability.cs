using UnityEngine;
using System.Collections.Generic;

public class Ability {

	public string sName;
	public string sDescription;

	public string sSoundset;
	public string sParticleStart;
	public string sParticleRun;
	public string sParticleFinish;

	public ABILITY_TYPE iType;
	public EFFECT_TARGET iTargetType;
	public FEATURE_TYPE eFeatureTarget;

	public bool bMagicalAttack;
	public bool bSetDamageUser;
	public bool bSetDamageTarget;
	public bool bGuaranteedHit;
	public bool bRangedAttack;
	public bool bStopOnCollide;

	public int iRange;
	public int iAccuracy;
	public int iArea;

	public float fDelayStart;
	public float fDelayRun;
	public float fDelayFinish;

	public Dictionary<string, Effect> d_EffectsActivation = new Dictionary<string, Effect>();
	public Dictionary<string, Effect> d_EffectsResolution = new Dictionary<string, Effect>();

	public Ability() {
		iType = ABILITY_TYPE.NONE;
		//fIntensity = 0.0f;
		iTargetType = EFFECT_TARGET.NONE;
		iRange = 0;
	}

//	public Ability(ABILITY_TYPE type) {
//		iType = type;
//		fIntensity = 0.0f;
//		iTargetType = EFFECT_TARGET.NONE;
//	}
//
//	public Ability(ABILITY_TYPE type, float intensity) {
//		iType = type;
//		fIntensity = intensity;
//		iTargetType = EFFECT_TARGET.NONE;
//	}

	public Ability(ABILITY_TYPE type, EFFECT_TARGET target = EFFECT_TARGET.NONE, float intensity = 0.0f, int range = 0) {
		iType = type;
		//fIntensity = intensity;
		iTargetType = target;
		iRange = range;
	}
}
