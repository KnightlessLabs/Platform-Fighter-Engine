using System;
using System.Collections;
using UnityEngine;
using InControl;

// This custom profile is enabled by adding it to the Custom Profiles list
// on the InControlManager component, or you can attach it yourself like so:
// InputManager.AttachDevice( new UnityInputDevice( "KeyboardAndMouseProfile" ) );
// 
public class KAMProfile : UnityInputDeviceProfile{
	public KAMProfile(){
		Name = "Keyboard/Mouse";
		Meta = "A keyboard and mouse combination profile.";

		// This profile only works on desktops.
		SupportedPlatforms = new[]{
			"Windows",
			"Mac",
			"Linux"
		};

		Sensitivity = 1.0f;
		LowerDeadZone = 0.0f;
		UpperDeadZone = 1.0f;

		ButtonMappings = new[]{
			new InputControlMapping
			{
				Handle = "Attack",
				Target = InputControlType.Action1,
				Source = KeyCodeButton( KeyCode.P )
			},
			new InputControlMapping
			{
				Handle = "Special",
				Target = InputControlType.Action2,
                Source = KeyCodeButton( KeyCode.O )
            },
            new InputControlMapping
            {
                Handle = "Grab",
                Target = InputControlType.RightBumper,
                Source = KeyCodeButton( KeyCode.U )
            },
            new InputControlMapping
            {
                Handle = "Shield",
                Target = InputControlType.RightTrigger,
                Source = KeyCodeButton( KeyCode.I )
            },
            new InputControlMapping
			{
				Handle = "Jump",
				Target = InputControlType.Action4,
				Source = KeyCodeButton( KeyCode.Space )
			},
            new InputControlMapping
            {
                Handle = "Jump",
                Target = InputControlType.Action3,
                Source = KeyCodeButton( KeyCode.Space )
            }
        };

		AnalogMappings = new[]
		{
			new InputControlMapping
			{
				Handle = "Left Stick X",
				Target = InputControlType.LeftStickX,
				Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
			},
			new InputControlMapping
			{
				Handle = "Left Stick Y",
				Target = InputControlType.LeftStickY,
				Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
			},
			new InputControlMapping {
				Handle = "Left Stick X Alt",
				Target = InputControlType.LeftStickX,
				Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
			},
			new InputControlMapping {
				Handle = "Left Stick Y Alt",
				Target = InputControlType.LeftStickY,
				Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
			}
		};
	}
}
