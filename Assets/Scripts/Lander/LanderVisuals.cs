using System;
using System.Collections;
using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private ParticleSystem explosionParticleSystem;

    private Lander _lander;
    private LanderMover _landerMover;

    private void Awake()
    {
        _lander = GetComponent<Lander>();
        _landerMover = GetComponent<LanderMover>();
        LanderMoverEngineOff(null, null);
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
        var ps = Instantiate(explosionParticleSystem, transform.position, Quaternion.Euler(90f, 0f, 0f));
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration);
    }

    private void LanderMoverEngineStateChanged(bool isEngieActive)
    {
        if (isEngieActive == false)
            LanderMoverEngineOff(null, null);
    }

    private void LanderMoverEngineOff(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void LanderMoverOnRightForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, true);
    }

    private void LanderMoverOnLeftForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void LanderMoverOnUpForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(middleThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool isEnabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = isEnabled;
    }
}
