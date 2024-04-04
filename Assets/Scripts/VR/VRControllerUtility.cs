using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRControllerUtility
{
    public static IEnumerator VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRPlugin.SetControllerVibration((uint)controller, frequency, amplitude);
        // OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.All);
        yield return new WaitForSeconds(duration);
        // OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.All);
        OVRPlugin.SetControllerVibration((uint)controller, 0, 0);
    }
}