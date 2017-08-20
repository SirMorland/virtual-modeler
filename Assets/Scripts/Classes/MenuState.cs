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
	/// Initialize the state.
	/// </summary>
	public abstract void Init();

	/// <summary>
	/// Update menu.
	/// Should be called once every frame.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Move object up and color it red.
	/// </summary>
	/// <param name="transform">Transform</param>
	/// <param name="renderer">Sprite renderer</param>
	protected void FloatUp(Transform transform, SpriteRenderer renderer)
	{
		float speed = Time.deltaTime * 10f;
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, -0.2f), speed);
		renderer.color = Color.Lerp(renderer.color, new Color(1f, 0.2f, 0.2f, 1f), speed);
	}

	/// <summary>
	/// Move object down and color it white.
	/// </summary>
	/// <param name="transform">Transform</param>
	/// <param name="renderer">Sprite renderer</param>
	protected void FloatDown(Transform transform, SpriteRenderer renderer)
	{
		float speed = Time.deltaTime * 10f;
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, 0f), speed);
		renderer.color = Color.Lerp(renderer.color, new Color(1f, 1f, 1f, 1f), speed);
	}
}

#region Open/closed states

/// <summary>
/// Represents whether menu is open or closed.
/// </summary>
public abstract class MenuOpenState : MenuState
{
	public MenuOpenState(ControllerController cc) : base(cc) {}

	/// <summary>
	/// Assume this method is called when controller hits an object.
	/// </summary>
	/// <param name="collider">Collider</param>
	public abstract void OnTriggerEnter(Collider collider);
}

/// <summary>
/// Menu is closed.
/// </summary>
public class MenuStateClosed : MenuOpenState
{
	public MenuStateClosed(ControllerController cc) : base(cc) { }

	public override void Init() { }

	/// <summary>
	/// Open the menu if menu button is pressed.
	/// </summary>
	public override void Update()
	{
		if (cc.tool != null)
		{
			cc.tool.Update();
		}

		if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			cc.menuOpenState = new MenuStateOpening(cc);
		}
	}

	/// <summary>
	/// Relay OnTriggerEnter messages to currently used tool.
	/// </summary>
	/// <param name="collider">Collider</param>
	public override void OnTriggerEnter(Collider collider)
	{
		if (cc.tool != null)
		{
			cc.tool.OnTriggerEnter(collider);
		}
	}
}

/// <summary>
/// Menu is opening.
/// </summary>
public class MenuStateOpening : MenuOpenState
{
	public MenuStateOpening(ControllerController cc) : base(cc) { }

	public override void Init() { }

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

	public override void OnTriggerEnter(Collider collider) { }
}

/// <summary>
/// Menu is open.
/// </summary>
public class MenuStateOpened : MenuOpenState
{
	public MenuStateOpened(ControllerController cc) : base(cc) { }

	public override void Init() { }

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

	public override void OnTriggerEnter(Collider collider) { }
}

/// <summary>
/// Menu is closing.
/// </summary>
public class MenuStateClosing : MenuOpenState
{
	public MenuStateClosing(ControllerController cc) : base(cc) { }

	public override void Init() { }

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

	public override void OnTriggerEnter(Collider collider) { }
}

#endregion

#region Setting page states

/// <summary>
/// Select mode (object/edit).
/// </summary>
public class MenuStateMode : MenuState
{
	Transform choise1Transform;
	Transform choise2Transform;

	SpriteRenderer choise1Renderer;
	SpriteRenderer choise2Renderer;

