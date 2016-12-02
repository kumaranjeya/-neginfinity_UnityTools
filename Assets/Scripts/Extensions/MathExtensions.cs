﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathExtensions{
	public static class Extensions{
		public static float dot(this Quaternion lhs, Quaternion rhs){
			return Quaternion.Dot(lhs, rhs);
		}

		public static Quaternion normalized(this Quaternion arg){
			float l = arg.dot(arg);
			return arg.scale(1.0f/l);
		}

		public static Quaternion conjugate(this Quaternion arg){
			return new Quaternion(-arg.x, -arg.y, -arg.z, arg.w);
		}

		public static Quaternion scale(this Quaternion arg, float s){
			return new Quaternion(arg.x * s, arg.y * s, arg.z * s, arg.w * s);
		}

		public static Quaternion add(this Quaternion lhs, Quaternion rhs){
			return new Quaternion(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
		}

		public static Quaternion negative(this Quaternion val){
			return new Quaternion(-val.x, -val.y, -val.z, -val.w);
		}

		public static Matrix4x4 toMatrix(this Quaternion v){
			Matrix4x4 result = Matrix4x4.identity;
			result.SetTRS(Vector3.zero, v, Vector3.one);
			return result;
		}

		public static Vector3 getVectorPart(this Quaternion v){
			return new Vector3(v.x, v.y, v.z);
		}

		public static Vector3 transformVector(this Quaternion r, Vector3 v){
			var result = r * new Quaternion(v.x, v.y, v.z, 0.0f) * r.conjugate();
			return result.getVectorPart();
		}
	}
}
