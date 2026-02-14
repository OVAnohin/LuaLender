using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float volume;

    public event Action Collected;
    public float GetVolume => volume;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        Collected?.Invoke();
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
