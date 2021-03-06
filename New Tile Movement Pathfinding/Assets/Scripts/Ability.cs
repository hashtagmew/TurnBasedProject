﻿using UnityEngine;
using System.Collections;



public class Ability {

	public ABILITY_TYPE iType;
	public float fIntensity;

	public Ability() {
		iType = ABILITY_TYPE.NONE;
	}

	public Ability(ABILITY_TYPE type) {
		iType = type;
		fIntensity = 0.0f;
	}

	public Ability(ABILITY_TYPE type, float intensity) {
		iType = type;
		fIntensity = intensity;
	}
}
