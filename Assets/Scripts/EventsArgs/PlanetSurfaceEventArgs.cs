using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSurfaceEventArgs : EventArgs
{
    public List<Vector2> SpawnPointsLandingPad { get; private set; }
    public Vector2 SpawnPointLander { get; private set; }

    public PlanetSurfaceEventArgs(Vector2 spawnPointLander, List<Vector2> spawnPointsLandingPad)
    {
        SpawnPointLander = spawnPointLander;
        SpawnPointsLandingPad = spawnPointsLandingPad;
    }
}
