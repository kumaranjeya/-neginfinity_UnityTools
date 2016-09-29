using UnityEngine;
using System.Collections;

public class ConeGizmo: BaseGizmo{
	public float radius = 1.0f;
	public float height = 1.0f;
	public float pivot = 0.0f;
	public int uSegments = 8;
	public int vSegments = 1;
	protected override void onDrawGizmo(){
		GizmoTools.drawWireCone(transform, radius, height, pivot, uSegments, vSegments);
	}
}
