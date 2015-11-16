using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System;
using System.Text;

[ExecuteInEditMode]
public class TilePicker : EditorWindow {

	static private MapEditor s_maped;

	static private Vector2 s_vScrollPos;
	static private MapEditorLogic s_maplogic;

	static public TERRAIN_TYPE s_iLastSelection = TERRAIN_TYPE.BIOMASS;
	static public TERRAIN_TYPE s_iSelection = TERRAIN_TYPE.NONE;

	static public FEATURE_TYPE s_iLastFeatureSelection = FEATURE_TYPE.TREE;
	static public FEATURE_TYPE s_iFeatureSelection = FEATURE_TYPE.NONE;

	static public TERRAIN_TYPE s_eTransition = TERRAIN_TYPE.NONE;
	static public TERRAIN_ORIENTATION s_eTileRot = TERRAIN_ORIENTATION.UP;

	static public MAPED_TOOL s_iTool;

	static public Texture s_texTile;

	static private XDocument s_xmlDoc;
	static private string s_sLastFile;

	static private bool s_bPaintMode = true;

	static private string s_sName = "";
	static private string s_sDescription = "";

	static public int iColumns;
	static public int iRows;
	
	static public List<Light> s_a_goLight;
	static public Color colLight = Color.white;
	static public Vector3 vLightPos = new Vector3(10, 6, -4);
	static public float fLightRange = 50.0f;
	static public float fLightIntensity = 1;

	//Window
	[MenuItem("Map Edit/Tile Picker")]
	static void Init() {
		TilePicker window = (TilePicker)EditorWindow.GetWindow(typeof(TilePicker));

		if (GameObject.FindGameObjectWithTag("MapLight") != null) {
			s_a_goLight = new List<Light>();

			GameObject[] goObjs = GameObject.FindGameObjectsWithTag("MapLight");

			s_a_goLight.Clear();

			foreach (GameObject obj in goObjs) {
				s_a_goLight.Add(obj.GetComponent<Light>());
			}
		}
		
		window.Show();
	}

	void Update() {
		if (EditorApplication.currentScene.Contains("map-editor")) {
			//Find editor
			if (s_maped == null) {
				if (GameObject.FindGameObjectWithTag("GameMap") != null) {
					s_maped = GameObject.FindGameObjectWithTag("GameMap").GetComponent<MapEditor>();
					if (s_maped != null) {
						Repaint();
						Debug.Log("Found map!");
					}
				}
			}

			//Find Editor Logic
			if (s_maplogic == null) {
				if (GetMapEditorLogic() != null) {
					s_maplogic = GetMapEditorLogic();
					Repaint();
					Debug.Log("Found map logic!");
				}
			}
			else {
				if (s_maplogic.bPaintMode != s_bPaintMode) {
					s_maplogic.bPaintMode = s_bPaintMode;

					if (s_bPaintMode) {
						Debug.Log("Changed paint mode: TERRAIN.");
					}
					else {
						Debug.Log("Changed paint mode: FEATURES.");
					}
				}
			}

			//Check for a "picked up" tile
			if (s_maplogic != null) {
				if (s_maplogic.iLastCapturedTile != s_maplogic.iCapturedTile) {
					s_iLastSelection = (TERRAIN_TYPE)s_maplogic.iLastCapturedTile;
					s_iSelection = s_iLastSelection;
					s_maplogic.iCapturedTile = s_maplogic.iLastCapturedTile;

					LoadTileTexture();
					Repaint();
				}
			}
			else {
				//Debug.Log("nolog");
			}
		}
	}
	
	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		s_bPaintMode = EditorGUILayout.ToggleLeft("", s_bPaintMode);
		if (s_bPaintMode) {
			GUILayout.Label("Paint Terrain");
		}
		else {
			GUILayout.Label("Paint Features");
		}
		GUILayout.EndHorizontal();

