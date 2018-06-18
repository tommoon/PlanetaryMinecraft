using System.Collections;
using UnityEngine;
using System;

public class voxel {

	public string ident;
	public string face;
	public string BlockID;

	public Vector3 location;
	public Vector3 idenLoc;
	public Vector3 absLoc;
	public Material leafy;

	public bool assignedTo = false;

	public float colorheight;
	public int treeNum;

	public GameObject assignedToCube;
	public voxelHolder voxelHolder;


	public voxel (Vector3 _loc){
		
		location = _loc;
	}
}
