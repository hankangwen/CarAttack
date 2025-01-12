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

    private Coroutine spawnRoutine;
    public void Init(Car car)
    {
        this.car = car;
        enemyPool = new GameObjectsPool<StickmanEnemy>(enemyPrefab, 30);
    }

    public void StartSpawning(Vector3 spawnOrigin, float mapEnd)
    {
        if (spawnRoutine != null) throw new System.Exception("EnemySpawner.StartSpawning cant start spawning twice");
        enemyLines = new LinkedList<EnemyLine>();
        SpawnLinesWhileInGenDist(spawnOrigin, spawnOrigin, mapEnd);
        spawnRoutine = StartCoroutine(Spawning(spawnOrigin, mapEnd));
    }

    private IEnumerator Spawning(Vector3 spawnOrigin, float genStopDistance)
    {
        EnemyLine firstLine;
        do
        {
            //deleting last
            EnemyLine lastLine = enemyLines.Last.Value;
            if (lastLine.position.z - car.transform.position.z < -distForEnemiesToDissappear)
            {
                lastLine.ClearLine(enemyPool);
                enemyLines.RemoveLast();
            }
            //appending first
            firstLine = enemyLines.First.Value;
            if (firstLine.position.z - car.transform.position.z < distForEnemiesToGen)
            {
                SpawnLinesWhileInGenDist(firstLine.position + new Vector3(0, 0, distanceBetweenLines),
                    car.transform.position, genStopDistance);
            }
            yield return new WaitForEndOfFrame();
        } while (Mathf.Abs(firstLine.position.z - spawnOrigin.z) < genStopDistance);
        Debug.Log("finished spawning");
    }

    public void StopSpawningAndClearEnemies()
    {
        foreach (EnemyLine line in enemyLines)
        {
            foreach (var enemy in line.enemies)
            {
                if(enemy.vitality.hp > 0) enemy.Die();
            }
            line.ClearLine(enemyPool);
        }
        enemyLines.Clear();
        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private void SpawnLinesWhileInGenDist(Vector3 spawnOrigin, Vector3 pointToCalcDist,
        float genStopDistance)
    {
        Vector3 linePos = spawnOrigin;
        while (Mathf.Abs(pointToCalcDist.z - linePos.z) < distForEnemiesToGen 
            && Mathf.Abs(linePos.z - spawnOrigin.z) < genStopDistance)
        {
            EnemyLine enemyLine = new EnemyLine(linePos);
            
            int enemyCount = Random.Range(1, maxEnemiesCountOnLine + 1);
            float xPosOffsetDrag = (maxEnemyXOffset * 2)/ enemyCount;
            float xPosOffset = -maxEnemyXOffset;
            while (enemyCount > 0)
            {
                StickmanEnemy enemyToAdd = enemyPool.Get();
                enemyLine.enemies.Add(enemyToAdd);
                enemyToAdd.transform.Rotate(new Vector3(0,Random.Range(0,360),0));
                enemyToAdd.transform.position = linePos +
                    new Vector3(Random.Range(xPosOffset, xPosOffset + xPosOffsetDrag), 0);
                enemyToAdd.Activate();
                enemyToAdd.vitality.HealToMax();
                enemyCount--;
                xPosOffset += xPosOffsetDrag;
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
