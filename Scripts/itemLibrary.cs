using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemLibrary : MonoBehaviour {

	public List<Item> library = new List<Item>();
	public Item nullItem;

	public Item find (string toBeFound){
		
		Item seeker = nullItem;

		foreach (var thing in library) {
			if (thing.ID == toBeFound) {
				seeker = thing;
			}
		}

		return seeker;
	}
}
