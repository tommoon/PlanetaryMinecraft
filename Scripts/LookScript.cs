using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookScript : MonoBehaviour {

	Camera main;
	inventory inv;
	Item[] items;
	public Sprite testy;


	// Use this for initialization
	void Start () {
		main = GetComponentInChildren<Camera> ();
		inv = GameObject.FindGameObjectWithTag("Inventory").GetComponent<inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0))
			check ();
	}

	void check() {
		RaycastHit hit;
		Ray forwardRay = new Ray (main.transform.position, main.transform.forward);


		if(Physics.Raycast(forwardRay, out hit, 4) && Time.timeScale !=0){
			
		if (hit.collider.isTrigger) {

				if (hit.collider.GetComponent<voxelHolder> ().interact (3) == "yes") {
					Debug.Log ("TOUCH WATER");
				Item loot = hit.collider.GetComponent<voxelHolder> ().itemheld;
				inv.AddItem (loot);
			}
			return;
		}
			if (hit.collider.GetComponent<voxelHolder> ().interact (3) == "yes") {
				Item loot = hit.collider.GetComponent<voxelHolder> ().itemheld;
				inv.AddItem (loot);
			} 
		}
	}
}
