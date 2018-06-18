using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voxelHolder : MonoBehaviour {

	public string BlockID;
	public string Loot;
	public int value = 10;
	public Item itemheld;


	public string interact (int strength) {
		
		if (strength >= value) {
			return "yes";
		} else {
			return "no";
		}
		
	}
}
