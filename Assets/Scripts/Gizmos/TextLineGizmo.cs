using UnityEngine;
using System.Collections;

public class TextLineGizmo : BaseGizmo{
	public string text = "TextLine";
	public float letterSize = 0.25f;
	protected override void onDrawGizmo(){
		GizmoTools.drawVectorTextLine(text, transform, letterSize);
	}
}
