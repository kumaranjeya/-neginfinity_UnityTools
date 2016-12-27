using UnityEngine;
using System.Collections;

namespace Demos{
	namespace Demo1{
		public class DemoGizmo: MonoBehaviour{
			[System.Serializable]
			public abstract class BaseShape{
				public Color color = ColorNames.Azure.toColor();
				public bool enabled = true;

				protected abstract void onDraw(Transform transform);
				public void draw(Transform transform){
					if (!enabled)
						return;
					var oldColor = Gizmos.color;
					Gizmos.color = oldColor;
					onDraw(transform);
					Gizmos.color = oldColor;
				}
			}
		}
	}
}