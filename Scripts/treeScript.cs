using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeScript : MonoBehaviour {

	public int tring;
	voxel[,,] spaces;
	public voxel seed;
	PlanetGenerator planetgen;
	Vector3 identity;
	Vector3 idenspawn;

	public Material leafy;
	public Material loggy;

	public itemLibrary itemlib;

	int limit;
	int height;
	float refCord;

	Vector3 facing;

	List<voxel> trunk = new List<voxel> ();
	List<voxel> leaves = new List<voxel> ();

	// Use this for initialization
	void Start () {
		spaces = GetComponentInParent<PlanetGenerator> ().spaces;
		planetgen = GetComponentInParent<PlanetGenerator> ();
		limit = Random.Range (2, 5);
		identity = seed.idenLoc;

		Settings ();


	}

	void Settings() {
		
		if (seed.ident == "front") {
			facing = Vector3.back;
			refCord = seed.absLoc.z;
		}

		if (seed.ident == "back") {
			facing = Vector3.forward;
			refCord = seed.absLoc.z;
		}

		if (seed.ident == "left") {
			facing = Vector3.right;
			refCord = seed.absLoc.x;
		}

		if (seed.ident == "right") {
			facing = Vector3.left;
			refCord = seed.absLoc.x;
		}


		int check = Mathf.RoundToInt(refCord) + limit;

		while (check >= planetgen.playSize/2) {
				Debug.Log ("tall boy");
				limit -= 1;
				check = Mathf.RoundToInt(refCord) + limit;
			}
			
		height = limit;
		plant ();

	}

	void plant () {
		Vector3 loc = seed.location + facing;
		idenspawn = seed.idenLoc - facing;
		while (limit > 0) {
			limit -= 1;
			voxel treeVox = spaces [Mathf.RoundToInt(idenspawn.x), Mathf.RoundToInt(idenspawn.y), Mathf.RoundToInt(idenspawn.z)];
			Transform Treetrunk = Instantiate (planetgen.Voxelprefab, treeVox.location, Quaternion.identity, transform) as Transform;
			treeVox.assignedTo = true;
			treeVox.face = seed.face;
			Treetrunk.name = "Tree " + tring + " (" + limit + ")";
			treeVox.ident = "tree";
			treeVox.treeNum = limit;
			Treetrunk.gameObject.GetComponent<voxelHolder> ().BlockID = "tree";
			Treetrunk.gameObject.GetComponent<voxelHolder> ().value = 6;
			Treetrunk.gameObject.GetComponent<MeshRenderer> ().material = loggy;
			treeVox.assignedToCube = Treetrunk.gameObject;
			planetgen.trees.Add (treeVox);
			idenspawn -= facing;
			loc += facing;
			trunk.Add (treeVox);
		}

		foliage ();
	}


	void foliage () {
		
		foreach (var trun in trunk) {

			Vector3 leafIdentitySpawn = Vector3.zero;
			Vector3 location = Vector3.zero;

			//top leaf
			if (trun.treeNum == 0) {
				leafIdentitySpawn = trun.idenLoc + facing;
				location = trun.location + facing;

				Transform leaf = Instantiate (planetgen.Voxelprefab, location, Quaternion.identity, transform) as Transform;
				leaf.name = "leaf " + trun.location + trun.idenLoc;
				leaf.gameObject.GetComponent<voxelHolder> ().BlockID = "leaf";
				leaf.gameObject.GetComponent<voxelHolder> ().value = 2;
				leaf.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Dry Sticks");
				leaf.gameObject.GetComponent<MeshRenderer> ().material = leafy;

				voxel leafVox = spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)];
				leafVox.assignedTo = true;
				leafVox.ident = "leaf";
				leafVox.assignedToCube = leaf.gameObject;
				leaves.Add (leafVox);

				if ((trun.face  == "front") || (trun.face  == "back")) {
					for (int i = -1; i <= 1; i++) {
						for (int j = -1; j <= 1; j++) {
							if ((i == 0 && j == 0) || (i == -1 && j == -1) || (i == 1 && j == 1) || (i == -1 && j == 1) || (i == 1 && j == -1))
								continue;
		

							Vector3Int addit = new Vector3Int (i, j, 0);

							leafIdentitySpawn = trun.idenLoc + addit;
							location = trun.location + addit;

							if (spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)].assignedTo == true)
								continue;
							
							leaf = Instantiate (planetgen.Voxelprefab, location, Quaternion.identity, transform) as Transform;
							leaf.name = "leaf " + trun.location + addit;
							leaf.gameObject.GetComponent<voxelHolder> ().BlockID = "leaf";
							leaf.gameObject.GetComponent<voxelHolder> ().value = 2;
							leaf.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Dry Sticks");
							leaf.gameObject.GetComponent<MeshRenderer> ().material = leafy;

							leafVox = spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)];
							leafVox.assignedTo = true;
							leafVox.ident = "leaf";
							leafVox.assignedToCube = leaf.gameObject;
							leaves.Add (leafVox);
						}
					}
				}

				if ((trun.face  == "left") || (trun.face  == "right")) {
					for (int i = -1; i <= 1; i++) {
						for (int j = -1; j <= 1; j++) {
							if ((i == 0 && j == 0) || (i == -1 && j == -1) || (i == 1 && j == 1) || (i == -1 && j == 1) || (i == 1 && j == -1))
								continue;


							Vector3Int addit = new Vector3Int (0, j, i);

							leafIdentitySpawn = trun.idenLoc + addit;
							location = trun.location + addit;

							if (spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)].assignedTo == true)
								continue;

							leaf = Instantiate (planetgen.Voxelprefab, location, Quaternion.identity, transform) as Transform;
							leaf.name = "leaf " + trun.location + addit;
							leaf.gameObject.GetComponent<voxelHolder> ().BlockID = "leaf";
							leaf.gameObject.GetComponent<voxelHolder> ().value = 2;
							leaf.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Dry Sticks");
							leaf.gameObject.GetComponent<MeshRenderer> ().material = leafy;

							leafVox = spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)];
							leafVox.assignedTo = true;
							leafVox.ident = "leaf";
							leafVox.assignedToCube = leaf.gameObject;
							leaves.Add (leafVox);
						}
					}
				}
			}

			//mid leafs
			if (trun.treeNum == 1 && height >= 3) {
				
				if ((trun.face  == "front") || (trun.face  == "back")) {
					for (int i = -2; i <= 2; i++) {
						for (int j = -2; j <= 2; j++) {
							if ((i == 0 && j == 0) || (i == -2 && j == -2) || (i == 2 && j == 2) || (i == -2 && j == 2) || (i == 2 && j == -2))
								continue;

							if ((i == -2 && j == -1) || (i == 2 && j == 1) || (i == -2 && j == 1) || (i == 2 && j == -1))
								continue;

							if ((i == -1 && j == -2) || (i == 1 && j == 2) || (i == -1 && j == 2) || (i == 1 && j == -2))
								continue;
							
							

							Vector3Int addit = new Vector3Int (i, j, 0);

							leafIdentitySpawn = trun.idenLoc + addit;
							location = trun.location + addit;

							if (spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)].assignedTo == true)
								continue;

							Transform leaf = Instantiate (planetgen.Voxelprefab, location, Quaternion.identity, transform) as Transform;
							leaf.name = "leaf " + trun.location + addit;
							leaf.gameObject.GetComponent<voxelHolder> ().BlockID = "leaf";
							leaf.gameObject.GetComponent<voxelHolder> ().value = 2;
							leaf.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Dry Sticks");
							leaf.gameObject.GetComponent<MeshRenderer> ().material = leafy;

							voxel leafVox = spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)];
							leafVox.assignedTo = true;
							leafVox.ident = "leaf";
							leafVox.assignedToCube = leaf.gameObject;
							leaves.Add (leafVox);
						}
					}
				}

				if ((trun.face  == "left") || (trun.face  == "right")) {
					for (int i = -2; i <= 2; i++) {
						for (int j = -2; j <= 2; j++) {
							if ((i == 0 && j == 0) || (i == -2 && j == -2) || (i == 2 && j == 2) || (i == -2 && j == 2) || (i == 2 && j == -2))
								continue;

							if ((i == -2 && j == -1) || (i == 2 && j == 1) || (i == -2 && j == 1) || (i == 2 && j == -1))
								continue;

							if ((i == -1 && j == -2) || (i == 1 && j == 2) || (i == -1 && j == 2) || (i == 1 && j == -2))
								continue;

							Vector3Int addit = new Vector3Int (0, j, i);

							leafIdentitySpawn = trun.idenLoc + addit;
							location = trun.location + addit;

							if (spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)].assignedTo == true)
								continue;

							Transform leaf = Instantiate (planetgen.Voxelprefab, location, Quaternion.identity, transform) as Transform;
							leaf.name = "leaf " + trun.location + addit;
							leaf.gameObject.GetComponent<voxelHolder> ().BlockID = "leaf";
							leaf.gameObject.GetComponent<voxelHolder> ().value = 2;
							leaf.gameObject.GetComponent<voxelHolder> ().itemheld = itemlib.find ("Dry Sticks");
							leaf.gameObject.GetComponent<MeshRenderer> ().material = leafy;

							voxel leafVox = spaces [Mathf.RoundToInt (leafIdentitySpawn.x), Mathf.RoundToInt (leafIdentitySpawn.y), Mathf.RoundToInt (leafIdentitySpawn.z)];
							leafVox.assignedTo = true;
							leafVox.ident = "leaf";
							leafVox.assignedToCube = leaf.gameObject;
							leaves.Add (leafVox);

						}
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var leaf in leaves) {
			int xsample = 0;
			int zsample = Mathf.RoundToInt (leaf.absLoc.y);
			float fixedHeight = 0;

			if(leaf.ident == "front" || leaf.ident == "back"){
				xsample = Mathf.RoundToInt(leaf.absLoc.x);
				fixedHeight = leaf.assignedToCube.transform.localPosition.z;
					}

			if(leaf.ident == "left" || leaf.ident == "right"){
				xsample = Mathf.RoundToInt(leaf.absLoc.z);
				fixedHeight = leaf.assignedToCube.transform.localPosition.x;
			}


			float Height = 0.3f * Mathf.PerlinNoise(Time.time+(xsample*0.2f), Time.time+(zsample*0.2f));

			Vector3 newVector = Vector3.zero;

		}
	}
}
