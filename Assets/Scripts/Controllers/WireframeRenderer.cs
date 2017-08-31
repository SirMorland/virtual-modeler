using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Renders everything in red wireframe.
/// </summary>
public class WireframeRenderer : MonoBehaviour
{
	public Material material;
	public Material gridMaterial;
	public Vector3 target;
	public bool showGuides = false;
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

		if (CommonInformationHolder.selectedObject != null)
		{
			Transform tra = CommonInformationHolder.selectedObject.transform;
			gridMaterial.SetPass(0);

			GL.PushMatrix();
			GL.Begin(GL.LINES);
			//DrawGrid(tra);
			DrawAxis(tra);
			if (showGuides)
			{
				DrawGuides(tra);
			}
			GL.End();
			GL.PopMatrix();
		}
	}


	/// <summary>
	/// Draw x, y and z axises on given transform's local orientation.
	/// </summary>
	/// <param name="tra">Transform</param>
	void DrawAxis(Transform tra)
	{
		Vector3 left = tra.TransformPoint(new Vector3(-5f, 0f, 0f));
		Vector3 right = tra.TransformPoint(new Vector3(5f, 0f, 0f));
		Vector3 up = tra.TransformPoint(new Vector3(0f, 5f, 0f));
		Vector3 down = tra.TransformPoint(new Vector3(0f, -5f, 0f));
		Vector3 forward = tra.TransformPoint(new Vector3(0f, 0f, 5f));
		Vector3 back = tra.TransformPoint(new Vector3(0f, 0f, -5f));

		GL.Color(new Color(1f, 0f, 0f));
		GL.Vertex3(left.x, left.y, left.z);
		GL.Vertex3(right.x, right.y, right.z);

		GL.Color(new Color(0f, 1f, 0f));
		GL.Vertex3(up.x, up.y, up.z);
		GL.Vertex3(down.x, down.y, down.z);

		GL.Color(new Color(0f, 0f, 1f));
		GL.Vertex3(forward.x, forward.y, forward.z);
		GL.Vertex3(back.x, back.y, back.z);
	}
	
	/// <summary>
	/// Draw guidelines to show transform's position relative to x, y and z -axises.
	/// </summary>
	/// <param name="tra">Transform</param>
	public void DrawGuides(Transform tra)
	{
		Vector3 x = tra.TransformPoint(new Vector3(target.x, 0f, 0f));
		Vector3 y = tra.TransformPoint(new Vector3(target.x, 0f, target.z));
		Vector3 z = tra.TransformPoint(new Vector3(0f, 0f, target.z));
		Vector3 o = tra.TransformPoint(target);

		GL.Color(new Color(0f, 0f, 0.2f));
		GL.Vertex3(x.x, x.y, x.z);
		GL.Vertex3(y.x, y.y, y.z);

		GL.Color(new Color(0.2f, 0f, 0f));
		GL.Vertex3(z.x, z.y, z.z);
		GL.Vertex3(y.x, y.y, y.z);

		GL.Color(new Color(0f, 0.2f, 0f));
		GL.Vertex3(y.x, y.y, y.z);
		GL.Vertex3(o.x, o.y, o.z);
	}
}
