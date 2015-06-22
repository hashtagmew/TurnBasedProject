using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EffectBox {
	
	public static Dictionary<string, Effect> s_dEffectLookup;
	
	static EffectBox() {
		s_dEffectLookup = new Dictionary<string, Effect>();
		
		//s_dAbilityLookup.Add("Melee Strike", new Ability(ABILITY_TYPE.ACTIVE, 1.0f));
	}
}