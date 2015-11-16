using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combat {
	public class DamageData : ScriptableObject {

		public ABILITY_ELEMENT eElement = ABILITY_ELEMENT.NONE;
		public float fDamage = 0.0f;
	}
}