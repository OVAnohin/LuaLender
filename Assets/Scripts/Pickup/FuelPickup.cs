using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float volume;

    public float GetVolume => volume;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
