using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tool for selecting objects.
/// </summary>
public class SelectObjectTool : Tool
{
	const int WIREFRAME_LAYER = 8;

	/// <summary>
	/// Unselect selected object.
	/// </summary>
	/// <param name="cc"></param>
	public SelectObjectTool(ControllerController cc) : base(cc)
	{
		Unselect();
	}

	public override void Update() { }

	/// <summary>
	/// Select the collided object.
	/// Then open Object menu.
	/// </summary>
	/// <param name="collider"></param>
	public override void OnTriggerEnter(Collider collider)
	{
		SelectObject(collider.gameObject, cc);

		cc.menuOpenState = new MenuStateOpening(cc);
		cc.menuSettingState = new MenuStateObject(cc);
		cc.menuSettingState.Init();
	}

	/// <summary>
	/// First unselect selected object and move the given game object to wireframe layer.
	/// </summary>
	/// <param name="gameObject"></param>
	/// <param name="cc"></param>
	public static void SelectObject(GameObject gameObject, ControllerController cc)
	{
		Unselect();
		CommonInformationHolder.selectedObject = gameObject;
		gameObject.layer = WIREFRAME_LAYER;
		cc.wireframeRenderer.material = gameObject.GetComponent<MeshRenderer>().material;
	}

	/// <summary>
	/// Return selected object to default layer and change selected object to null.
	/// </summary>
	static void Unselect()
	{
		if (CommonInformationHolder.selectedObject)
		{
			CommonInformationHolder.selectedObject.layer = 0;
			CommonInformationHolder.selectedObject = null;
		}
	}
}
