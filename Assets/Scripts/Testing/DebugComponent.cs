using UnityEngine;
using System.Collections;

public class DebugComponent : MonoBehaviour {
	//public Vector3 pivot = new Vector3(0.5f, 0.5f, 0.5f);
	//public Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
	public Color color = ColorNames.Aqua.toColor();
	public float sphereRadius = 1.0f;
	public int sphereU = 16;
	public int sphereV = 16;

	public float cylinderRadius = 1.0f;
	public float cylinderHeight = 1.0f;
	public float cylinderVPivot = 0.0f;
	public int cylinderU = 8;
	public int cylinderV = 1;

	public Transform targetObject;
	public float lineRadius = 1.0f;
	public int lineUSegments = 8;
	public int lineVSegments = 8;

	public float coneHeight = 2.0f;
	public float coneRadius = 0.5f;
	public float conePivot = 0.0f;
	public int coneUSegments = 5;
	public int coneVSegments = 1;

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
		//GizmoTools.drawWireSphere(transform, sphereRadius, sphereU, sphereV);
		//GizmoTools.drawWireCylinder(transform, cylinderRadius, cylinderHeight, cylinderVPivot, cylinderU, cylinderV);
		Gizmos.color = oldColor;

		GizmoTools.drawWireCone(transform, coneRadius, coneHeight, conePivot, coneUSegments, coneVSegments);

		if (targetObject)
			GizmoTools.drawConeLineAdaptive(transform.position, targetObject.position, lineRadius, lineUSegments);
	}
}
