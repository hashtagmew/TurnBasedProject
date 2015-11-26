using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using System.IO;

public class NetGameMap : MonoBehaviour {

	public GameObject goProtoTile;
	public GameObject goProtoFeature;
	public GameObject goProtoLight;

	public float fTileSize = 1.0f;
	
	public int iMapHorzSize = 13;
	public int iMapVertSize = 11;
	List <List<MapTile>> map = new List<List<MapTile>>();

	public bool bGridToggle = false;

	public string sName = "Map";
	public string sDescription = "";

	public string sMapFile;
	
	void Start() {
		if (PhotonNetwork.room != null) {
			sMapFile = "Maps/" + PhotonNetwork.room.customProperties["Map"] as string;
			LoadMap(sMapFile);
		}
		else {
			Debug.LogError("There is no network room!");
			LoadMap("Maps/janette_map_1");
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.G)) {
			ToggleGrid();
		}
	}

	public void Resize(int rows, int cols) {
		iMapHorzSize = rows;
		iMapVertSize = cols;
	}

	public void ToggleGrid() {
		bGridToggle = !bGridToggle;
		
		for (int i = 0; i < iMapHorzSize; i++) {
			for (int j = 0; j < iMapVertSize; j++) {
				GetTile(i, j).bGridEnabled = bGridToggle;
			}
		}
	}
	
	public MapTile GetTile(int x, int y) {
		return map[y][x];
	}

	public void LoadMap(string path) {
		XDocument s_xmlDoc = new XDocument();
		TextAsset taLevel = Resources.Load<TextAsset>(path);
		XmlDocument xdoc = new XmlDocument();

		xdoc.LoadXml(taLevel.text);
		
		if (taLevel == null) {
			Debug.LogError("Failed to load level " + path + "!");
		}

		//Convert XmlDocument to XDocument
		using (MemoryStream memStream = new MemoryStream()) {
			using (XmlWriter tempwrite = XmlWriter.Create(memStream)) {
				xdoc.WriteContentTo(tempwrite);
			}
			memStream.Seek(0, SeekOrigin.Begin);
			using (XmlReader tempread = XmlReader.Create(memStream)) {
				s_xmlDoc = XDocument.Load(tempread);
			}
		}
		
		if (s_xmlDoc == null) {
			Debug.LogError("Failed to load xml " + path + "!");
		}
		
		int tempcols = 0;
		int temprows = 0;

		GameObject goTileCheck;

		//Erase old map
//		for (int i = 0; i < iMapVertSize; i++) {
//			for (int j = 0; j < iMapHorzSize; j++) {
//				goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
//				if (goTileCheck != null) {
//					GameObject.Destroy(goTileCheck);
//				}
//			}
//		}

		//Clear all the old scene lights
//		GameObject[] lighting = GameObject.FindGameObjectsWithTag("MapLight");
//		if (lighting != null && lighting.Count() > 0) {
//			for (int lighti = lighting.Count() - 1; lighti > -1; lighti--) {
//				GameObject.DestroyImmediate(lighting[lighti]);
//			}
//		}

		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1_properties in xroot.Elements()) {
				if (xlayer1_properties.Name == "name") {
					sName = xlayer1_properties.Value;
				}
				else if (xlayer1_properties.Name == "description") {
					sDescription = xlayer1_properties.Value;
				}
				else if (xlayer1_properties.Name == "mapsizex") {
					temprows = int.Parse(xlayer1_properties.Value);
				}
				else if (xlayer1_properties.Name == "mapsizey") {
					tempcols = int.Parse(xlayer1_properties.Value);
				}

				iMapHorzSize = temprows;
				iMapVertSize = tempcols;
				
				if (xlayer1_properties.Name == "lights") {
					foreach (XElement xlayer2_tiles in xlayer1_properties.Elements()) {
						GameObject TempLightObj = GameObject.Instantiate(goProtoLight);
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
							else if (xlayer3_tiledata.Name == "rotationx") {
								TempLight.transform.localEulerAngles = new Vector3(float.Parse(xlayer3_tiledata.Value), 0.0f, 0.0f);
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
					Resize(temprows, tempcols);
					foreach (XElement xlayer2_tiles in xlayer1_properties.Elements()) {
						GameObject TempTile = GameObject.Instantiate(goProtoTile);
						TempTile.transform.SetParent(this.transform);
						
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
								GameObject tempobj = GameObject.Instantiate(goProtoFeature);
								TempTile.GetComponent<MapTile>().l_tfFeatures.Add(tempobj.GetComponent<TerrainFeature>());
								
								tempobj.transform.SetParent(TempTile.transform);
								tempobj.transform.localRotation = TempTile.transform.localRotation;
								int value = int.Parse(xlayer3_tiledata.Attribute("type").Value);
								tempobj.GetComponent<TerrainFeature>().Terraform((FEATURE_TYPE)value);
								//tempobj.transform.localPosition = new Vector3(0.0f, 0.0f, (tempobj.transform.localScale.z / 2) * -1);
								tempobj.transform.localPosition = Vector3.zero;
								//Scale doesn't load properly?
								//tempobj.transform.localScale = new Vector3(0.1f, 8.0f, 1.0f);
								tempobj.transform.localScale = new Vector3(1, 1, 10);
							}
						}
						
						TempTile.GetComponent<MapTile>().Terraform(TempTile.GetComponent<MapTile>().iType, TempTile.GetComponent<MapTile>().iTransitionType, TempTile.GetComponent<MapTile>().eOrient);
						
						TempTile.name = string.Format("Tile_{0}_{1}", TempTile.GetComponent<MapTile>().vGridPosition.x, TempTile.GetComponent<MapTile>().vGridPosition.y);
						TempTile.transform.localPosition = new Vector3(TempTile.GetComponent<MapTile>().vGridPosition.x + 0.5f, 0.0f, TempTile.GetComponent<MapTile>().vGridPosition.y + 0.5f);
						TempTile.transform.localEulerAngles = new Vector3(90.0f, 0.0f, (float)TempTile.GetComponent<MapTile>().eOrient);
						TempTile.transform.localScale = new Vector3(1, 1, 0.1f);
					}
				}
			}
		}
	}
}
