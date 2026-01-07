using System;
using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem LeftThrusterParticleSystem;
    [SerializeField] private ParticleSystem MiddleThrusterParticleSystem;
    [SerializeField] private ParticleSystem RightThrusterParticleSystem;

    private LanderMover _landerMover;

    private void Awake()
    {
        _landerMover = GetComponent<LanderMover>();
    }

    private void OnEnable()
    {
        _landerMover.OnUpForce += LanderMoverOnUpForce;
        _landerMover.OnLeftForce += LanderMoverOnLeftForce;
        _landerMover.OnRightForce += LanderMoverOnRightForce;
        _landerMover.OnBeforeForce += LanderMoverOnBeforeForce;
    }

    private void OnDisable()
    {
        _landerMover.OnUpForce -= LanderMoverOnUpForce;
        _landerMover.OnLeftForce -= LanderMoverOnLeftForce;
        _landerMover.OnRightForce -= LanderMoverOnRightForce;
        _landerMover.OnBeforeForce -= LanderMoverOnBeforeForce;
    }

    private void LanderMoverOnBeforeForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(MiddleThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(RightThrusterParticleSystem, false);
    }

    private void LanderMoverOnRightForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticleSystem, true);
    }

    private void LanderMoverOnLeftForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(RightThrusterParticleSystem, true);
    }

    private void LanderMoverOnUpForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(LeftThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(MiddleThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(RightThrusterParticleSystem, true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool isEnabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = isEnabled;
    }
}
