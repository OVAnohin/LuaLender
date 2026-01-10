using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float Volume;

    public float GetVolume => Volume;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
