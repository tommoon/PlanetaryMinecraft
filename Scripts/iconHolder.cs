using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class iconHolder : MonoBehaviour {
	
	public Item held;

	public GameObject descBox;
	public Text desc;

	public bool holding; 
	// Use this for initialization
	void Start () {
		holding = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0) {
			transform.position = Input.mousePosition;
		}
	}
}
