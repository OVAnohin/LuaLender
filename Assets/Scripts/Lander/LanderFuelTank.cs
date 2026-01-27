using System;
using UnityEngine;

public class LanderFuelTank : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] private float fuelAmount = 20f;
    [SerializeField] private float fuelAmountMax = 20f;

    private bool _hasFuel = true;
    public bool HasFuel => _hasFuel;

    public float AddFuel
    {
        set
        {
            if (value > 0)
                fuelAmount += value;

            if (fuelAmount > fuelAmountMax)
                fuelAmount = fuelAmountMax;

            if (fuelAmount > 0)
                _hasFuel = true;
        }
    }

    public float Fuel
    {
        get
        {
            return fuelAmount / fuelAmountMax;
        }
    }

    public event EventHandler FuelChanged;

    private void Start()
    {
        FuelChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Consume(int flowRateCoefficient, float fuelConsumptionAmount)
    {
        if (!_hasFuel)
            return;

        fuelAmount -= fuelConsumptionAmount * Time.fixedDeltaTime * flowRateCoefficient;

        if (fuelAmount <= 0f)
        {
            fuelAmount = 0f;
            _hasFuel = false;
        }

        FuelChanged?.Invoke(this, EventArgs.Empty);
    }
}
