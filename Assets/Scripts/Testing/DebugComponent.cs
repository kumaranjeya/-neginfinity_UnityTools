using UnityEngine;
using System.Collections;

public class DebugComponent : MonoBehaviour {
	//public Vector3 pivot = new Vector3(0.5f, 0.5f, 0.5f);
	//public Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
	public Color color = ColorNames.Aqua.toColor();
	public float sphereRadius = 1.0f;
	public int sphereU = 16;
	public int sphereV = 16;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos(){
		var oldColor = Gizmos.color;
		Gizmos.color = color;
		//GizmoTools.drawWireCube(transform, size, pivot, true);
		GizmoTools.drawWireSphere(transform, sphereRadius, sphereU, sphereV);
		Gizmos.color = oldColor;
	}
}
