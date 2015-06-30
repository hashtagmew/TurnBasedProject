using UnityEngine;
using System.Collections;



public class Effect {

	public string sName = "Nothing";
	public EFFECT_TYPE iType = EFFECT_TYPE.NONE;

	public Effect() {
		//
	}

	public Effect(string name, EFFECT_TYPE type) {
		sName = name;
		iType = type;
	}
}
