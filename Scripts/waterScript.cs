using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScript : MonoBehaviour {

	public float height;
	public int xSample;
	public int zSample;
	public float fixedHeight;

	public float Scale = 0.2f;
	public float ScaleModifier = 0.3f;

	public string identifier;

	public Vector3 heightVariable;

	
	// Update is called once per frame
	void Update () {
		if (identifier == "front" || identifier == "back" || identifier == "right" || identifier == "left") 
		moveWater ();	
	}

	void Start () {

		if (identifier == "north" || identifier == "south")
			moveWater ();
	}

	void moveWater () {
		float Height = ScaleModifier * Mathf.PerlinNoise(Time.time+(xSample*Scale), Time.time+(zSample*Scale));

		SetMatColor(Height);
		ApplyHeight(Height);
	}

	void SetMatColor(float Height)
	{
		GetComponent<Renderer> ().material.color = new Color (0, Height, Height, 0);
	}

	void ApplyHeight(float Height)
	{

		Vector3 newVector = Vector3.zero;

		if (identifier == "front") {
			newVector = new Vector3 (transform.localPosition.x, transform.localPosition.y, fixedHeight + Height);
		}
		if (identifier == "back")
			newVector = new Vector3 (transform.localPosition.x, transform.localPosition.y, fixedHeight - Height);

		if (identifier == "right")
			newVector = new Vector3 (fixedHeight + Height, transform.localPosition.y, transform.localPosition.z);

		if (identifier == "left")
			newVector = new Vector3 (fixedHeight - Height, transform.localPosition.y, transform.localPosition.z);

		if (identifier == "north")
			newVector = new Vector3 (transform.localPosition.x, fixedHeight - Height, transform.localPosition.z);

		if (identifier == "south")
			newVector = new Vector3 (transform.localPosition.x, fixedHeight + Height, transform.localPosition.z);


		transform.localPosition = newVector;
	}		
}
