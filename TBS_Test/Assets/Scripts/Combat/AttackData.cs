using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combat {
	public class AttackData : ScriptableObject {

		public List<DamageData> l_damage = new List<DamageData>();
	}
}