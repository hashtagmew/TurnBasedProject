using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public class TilePicker : EditorWindow {

	static private MapEditor s_maped;

	static private Vector2 s_vScrollPos;
	static private MapEditorLogic s_maplogic;

	static public TERRAIN_TYPE s_iLastSelection = TERRAIN_TYPE.BIOMASS;
	static public TERRAIN_TYPE s_iSelection = TERRAIN_TYPE.NONE;
	static public MAPED_TOOL s_iTool;

	static public Texture s_texTile;

	static private XDocument s_xmlDoc;
	static private string s_sLastFile;
	static private string s_sName;

	static public int iColumns;
	static public int iRows;

	static public Light goLight;
	static public Color colLight = Color.white;
	static public Vector3 vLightPos = new Vector3(10, 6, -4);
	static public float fLightRange = 50.0f;
	static public float fLightIntensity = 1;

	//Window
	[MenuItem("Map Edit/Tile Picker")]
	static void Init() {
		TilePicker window = (TilePicker)EditorWindow.GetWindow(typeof(TilePicker));

		if (GameObject.FindGameObjectWithTag("MapLight") != null) {
			goLight = GameObject.FindGameObjectWithTag("MapLight").GetComponent<Light>();
		}
		
		window.Show();
	}

	void Update() {
		if (Application.loadedLevelName == "map-editor") {
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
		}
	}

	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);



		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		GUI.SetNextControlName("Top");
		GUILayout.Label("Tile");
		s_iSelection = (TERRAIN_TYPE)EditorGUILayout.EnumPopup(s_iSelection);
		GUILayout.EndHorizontal();

		if (s_iLastSelection != s_iSelection) {
			LoadTileTexture();
		}
		GUILayout.Label(s_texTile, GUILayout.Width(100), GUILayout.Height(100));
		GUILayout.EndVertical();


		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		GUILayout.Label("Tools");
		s_iTool = (MAPED_TOOL)EditorGUILayout.EnumPopup(s_iTool);
		GUILayout.EndHorizontal();

		s_sName = EditorGUILayout.TextField("Map name:", s_sName);

		if (GUILayout.Button("Clear All")) {
			if (s_maplogic != null) {
				s_maplogic.ClearAll();
			}
		}

		if (GUILayout.Button("Erase All")) {
			if (s_maplogic != null) {
				s_maplogic.EraseAll();
			}
		}
		GUILayout.EndVertical();


		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		if (s_maped != null) {
			GUILayout.Label("Current size: " + s_maped.iColumns.ToString() + " - " + s_maped.iRows.ToString());
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		iColumns = EditorGUILayout.IntField("C", iColumns);
		iColumns = Mathf.Clamp(iColumns, 5, 100);
		iRows = EditorGUILayout.IntField("R", iRows);
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
			goLight = FindMapLight();
			if (goLight != null) {
				ReloadLightSettings();
			}
		}
		GUILayout.EndHorizontal();
		if (goLight != null) {
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
				goLight.color = colLight;
				goLight.transform.position = vLightPos;
				goLight.range = fLightRange;
				goLight.intensity = fLightIntensity;
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
			if (s_sName != "") {
				s_sLastFile = EditorUtility.SaveFilePanel("Save map", "/Resources/Maps/", s_sName.ToLower(), "xml");
				if (s_sLastFile.Length != 0) {
					GUI.FocusControl("Top");
					SaveMapToXML(s_sLastFile);
				}
			}
		}

		if (GUILayout.Button("Load Map")) {
			s_sLastFile = EditorUtility.OpenFilePanel("Load map", "/Resources/Maps/", "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				LoadMap(s_sLastFile);
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

	Light FindMapLight() {
		if (GameObject.FindGameObjectWithTag("MapLight") != null) {
			return GameObject.FindGameObjectWithTag("MapLight").GetComponent<Light>();
		}
		else {
			return null;
		}
	}

	void ReloadLightSettings() {
		colLight = goLight.color;
		vLightPos = goLight.transform.position;
		fLightRange = goLight.range;
		fLightIntensity = goLight.intensity;
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
		
		//Light
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("light");
		writer.WriteValue(s_sName);
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
			writer.WriteStartElement("xpos");
			writer.WriteValue(temptile.vGridPosition.x);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t\t");
			writer.WriteStartElement("ypos");
			writer.WriteValue(temptile.vGridPosition.y);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			writer.WriteWhitespace("\t\t");
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		//Document end
		writer.WriteWhitespace("\n");
		writer.WriteEndElement();
		writer.WriteEndDocument();
		
		writer.Close();

	}

	void LoadMap(string path) {
		s_xmlDoc = XDocument.Load(path);
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1_properties in xroot.Elements()) {
				if (xlayer1_properties.Name == "name") {
					s_sName = xlayer1_properties.Value;
				}
				else if (xlayer1_properties.Name == "mapsizex") {
					s_maped.iRows = int.Parse(xlayer1_properties.Value);
				}
				else if (xlayer1_properties.Name == "mapsizey") {
					s_maped.iColumns = int.Parse(xlayer1_properties.Value);
				}
				
				if (xlayer1_properties.Name == "tiles") {
					foreach (XElement xlayer2_tiles in xlayer1_properties.Elements()) {
						foreach (XElement xlayer3_tiledata in xlayer2_tiles.Elements()) {
							GameObject TempTile = GameObject.Instantiate(s_maped.goProtoTile);
							TempTile.transform.SetParent(s_maped.transform);
							if (xlayer3_tiledata.Name == "type") {
								TempTile.GetComponent<MapTile>().iType = (TERRAIN_TYPE)int.Parse(xlayer3_tiledata.Value);
							}
							else if (xlayer3_tiledata.Name == "xpos") {
								TempTile.GetComponent<MapTile>().vGridPosition = new Vector2(int.Parse(xlayer3_tiledata.Value), TempTile.GetComponent<MapTile>().vGridPosition.y);
							}
							else if (xlayer3_tiledata.Name == "ypos") {
								TempTile.GetComponent<MapTile>().vGridPosition = new Vector2(TempTile.GetComponent<MapTile>().vGridPosition.x, int.Parse(xlayer3_tiledata.Value));
							}
							TempTile.transform.position = new Vector3(TempTile.GetComponent<MapTile>().vGridPosition.x, TempTile.GetComponent<MapTile>().vGridPosition.y, 0);
						}
					}
				}
			}
		}
	}
}
