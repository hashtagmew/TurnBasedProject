using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class FightTestManager : MonoBehaviour {

	public GameUnit guAttacker;
	public GameUnit guDefender;

	public Button butMelee;

	public Text txtAttacker;
	public Text txtDefender;

	public Text txtDistance;
	public Text txtPhys;
	public Text txtRang;
	public Text txtMagi;

	public int iDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		txtAttacker.text = "HP: " + guAttacker.fHealth.ToString() + "/" + guAttacker.fMaxHealth.ToString() + "\n" +
			"AP: " + guAttacker.fAP.ToString() + "/" + guAttacker.fMaxAP.ToString() + "\n" +
			"Phys ATK: " + guAttacker.fPhysAttack.ToString() + "\n" +
			"Rang ATK: " + guAttacker.fRangAttack.ToString() + "\n" +
			"Magi ATK: " + guAttacker.fMagiAttack.ToString() + "\n" +
			"DEF: " + guAttacker.fDefence.ToString() + "\n" +
			"RES: " + guAttacker.fResistance.ToString();

		txtDefender.text = "HP: " + guDefender.fHealth.ToString() + "/" + guDefender.fMaxHealth.ToString() + "\n" +
			"AP: " + guDefender.fAP.ToString() + "/" + guDefender.fMaxAP.ToString() + "\n" +
			"Phys ATK: " + guDefender.fPhysAttack.ToString() + "\n" +
			"Rang ATK: " + guDefender.fRangAttack.ToString() + "\n" +
			"Magi ATK: " + guDefender.fMagiAttack.ToString() + "\n" +
			"DEF: " + guDefender.fDefence.ToString() + "\n" +
			"RES: " + guDefender.fResistance.ToString();

		txtDistance.text = "Distance: " + iDistance.ToString();

		txtPhys.text = ChanceToHit(iDistance, 0, 0).ToString() + "% for " + 
			CalculateDamage((int)guAttacker.fPhysAttack, 0, (int)guDefender.fDefence, 0).ToString() + " damage!";

		txtRang.text = ChanceToHit(iDistance, 0, 0, false, true).ToString() + "% for " + 
			CalculateDamage((int)guAttacker.fRangAttack, 0, (int)guDefender.fDefence, 0).ToString() + " damage!";

		txtMagi.text = ChanceToHit(iDistance, 0, 0, true).ToString() + "% for " + 
			CalculateDamage((int)guAttacker.fMagiAttack, 0, (int)guDefender.fResistance, 0).ToString() + " damage!";

		butMelee.interactable = iDistance > 1 ? false : true;
	}

	public void AttackPhys() {
		if (RollChanceToHit(iDistance, 0, 0)) {
			guDefender.fHealth -= CalculateDamage((int)guAttacker.fPhysAttack, 0, (int)guDefender.fDefence, 0);
		}
		else {
			Debug.Log("MISS!");
		}
	}

	public void AttackRang() {
		if (RollChanceToHit(iDistance, 0, 0, false, true)) {
			guDefender.fHealth -= CalculateDamage((int)guAttacker.fRangAttack, 0, (int)guDefender.fDefence, 0);
		}
		else {
			Debug.Log("MISS!");
		}
	}

	public void AttackMagi() {
		if (RollChanceToHit(iDistance, 0, 0, true)) {
			guDefender.fHealth -= CalculateDamage((int)guAttacker.fMagiAttack, 0, (int)guDefender.fResistance, 0);
		}
		else {
			Debug.Log("MISS!");
		}
	}

	public void AdjustDistance(int dis) {
		iDistance += dis;
		iDistance = Mathf.Clamp(iDistance, 1, 100);
	}

	private int CalculateDamage(int attack, int attackbonus, int defence, int defencebonus, bool ignoredef = false) {
		return Combat.CombatMechanics.CalculateDamage(attack, attackbonus, defence, defencebonus, ignoredef);
	}

	public int ChanceToHit(int distance, int hitbonus, int hitpenalty, bool guaranteed = false, bool inverserange = false) {
		return Combat.CombatMechanics.ChanceToHit(distance, hitbonus, hitpenalty, guaranteed, inverserange);
	}

	private bool RollChanceToHit(int distance, int hitbonus, int hitpenalty, bool guaranteed = false, bool inverserange = false) {
		return Combat.CombatMechanics.RollChanceToHit(distance, hitbonus, hitpenalty, guaranteed, inverserange);
	}

	public void SwapUnits() {
		GameUnit temp = guAttacker;
		guAttacker = guDefender;
		guDefender = temp;
	}

	public void LeaveScene() {
		Application.LoadLevel("main-menu");
	}
}
