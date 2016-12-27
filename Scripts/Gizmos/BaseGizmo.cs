using UnityEngine;
using System.Collections;

public abstract class BaseGizmo: MonoBehaviour{
	public Color activeColor = Color.white;
	public Color inactiveColor = Color.cyan;
	public bool drawInactive = true;
	protected abstract void onDrawGizmo();

	void drawGizmos(Color color){
		var oldColor = Gizmos.color;
		Gizmos.color = oldColor;
		Gizmos.color = color;
		onDrawGizmo();
		Gizmos.color = oldColor;
	}

	void OnDrawGizmos(){
		if (!drawInactive)
			return;
		drawGizmos(inactiveColor);
	}

	void OnDrawGizmosSelected(){
		drawGizmos(activeColor);
	}
}
