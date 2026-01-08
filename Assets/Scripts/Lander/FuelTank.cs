using System;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] private float FuelAmount = 100f;

    private bool _hasFuel = true;
    public bool HasFuel => _hasFuel;

    public float AddFuel
    {
        set
        {
            if (value >= 0)
                FuelAmount += value;
        }
    }

    public event EventHandler FuelTankEmpty;

    public void Consume(int flowRateCoefficient, float fuelConsumptionAmount)
    {
        if (!_hasFuel)
            return;

        FuelAmount -= fuelConsumptionAmount * Time.fixedDeltaTime * flowRateCoefficient;

        if (FuelAmount <= 0f)
        {
            FuelAmount = 0f;
            _hasFuel = false;
            FuelTankEmpty?.Invoke(this, EventArgs.Empty);
        }

        Debug.Log(FuelAmount);
    }
}
