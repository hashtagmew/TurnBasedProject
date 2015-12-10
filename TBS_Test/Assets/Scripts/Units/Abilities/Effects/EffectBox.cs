using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EffectBox {
	
	public static Dictionary<string, Effect> s_dEffectLookup;
	
	static EffectBox() {
		s_dEffectLookup = new Dictionary<string, Effect>();

		//Damage
		s_dEffectLookup.Add("Damage", new Effect("Damage", EFFECT_TYPE.RESOLUTION, true, "Amount"));
		s_dEffectLookup.Add("Heal", new Effect("Heal", EFFECT_TYPE.RESOLUTION, true, "Amount"));

		//Passive Buffs
		s_dEffectLookup.Add("Defence Buff", new Effect("Defence Buff", EFFECT_TYPE.PASSIVE, true, "Amount"));

		//Terrain Manipulation
		s_dEffectLookup.Add("Push Mountain", new Effect("Push Mountain", EFFECT_TYPE.RESOLUTION, true, "Tiles"));
		s_dEffectLookup.Add("Summon Wall", new Effect("Summon Wall", EFFECT_TYPE.RESOLUTION, false));

		//Sprite Effects
		s_dEffectLookup.Add("Hide Sprite", new Effect("Hide Sprite", EFFECT_TYPE.ACTIVATION, false));
		s_dEffectLookup.Add("Show Sprite", new Effect("Show Sprite", EFFECT_TYPE.RESOLUTION, false));

		//Movement
		s_dEffectLookup.Add("Teleport Caster to Target", new Effect("Teleport", EFFECT_TYPE.RESOLUTION, false));

		//Camera
		s_dEffectLookup.Add("Pan Camera to Target (A)", new Effect("PanCameraTargetA", EFFECT_TYPE.ACTIVATION, true, "Pan Speed"));
		s_dEffectLookup.Add("Pan Camera to Target (R)", new Effect("PanCameraTargetR", EFFECT_TYPE.RESOLUTION, true, "Pan Speed"));

		s_dEffectLookup.Add("Pan Camera to Caster (A)", new Effect("PanCameraCasterA", EFFECT_TYPE.ACTIVATION, true, "Pan Speed"));
		s_dEffectLookup.Add("Pan Camera to Caster (R)", new Effect("PanCameraCasterR", EFFECT_TYPE.RESOLUTION, true, "Pan Speed"));

		s_dEffectLookup.Add("Teleport Camera to Target (A)", new Effect("TeleportCameraTargetA", EFFECT_TYPE.ACTIVATION, false));
		s_dEffectLookup.Add("Teleport Camera to Target (R)", new Effect("TeleportCameraTargetR", EFFECT_TYPE.RESOLUTION, false));
		
		s_dEffectLookup.Add("Teleport Camera to Caster (A)", new Effect("TeleportCameraCasterA", EFFECT_TYPE.ACTIVATION, false));
		s_dEffectLookup.Add("Teleport Camera to Caster (R)", new Effect("TeleportCameraCasterR", EFFECT_TYPE.RESOLUTION, false));

		//Races
		s_dEffectLookup.Add("(Race) Magical", new Effect("Magical", EFFECT_TYPE.PASSIVE, false));
		s_dEffectLookup.Add("(Race) Mechanical", new Effect("Mechanical", EFFECT_TYPE.PASSIVE, false));

		//Types
		s_dEffectLookup.Add("(Unit Type) Infantry", new Effect("Infantry", EFFECT_TYPE.PASSIVE, false));
		s_dEffectLookup.Add("(Unit Type) Support", new Effect("Support", EFFECT_TYPE.PASSIVE, false));
	}
}