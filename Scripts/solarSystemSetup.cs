using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solarSystemSetup : MonoBehaviour {

	public Transform voxel;
	public Transform worldSeed;
	public Transform Sunlight;

	public Material sunmat;

	Transform lightbeam;
	Transform world;
	Transform sun;

	int sunSize;
	int orbit;
	List <Vector3> planetLoc;

	public float speed = 0.5f;

	public int [] planetradi;

	public itemLibrary itemlib;

	// Use this for initialization
	void Start () {
		
		spawnSun ();
		orbit = Random.Range (100, 200);

		world = Instantiate (worldSeed, transform.position + new Vector3(orbit,0,0), Quaternion.identity) as Transform;
		world.name = "World";
		world.gameObject.GetComponent<PlanetGenerator> ().itemlib = itemlib;

	}
	
	// Update is called once per frame


	void spawnSun () {
		sunSize = Random.Range (10, 100);
		float suncolorsize = 1 - (sunSize / 100f);
		Color suncol = new Color (1, suncolorsize, 0);

		sun = Instantiate (voxel, transform.position, Quaternion.identity) as Transform;
		sun.name = "sun";

		sun.gameObject.transform.localScale = new Vector3(sunSize,sunSize,sunSize);

		MeshRenderer sunrend = sun.gameObject.GetComponent<MeshRenderer> ();
		sunrend.material = sunmat;
		sunrend.material.SetColor("_EmissionColor", suncol);
		sunrend.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
		sunrend.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		sunrend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		foreach (var planet in planetradi) {
			lightbeam = Instantiate (Sunlight, transform.position, Quaternion.identity, transform) as Transform;
		}
	}

	void spawnPlanets () {
		int planNum = 1;
		Vector3 worldPosition = new Vector3(100,0,0);
		foreach (var planet in planetradi) {
			world = Instantiate (worldSeed, worldPosition, Quaternion.identity, sun);
			world.name = "Planet " + planNum;
			planNum += 1;


		}
	}

	void FixedUpdate () {
		lightbeam.transform.LookAt (world);
		world.Rotate (Vector3.up * Time.fixedDeltaTime);
		world.RotateAround (sun.position, Vector3.up, speed * Time.deltaTime);
	}

	void trigProblem () {
	
	}
}
