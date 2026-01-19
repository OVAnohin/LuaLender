using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private GameObject LanderPrefab;
    [SerializeField] private GameFlowController GameFlowController;

    //[Header("Spawn points")]
    //[SerializeField] private Transform landerSpawnPoint;

    [Header("Prefabs for level")]
    //[SerializeField] private GameObject[] landingPadPrefabs;
    [SerializeField] private GameObject[] fuelPickupPrefabs;
    [SerializeField] private GameObject[] coinPickupPrefabs;

    //public event Action<Lander> LanderSpawned;
    public event EventHandler<LanderArgs> LanderSpawned;
    public event EventHandler<LanderArgs> LanderDestroyed;

    private void Start()
    {
        SpawnLander();
        GenerateLevel();
    }

    private void SpawnLander()
    {
        var lander = Instantiate(LanderPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Lander>();
        lander.Initialize(GameFlowController);

        lander.Crashed += OnLanderCrashed;

        LanderSpawned?.Invoke(this, new LanderArgs(lander));
    }

    private void OnLanderCrashed(object sender, EventArgs e)
    {
        var lander = (Lander)sender;
        lander.Crashed -= OnLanderCrashed;

        LanderDestroyed?.Invoke(this, new LanderArgs(lander));
    }

    private void GenerateLevel()
    {
        Vector3 randomPos;
        //// Пример спавна LandingPad
        //Vector3 randomPos = new Vector3(Random.Range(-10f, 10f), 0f, 0f);
        //Instantiate(landingPadPrefabs[Random.Range(0, landingPadPrefabs.Length)], randomPos, Quaternion.identity);

        // Пример спавна FuelPickup
        randomPos = new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 5f), 0f);
        Instantiate(fuelPickupPrefabs[Random.Range(0, fuelPickupPrefabs.Length)], randomPos, Quaternion.identity);

        // Пример спавна CoinPickup
        randomPos = new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 5f), 0f);
        Instantiate(coinPickupPrefabs[Random.Range(0, coinPickupPrefabs.Length)], randomPos, Quaternion.identity);
    }
}
