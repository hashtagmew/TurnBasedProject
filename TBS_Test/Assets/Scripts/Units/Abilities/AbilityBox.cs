using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityBox {

	public static Dictionary<string, Ability> s_dAbilityLookup;

	public Ability GetAbility (string name) {
		if (s_dAbilityLookup.ContainsKey(name)) {
			Ability newmove = new Ability();
			if (s_dAbilityLookup.TryGetValue(name, out newmove)) {
				return newmove;
			}
			else {
				Debug.LogError("Ability " + name + " failed to be gotten!");
				return null;
			}
		}
		else {
			return null;
		}
	}
}
