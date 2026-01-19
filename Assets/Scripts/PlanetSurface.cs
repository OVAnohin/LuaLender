using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class PlanetSurface : MonoBehaviour
{
    [SerializeField] private int Points = 32;
    [SerializeField] private float Radius = 5f;

    private SpriteShapeController _spriteShapeController;

    private void Awake()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();
        GenerateCircle();
    }

    private void GenerateCircle()
    {
        var spline = _spriteShapeController.spline;
        spline.Clear();

        for (int i = 0; i < Points; i++)
        {
            float angle = (float)i / Points * Mathf.PI * 2f;
            Vector2 pos = new Vector2(
                Mathf.Cos(angle) * Radius,
                Mathf.Sin(angle) * Radius
            );

            spline.InsertPointAt(i, pos);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

        spline.isOpenEnded = false;
    }
}
