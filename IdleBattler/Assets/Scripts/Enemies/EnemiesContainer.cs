using System.Collections.Generic;
using UnityEngine;

public class EnemiesContainer : MonoBehaviour
{
    public Enemy GetNearestEnemy(Vector2 positionFrom, float searchRadius, bool includingBoss = true, List<Enemy> excludedEnemies = null)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(positionFrom, searchRadius);

        if (targets.Length == 0) return null;

        Enemy nearestEnemy = null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < targets.Length; i++)
        {
            var enemy = targets[i].transform.GetComponent<Enemy>();

            if (enemy == null) continue;

            if (includingBoss == false && enemy.EnemyType == EnemyType.Boss) continue;

            if (excludedEnemies != null && excludedEnemies.Contains(enemy)) continue;

            var distance = Vector2.Distance(targets[i].transform.position, positionFrom);

            if (minDistance > distance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}