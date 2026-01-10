using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int PointsValue;

    public int GetPoints => PointsValue;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
