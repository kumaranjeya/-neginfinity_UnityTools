using UnityEngine;
using UnityEditor;
using System.Collections;

public static class TransformTools{
	static void resetTransforms(bool position, bool rotation, bool scale){
		GameObject obj = Selection.activeGameObject;

		if (!obj){
			Debug.LogWarningFormat("No object to process");
			return;
		}

		Undo.RecordObject(obj.transform, "Transfofrm reset");
		if (position)
			obj.transform.localPosition = Vector3.zero;
		if (rotation)
			obj.transform.localRotation = Quaternion.identity;
		if (scale)
			obj.transform.localScale = Vector3.one;
	}

	[MenuItem("GameObject/Transform Tools/Reset local transform", false, 0)]
	public static void resetLocalTransform(MenuCommand menuCommand){
		resetTransforms(true, true, true);
	}	
	[MenuItem("GameObject/Transform Tools/Reset local position", false, 0)]
	public static void resetLocalPos(MenuCommand menuCommand){
		resetTransforms(true, false, false);
	}	
	[MenuItem("GameObject/Transform Tools/Reset local rotation", false, 0)]
	public static void resetLocalQuat(MenuCommand menuCommand){
		resetTransforms(false, true, false);
	}	
	[MenuItem("GameObject/Transform Tools/Reset local scale", false, 0)]
	public static void resetLocalScale(MenuCommand menuCommand){
		resetTransforms(false, false, true);
	}
}
