using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassScript : MonoBehaviour {

	public Vector3 up;
	public Vector3 facing;
	public GameObject player;
	public Transform basevox;
	public string ident;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

	}
}
