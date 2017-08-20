using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Renders everything in red wireframe.
/// </summary>
public class WireframeRenderer : MonoBehaviour
{
	public Material material;
	Color originalColor;

	void OnPreRender()
	{
		if (material != null)
		{
			GL.wireframe = true;
			originalColor = material.color;
			material.color = new Color(1f, 0.2f, 0.2f);
		}
	}
	void OnPostRender()
	{
		if (material != null)
		{
			GL.wireframe = false;
			material.color = originalColor;
		}
	}
}