		//Painting terrain tiles
		if (s_bPaintMode) {
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("Top");
			GUILayout.Label("Tile");
			s_iSelection = (TERRAIN_TYPE)EditorGUILayout.EnumPopup(s_iSelection);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Transition");
			s_eTransition = (TERRAIN_TYPE)EditorGUILayout.EnumPopup(s_eTransition);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Rotation");
			s_eTileRot = (TERRAIN_ORIENTATION)EditorGUILayout.EnumPopup(s_eTileRot);
			GUILayout.EndHorizontal();

			if (s_iLastSelection != s_iSelection) {
				LoadTileTexture();
			}
			GUILayout.Label(s_texTile, GUILayout.Width(100), GUILayout.Height(100));
		}
		//Painting terrain features
		else {
			GUILayout.BeginHorizontal();
			
			GUI.SetNextControlName("Top");
			GUILayout.Label("Feature");
			s_iFeatureSelection = (FEATURE_TYPE)EditorGUILayout.EnumPopup(s_iFeatureSelection);

			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		GUILayout.Label("Tools");
		s_iTool = (MAPED_TOOL)EditorGUILayout.EnumPopup(s_iTool);
		GUILayout.EndHorizontal();

		if (GUILayout.Button("Toggle Tips")) {
			s_maplogic.bShowTips = !s_maplogic.bShowTips;
		}

		if (GUILayout.Button("Erase All")) {
			if (s_maplogic != null) {
				s_maplogic.EraseAll();
			}
		}
		GUILayout.EndVertical();


		GUILayout.BeginVertical("box");
		GUILayout.Label("Map Properties");
		s_sName = EditorGUILayout.TextField("Map name:", s_sName);
		GUILayout.Label("Map description:");
		s_sDescription = EditorGUILayout.TextArea(s_sDescription, GUILayout.Height(50));

		GUILayout.BeginHorizontal();

		if (s_maped != null) {
			GUILayout.Label("Current size: " + s_maped.iColumns.ToString() + " - " + s_maped.iRows.ToString());
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		iColumns = EditorGUILayout.IntField("Columns", iColumns);
		iColumns = Mathf.Clamp(iColumns, 5, 100);
		iRows = EditorGUILayout.IntField("Rows", iRows);
		iRows = Mathf.Clamp(iRows, 5, 100);
		GUILayout.EndHorizontal();


		if (GUILayout.Button("Resize")) {
			if (GetMapEditorLogic() != null) {
				GetMapEditorLogic().Resize(iRows, iColumns);
				GUI.FocusControl("Top");
			}
		}

		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Light");
		if (GUILayout.Button("Reload")) {
			FindMapLights();
			if (s_a_goLight.Count > 0) {
				ReloadLightSettings();
			}
		}
		GUILayout.EndHorizontal();
		if (s_a_goLight != null && s_a_goLight.Count > 0) {
			colLight = EditorGUILayout.ColorField("Color", colLight);
			vLightPos = EditorGUILayout.Vector3Field("Position", vLightPos);
			fLightRange = EditorGUILayout.FloatField("Range", fLightRange);
			fLightRange = Mathf.Clamp(fLightRange, 0.0f, 1000.0f);
			fLightIntensity = EditorGUILayout.FloatField("Intensity", fLightIntensity);
			fLightIntensity = Mathf.Clamp(fLightIntensity, 0.0f, 8.0f);

			if (GUILayout.Button("Center on map")) {
				vLightPos = new Vector3(0 + (float)s_maped.iRows / 2.0f, 
				                        0 + (float)s_maped.iColumns / 2.0f, 
				                        vLightPos.z);
			}
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Make Changes")) {
				s_a_goLight.ElementAt(0).color = colLight;
				s_a_goLight.ElementAt(0).transform.position = vLightPos;
				s_a_goLight.ElementAt(0).range = fLightRange;
				s_a_goLight.ElementAt(0).intensity = fLightIntensity;
			}
			if (GUILayout.Button("Cancel Changes")) {
				ReloadLightSettings();
			}
			GUILayout.EndHorizontal();
		}
		else {
			GUILayout.Label("Please tag a light(s) with \"MapLight\" then press Reload");
		}

		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if (GUILayout.Button("Save Map")) {
			if (s_sName.Length > 2) {
				s_sLastFile = EditorUtility.SaveFilePanel("Save map", "/Resources/Maps/", s_sName.ToLower(), "xml");
				if (s_sLastFile.Length != 0) {
					GUI.FocusControl("Top");
					SaveMapToXML(s_sLastFile);
				}
			}
			else {
				EditorUtility.DisplayDialog("Nope", "Please make sure you have a map name!", "OK");
			}
		}

		if (GUILayout.Button("Load Map")) {
			if (EditorUtility.DisplayDialog("Are you sure?", "Loading a new map will cause you to lose any unsaved changes.", "Continue", "Cancel")) {
				s_sLastFile = EditorUtility.OpenFilePanel("Load map", "/Resources/Maps/", "xml");
				if (s_sLastFile.Length != 0) {
					GUI.FocusControl("Top");
					LoadMap(s_sLastFile);
				}
			}
		}
		GUILayout.EndVertical();

		GUILayout.EndScrollView();
	}

