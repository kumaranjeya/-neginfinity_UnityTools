using UnityEngine;
using System.Collections;

namespace ObjectExtensions{
	public static class Extensions{
		static void nullCheck(GameObject gameObject){
			if (!gameObject)
				throw new System.ArgumentNullException();
		}

		public static Comp getOrCreateComponent<Comp>(this GameObject gameObject, System.Action<Comp> initializer = null) where Comp: Component{
			nullCheck(gameObject);
			var result = gameObject.GetComponent<Comp>();
			if (!result){
				result = gameObject.AddComponent<Comp>();
				if (initializer != null)
					initializer(result);
			}
			return result;
		}

		public static bool processComponent<Comp>(this GameObject gameObject, System.Action<Comp> action) where Comp: Component{
			nullCheck(gameObject);
			var comp = gameObject.GetComponent<Comp>();
			if (!comp)
				return false;
			if (action != null){
				action(comp);
			}
			return true;
		}

		public static bool hasComponent<Comp>(this GameObject gameObject) where Comp: Component{
			nullCheck(gameObject);
			return gameObject.GetComponent<Comp>();
		}

		public static void setLocalTransform(this GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale){
			nullCheck(gameObject);

			gameObject.transform.localPosition = position;
			gameObject.transform.localRotation = rotation;
			gameObject.transform.localScale = scale;
		}

		public static void setLocalPosition(this GameObject gameObject, Vector3 position){
			nullCheck(gameObject);

			gameObject.transform.localPosition = position;
		}

		public static Vector3 getLocalPosition(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localPosition;
		}

		public static Quaternion getLocalRotation(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localRotation;
		}

		public static Vector3 getLocalScale(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localScale;
		}

		public static void setLocalRotation(this GameObject gameObject, Quaternion rotation){
			nullCheck(gameObject);
			gameObject.transform.localRotation = rotation;
		}

		public static void setLocalScale(this GameObject gameObject, Vector3 scale){
			nullCheck(gameObject);
			gameObject.transform.localScale = scale;
		}

		public static void resetLocalTransform(this GameObject gameObject){
			nullCheck(gameObject);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

		public static void setParent(this GameObject gameObject, GameObject newParent, bool worldPositionStays = true){
			nullCheck(gameObject);
			Transform newParentTransform = null;
			if (newParent)
				newParentTransform = newParent.transform;
			gameObject.transform.SetParent(newParentTransform, worldPositionStays);
		}

		public static void setParent(this GameObject gameObject, Transform newParent, bool worldPositionStays = true){
			nullCheck(gameObject);
			gameObject.transform.SetParent(newParent, worldPositionStays);
		}
	}		
}