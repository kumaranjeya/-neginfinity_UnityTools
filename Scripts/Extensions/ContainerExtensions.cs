using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContainerExtensions{
	public static class Extensions{
		public static Value getValueOrDefault<T, Key, Value>(this T arg, Key key, Value defaultValue = default(Value)) where T: IDictionary<Key, Value>{
			Value result;
			if (!arg.TryGetValue(key, out result)){
				return defaultValue;
			}
			return result;
			//if (arg.
		}
	}
}
