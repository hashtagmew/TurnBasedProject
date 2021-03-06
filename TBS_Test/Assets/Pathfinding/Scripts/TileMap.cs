﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {

	//public DeploymentScript D_S;
	public NetGameMap map;

	public GameObject selectedUnit;
	public GameObject deselectedUnit;

	public TileType[] tileTypes;

	public int[,] tiles;
	Node[,] graph;

	public bool bInitialised = false;

	int mapSizeX = 25;
	int mapSizeY = 3;

	public void Initialise() {


		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
		Debug.Log ("TileMap initialised.");

		bInitialised = true;
	}

	void Start() {

		selectedUnit = null;
		
		// Setup the selectedUnit's variable
//		selectedUnit.GetComponent<GameUnit>().tileX = (int)selectedUnit.transform.position.x;
//		selectedUnit.GetComponent<GameUnit>().tileY = (int)selectedUnit.transform.position.z;
//		selectedUnit.GetComponent<Unit>().map = this;


//		if (gameObject.tag == "Unit") {
//			this.gameObject.GetComponent<GameUnit>().tileX = (int)selectedUnit.transform.position.x;
//			this.gameObject.GetComponent<GameUnit>().tileY = (int)selectedUnit.transform.position.z;
//		}
		
//		GenerateMapData();
//		GeneratePathfindingGraph();
//		GenerateMapVisual();
	}

	void Update(){
		//if (selectedUnit == null) {
			//Debug.Log("No unit selected");
		//}

//		for (int y = 0; y < mapSizeY; y++) {
//			for (int x = 0; x < mapSizeX; x++) {
//				//Debug.Log(gamemap.sMapFile + " " + gamemap.GetTile(1, 1).gameObject.name);
//				if (gamemap.GetTile(x, y).l_tfFeatures != null && gamemap.GetTile(x, y).l_tfFeatures.Count > 0) {
//					if (gamemap.GetTile(x, y).l_tfFeatures[0].iType == FEATURE_TYPE.TREE) {
//						tiles[x, y] = 1;
//					}
//					if (gamemap.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.MOUNTAIN){
//						tiles[x,y] = 1;
//					}
//					if (gamemap.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.WALL){
//						tiles[x,y] = 1;
//					}
//				}
//			}
//		}
	}
	
	void GenerateMapData() {
		mapSizeX = map.iMapHorzSize;
		mapSizeY = map.iMapVertSize;

		// Allocate our map tiles
		tiles = new int[mapSizeX, mapSizeY];
		
		int x,y;
		
		// Initialize our map tiles to be grass
		for(x=0; x < mapSizeX; x++) {
			for(y=0; y < mapSizeX; y++) {
				tiles[x,y] = 0;
				//Debug.Log("TileMap gen " + x.ToString() + ", " + y.ToString());
			}
		}

		for (y = 0; y < mapSizeY; y++) {
			for (x = 0; x < mapSizeX; x++) {
				//Debug.Log(gamemap.sMapFile + " " + gamemap.GetTile(1, 1).gameObject.name);
				if (map.GetTile(x, y).l_tfFeatures != null && map.GetTile(x, y).l_tfFeatures.Count > 0) {
					if (map.GetTile(x, y).l_tfFeatures[0].iType == FEATURE_TYPE.TREE) {
						tiles[x, y] = 1;
					}
					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.MOUNTAIN || map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.HILL){
						tiles[x,y] = 1;
					}
					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.WALL){
						tiles[x,y] = 1;
					}
				}
			}
		}

		// Make a big swamp area
