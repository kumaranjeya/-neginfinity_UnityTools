using UnityEngine;
using System.Collections;

public class CylinderGizmo : BaseGizmo{
	public float radius = 1.0f;
	public float height = 1.0f;
	public float pivot = 0.0f;
	public int uSegments = 8;
	public int vSegments = 1;
	protected override void onDrawGizmo(){
		GizmoTools.drawWireCylinder(transform, radius, height, pivot, uSegments, vSegments);
	}
}
