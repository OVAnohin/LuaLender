using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private GameObject landerPrefab;
    [SerializeField] private LevelStateController levelStateController;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private PlanetSurface planetSurface;

    [Header("Prefabs for level")]
    [SerializeField] private GameObject[] landingPadPrefabs;
    [SerializeField] private GameObject[] fuelPickupPrefabs;
    [SerializeField] private GameObject[] coinPickupPrefabs;

    public event EventHandler<LanderEventArgs> LanderSpawned;
    public event EventHandler<LanderEventArgs> LanderDestroyed;

    private Lander _lander;
    private List<Vector2> _spawnPointsLandingPad;
    private Vector2 _spawnPointLander;

    private void OnEnable()
    {
        planetSurface.PlanetSurfaceRendered += PlanetSurfaceRendered;
    }

    private void OnDisable()
    {
        planetSurface.PlanetSurfaceRendered -= PlanetSurfaceRendered;
    }

    private void Start()
    {
        SpawnLander();
        cinemachineCamera.Follow = _lander.transform;
        cinemachineCamera.LookAt = _lander.transform;
        GenerateLevel();
    }

    private void PlanetSurfaceRendered(object sender, PlanetSurfaceEventArgs surfaceEventArgs)
    {
        _spawnPointsLandingPad = surfaceEventArgs.SpawnPointsLandingPad;
        _spawnPointLander = surfaceEventArgs.SpawnPointLander;
    }

    private void SpawnLander()
    {
        _lander = Instantiate(landerPrefab, new Vector3(0f, _spawnPointLander.y, 0f), Quaternion.identity).GetComponent<Lander>();

        int score = AppBootstrap.Instance.ProfileService.ActiveProfile.Statistics.TotalScore;
        _lander.Initialize(levelStateController, score);

        _lander.Crashed += OnLanderCrashed;

        LanderSpawned?.Invoke(this, new LanderEventArgs(_lander));
    }

    private void OnLanderCrashed(object sender, EventArgs e)
    {
        var lander = (Lander)sender;
        lander.Crashed -= OnLanderCrashed;

        LanderDestroyed?.Invoke(this, new LanderEventArgs(lander));
    }

    private void GenerateLevel()
    {
        for (int i = 0; i < _spawnPointsLandingPad.Count; i++)
        {
            Vector3 randomPos = new Vector3(_spawnPointsLandingPad[i].x, _spawnPointsLandingPad[i].y, 0f);
            Instantiate(landingPadPrefabs[Random.Range(0, landingPadPrefabs.Length)], randomPos, Quaternion.identity);

            float pointX = Random.Range(_spawnPointsLandingPad[i].x - 3, _spawnPointsLandingPad[i].x + 3);
            float pointY = Random.Range(_spawnPointsLandingPad[i].y + 5, _spawnPointsLandingPad[i].y + 9);
            randomPos = new Vector3(pointX, pointY, 0f);
            Instantiate(fuelPickupPrefabs[Random.Range(0, fuelPickupPrefabs.Length)], randomPos, Quaternion.identity);

            pointX = Random.Range(_spawnPointsLandingPad[i].x - 3, _spawnPointsLandingPad[i].x + 3);
            pointY = Random.Range(_spawnPointsLandingPad[i].y + 10, _spawnPointsLandingPad[i].y + 12);
            randomPos = new Vector3(pointX, pointY, 0f);
            Instantiate(coinPickupPrefabs[Random.Range(0, coinPickupPrefabs.Length)], randomPos, Quaternion.identity);
        }
    }
}
