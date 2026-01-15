using System;
using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem LeftThrusterParticleSystem;
    [SerializeField] private ParticleSystem MiddleThrusterParticleSystem;
    [SerializeField] private ParticleSystem RightThrusterParticleSystem;
    [SerializeField] private ParticleSystem ExplosionParticleSystem;

    private Lander _lander;
    private LanderMover _landerMover;

    private void Awake()
    {
        _lander = GetComponent<Lander>();
        _landerMover = GetComponent<LanderMover>();
        ExplosionParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnEnable()
    {
        _lander.Crashed += LanderCrashed;
        _landerMover.OnUpForce += LanderMoverOnUpForce;
        _landerMover.OnLeftForce += LanderMoverOnLeftForce;
        _landerMover.OnRightForce += LanderMoverOnRightForce;
        _landerMover.EngineStateChanged += LanderMoverEngineStateChanged;
    }

    private void OnDisable()
    {
        _lander.Crashed -= LanderCrashed;
        _landerMover.OnUpForce -= LanderMoverOnUpForce;
        _landerMover.OnLeftForce -= LanderMoverOnLeftForce;
        _landerMover.OnRightForce -= LanderMoverOnRightForce;
        _landerMover.EngineStateChanged -= LanderMoverEngineStateChanged;
    }

    private void LanderCrashed(object sender, EventArgs e)
    {
        ExplosionParticleSystem.Play();
    }

    private void LanderMoverEngineStateChanged(bool isEngieActive)
    {
        if (isEngieActive == false)
            LanderMoverEngineOff(null, null);
    }

    private void LanderMoverEngineOff(object sender, EventArgs e)
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
