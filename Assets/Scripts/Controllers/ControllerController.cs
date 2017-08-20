using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Vive controllers' actions
/// </summary>
public class ControllerController : MonoBehaviour
{
	public MenuOpenState menuOpenState;
	public MenuState menuSettingState;
	public Tool tool;
	public WireframeRenderer wireframeRenderer;

	[HideInInspector] public GameObject menu;
	[HideInInspector] public GameObject cursor;
	[HideInInspector] public GameObject setting;
	[HideInInspector] public GameObject choises1;
	[HideInInspector] public GameObject choises2;
	[HideInInspector] public GameObject choises3;
	[HideInInspector] public GameObject choises4;
	[HideInInspector] public GameObject back;

	public Sprite settingModeSprite;
	public Sprite settingObjectModeSprite;

	public Sprite choiseObjectSprite;
	public Sprite choiseEditSprite;
	public Sprite choiseSelectSprite;
	public Sprite choiseCreateNewSprite;

	public Sprite back2x1Sprite;

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
		cursor = menu.transform.GetChild(0).gameObject;
		setting = menu.transform.GetChild(1).gameObject;
		Transform choises = menu.transform.GetChild(2);
		choises1 = choises.GetChild(0).gameObject;
		choises2 = choises.GetChild(1).gameObject;
		choises3 = choises.GetChild(2).gameObject;
		choises4 = choises.GetChild(3).gameObject;
		back = menu.transform.GetChild(3).GetChild(0).gameObject;

		menuOpenState = new MenuStateOpened(this);
		menuSettingState = new MenuStateMode(this);
		menuSettingState.Init();
	}

	void Update()
	{
		menuOpenState.Update();

		//For debuggin maybe
		if (Controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
		{
			cursor.SetActive(true);
		}
		if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Vector2 trackpad = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * 2.5f;
			cursor.transform.localPosition = new Vector3(trackpad.x, trackpad.y, -0.3f);
		}
		if (Controller.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			cursor.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		menuOpenState.OnTriggerEnter(collider);
	}
}