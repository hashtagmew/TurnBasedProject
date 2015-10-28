using UnityEngine;
using System.Collections;



public class Ability {

	public ABILITY_TYPE iType;
	public float fIntensity;
	public EFFECT_TARGET iTargetType;
	public int iRange;

	public Ability() {
		iType = ABILITY_TYPE.NONE;
		fIntensity = 0.0f;
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
		fIntensity = intensity;
		iTargetType = target;
		iRange = range;
	}
}
