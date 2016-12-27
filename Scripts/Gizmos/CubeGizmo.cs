using UnityEngine;
using System.Collections;

public class CubeGizmo : BaseGizmo{
	public Vector3 size = Vector3.one;
	public Vector3 pivot = new Vector3(0.5f, 0.5f, 0.5f);
	public bool pivotAxes = true;
	protected override void onDrawGizmo(){
		GizmoTools.drawWireCube(transform, size, pivot, pivotAxes);
	}
}
