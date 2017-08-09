using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Vive controllers' actions
/// </summary>
public class ControllerController : MonoBehaviour {

	public MenuState menuOpenState;
	public MenuState menuSettingState;
	public GameObject menu;
	public GameObject setting;
	public GameObject choises;
	public GameObject menuCursor;

	private SteamVR_TrackedObject trackedObj;
	public SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}


	void Start ()
	{
		menu = transform.GetChild(0).gameObject;
		setting = menu.transform.GetChild(0).gameObject;
		choises = menu.transform.GetChild(1).gameObject;
		menuCursor = menu.transform.GetChild(2).gameObject;

		menuOpenState = new MenuStateOpened(this);
		menuSettingState = new MenuStateMode(this);
	}

	void Update()
	{
		menuOpenState.Update();

		//For debuggin maybe
		if (Controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
		{
			menuCursor.SetActive(true);
		}
		if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 trackpad = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * 2.5f;
			menuCursor.transform.localPosition = new Vector3(trackpad.x, trackpad.y, -0.3f);
		}
		if (Controller.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			menuCursor.SetActive(false);
		}
	}
}