	/// <summary>
	/// Create new menu state for mode selection.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public MenuStateMode(ControllerController cc) : base(cc)
	{
		choise1Transform = cc.choises1.transform;
		choise2Transform = cc.choises2.transform;

		choise1Renderer = choise1Transform.GetComponent<SpriteRenderer>();
		choise2Renderer = choise2Transform.GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Change all the sprites.
	/// </summary>
	public override void Init()
	{
		cc.setting.GetComponent<SpriteRenderer>().sprite = cc.settingModeSprite;
		choise1Renderer.sprite = cc.choiseObjectSprite;
		choise2Renderer.sprite = cc.choiseEditSprite;
		cc.choises3.GetComponent<SpriteRenderer>().sprite = null;
		cc.choises4.GetComponent<SpriteRenderer>().sprite = null;
		cc.back.GetComponent<SpriteRenderer>().sprite = null;
	}

	/// <summary>
	/// Play float animations on hover over butttons.
	/// Go to object mode if object-button is pressed.
	/// </summary>
	public override void Update()
	{
		if (cc.Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 trackpad = cc.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

			if (trackpad.x < -0.25f && trackpad.x > -0.75f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatUp(choise1Transform, choise1Renderer);
				FloatDown(choise2Transform, choise2Renderer);

				if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				{
					cc.menuSettingState = new MenuStateFadeOut(cc, new MenuStateObjectMode(cc));
					cc.menuSettingState.Init();
				}
			}
			else if(trackpad.x < 0.75f && trackpad.x > 0.25f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatDown(choise1Transform, choise1Renderer);
				FloatUp(choise2Transform, choise2Renderer);
			}
			else
			{
				FloatDown(choise1Transform, choise1Renderer);
				FloatDown(choise2Transform, choise2Renderer);
			}
		}
		else
		{
			FloatDown(choise1Transform, choise1Renderer);
			FloatDown(choise2Transform, choise2Renderer);
		}
	}
}

/// <summary>
/// Select object mode tool.
/// </summary>
public class MenuStateObjectMode : MenuState
{
	Transform choise1Transform;
	Transform choise2Transform;
	Transform backTransform;

	SpriteRenderer choise1Renderer;
	SpriteRenderer choise2Renderer;
	SpriteRenderer backRenderer;

