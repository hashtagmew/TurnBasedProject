using UnityEngine;
using System.Collections;

public enum ABILITY_TYPE {
	NONE = 0,
	ACTIVE,
	PASSIVE
}

public class Ability : ScriptableObject {

	public ABILITY_TYPE iType;
}
