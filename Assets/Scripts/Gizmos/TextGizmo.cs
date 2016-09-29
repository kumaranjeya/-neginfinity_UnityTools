using UnityEngine;
using System.Collections;

public class TextGizmo : BaseGizmo{
	[TextArea(3, 20)]
	public string text = "Multi-line Text.";
	public float letterSize = 0.25f;
	public TextAnchor textAnchor = TextAnchor.MiddleCenter;
	public TextAnchor textAlignment = TextAnchor.MiddleCenter;
	protected override void onDrawGizmo(){
		GizmoTools.drawVectorText(text, transform, letterSize, textAnchor, textAlignment);
	}
}
