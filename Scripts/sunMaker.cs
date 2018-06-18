using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunMaker : MonoBehaviour {
	
	public Material sun;
	public Transform voxel;
	Transform theSun;
	MeshRenderer mesh;
	public Transform Sunlight;
	Transform lightbeam;

	// Use this for initialization
	void Start () {

		theSun = Instantiate (voxel, transform.position, transform.rotation,transform) as Transform; 
		mesh = theSun.GetComponent<MeshRenderer> ();
		mesh.material = sun;
		mesh.receiveShadows = false;
		mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		mesh.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
		mesh.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

		lightbeam = Instantiate (Sunlight, transform.position, Quaternion.identity, transform) as Transform;

	}
	

}
