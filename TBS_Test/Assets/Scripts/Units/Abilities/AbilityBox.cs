using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

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

		TextAsset[] taAbilities = Resources.LoadAll<TextAsset> ("UnitAbilities/");
		
		foreach (TextAsset ta in taAbilities) {
			//s_xmlDoc = XDocument.Load ("Assets/Resources/UnitAbilities/" + ta.name + ".xml");
			XDocument s_xmlDoc = XDocument.Load(new StringReader(ta.text));
			
			Ability TempAbil = new Ability (ABILITY_TYPE.NONE);
			
			//DO MORE!!
			foreach (XElement xroot in s_xmlDoc.Elements()) {
				foreach (XElement xlayer1 in xroot.Elements()) {
					if (xlayer1.Name == "type") {
						TempAbil.iType = (ABILITY_TYPE)int.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "description") {
						TempAbil.sDescription = xlayer1.Value;
					} 
					else if (xlayer1.Name == "name") {
						TempAbil.sName = xlayer1.Value;
					}
					else if (xlayer1.Name == "use_ranged_calc") {
						int tempint = int.Parse (xlayer1.Value);
						if (tempint == 0) {
							TempAbil.bRangedAttack = false;
						}
						else {
							TempAbil.bRangedAttack = true;
						}
					} 
					else if (xlayer1.Name == "target") {
						TempAbil.iTargetType = (EFFECT_TARGET)int.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "range") {
						TempAbil.iRange = int.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "soundset") {
						TempAbil.sSoundset = xlayer1.Value.ToString();
					} 
					else if (xlayer1.Name == "activation_delay") {
						TempAbil.fDelayStart = float.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "execution_delay") {
						TempAbil.fDelayRun = float.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "resolution_delay") {
						TempAbil.fDelayFinish = float.Parse (xlayer1.Value);
					} 
					else if (xlayer1.Name == "activation_particle") {
						TempAbil.sParticleStart = xlayer1.Value.ToString();
					} 
					else if (xlayer1.Name == "execution_particle") {
						TempAbil.sParticleRun = xlayer1.Value.ToString();
					} 
					else if (xlayer1.Name == "resolution_particle") {
						TempAbil.sParticleFinish = xlayer1.Value.ToString();
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

					if (xlayer1.Name == "resolution_effects") {
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
