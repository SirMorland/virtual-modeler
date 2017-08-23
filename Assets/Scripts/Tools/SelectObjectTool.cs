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

	public SelectObjectTool(ControllerController cc) : base(cc) { }

	public override void Update() { }

	/// <summary>
	/// Select the collided object.
	/// First return the previously selected object to default layer and move the collided object to wireframe layer.
	/// Then open Object menu.
	/// </summary>
	/// <param name="collider"></param>
	public override void OnTriggerEnter(Collider collider)
	{
		if (CommonInformationHolder.selectedObject)
		{
			CommonInformationHolder.selectedObject.layer = 0;
		}
		CommonInformationHolder.selectedObject = collider.gameObject;
		collider.gameObject.layer = WIREFRAME_LAYER;
		cc.wireframeRenderer.material = collider.gameObject.GetComponent<MeshRenderer>().material;

		cc.menuOpenState = new MenuStateOpening(cc);
		cc.menuSettingState = new MenuStateObject(cc);
		cc.menuSettingState.Init();
	}
}
