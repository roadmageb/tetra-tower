using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject enemyPrefab;
    public void SpawnEnemy(Transform parent)
    {
        var obj = Instantiate(enemyPrefab, parent);
        obj.transform.localPosition = Vector3.zero;
    }
}
