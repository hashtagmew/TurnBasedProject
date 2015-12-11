using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;

public static class AbilityBox {

	public static Dictionary<string, Ability> s_dAbilityLookup;

	static AbilityBox() {
		ReloadAbilites ();
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

	public static void ReloadAbilites() {
		s_dAbilityLookup = new Dictionary<string, Ability> ();
		
		//#if UNITY_EDITOR
		TextAsset[] taAbilities = Resources.LoadAll<TextAsset> ("UnitAbilities/");
		XDocument s_xmlDoc = new XDocument ();
		
		foreach (TextAsset ta in taAbilities) {
			//FIX FOR GAME
			//#if UNITY_EDITOR
			s_xmlDoc = XDocument.Load ("Assets/Resources/UnitAbilities/" + ta.name + ".xml");
			//#endif
			
			Ability TempAbil = new Ability (ABILITY_TYPE.NONE);
			
			//DO MORE!!
			foreach (XElement xroot in s_xmlDoc.Elements()) {
				foreach (XElement xlayer1 in xroot.Elements()) {
					if (xlayer1.Name == "type") {
						TempAbil.iType = (ABILITY_TYPE)int.Parse (xlayer1.Value);
					} else if (xlayer1.Name == "description") {
						TempAbil.sDescription = xlayer1.Value;
					} else if (xlayer1.Name == "name") {
						TempAbil.sName = xlayer1.Value;
					}
					
					if (xlayer1.Name == "activation_effects") {
						Effect TempEffect = new Effect ();
						foreach (XElement xlayer2 in xlayer1.Elements()) {
							TempEffect = EffectBox.s_dEffectLookup [xlayer2.Value];
							
							//Number val?
							if (xlayer2.Attribute ("power") != null) {
								//Debug.Log("FA NUM: " + xlayer2.FirstAttribute.Value);
								TempEffect.fAdjustFloat = float.Parse (xlayer2.FirstAttribute.Value);
							}
							
							//String val?
							if (xlayer2.Attribute ("path") != null) {
								//Debug.Log("FA STR: " + xlayer2.FirstAttribute.Value);
								TempEffect.sAdjustString = xlayer2.FirstAttribute.Value;
							}
							
							TempAbil.d_EffectsActivation.Add (xlayer2.Value, TempEffect);
						}
					}
				}
			}
			
			s_dAbilityLookup.Add (ta.name, TempAbil);
		}
	}
}
