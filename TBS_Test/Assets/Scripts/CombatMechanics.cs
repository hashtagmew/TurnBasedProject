using UnityEngine;
using System.Collections;

public static class CombatMechanics {

	public static int CalculateDamage(int attack, int attackbonus, int defence, int defencebonus, bool ignoredef = false) {
		int result = 0;
		
		if (!ignoredef) {
			result = (attack + attackbonus) - ((defence + defencebonus) / 2);
		}
		else {
			result = (attack + attackbonus);
		}
		
		result = Mathf.Clamp(result, 0, 99999);
		
		return result;
	}
	
	public static int ChanceToHit(int distance, int hitbonus, int hitpenalty, bool guaranteed = false, bool inverserange = false) {
		if (guaranteed) {
			return 100;
		}
		
		int chance = 0;
		if (!inverserange) {
			chance = 100 + hitbonus - (distance * 5) - hitpenalty;
		}
		else {
			float distmod = 85.0f * (15.0f / ((distance + 1) * 5.0f));
			if (distance < 2) {
				distmod = 85.0f * 0.5f;
			}
			
			chance = 10 + hitbonus + (int)distmod - hitpenalty;
		}
		chance = Mathf.Clamp(chance, 0, 99);
		
		return chance;
	}
	
	public static bool RollChanceToHit(int distance, int hitbonus, int hitpenalty, bool guaranteed = false, bool inverserange = false) {
		if (guaranteed) {
			return true;
		}
		
		int rand = Random.Range(0, 100);
		int chance = ChanceToHit(distance, hitbonus, hitpenalty, guaranteed, inverserange);

		if (Application.isEditor) {
			Debug.Log("Goal: " + rand.ToString() + "\nRoll: " + chance.ToString());
		}
		
		return chance > rand ? true : false;
	}
}
