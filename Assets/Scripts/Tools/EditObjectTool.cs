using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tool for moving vertices.
/// </summary>
public class EditObjectTool : Tool
{
	Mesh mesh;
	MeshCollider meshCollider;
	Material gridMaterial;

	List<int> selectedVertices = new List<int>();

	/// <summary>
	/// Select first object if no object is selected.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public EditObjectTool(ControllerController cc) : base(cc)
	{
		if(CommonInformationHolder.selectedObject == null)
		{
			SelectObjectTool.SelectObject(GameObject.FindGameObjectWithTag("Object"), cc);
		}
		mesh = CommonInformationHolder.selectedObject.GetComponent<MeshFilter>().mesh;
		meshCollider = CommonInformationHolder.selectedObject.GetComponent<MeshCollider>();

		gridMaterial = cc.wireframeRenderer.gridMaterial;
	}

	/// <summary>
	/// Grab vertex if trigger button is pressed.
	/// Move vertex if holding trigger button down.
	/// Release vertex if trigger button is released.
	/// </summary>
	public override void Update()
	{

		if (cc.Controller.GetHairTriggerDown())
		{
			GrabVertex();
		}
		if (cc.Controller.GetHairTrigger())
		{
			MoveVertex();
		}
		if (cc.Controller.GetHairTriggerUp())
		{
			selectedVertices.Clear();
			HideDisplay();
			cc.wireframeRenderer.showGuides = false;
		}
	}

	public override void OnTriggerEnter(Collider collider) { }

	/// <summary>
	/// Select all vertices close enough controller.
	/// </summary>
	void GrabVertex()
	{
		Vector3[] verts = mesh.vertices;
		GameObject selectedObject = CommonInformationHolder.selectedObject;

		for (int i = 0; i < verts.Length; i++)
		{
			if (Vector3.Distance(selectedObject.transform.TransformPoint(verts[i]), cc.transform.position) < 0.05f)
			{
				selectedVertices.Add(i);
				ShowDisplay();
				cc.wireframeRenderer.showGuides = true;
			}
		}
	}

	/// <summary>
	/// Change all selected vertices position to controller's position.
	/// If grip button is pressed snap position to 0.1f.
	/// </summary>
	void MoveVertex()
	{
		Vector3[] verts = mesh.vertices;
		GameObject selectedObject = CommonInformationHolder.selectedObject;
		Vector3 newPos = selectedObject.transform.InverseTransformPoint(cc.transform.position);
		if (cc.Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
		{
			newPos = newPos.Round(0.1f);
		}
		else
		{
			newPos = newPos.Round(0.001f);
		}

		foreach (int selectedVertex in selectedVertices)
		{
			if (selectedVertex < verts.Length && selectedVertex >= 0)
			{
				verts[selectedVertex] = newPos;
				UpdateDisplay(newPos);
				cc.wireframeRenderer.target = newPos;
			}
		}

		mesh.vertices = verts;
		meshCollider.sharedMesh = mesh;
	}

	/// <summary>
	/// Show info display.
	/// </summary>
	void ShowDisplay()
	{
		cc.display.SetActive(true);
	}

	/// <summary>
	/// Write position to info display.
	/// </summary>
	/// <param name="position">Position</param>
	void UpdateDisplay(Vector3 position)
	{
		String text = "<size=50><b>VERTEX POSITION</b></size>\n( <color=#f00>" +
						position.x.ToString("F3") + "</color> , <color=#0f0>" +
						position.y.ToString("F3") + "</color> , <color=#00f>" +
						position.z.ToString("F3") + "</color> )";
		cc.displayText.text = text;
	}

	/// <summary>
	/// Hide info display.
	/// </summary>
	void HideDisplay()
	{
		cc.display.SetActive(false);
	}
}
