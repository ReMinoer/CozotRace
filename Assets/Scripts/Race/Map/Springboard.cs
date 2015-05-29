using UnityEngine;

public class Springboard : MonoBehaviour
{
    private void OnTriggerEnter(Collider player)
    {
        var vehiclemotor = player.GetComponent<VehicleMotor>();
        if (vehiclemotor == null)
            return;
        vehiclemotor.IsBoosted = true;
        vehiclemotor.ForwardSpeedMaxActual = 50f;
    }

    private void OnTriggerQuit(Collider player)
    {
        var vehiclemotor = player.GetComponent<VehicleMotor>();
        if (vehiclemotor == null)
            return;
        vehiclemotor.IsBoosted = false;
        vehiclemotor.ForwardSpeedMaxActual = 25f;
    }
}