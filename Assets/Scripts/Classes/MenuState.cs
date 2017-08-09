using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for menu states.
/// Menu states can represent open/closed states and menu setting page states.
/// </summary>
public abstract class MenuState
{
	protected ControllerController cc;

	/// <summary>
	/// Create new menu state.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public MenuState(ControllerController cc) {
		this.cc = cc;
	}

	/// <summary>
	/// Update menu.
	/// Should be called once every frame.
	/// </summary>
	public abstract void Update();
}

#region Open/closed states

/// <summary>
/// Menu is closed.
/// </summary>
public class MenuStateClosed : MenuState
{
	public MenuStateClosed(ControllerController cc) : base(cc) {}

	/// <summary>
	/// Open the menu if menu button is pressed.
	/// </summary>
	public override void Update()
	{
		if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			cc.menuOpenState = new MenuStateOpening(cc);
		}
	}
}

/// <summary>
/// Menu is opening.
/// </summary>
public class MenuStateOpening : MenuState
{
	public MenuStateOpening(ControllerController cc) : base(cc) {}

	/// <summary>
	/// Play menu opening animation.
	/// </summary>
	public override void Update()
	{
		if (cc.menu.transform.localScale.y < 0.05f)
		{
			cc.menu.transform.localScale += new Vector3(0f, Time.deltaTime * 0.5f, Time.deltaTime * 0.5f);
		}
		else
		{
			cc.menu.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			cc.menuOpenState = new MenuStateOpened(cc);
		}
	}
}

/// <summary>
/// Menu is open.
/// </summary>
public class MenuStateOpened : MenuState
{
	public MenuStateOpened(ControllerController cc) : base(cc) { }

	/// <summary>
	/// Update menu setting page state.
	/// Close the menu if menu button is pressed.
	/// </summary>
	public override void Update()
	{
		cc.menuSettingState.Update();

		if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			cc.menuOpenState = new MenuStateClosing(cc);
		}
	}
}

/// <summary>
/// Menu is closing.
/// </summary>
public class MenuStateClosing : MenuState
{
	public MenuStateClosing(ControllerController cc) : base(cc) { }

	/// <summary>
	/// Play menu closing animation.
	/// </summary>
	public override void Update()
	{
		if (cc.menu.transform.localScale.y > 0f)
		{
			cc.menu.transform.localScale -= new Vector3(0f, Time.deltaTime * 0.5f, Time.deltaTime * 0.5f);
		}
		else
		{
			cc.menu.transform.localScale = new Vector3(0.05f, 0f, 0f);
			cc.menuOpenState = new MenuStateClosed(cc);
		}
	}
}

#endregion

#region Setting page states

/// <summary>
/// Select mode (object/edit).
/// </summary>
public class MenuStateMode : MenuState
{
	Transform objectMode;
	Transform editMode;

	SpriteRenderer objectRenderer;
	SpriteRenderer editRenderer;

	/// <summary>
	/// Create new menu state for mode selection.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public MenuStateMode(ControllerController cc) : base(cc)
	{
		objectMode = cc.choises.transform.GetChild(0);
		editMode = cc.choises.transform.GetChild(1);

		objectRenderer = objectMode.GetComponent<SpriteRenderer>();
		editRenderer = editMode.GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Play float animations on hover over butttons.
	/// </summary>
	public override void Update()
	{
		if (cc.Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 trackpad = cc.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

			if (trackpad.x < -0.25f && trackpad.x > -0.75f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatUp(objectMode, objectRenderer);
				FloatDown(editMode, editRenderer);
			}
			else if(trackpad.x < 0.75f && trackpad.x > 0.25f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatDown(objectMode, objectRenderer);
				FloatUp(editMode, editRenderer);
			}
			else
			{
				FloatDown(objectMode, objectRenderer);
				FloatDown(editMode, editRenderer);
			}
		}
		else
		{
			FloatDown(objectMode, objectRenderer);
			FloatDown(editMode, editRenderer);
		}
	}

	/// <summary>
	/// Move transform up and color it red.
	/// </summary>
	/// <param name="transform">Transform</param>
	/// <param name="renderer">Sprite renderer</param>
	void FloatUp(Transform transform, SpriteRenderer renderer)
	{
		float speed = Time.deltaTime * 10f;
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, -0.2f), speed);
		renderer.color = Color.Lerp(renderer.color, new Color(1f, 0.2f, 0.2f), speed);
	}

	/// <summary>
	/// Move transform down and color it white.
	/// </summary>
	/// <param name="transform">Transform</param>
	/// <param name="renderer">Sprite renderer</param>
	void FloatDown(Transform transform, SpriteRenderer renderer)
	{
		float speed = Time.deltaTime * 10f;
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, 0f), speed);
		renderer.color = Color.Lerp(renderer.color, new Color(1f, 1f, 1f), speed);
	}
}

#endregion