//		for(x=3; x <= 5; x++) {
//			for(y=0; y < 4; y++) {
//				tiles[x,y] = 1;
//			}
//		}
//		//mountain left
//		tiles[2, 5] = 2;
//		tiles[2, 6] = 2;
//		tiles[1, 7] = 2;
//		tiles[1, 9] = 2;
//		tiles[1, 10] = 2;
//		tiles[0, 10] = 2;
//		tiles[0, 12] = 2;
//		tiles[0, 13] = 2;
//		tiles[0, 16] = 2;
//		tiles[0, 17] = 2;
//		tiles[1, 18] = 2;
//		tiles[2, 19] = 2;
//
//		//mountain right
//		tiles[23, 5] = 2;
//		tiles[23, 6] = 2;
//		tiles[23, 7] = 2;
//		tiles[24, 7] = 2;
//		tiles[24, 8] = 2;
//		tiles[24, 9] = 2;
//		tiles[24, 10] = 2;
//		tiles[24, 11] = 2;
//		tiles[24, 12] = 2;
//		tiles[24, 13] = 2;
//		tiles[23, 14] = 2;
//		tiles[23, 16] = 2;
//		tiles[22, 17] = 2;
//
//		//trees right
//		tiles[20, 2] = 2;
//		tiles[20, 3] = 2;
//		tiles[21, 3] = 2;
//		tiles[21, 4] = 2;
//		tiles[21, 5] = 2;
//		tiles[22, 6] = 2;
//		tiles[22, 7] = 2;
//		tiles[22, 8] = 2;
//		tiles[22, 9] = 2;
//		tiles[22, 10] = 2;
//		tiles[22, 11] = 2;
//		tiles[22, 12] = 2;
//		tiles[22, 13] = 2;
//		tiles[22, 14] = 2;
//		tiles[21, 15] = 2;
//		tiles[21, 16] = 2;
//		tiles[20, 17] = 2;
//		tiles[19, 19] = 2;
//		tiles[19, 20] = 2;
//		tiles[18, 20] = 2;
//		tiles[18, 21] = 2;
//		
//		//trees left
//		tiles[6, 3] = 2;
//		tiles[7, 3] = 2;
//		tiles[6, 4] = 2;
//		tiles[5, 5] = 2;
//		tiles[4, 6] = 2;
//		tiles[3, 7] = 2;
//		tiles[2, 8] = 2;
//		tiles[2, 10] = 2;
//		tiles[2, 11] = 2;
//		tiles[1, 12] = 2;
//		tiles[2, 13] = 2;
//		tiles[2, 14] = 2;
//		tiles[2, 15] = 2;
//		tiles[2, 16] = 2;
//		tiles[3, 16] = 2;
//		tiles[3, 17] = 2;
//		tiles[4, 17] = 2;
//		tiles[4, 18] = 2;
//		tiles[5, 18] = 2;
//		tiles[5, 19] = 2;
//
//		//fence
//		tiles[4, 21] = 2;
//		tiles[5, 21] = 2;
//		tiles[6, 21] = 2;
//		tiles[7, 21] = 2;
	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {

		TileType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) {
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}

		return cost;

	}

	void GeneratePathfindingGraph() {
		// Initialize the array
		graph = new Node[mapSizeX,mapSizeY];

		// Initialize a Node for each spot in the array
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their neighbours
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {

				// This is the 4-way connection version:
				if(x > 0)
					graph[x,y].neighbours.Add( graph[x-1, y] );
				if(x < mapSizeX-1)
					graph[x,y].neighbours.Add( graph[x+1, y] );
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );
			}
		}
	}

	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity );

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) {
		Vector3 vec = new Vector3(x, 0, y);
		return vec;
	}

	public bool UnitCanEnterTile(int x, int y) {

		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.

		return tileTypes[ tiles[x,y] ].isWalkable;
	}

	public void GeneratePathTo(int x, int y) {
		// Clear out our unit's old path.
		if (selectedUnit != null) {
			selectedUnit.GetComponent<GameUnit> ().currentPath = null;
		}

		if (UnitCanEnterTile (x, y) == false) {
			// We probably clicked on a mountain or something, so just quit out.
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    selectedUnit.GetComponent<GameUnit>().tileX, 
		                    selectedUnit.GetComponent<GameUnit>().tileY
		                    ];
		

		Node target = graph[
		                    x, 
		                    y
		                    ];
		
		dist[source] = 0;
		prev[source] = null;

		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if(u == target) {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if(prev[target] == null) {
			// No route between our target and the source
			return;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// Right now, currentPath describes a route from our target to our source
		// So we need to invert it!

		currentPath.Reverse();

		selectedUnit.GetComponent<GameUnit>().currentPath = currentPath;
	}

}
