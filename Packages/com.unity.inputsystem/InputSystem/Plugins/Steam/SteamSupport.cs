#if (UNITY_STANDALONE || UNITY_EDITOR) && UNITY_ENABLE_STEAM_CONTROLLER_SUPPORT
using UnityEngine.Experimental.Input.Utilities;

////TODO: support polling Steam controllers on an async polling thread adhering to InputSystem.pollingFrequency

namespace UnityEngine.Experimental.Input.Plugins.Steam
{
    /// <summary>
    /// Adds support for Steam controllers.
    /// </summary>
    public static class SteamSupport
    {
        /// <summary>
        /// Wrapper around the Steam controller API.
        /// </summary>
        /// <remarks>
        /// This must be set by user code for Steam controller support to become functional.
        /// </remarks>
        public static ISteamControllerAPI api
        {
            get { return s_API; }
            set
            {
                s_API = value;
                if (value != null && !s_UpdateHookedIn)
                {
                    InputSystem.onUpdate += type => UpdateControllers();
                    s_UpdateHookedIn = true;
                }
            }
        }

        internal static ulong[] s_ConnectedControllers;
        internal static SteamController[] s_InputDevices;
        internal static int s_InputDeviceCount;
        internal static bool s_UpdateHookedIn;
        internal static ISteamControllerAPI s_API;

        private const int STEAM_CONTROLLER_MAX_COUNT = 16;

        /// <summary>
        /// Enable support for the Steam controller API.
        /// </summary>
        public static void Initialize()
        {
            // We use this as a base layout.
            InputSystem.RegisterLayout<SteamController>();

            if (api != null && !s_UpdateHookedIn)
            {
                InputSystem.onUpdate +=
                    type => UpdateControllers();
                s_UpdateHookedIn = true;
            }
        }

        private static void UpdateControllers()
        {
            if (api == null)
                return;

            // Check if we have any controllers have appeared or disappeared.
            if (s_ConnectedControllers == null)
                s_ConnectedControllers = new ulong[STEAM_CONTROLLER_MAX_COUNT];
            var numConnectedControllers = api.GetConnectedControllers(s_ConnectedControllers);
            for (var i = 0; i < numConnectedControllers; ++i)
            {
                var handle = s_ConnectedControllers[i];

                // See if we already have a device for this one.
                if (s_InputDevices != null)
                {
                    SteamController existingDevice = null;
                    for (var n = 0; n < s_InputDeviceCount; ++n)
                    {
                        if (s_InputDevices[n].steamControllerHandle == handle)
                        {
                            existingDevice = s_InputDevices[n];
                            break;
                        }
                    }

                    // Yes, we do.
                    if (existingDevice != null)
                        continue;
                }

                // No, so create a new device.
                var controllerLayouts = InputSystem.ListLayoutsBasedOn("SteamController");
                foreach (var layout in controllerLayouts)
                {
                    // Rather than directly creating a device with the layout, let it go through
                    // the usual matching process.
                    var device = InputSystem.AddDevice(new InputDeviceDescription
                    {
                        interfaceName = SteamController.kSteamInterface,
                        product = layout
                    });

                    // Make sure it's a SteamController we got.
                    var steamDevice = device as SteamController;
                    if (steamDevice == null)
                    {
                        Debug.LogError(string.Format(
                            "InputDevice created from layout '{0}' based on the 'SteamController' layout is not a SteamController",
                            device.layout));
                        continue;
                    }

                    // Assign it the Steam controller handle.
                    steamDevice.steamControllerHandle = handle;

                    ArrayHelpers.AppendWithCapacity(ref s_InputDevices, ref s_InputDeviceCount, steamDevice);
                }
            }
            if (s_InputDevices != null)
            {
                // Remove anything no longer there.
                for (var i = 0; i < s_InputDeviceCount; ++i)
                {
                    var device = s_InputDevices[i];
                    var handle = device.steamControllerHandle;
                    var stillExists = false;
                    for (var n = 0; n < numConnectedControllers; ++n)
                        if (s_ConnectedControllers[n] == handle)
                        {
                            stillExists = true;
                            break;
                        }

                    if (!stillExists)
                    {
                        ArrayHelpers.EraseAtByMovingTail(s_InputDevices, ref s_InputDeviceCount, i);
                        InputSystem.RemoveDevice(device);
                    }
                }
            }
        }
    }
}

#endif // (UNITY_STANDALONE || UNITY_EDITOR) && UNITY_ENABLE_STEAM_CONTROLLER_SUPPORT
