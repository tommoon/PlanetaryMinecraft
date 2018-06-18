using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour {

	public GravityAttractor attractor;
	private Transform mytransfrom;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = false;
		mytransfrom = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		attractor.Attract (mytransfrom);
	}
}
