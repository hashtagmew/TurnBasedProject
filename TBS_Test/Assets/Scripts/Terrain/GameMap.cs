using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

public class GameMap : MonoBehaviour {
	public GameObject TilePrefab;

	public float fTileSize = 0.32f;

	public int iMapHorzSize = 10;
	public int iMapVertSize = 11;
	List <List<MapTile>> map = new List<List<MapTile>>();

	public bool bGridToggle = false;

	static private XDocument s_xmlDoc;

	public GameObject goProtoTile;
	public GameObject goProtoFeature;
	public GameObject goProtoLight;

	public string sName = "Map";
	public string sDescription = "";


	void Start() {
		GenerateMap();  
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

	public void LoadMap(string path) {
		s_xmlDoc = XDocument.Load(path);
		
		int tempcols = 0;
		int temprows = 0;

		//Delete old map
		GameObject goTileCheck;

		for (int i = 0; i < iMapVertSize; i++) {
			for (int j = 0; j < iMapHorzSize; j++) {
				goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
				if (goTileCheck != null) {
					GameObject.Destroy(goTileCheck);
				}
			}
		}
		
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
						TempTile.transform.SetParent(transform);
						
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
	}

	public void GenerateMap() {
		map = new List<List<MapTile>>();
		for (int i = 0; i < iMapHorzSize; i++) {
			List<MapTile> row = new List<MapTile>();
			for (int j = 0; j < iMapVertSize; j++) {
				MapTile tile = ((GameObject)Instantiate(TilePrefab, 
				                                        new Vector3(i * fTileSize, 0, j * -fTileSize),
				                                         Quaternion.identity)).GetComponent<MapTile>();
				tile.vGridPosition = new Vector2(i, j);
				tile.gameObject.name = "Tile_" + i.ToString() + "_" + j.ToString();
				tile.transform.parent = this.gameObject.transform;
				row.Add(tile);

				int rnd = Random.Range(0, 11);
				if (rnd <= 6) {
					tile.Terraform(TERRAIN_TYPE.PLAINS, TERRAIN_TYPE.NONE, TERRAIN_ORIENTATION.UP);
				}
				else if (rnd <= 9) {
					tile.Terraform(TERRAIN_TYPE.DUST_BOWL, TERRAIN_TYPE.NONE, TERRAIN_ORIENTATION.UP);
				}
				else {
					tile.Terraform(TERRAIN_TYPE.PAVEMENT, TERRAIN_TYPE.NONE, TERRAIN_ORIENTATION.UP);
				}
			}

			map.Add(row);
		}
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
}