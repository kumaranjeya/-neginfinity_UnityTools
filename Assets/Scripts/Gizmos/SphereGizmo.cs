using UnityEngine;
using System.Collections;

public class SphereGizmo : BaseGizmo{
	public float radius = 1.0f;
	public int uSegments = 8;
	public int vSegments = 8;
	protected override void onDrawGizmo(){
		GizmoTools.drawWireSphere(transform, radius, uSegments, vSegments);
	}
}
