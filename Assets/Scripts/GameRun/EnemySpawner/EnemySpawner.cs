using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private StickmanEnemy enemyPrefab;
    [SerializeField] private float distanceBetweenLines;
    [SerializeField] private float maxEnemyXOffset;
    [SerializeField] private int maxEnemiesCountOnLine = 2;
    [SerializeField] private float distForEnemiesToGen;
    [SerializeField] private float distForEnemiesToDissappear;

    private Car car;
    private GameObjectsPool<StickmanEnemy> enemyPool;
    private LinkedList<EnemyLine> enemyLines;
    public void Init(Car car)
    {
        this.car = car;
        enemyPool = new GameObjectsPool<StickmanEnemy>(enemyPrefab, 30);
    }

    public void StartSpawning(Vector3 spawnOrigin)
    {
        enemyLines = new LinkedList<EnemyLine>();
        SpawnLinesWhileInGenDist(spawnOrigin);
    }

    public void CustomUpdate(float deltaTime)
    {
        //deleting last
        EnemyLine lastLine = enemyLines.Last.Value;
        if(lastLine.position.z - car.transform.position.z < -distForEnemiesToDissappear)
        {
            lastLine.ClearLine(enemyPool);
            enemyLines.RemoveLast();
        }
        //appending first
        EnemyLine firstLine = enemyLines.First.Value;
        if (firstLine.position.z - car.transform.position.z < distForEnemiesToGen)
        {
            SpawnLinesWhileInGenDist(firstLine.position + new Vector3(0, 0, distanceBetweenLines));
        }
    }

    private void SpawnLinesWhileInGenDist(Vector3 spawnOrigin)
    {
        Vector3 linePos = spawnOrigin;
        while (Mathf.Abs(spawnOrigin.z - linePos.z) < distForEnemiesToGen)
        {
            EnemyLine enemyLine = new EnemyLine(linePos);
            int enemyCount = Random.Range(0, maxEnemiesCountOnLine);
            while (enemyCount > 0)
            {
                StickmanEnemy enemyToAdd = enemyPool.Get();
                enemyLine.enemies.Add(enemyToAdd);
                enemyToAdd.transform.position = linePos +
                    new Vector3(Random.Range(-maxEnemyXOffset, maxEnemyXOffset), 0);
                enemyToAdd.Activate();
                enemyCount--;
            }
            enemyLines.AddFirst(enemyLine);
            linePos += new Vector3(0, 0, distanceBetweenLines);
        } 
    }

    private class EnemyLine
    {
        public List<StickmanEnemy> enemies {  get; private set; }
        public Vector3 position { get; private set; }
        public EnemyLine(Vector3 position)
        {
            enemies = new List<StickmanEnemy>();
            this.position = position;
        }

        public void ClearLine(GameObjectsPool<StickmanEnemy> enemyPool)
        {
            foreach (StickmanEnemy enemy in enemies)
            {
                enemyPool.Return(enemy);
            }
        }
    }
}
