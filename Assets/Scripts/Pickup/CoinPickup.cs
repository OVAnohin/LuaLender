using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int pointsValue;

    public event Action Collected;

    public int GetPoints => pointsValue;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        Collected?.Invoke();
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
