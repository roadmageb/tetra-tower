using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridInfo : MonoBehaviour
{
    public Tilemap floorTile;
    public Transform enemySpawnPoints;
    public Transform centerPoint;
    public Transform dashZones;
    public Transform clearDashZones;

    [Header("StageInfo")]
    public int stage = 0;
    public int themeMask = 0;
}