	/// <summary>
	/// Create new menu state for object mode tool selection.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	public MenuStateObjectMode(ControllerController cc) : base(cc)
	{
		choise1Transform = cc.choises1.transform;
		choise2Transform = cc.choises2.transform;
		backTransform = cc.back.transform;

		choise1Renderer = choise1Transform.GetComponent<SpriteRenderer>();
		choise2Renderer = choise2Transform.GetComponent<SpriteRenderer>();
		backRenderer = backTransform.GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Change all the sprites
	/// </summary>
	public override void Init()
	{
		cc.setting.GetComponent<SpriteRenderer>().sprite = cc.settingObjectModeSprite;
		choise1Renderer.sprite = cc.choiseSelectSprite;
		choise2Renderer.sprite = cc.choiseCreateNewSprite;
		cc.choises3.GetComponent<SpriteRenderer>().sprite = null;
		cc.choises4.GetComponent<SpriteRenderer>().sprite = null;
		backRenderer.sprite = cc.back2x1Sprite;
	}

	/// <summary>
	/// Play float animations on hover over butttons.
	/// Close the menu if select-button is pressed.
	/// Go back to mode selection if back-button is pressed.
	/// </summary>
	public override void Update()
	{
		if (cc.Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 trackpad = cc.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

			if (trackpad.x < -0.25f && trackpad.x > -0.75f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatUp(choise1Transform, choise1Renderer);
				FloatDown(choise2Transform, choise2Renderer);
				FloatDown(backTransform, backRenderer);

				if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				{
					cc.menuOpenState = new MenuStateClosing(cc);
					cc.tool = new SelectObjectTool(cc);
				}
			}
			else if (trackpad.x < 0.75f && trackpad.x > 0.25f && trackpad.y < 0.25f && trackpad.y > -0.25f)
			{
				FloatDown(choise1Transform, choise1Renderer);
				FloatUp(choise2Transform, choise2Renderer);
				FloatDown(backTransform, backRenderer);
			}
			else if (trackpad.x < 0.25f && trackpad.x > -0.25f && trackpad.y < -0.5f && trackpad.y > -1f)
			{
				FloatDown(choise1Transform, choise1Renderer);
				FloatDown(choise2Transform, choise2Renderer);
				FloatUp(backTransform, backRenderer);

				if (cc.Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
				{
					cc.menuSettingState = new MenuStateFadeOut(cc, new MenuStateMode(cc));
				}
			}
			else
			{
				FloatDown(choise1Transform, choise1Renderer);
				FloatDown(choise2Transform, choise2Renderer);
				FloatDown(backTransform, backRenderer);
			}
		}
		else
		{
			FloatDown(choise1Transform, choise1Renderer);
			FloatDown(choise2Transform, choise2Renderer);
			FloatDown(backTransform, backRenderer);
		}
	}
}

#endregion

#region Effect states

/// <summary>
/// Fade menu elements out.
/// </summary>
public class MenuStateFadeOut : MenuState
{
	MenuState state;

	Transform[] transforms = new Transform[5];
	SpriteRenderer[] renderers = new SpriteRenderer[6];

	/// <summary>
	/// Create new menu state for fading out menu elements.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	/// <param name="state">State to fade to</param>
	public MenuStateFadeOut(ControllerController cc, MenuState state) : base(cc)
	{
		this.state = state;
		
		transforms[0] = cc.choises1.transform;
		transforms[1] = cc.choises2.transform;
		transforms[2] = cc.choises3.transform;
		transforms[3] = cc.choises4.transform;
		transforms[4] = cc.back.transform;

		renderers[0] = cc.setting.GetComponent<SpriteRenderer>();
		renderers[1] = cc.choises1.GetComponent<SpriteRenderer>();
		renderers[2] = cc.choises2.GetComponent<SpriteRenderer>();
		renderers[3] = cc.choises3.GetComponent<SpriteRenderer>();
		renderers[4] = cc.choises4.GetComponent<SpriteRenderer>();
		renderers[5] = cc.back.GetComponent<SpriteRenderer>();
	}

	public override void Init() { }

	/// <summary>
	/// Fade menu elements out and change to fade in.
	/// </summary>
	public override void Update()
	{
		float speed = Time.deltaTime * 10f;
		for (int i = 0; i < transforms.Length; i++)
		{
			transforms[i].localPosition = Vector3.Lerp(transforms[i].localPosition, new Vector3(0f, 0f, 0f), speed);
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].color = Color.Lerp(renderers[i].color, new Color(1, 1, 1, 0), speed);
		}

		if(renderers[0].color.a < 0.1)
		{
			state.Init();
			cc.menuSettingState = new MenuStateFadeIn(cc, state);
		}
	}
}

/// <summary>
/// Fade menu elements in.
/// </summary>
public class MenuStateFadeIn : MenuState
{
	MenuState state;

	SpriteRenderer[] renderers = new SpriteRenderer[6];

	/// <summary>
	/// Create new menu state for fading in menu elements.
	/// </summary>
	/// <param name="cc">Vive controller controller</param>
	/// <param name="state">State to fade to</param>
	public MenuStateFadeIn(ControllerController cc, MenuState state) : base(cc)
	{
		this.state = state;

		renderers[0] = cc.setting.GetComponent<SpriteRenderer>();
		renderers[1] = cc.choises1.GetComponent<SpriteRenderer>();
		renderers[2] = cc.choises2.GetComponent<SpriteRenderer>();
		renderers[3] = cc.choises3.GetComponent<SpriteRenderer>();
		renderers[4] = cc.choises4.GetComponent<SpriteRenderer>();
		renderers[5] = cc.back.GetComponent<SpriteRenderer>();
	}

	public override void Init() { }

	/// <summary>
	/// Fade menu elements in and change to given state.
	/// </summary>
	public override void Update()
	{
		state.Update();

		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].color = Color.Lerp(renderers[i].color, new Color(1, 1, 1, 1), Time.deltaTime * 10f);
		}

		if (renderers[0].color.a > 0.9)
		{
			cc.menuSettingState = state;
		}
	}
}

#endregion