	static private MapEditorLogic GetMapEditorLogic() {
		MapEditorLogic[] editors = (MapEditorLogic[])Resources.FindObjectsOfTypeAll(typeof(MapEditorLogic));
		if (editors.Length > 0) {
			return editors[0];
		}
		else {
			return null;
		}
	}

	static private void LoadTileTexture() {
		if (s_iSelection == TERRAIN_TYPE.DUST_BOWL) {
			s_texTile = ((Material)Resources.Load("Terrain/dirt")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.PLAINS) {
			s_texTile = ((Material)Resources.Load("Terrain/grass")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.RIVER) {
			s_texTile = ((Material)Resources.Load("Terrain/water")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.WASTELAND) {
			s_texTile = ((Material)Resources.Load("Terrain/wasteland")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.DESERT) {
			s_texTile = ((Material)Resources.Load("Terrain/desert")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.QUAGMIRE) {
			s_texTile = ((Material)Resources.Load("Terrain/swamp")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.BIOMASS) {
			s_texTile = ((Material)Resources.Load("Terrain/flesh")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.PAVEMENT) {
			s_texTile = ((Material)Resources.Load("Terrain/tiles")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.LAVA) {
			s_texTile = ((Material)Resources.Load("Terrain/lava")).GetTexture(0);
		}
		else {
			s_texTile = ((Material)Resources.Load("Terrain/none")).GetTexture(0);
		}

		s_iLastSelection = s_iSelection;
	}

	void FindMapLights() {
		if (GameObject.FindGameObjectWithTag("MapLight") != null) {
			s_a_goLight = new List<Light>();

			GameObject[] goObjs = GameObject.FindGameObjectsWithTag("MapLight");
			
			s_a_goLight.Clear();
			
			foreach (GameObject obj in goObjs) {
				s_a_goLight.Add(obj.GetComponent<Light>());
			}
		}
		else {
			s_a_goLight.Clear();
		}
	}

	void ReloadLightSettings() {
		colLight = s_a_goLight.ElementAt(0).color;
		vLightPos = s_a_goLight.ElementAt(0).transform.position;
		fLightRange = s_a_goLight.ElementAt(0).range;
		fLightIntensity = s_a_goLight.ElementAt(0).intensity;
	}

	void SaveMapToXML(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;
		
		XmlWriter writer = XmlWriter.Create(s_sLastFile, xsettings);
		
		//Document open
		writer.WriteStartDocument();
		writer.WriteStartElement("thernion_map");
		writer.WriteWhitespace("\n");
		
		//Name
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("name");
		writer.WriteValue(s_sName);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Desc
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("description");
		writer.WriteValue(s_sDescription);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		//Lights
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("lights");
		writer.WriteWhitespace("\n");

		if (s_a_goLight == null || s_a_goLight.Count < 1) {
			EditorUtility.DisplayDialog("Error", "Error saving map: please ensure a light is loaded in the TileEditor!\nThe map has not been saved.", "OK");
		}

		foreach (Light bulb in s_a_goLight) {
			writer.WriteWhitespace("\t\t");
			writer.WriteStartElement("light");
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("type");
			writer.WriteValue((int)bulb.type);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("color");
			writer.WriteValue(bulb.color.ToHexStringRGBA());
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("positionx");
			writer.WriteValue(bulb.gameObject.transform.position.x);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("positiony");
			writer.WriteValue(bulb.gameObject.transform.position.y);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("positionz");
			writer.WriteValue(bulb.gameObject.transform.position.z);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("range");
			writer.WriteValue(bulb.range);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("intensity");
			writer.WriteValue(bulb.intensity);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t");
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}

		writer.WriteWhitespace("\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Size
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("mapsizex");
		writer.WriteValue(s_maped.iRows);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		writer.WriteWhitespace("\t");

		writer.WriteStartElement("mapsizey");
		writer.WriteValue(s_maped.iColumns);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Tiles
		MapTile temptile = null;

		writer.WriteWhitespace("\t");
		writer.WriteStartElement("tiles");
		writer.WriteWhitespace("\n");
		for (int i = 0; i < s_maped.transform.childCount; i++) {
			temptile = (MapTile)s_maped.transform.GetChild(i).GetComponent<MapTile>();

			writer.WriteWhitespace("\t\t");
			writer.WriteStartElement("tile");
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("type");
			writer.WriteValue((int)temptile.iType);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("transition");
			writer.WriteValue((int)temptile.iTransitionType);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("rotation");
			writer.WriteValue((int)temptile.eOrient);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("xpos");
			writer.WriteValue(temptile.vGridPosition.x);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("ypos");
			writer.WriteValue(temptile.vGridPosition.y);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			if (temptile.l_tfFeatures.Count > 0) {
				foreach (TerrainFeature tf in temptile.l_tfFeatures) {
					writer.WriteWhitespace("\t\t\t");
					writer.WriteStartElement("feature");
					writer.WriteAttributeString("type", ((int)tf.iType).ToString());
					writer.WriteEndElement();
					writer.WriteWhitespace("\n");
				}
			}

			writer.WriteWhitespace("\t\t");
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}
		writer.WriteEndElement();
		
		//Document end
		writer.WriteWhitespace("\n");
		writer.WriteEndElement();
		writer.WriteEndDocument();
		
		writer.Close();

	}

	void LoadMap(string path) {
		s_xmlDoc = XDocument.Load(path);

		int tempcols = 0;
		int temprows = 0;

		if (s_maped == null) {
			Debug.Log("NO MAPED");
		}

		s_maplogic.EraseAll();
		//Clear all the old scene lights
		GameObject[] lighting = GameObject.FindGameObjectsWithTag("MapLight");
		if (lighting != null && lighting.Count() > 0) {
			for (int lighti = lighting.Count() - 1; lighti > -1; lighti--) {
				GameObject.DestroyImmediate(lighting[lighti]);
			}
		}
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1_properties in xroot.Elements()) {
				if (xlayer1_properties.Name == "name") {
					s_sName = xlayer1_properties.Value;
				}
				else if (xlayer1_properties.Name == "description") {
					s_sDescription = xlayer1_properties.Value;
				}
				else if (xlayer1_properties.Name == "mapsizex") {
					temprows = int.Parse(xlayer1_properties.Value);
				}
				else if (xlayer1_properties.Name == "mapsizey") {
					tempcols = int.Parse(xlayer1_properties.Value);
				}

				if (xlayer1_properties.Name == "lights") {
					foreach (XElement xlayer2_tiles in xlayer1_properties.Elements()) {
						GameObject TempLightObj = GameObject.Instantiate(s_maped.goProtoLight);
						Light TempLight = TempLightObj.GetComponent<Light>();
						TempLight.tag = "MapLight";

						foreach (XElement xlayer3_tiledata in xlayer2_tiles.Elements()) {
							if (xlayer3_tiledata.Name == "type") {
								TempLight.type = (LightType)int.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "positionx") {
								TempLight.transform.position = new Vector3(float.Parse(xlayer3_tiledata.Value), TempLight.transform.position.y, TempLight.transform.position.z);
							}
							else if (xlayer3_tiledata.Name == "positiony") {
								TempLight.transform.position = new Vector3(TempLight.transform.position.x, float.Parse(xlayer3_tiledata.Value), TempLight.transform.position.z);
							}
							else if (xlayer3_tiledata.Name == "positionz") {
								TempLight.transform.position = new Vector3(TempLight.transform.position.x, TempLight.transform.position.y, float.Parse(xlayer3_tiledata.Value));
							}
							else if (xlayer3_tiledata.Name == "range") {
								TempLight.range = float.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "intensity") {
								TempLight.intensity = float.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "color") {
								Color TempColor = Color.black;
								Color.TryParseHexString(xlayer3_tiledata.Value, out TempColor);
								TempLight.color = TempColor;
							}
						}
					}
				}
				
				if (xlayer1_properties.Name == "tiles") {
					GetMapEditorLogic().Resize(temprows, tempcols);
					foreach (XElement xlayer2_tiles in xlayer1_properties.Elements()) {
						GameObject TempTile = GameObject.Instantiate(s_maped.goProtoTile);
						TempTile.transform.SetParent(s_maped.transform);

						foreach (XElement xlayer3_tiledata in xlayer2_tiles.Elements()) {
							if (xlayer3_tiledata.Name == "type") {
								TempTile.GetComponent<MapTile>().iType = (TERRAIN_TYPE)int.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "transition") {
								TempTile.GetComponent<MapTile>().iTransitionType = (TERRAIN_TYPE)int.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "rotation") {
								TempTile.GetComponent<MapTile>().eOrient = (TERRAIN_ORIENTATION)int.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "xpos") {
								TempTile.GetComponent<MapTile>().vGridPosition = new Vector2(int.Parse(xlayer3_tiledata.Value), TempTile.GetComponent<MapTile>().vGridPosition.y);
							}
							else if (xlayer3_tiledata.Name == "ypos") {
								TempTile.GetComponent<MapTile>().vGridPosition = new Vector2(TempTile.GetComponent<MapTile>().vGridPosition.x, int.Parse(xlayer3_tiledata.Value));
							}
							else if (xlayer3_tiledata.Name == "feature") {
								GameObject tempobj = GameObject.Instantiate(s_maped.goProtoFeature);
								TempTile.GetComponent<MapTile>().l_tfFeatures.Add(tempobj.GetComponent<TerrainFeature>());

								tempobj.transform.SetParent(TempTile.transform);
								tempobj.transform.localRotation = TempTile.transform.localRotation;
								int value = int.Parse(xlayer3_tiledata.Attribute("type").Value);
								tempobj.GetComponent<TerrainFeature>().Terraform((FEATURE_TYPE)value);
								tempobj.transform.localPosition = new Vector3(0.0f, 0.0f, (tempobj.transform.localScale.z / 2) * -1);
							}
						}

						TempTile.GetComponent<MapTile>().Terraform(TempTile.GetComponent<MapTile>().iType, TempTile.GetComponent<MapTile>().iTransitionType, TempTile.GetComponent<MapTile>().eOrient);

						TempTile.name = string.Format("Tile_{0}_{1}", TempTile.GetComponent<MapTile>().vGridPosition.x, TempTile.GetComponent<MapTile>().vGridPosition.y);
						TempTile.transform.localPosition = new Vector3(TempTile.GetComponent<MapTile>().vGridPosition.x + 0.5f, TempTile.GetComponent<MapTile>().vGridPosition.y + 0.5f, 0);
						TempTile.transform.localRotation = Quaternion.identity;
						TempTile.transform.localScale = new Vector3(1, 1, 0.1f);
					}
				}
			}
		}

		Repaint();
	}
}
