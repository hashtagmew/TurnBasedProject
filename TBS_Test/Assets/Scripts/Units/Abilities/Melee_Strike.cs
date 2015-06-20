using UnityEngine;
using System.Collections;

public class Melee_Strike : Ability, IActivatable {

	public virtual void OnActivation() {
		iType = ABILITY_TYPE.ACTIVE;
	}
}
