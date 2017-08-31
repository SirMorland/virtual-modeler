using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Additional methods for classes.
/// </summary>
public static class ExtensionMethods
{
	/// <summary>
	/// Round x, y and z component of this vector to given accuracy.
	/// Examples:
	/// Accuracy | Value | Output
	/// ---------+-------+-------
	///	0.1      | 1.234 | 1.2
	///	0.1      | 1.334 | 1.3
	///	0.01     | 1.234 | 1.23
	///	0.01     | 1.334 | 1.33
	///	0.2      | 1.234 | 1.2
	///	0.2      | 1.334 | 1.4
	/// </summary>
	/// <param name="v">This vector</param>
	/// <param name="accuracy">Accuracy</param>
	/// <returns></returns>
	public static Vector3 Round(this Vector3 v, float accuracy)
	{
		v.Set(Mathf.Round(v.x / accuracy) * accuracy, Mathf.Round(v.y / accuracy) * accuracy, Mathf.Round(v.z / accuracy) * accuracy);
		return v;
	}
}
