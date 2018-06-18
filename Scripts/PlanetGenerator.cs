using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetGenerator : MonoBehaviour {

	public itemLibrary itemlib;
	public occluder Occ;


	public int planetRadius;
	public int coreRadius;
	public int treeNum;
	public int playSize;
	int worth;
	public float direct;

	public Material edge;
	public Material grass;
	public Material snow;
	public Material core;
	public Material ground;
	public Material agua;
	public Material leaf;
	public Material log;
	public Material coldWater;
	public Material sand;


	Vector3 worldRotation;
	Vector3 startPos;

	List<voxel> North = new List<voxel> ();
	List<voxel> South = new List<voxel> ();
	List<voxel> Left = new List<voxel> ();
	List<voxel> Right = new List<voxel> ();
	List<voxel> Front = new List<voxel> ();
	List<voxel> Back = new List<voxel> ();

	List<voxel> Surface = new List<voxel> ();
	public List<voxel> trees = new List<voxel> ();
	List<voxel> watervoxels = new List<voxel> ();

	public voxel[,,] spaces;

	public Transform Voxelprefab;
	public Transform Player;
	public Transform TreeSeed;
	public Transform grassprefab;

	Transform playertrans;
	public Transform sun;

	// Use this for initialization
	void Start () {

		planetRadius = 7;
		coreRadius = 3;
		treeNum = 4;
		playSize = Mathf.RoundToInt (planetRadius * 4f);
		Map (planetRadius, coreRadius, playSize);
		assignSurface ();
		AssignUnderWorld ();
		fill ();
		Water ();
		Beaches ();
		decorate ();
		addTrees ();



		sun = GameObject.Find ("sun").transform;
		dropPlayer (planetRadius);
		addGrass ();

	}

	void Update () {
		Vector3 sunDirection = (sun.position - transform.position).normalized;
		Vector3 playerUp = playertrans.up.normalized;
		direct = Vector3.Dot (sunDirection, playerUp);

		Vector3 playerOcc = GameObject.FindGameObjectWithTag ("Player").transform.localPosition.normalized;

	}


	void Map (int rad, int coreRad, int playArea) {
		spaces = new voxel[playArea, playArea, playArea];
		for (int i = 0; i <= playSize - 1; i++) {
			for (int j = 0; j <= playSize - 1; j++) {
				for (int k = 0; k <= playSize - 1; k++) {

					Vector3 locator = (CoordToPos (i, j, k));

					float absX = Mathf.Abs (locator.x);
					float absy = Mathf.Abs (locator.y);
					float absz = Mathf.Abs (locator.z);

					Vector3 absLoc = new Vector3 (absX, absy, absz);
					//the voxel with its location in space
					spaces [i, j, k] = new voxel (transform.position - locator);
					spaces [i, j, k].ident = "null";
					//the voxels pposition in the array;
					spaces [i, j, k].idenLoc = new Vector3 (i, j, k);
					//the voxels location in space with no negative values.
					spaces [i, j, k].absLoc = absLoc;

					if ((locator.x >= -coreRad && locator.x <= coreRad) && (locator.y >= -coreRad && locator.y <= coreRad) && (locator.z >= -coreRad && locator.z <= coreRad)) {
						if ((absX == 0) && (absy == 0) && (absz == 0)) {
							spaces [i, j, k].ident = "edge";

						}


						continue;
					}




					if (((absX == absy) && (absz <= absX)) || ((absX == absz) && (absy <= absX)) || ((absz == absy) && (absX <= absy))) {
						if ((absX <= rad) && (absy <= rad) && (absz <= rad)) {
							spaces [i, j, k].ident = "edge";
						}
					}



				}
			}
		}
	}
						
	void assignSurface () {

		foreach (var item in spaces) {
			Vector3 locator = item.location - transform.position;
			Vector3 idenLoc = item.idenLoc;



			if ((item.absLoc.y > item.absLoc.x) && (item.absLoc.y > item.absLoc.z)) {
				// north
				if (Mathf.RoundToInt (locator.y) == planetRadius) {
					
					int new_y = perlinifiy (idenLoc.y, idenLoc.x, idenLoc.z, 10, 5) - 2;
					voxel holder = spaces [Mathf.RoundToInt(idenLoc.x), new_y, Mathf.RoundToInt(idenLoc.z)];
					if (holder.absLoc.y > holder.absLoc.x && holder.absLoc.y > holder.absLoc.z) {
						holder.ident = "north";
						holder.face = "north";
						North.Add (holder);
					}
				}
				//south
				if (Mathf.RoundToInt (locator.y) == -planetRadius) {
					int new_y = perlinifiy (idenLoc.y, idenLoc.x, idenLoc.z, 4, 4) - 2;
					voxel holder = spaces [Mathf.RoundToInt(idenLoc.x), new_y, Mathf.RoundToInt(idenLoc.z)];
					if (holder.absLoc.y > holder.absLoc.x && holder.absLoc.y > holder.absLoc.z) {
						holder.ident = "south";
						holder.face = "south";
						South.Add (holder);
					}
				}
			}
			if ((item.absLoc.y < planetRadius) && (item.absLoc.z < planetRadius)) {
				//left
				if (Mathf.RoundToInt (locator.x) == planetRadius) {
					int new_X = perlinifiy (idenLoc.x, idenLoc.y, idenLoc.z, 15, 6) - 3;
					voxel holder = spaces [new_X, Mathf.RoundToInt(idenLoc.y), Mathf.RoundToInt(idenLoc.z)];
					if (holder.absLoc.z < holder.absLoc.x && holder.absLoc.y < holder.absLoc.x) {
						holder.ident = "left";
						holder.face = "left";
						Left.Add (holder);
					}
				}
				//right
				if (Mathf.RoundToInt (locator.x) == -planetRadius) {
					int new_X = perlinifiy (idenLoc.x, idenLoc.y, idenLoc.z, 6, 10) - 3;
					voxel holder = spaces [new_X, Mathf.RoundToInt(idenLoc.y), Mathf.RoundToInt(idenLoc.z)];
					if (holder.absLoc.z < holder.absLoc.x && holder.absLoc.y < holder.absLoc.x) {
						holder.ident = "right";
						holder.face = "right";
						Right.Add (holder);
					}
				}
			}
			if ((item.absLoc.y < planetRadius) && (item.absLoc.x < planetRadius)) {
			//back
				if (Mathf.RoundToInt (locator.z) == planetRadius) {
					int new_z = perlinifiy (idenLoc.z, idenLoc.y, idenLoc.x, 20, 5) - 3 ;
					voxel holder = spaces [Mathf.RoundToInt(idenLoc.x), Mathf.RoundToInt(idenLoc.y), new_z];
					if (holder.absLoc.x < holder.absLoc.z && holder.absLoc.y < holder.absLoc.z) {
						holder.ident = "back";
						holder.face = "back";
						Back.Add (holder);
					}
				}
				//front
				if (Mathf.RoundToInt (locator.z) == -planetRadius) {
					int new_z = perlinifiy (idenLoc.z, idenLoc.y, idenLoc.x, 10, 3) - 1 ;
					voxel holder = spaces [Mathf.RoundToInt(idenLoc.x), Mathf.RoundToInt(idenLoc.y), new_z];
					if (holder.absLoc.x < holder.absLoc.z && holder.absLoc.y < holder.absLoc.z) {
						holder.ident = "front";
						holder.face = "front";
						Front.Add (holder);
					}
				}
			}
		}
	}
	void AssignUnderWorld () {
	//north
		foreach (var vox in North) {
			int y = Mathf.RoundToInt(vox.location.y - transform.position.y) - 1;
			int locY = Mathf.RoundToInt(vox.idenLoc.y) + 1;
			while (y > coreRadius) {
				voxel holder = spaces [Mathf.RoundToInt (vox.idenLoc.x), locY, Mathf.RoundToInt (vox.idenLoc.z)];
				if (holder.absLoc.y > holder.absLoc.x && holder.absLoc.y > holder.absLoc.z) {
					if (holder.absLoc.y > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "north";
					} else {
						holder.ident = "core";
					}
				}
				y -= 1;
				locY += 1;
			}
		}

		foreach (var vox in South) {
			int y = Mathf.RoundToInt(vox.location.y - transform.position.y) + 1;
			int locY = Mathf.RoundToInt(vox.idenLoc.y) - 1;
			while (y < -coreRadius) {
				voxel holder = spaces [Mathf.RoundToInt (vox.idenLoc.x), locY, Mathf.RoundToInt (vox.idenLoc.z)];
				if (holder.absLoc.y > holder.absLoc.x && holder.absLoc.y > holder.absLoc.z) {
					if (holder.absLoc.y > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "south";
					} else {
						holder.ident = "core";
					}
				}
				y += 1;
				locY -= 1;
			}
		}

		foreach (var vox in Left) {
			int x = Mathf.RoundToInt(vox.location.x - transform.position.x) - 1;
			int locX = Mathf.RoundToInt(vox.idenLoc.x) + 1;
			while (x > coreRadius) {
				voxel holder = spaces [locX, Mathf.RoundToInt (vox.idenLoc.y), Mathf.RoundToInt (vox.idenLoc.z)];
				if (holder.absLoc.x > holder.absLoc.y && holder.absLoc.x > holder.absLoc.z) {
					if (holder.absLoc.x > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "left";
					} else {
						holder.ident = "core";
					}
				}
				x -= 1;
				locX += 1;
			}
		}

		foreach (var vox in Right) {
			int x = Mathf.RoundToInt(vox.location.x - transform.position.x) + 1;
			int locX = Mathf.RoundToInt(vox.idenLoc.x) - 1;
			while (x < -coreRadius) {
				voxel holder = spaces [locX, Mathf.RoundToInt (vox.idenLoc.y), Mathf.RoundToInt (vox.idenLoc.z)];
				if (holder.absLoc.x > holder.absLoc.y && holder.absLoc.x > holder.absLoc.z) {
					if (holder.absLoc.x > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "right";
					} else {
						holder.ident = "core";
					}
				}
				x += 1;
				locX -= 1;
			}
		}

		foreach (var vox in Back) {
			int z = Mathf.RoundToInt(vox.location.z - transform.position.z) - 1;
			int locZ = Mathf.RoundToInt(vox.idenLoc.z) + 1;
			while (z > coreRadius) {
				voxel holder = spaces [Mathf.RoundToInt (vox.idenLoc.x), Mathf.RoundToInt (vox.idenLoc.y), locZ];
				if (holder.absLoc.z > holder.absLoc.x && holder.absLoc.z > holder.absLoc.y) {
					if (holder.absLoc.z > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "back";
					} else {
						holder.ident = "core";
					}
				}
				z -= 1;
				locZ += 1;
			}
		}

		foreach (var vox in Front) {
			int z = Mathf.RoundToInt(vox.location.z - transform.position.z) + 1;
			int locZ = Mathf.RoundToInt(vox.idenLoc.z) - 1;
			while (z <- coreRadius) {
				voxel holder = spaces [Mathf.RoundToInt (vox.idenLoc.x), Mathf.RoundToInt (vox.idenLoc.y), locZ];
				if (holder.absLoc.z > holder.absLoc.x && holder.absLoc.z > holder.absLoc.y) {
					if (holder.absLoc.z > planetRadius - 3) {
						holder.ident = "ground";
						holder.face = "front";
					} else {
						holder.ident = "core";
					}
				}
				z += 1;
				locZ -= 1;
			}
		}
	}

	void fill (){
		Material filler = null;
		string loot = null;
		string face = null;
		string itemassign = "Null";

		foreach (var cube in spaces) {
			if (cube.ident != "null") {
				if (cube.ident == "edge") {
					filler = edge;
					worth = 9;
				}

				if (cube.ident == "north") {
					filler = snow;
					face = "north";
					worth = 3;
					loot = "snow";
				}

				if (cube.ident == "south") {
					filler = snow;
					face = "south";
					worth = 3;
					loot = "snow";
				}

				if (cube.ident == "left") {
					filler = grass;
					face = "left";
					worth = 2;
					loot = "soil";
					itemassign = "Dirt";

					if(cube.absLoc.x >= planetRadius)
						Surface.Add (cube);
					cube.colorheight = Mathf.PerlinNoise (cube.idenLoc.y/15, cube.idenLoc.z/15);
				}

				if (cube.ident == "right") {
					filler = grass;
					face = "right";
					worth = 2;
					loot = "soil";
					itemassign = "Dirt";

					if(cube.absLoc.x >= planetRadius)
						Surface.Add (cube);
					cube.colorheight = Mathf.PerlinNoise (cube.idenLoc.y/6, cube.idenLoc.z/6);
				}

				if (cube.ident == "front") {
					filler = grass;
					face = "front";
					worth = 2;
					loot = "soil";
					itemassign = "Dirt";

					if(cube.absLoc.z >= planetRadius)
					Surface.Add (cube);
					cube.colorheight = Mathf.PerlinNoise (cube.idenLoc.y/10, cube.idenLoc.x/10);
				}

				if (cube.ident == "back") {
					filler = grass;
					face = "back";
					worth = 2;
					loot = "soil";
					itemassign = "Dirt";

					if(cube.absLoc.z >= planetRadius)
						Surface.Add (cube);
					cube.colorheight = Mathf.PerlinNoise (cube.idenLoc.y/20, cube.idenLoc.x/20);
				}

				if (cube.ident == "core") {
					filler = core;
				}

				if (cube.ident == "ground") {
					filler = ground;
					worth = 4;
					loot = "stone";
				}
				Transform vox = Instantiate (Voxelprefab, cube.location, Quaternion.identity, transform) as Transform;
				vox.name = cube.ident + cube.location;
				cube.assignedTo = true;
				cube.face = face;
				cube.assignedToCube = vox.gameObject;
				vox.gameObject.GetComponent<voxelHolder> ().BlockID = cube.ident;
				vox.gameObject.GetComponent<voxelHolder> ().value = worth;
				vox.gameObject.GetComponent<voxelHolder> ().Loot = loot;
				vox.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find (itemassign);

				if (filler != null) {
					vox.GetComponent<MeshRenderer> ().material = filler;
				}
				}
			}
		}

	void decorate () {
		foreach (var item in Surface) {
		/*	float hue, sat, vat;
			Color.RGBToHSV (item.assignedToCube.GetComponent<MeshRenderer>().material.color, out hue, out sat, out vat);
			Debug.Log (item.colorheight + " " + item.absLoc);
			Color newcol = Color.HSVToRGB (hue, sat, item.colorheight);
			item.assignedToCube.GetComponent<MeshRenderer>().material.color = newcol;
			*/
			float height = item.colorheight;
			Color newcol = new Color (height, height, height) * item.assignedToCube.GetComponent<MeshRenderer>().material.color;
			item.assignedToCube.GetComponent<MeshRenderer>().material.color = newcol;
		}
	}

	void Water () {
		foreach (var item in spaces) {
			if (!item.assignedTo) {
				Vector3 Abs = item.absLoc;
				if (((Abs.y == planetRadius) && (Abs.x < planetRadius) && (Abs.z < planetRadius))
				    || ((Abs.x == planetRadius) && (Abs.y < planetRadius) && (Abs.z < planetRadius))
				    || ((Abs.z == planetRadius) && (Abs.x < planetRadius) && (Abs.y < planetRadius))) {
					Transform water = Instantiate (Voxelprefab, item.location, Quaternion.identity, transform) as Transform;
					water.gameObject.GetComponent<voxelHolder> ().BlockID = "water";
					water.gameObject.GetComponent<voxelHolder> ().Loot = "water";
					water.gameObject.GetComponent<voxelHolder> ().value = 2;
					water.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Water");
					waterScript wetscript = water.gameObject.AddComponent<waterScript> ();
					item.assignedTo = true;
					item.assignedToCube = water.gameObject;
					if (Abs.y == planetRadius) {
						water.GetComponent<MeshRenderer> ().material = coldWater;
					} else {
						water.GetComponent<MeshRenderer> ().material = agua;
						water.GetComponent<Collider> ().isTrigger = true;
						watervoxels.Add (item);
					}

					float height = 0;
					int x = 0;
					int y = 0;
					float z = 0;
					string identifier = null;

					if (item.absLoc.y == planetRadius) {
						
						height = item.absLoc.y;
						x = Mathf.RoundToInt(item.absLoc.x);
						y = Mathf.RoundToInt(item.absLoc.z);
						z = water.transform.localPosition.y;

						if (item.location.y > transform.position.y) {
							identifier = "north";
						}
						if (item.location.y < transform.position.y) {
							identifier = "south";
						}
					}

					if (item.absLoc.x == planetRadius) {

						height = item.absLoc.x;
						x = Mathf.RoundToInt(item.absLoc.y);
						y = Mathf.RoundToInt(item.absLoc.z);
						z = water.transform.localPosition.x;

						if (item.location.x > transform.position.x) {
							identifier = "left";
						}
						if (item.location.x < transform.position.x) {
							identifier = "right";
						}
					}

					if (item.absLoc.z == planetRadius) {
						
						height = item.absLoc.z;
						x = Mathf.RoundToInt(item.absLoc.x);
						y = Mathf.RoundToInt(item.absLoc.y);
						z = water.transform.localPosition.z;

						if (item.location.z < transform.position.z) {
							identifier = "front";
						}
						if (item.location.z > transform.position.z) {
							identifier = "back";
						}
					}

					wetscript.height = height;
					wetscript.xSample = x;
					wetscript.zSample = y;
					wetscript.fixedHeight = z;
					wetscript.identifier = identifier;

					water.name = "water " + identifier + z;
				}
			}
		}
	}

	void Beaches () {
		foreach (var voxy in watervoxels) {
			foreach (var item in getNeighbours(voxy)) {
				if (item.ident == "front" || item.ident == "back" || item.ident == "left" || item.ident == "right") {
					item.assignedToCube.GetComponent<MeshRenderer> ().material = sand;
					item.assignedToCube.GetComponent<voxelHolder> ().BlockID = "sand";
					item.assignedToCube.GetComponent<voxelHolder> ().value = 3;
					item.assignedToCube.GetComponent<voxelHolder> ().Loot = "sand";
					item.assignedToCube.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Sand");
				}
			}
		}
	}

	void addTrees () {


		Vector3 facing = Vector3.zero;
		Vector3 spawn = Vector3.zero;
		Vector3 absSpawn = Vector3.zero;

		for (int i = 0; i < treeNum; i++) {
			
			voxel coord = Surface [Random.Range (0, Mathf.RoundToInt (Surface.Count - 1))];
			string face = coord.face;
			float refCord = 0;
		
			if (coord.ident == "front") {
				facing = Vector3.back;
				refCord = coord.absLoc.z;
			}

			if (coord.ident == "back") {
				facing = Vector3.forward;
				refCord = coord.absLoc.z;
			}

			if (coord.ident == "left") {
				facing = Vector3.right;
				refCord = coord.absLoc.x;
			}

			if (coord.ident == "right") {
				facing = Vector3.left;
				refCord = coord.absLoc.x;
			}
		
			bool goAhead = true;
			Vector3 idenspawn = coord.idenLoc + facing;
			voxel treeVox = spaces [Mathf.RoundToInt (idenspawn.x), Mathf.RoundToInt (idenspawn.y), Mathf.RoundToInt (idenspawn.z)];
			treeVox.face = face;
			foreach (var vox in getNeighbours(treeVox)) {
				if (vox.ident == "tree") {
					goAhead = false;
					treeNum += 1;
					Debug.Log ("hit");
					break;

				}
			}

			coord.assignedToCube.AddComponent<treeScript> ();
			treeScript tree = coord.assignedToCube.GetComponent<treeScript> ();
			tree.leafy = leaf;
			tree.itemlib = itemlib;
			tree.loggy = log;
			tree.seed = coord;
			tree.tring = i;
		}
	}

	void addGrass(){
		Vector3 facing = Vector3.zero;
		foreach (var item in Surface) {
			if (item.assignedToCube.GetComponent<voxelHolder> ().BlockID != "sand") {
				float val = Random.value;
				if (item.ident == "front") {
					facing = Vector3.back;
				}

				if (item.ident == "back") {
					facing = Vector3.forward;
				}

				if (item.ident == "left") {
					facing = Vector3.right;
				}

				if (item.ident == "right") {
					facing = Vector3.left;
				}

				if (val > 0.5f) {
					Vector3 loc = item.location + facing;
					Vector3 idenloc = item.idenLoc + facing;
					Quaternion up = Quaternion.Euler (facing - item.assignedToCube.transform.position);

					Transform grass = Instantiate (grassprefab, loc, up, transform) as Transform;
					grass.up = facing;
					grass.name = "grass" + item.location;
					grass.gameObject.GetComponent<MeshCollider> ().convex = true;
					grass.gameObject.GetComponent<MeshCollider> ().isTrigger = true;
					grass.gameObject.GetComponentInChildren<MeshCollider> ().convex = true;
					grass.gameObject.GetComponentInChildren<MeshCollider> ().isTrigger = true;
					grassScript gscript = grass.gameObject.AddComponent<grassScript> ();
					gscript.basevox = item.assignedToCube.transform;
					gscript.ident = item.ident;
				}
			}
		}
	}	


	public int perlinifiy (float value, float op1, float op2, int scaleValue, int scaleModifier){
		int newy = Mathf.RoundToInt(value) + (Mathf.RoundToInt (Mathf.PerlinNoise ((op1 + 10)/scaleValue, (op2 + 10)/scaleValue) * scaleModifier));
		return newy;
	}

	void dropPlayer(int rad){
		voxel startPos = Front [Random.Range (0, Mathf.RoundToInt (Front.Count - 1))];
		Vector3 pos = (startPos.location + new Vector3 (0,0,- 2));
		Transform player = Instantiate (Player, pos, Quaternion.Euler (Vector3.forward),transform);
		player.GetComponent<GravityBody> ().attractor = gameObject.GetComponent<GravityAttractor>();
		playertrans = player;
		player.gameObject.tag = "Player";
	}

	Vector3 CoordToPos(int x, int y, int z) {
		return new Vector3 (- playSize/2 + x, - playSize/2 + y, - playSize/2 + z);
	}

	public List<voxel> getNeighbours(voxel toCheck){
		List <voxel> neighbours = new List<voxel> ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					
					if (x == 0 && y == 0 && z == 0)
						continue;

					int checkX = Mathf.RoundToInt(toCheck.idenLoc.x) + x;
					int checkY = Mathf.RoundToInt(toCheck.idenLoc.y) + y;
					int checkZ = Mathf.RoundToInt(toCheck.idenLoc.z) + z;


					if (checkX >= 0 && checkX < playSize && checkY >= 0 && checkY < playSize && checkZ >= 0 && checkZ < playSize) {
						neighbours.Add (spaces [checkX, checkY, checkZ]);
					}
				}
			}
		}
		return neighbours;
	}
}
