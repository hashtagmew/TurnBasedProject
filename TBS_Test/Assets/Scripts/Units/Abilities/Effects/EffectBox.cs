using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EffectBox {
	
	public static Dictionary<string, Effect> s_dEffectLookup;
	
	static EffectBox() {
		s_dEffectLookup = new Dictionary<string, Effect>();
		
		s_dEffectLookup.Add("Target Self", new Effect("Target Self", EFFECT_TYPE.ACTIVATION, false));
		s_dEffectLookup.Add("Target Other", new Effect("Target Other", EFFECT_TYPE.ACTIVATION, true));
		s_dEffectLookup.Add("Target Area", new Effect("Target Area", EFFECT_TYPE.ACTIVATION, true));

		s_dEffectLookup.Add("Damage Target", new Effect("Damage Target", EFFECT_TYPE.RESOLUTION, true));
		s_dEffectLookup.Add("Heal Target", new Effect("Heal Target", EFFECT_TYPE.RESOLUTION, true));

		s_dEffectLookup.Add("Resist Damage", new Effect("Resist Damage", EFFECT_TYPE.PASSIVE, true));
		s_dEffectLookup.Add("Defence Buff", new Effect("Defence Buff", EFFECT_TYPE.PASSIVE, true));

		s_dEffectLookup.Add("(Race) Human", new Effect("Human", EFFECT_TYPE.PASSIVE, false));
		s_dEffectLookup.Add("(Race) Machine", new Effect("Machine", EFFECT_TYPE.PASSIVE, false));

		s_dEffectLookup.Add("(Unit Type) Infantry", new Effect("Infantry", EFFECT_TYPE.PASSIVE, false));
		s_dEffectLookup.Add("(Unit Type) Support", new Effect("Support", EFFECT_TYPE.PASSIVE, false));
	}
}