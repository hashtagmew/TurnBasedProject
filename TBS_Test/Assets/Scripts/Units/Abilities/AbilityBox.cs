using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AbilityBox {

	public static Dictionary<string, Ability> s_dAbilityLookup;

	static AbilityBox() {
		s_dAbilityLookup = new Dictionary<string, Ability>();

		s_dAbilityLookup.Add("Melee Strike", new Ability(ABILITY_TYPE.ACTIVE, 1.0f));
		s_dAbilityLookup.Add("Archery", new Ability(ABILITY_TYPE.ACTIVE, 1.0f));
		s_dAbilityLookup.Add("Magic Missile", new Ability(ABILITY_TYPE.ACTIVE, 1.0f));
		s_dAbilityLookup.Add("Heal", new Ability(ABILITY_TYPE.ACTIVE, 1.0f));

		s_dAbilityLookup.Add("Guard", new Ability(ABILITY_TYPE.PASSIVE));
		s_dAbilityLookup.Add("Fire Resistance", new Ability(ABILITY_TYPE.PASSIVE, 1.0f));

		s_dAbilityLookup.Add("Infantry", new Ability(ABILITY_TYPE.UNIT_TYPE));
		s_dAbilityLookup.Add("Archer", new Ability(ABILITY_TYPE.UNIT_TYPE));
		s_dAbilityLookup.Add("Cavalry", new Ability(ABILITY_TYPE.UNIT_TYPE));
		s_dAbilityLookup.Add("Support", new Ability(ABILITY_TYPE.UNIT_TYPE));
		s_dAbilityLookup.Add("Irregular", new Ability(ABILITY_TYPE.UNIT_TYPE));
		s_dAbilityLookup.Add("Hero", new Ability(ABILITY_TYPE.UNIT_TYPE));

		s_dAbilityLookup.Add("Human", new Ability(ABILITY_TYPE.RACE));
		s_dAbilityLookup.Add("Robot", new Ability(ABILITY_TYPE.RACE));
		s_dAbilityLookup.Add("Alien", new Ability(ABILITY_TYPE.RACE));
		s_dAbilityLookup.Add("Monster", new Ability(ABILITY_TYPE.RACE));
	}

//	~AbilityBox() {
//		foreach (KeyValuePair pair in s_dAbilityLookup) {
//			delete pair.Value;
//		}
//	}

//	public Ability GetAbility (string name) {
//		if (s_dAbilityLookup.ContainsKey(name)) {
//			Ability newmove = new Ability();
//			if (s_dAbilityLookup.TryGetValue(name, out newmove)) {
//				return newmove;
//			}
//			else {
//				Debug.LogError("Ability " + name + " failed to be gotten!");
//				return null;
//			}
//		}
//		else {
//			return null;
//		}
//	}
}
