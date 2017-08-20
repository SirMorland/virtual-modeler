using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for all tools.
/// </summary>
public abstract class Tool
{
	protected ControllerController cc;

	/// <summary>
	/// Create new tool.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public Tool(ControllerController cc)
	{
		this.cc = cc;
	}

	/// <summary>
	/// Update tool.
	/// Should be called once every frame.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Assume this method is called when controller hits an object.
	/// </summary>
	/// <param name="collider">Collider</param>
	public abstract void OnTriggerEnter(Collider collider);
}
