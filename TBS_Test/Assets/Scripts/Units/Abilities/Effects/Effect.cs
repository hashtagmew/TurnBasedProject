﻿using UnityEngine;
using System.Collections;



public class Effect {

	public string sName = "Nothing";
	public EFFECT_TYPE iType = EFFECT_TYPE.NONE;
	public bool bAdjustable = false;
	public string sAdjustName = "";
	public float fAdjustFloat = 0.0f;
	public bool bAdjustableString = false;
	public string sAdjustString = "";

	public Effect() {
		//
	}

	public Effect(string name, EFFECT_TYPE type, bool adjustable, string adjustname = "none", bool stringadjust = false) {
		sName = name;
		iType = type;
		bAdjustable = adjustable;
		sAdjustName = adjustname;
		bAdjustableString = stringadjust;
	}
}
