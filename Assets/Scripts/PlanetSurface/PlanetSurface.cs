using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteShapeController))]
[RequireComponent(typeof(EdgeCollider2D))]
public class PlanetSurface : MonoBehaviour
{
    [SerializeField] private SpriteShape spriteShape;

    public event EventHandler<PlanetSurfaceEventArgs> PlanetSurfaceRendered;

    private SpriteShapeController _spriteShapeController;
    private EdgeCollider2D _edgeCollider;
    private List<Vector2> _spawnPointsLandingPad;
    private Vector2 _spawnPointLander;

    private void Awake()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

        _spriteShapeController.spriteShape = spriteShape;

        GenerateSurface();
    }

    private void Start()
    {
        PlanetSurfaceRendered?.Invoke(this, new PlanetSurfaceEventArgs(_spawnPointLander, _spawnPointsLandingPad));
    }

    private void GenerateSurface()
    {
        Dictionary<string, Vector2> screenBounds2x = GetScreenBounds2x();
        _spawnPointsLandingPad = new List<Vector2>();

        Vector2 startPoint = new Vector2(-screenBounds2x["MaxX"].x, 0);
        List<Vector2> surface = new List<Vector2>();

        surface.Add(new Vector2(-screenBounds2x["MaxX"].x, -screenBounds2x["MaxY"].y));
        surface = MergeSurfaces(surface, GenerateFlatSurface(startPoint, 10));

        bool isExit = true;
        while (isExit)
        {
            float randomWidth = Random.Range(2f, 15f);
            float randomHeight = Random.Range(2f, 15f);
            int randomPoint = Random.Range(10, 20);
            int randomFlatSurface = Random.Range(3, 5);

            bool IsHillOrCrater = false;
            if (randomPoint > 11)
                IsHillOrCrater = true;

            surface = MergeSurfaces(surface, GenerateCurve(surface[surface.Count - 1], randomWidth, randomHeight, randomPoint, IsHillOrCrater));

            if (surface[surface.Count - 1].x >= screenBounds2x["MaxX"].x)
            {
                surface = MergeSurfaces(surface, GenerateFlatSurface(surface[surface.Count - 1], 10));
                isExit = false;
            }
            else
            {
                surface = MergeSurfaces(surface, GenerateFlatSurface(surface[surface.Count - 1], randomFlatSurface));
                if (randomFlatSurface < 5)
                    _spawnPointsLandingPad.Add(surface[surface.Count - 2]);
                else
                    _spawnPointsLandingPad.Add(surface[surface.Count - 3]);
            }
        }

        surface = Vector2Utils.RemoveDuplicates(surface);
        Vector2 lowestPoint = Vector2Utils.GetLowestPoint(surface);
        _spawnPointLander = Vector2Utils.GetHigestPoint(surface);
        _spawnPointLander.y = _spawnPointLander.y + 5;

        if (lowestPoint.y < surface[0].y)
        {
            Vector2 zeroPoint = surface[0];
            zeroPoint.y = lowestPoint.y - 5;
            surface[0] = zeroPoint;
            lowestPoint = zeroPoint;
        }

        surface.Add(new Vector2(surface[surface.Count - 1].x, lowestPoint.y));

        // 3. Добавляем сплайн в SpriteShape
        var spline = _spriteShapeController.spline;
        spline.Clear();
        for (int i = 0; i < surface.Count; i++)
        {
            spline.InsertPointAt(i, surface[i]);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

        spline.isOpenEnded = false;
        _spriteShapeController.RefreshSpriteShape();

        // 4. Обновляем EdgeCollider
        UpdateEdgeCollider(surface);
    }

    private Dictionary<string, Vector2> GetScreenBounds2x()
    {
        Camera camera = Camera.main;
        var result = new Dictionary<string, Vector2>();

        Vector2 center = camera.transform.position;

        float screenHeight = camera.orthographicSize * 2f;
        float screenWidth = screenHeight * camera.aspect;

        Vector2 maxX = center + new Vector2(screenWidth, 0);
        Vector2 maxY = center + new Vector2(0, screenHeight);

        result["Center"] = center;
        result["MaxX"] = maxX;
        result["MaxY"] = maxY;

        return result;
    }

    private List<Vector2> GenerateFlatSurface(Vector2 startPoint, int length)
    {
        List<Vector2> surface = new List<Vector2>();

        float step = 1f;
        int pointsCount = Mathf.RoundToInt(length / step);

        for (int i = 0; i <= pointsCount; i++)
        {
            Vector2 point = new Vector2(startPoint.x + i * step, startPoint.y);
            surface.Add(point);
        }

        return surface;
    }

    private List<Vector2> GenerateCurve(Vector2 startPoint, float width, float height, int segments = 10, bool up = true)
    {
        List<Vector2> points = new List<Vector2>();
        float radius = width / 2f;
        Vector2 center = startPoint + new Vector2(radius, 0);

        float randomFactor = Random.Range(0.7f, 1.3f);

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;

            float angle = Mathf.PI * t; 
            float curve = Mathf.Sin(angle * randomFactor); 

            float x = startPoint.x + t * width; 
            float y = startPoint.y + (up ? 1 : -1) * curve * height;

            points.Add(new Vector2(x, y));
        }

        return points;
    }

    private List<Vector2> MergeSurfaces(List<Vector2> first, List<Vector2> second)
    {
        List<Vector2> result = new List<Vector2>(first.Count + second.Count);

        result.AddRange(first);
        result.AddRange(second);

        return result;
    }

    private void UpdateEdgeCollider(List<Vector2> points)
    {
        _edgeCollider.points = points.ToArray();
    }
}
