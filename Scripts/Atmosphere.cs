using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atmosphere : MonoBehaviour {


	float dotProduct;

	Material skyBoy;


	PlanetGenerator planetGen;

	// Use this for initialization
	void Start () {
		planetGen = GetComponentInParent<PlanetGenerator> ();
		skyBoy = RenderSettings.skybox;
	}
	
	// Update is called once per frame
	void Update () {
		dotProduct = planetGen.direct;
		if (dotProduct < -0.4f)
			skyBoy.SetFloat ("_SkyBlend", 1);

		if (dotProduct > 0.4f)
			skyBoy.SetFloat ("_SkyBlend", 0);

		if (dotProduct < 0.4f && dotProduct > -0.4f) {
			float value = (dotProduct + 0.4f) / 0.8f;
			skyBoy.SetFloat ("_SkyBlend", 1 - value);
		}
	}
}
