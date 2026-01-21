using System;
using UnityEngine;

public class LanderFuelTank : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] private float FuelAmount = 20f;
    [SerializeField] private float FuelAmountMax = 20f;

    private bool _hasFuel = true;
    public bool HasFuel => _hasFuel;

    public float AddFuel
    {
        set
        {
            if (value > 0)
                FuelAmount += value;

            if (FuelAmount > FuelAmountMax)
                FuelAmount = FuelAmountMax;

            if (FuelAmount > 0)
                _hasFuel = true;
        }
    }

    public float Fuel
    {
        get
        {
            return FuelAmount / FuelAmountMax;
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

        FuelAmount -= fuelConsumptionAmount * Time.fixedDeltaTime * flowRateCoefficient;

        if (FuelAmount <= 0f)
        {
            FuelAmount = 0f;
            _hasFuel = false;
        }

        FuelChanged?.Invoke(this, EventArgs.Empty);
    }
}
