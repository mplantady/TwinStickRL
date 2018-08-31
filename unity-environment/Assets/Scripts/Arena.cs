using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class Arena : MonoBehaviour {
    
    public const float SpawnAreaWidth = 48f;
    public const float SpawnAreaHeight = 38f;

    [SerializeField]
    private List<Enemy> _enemyPrefabs = new List<Enemy>();

    [SerializeField]
    private GameObject _shipPrefab;

    private ShipAgent _spaceship;
    private float _nextSpawn;
    private int _spawnStrategy;
    private Academy _academy;
    private int _enemySpawned = 1;

    private List<Enemy> _enemies = new List<Enemy>();
    private List<Vector3> _edgePositions = new List<Vector3>();

    public void Initialize(TwinStickAcademy academy)
    {
        _academy = academy;

        _spaceship = Instantiate(_shipPrefab, transform, false).GetComponent<ShipAgent>();
        _spaceship.transform.localPosition = Vector3.zero;
        _spaceship.Arena = this;
        _spaceship.MoveController.GiveBrain(academy._movementBrain);
        _spaceship.ShootController.GiveBrain(academy._aimBrain);

        _edgePositions.Add(new Vector3(transform.position.x -SpawnAreaWidth, 0, transform.position.z + SpawnAreaHeight));
        _edgePositions.Add(new Vector3(transform.position.x -SpawnAreaWidth, 0, transform.position.z -SpawnAreaHeight));
        _edgePositions.Add(new Vector3(transform.position.x + SpawnAreaWidth, 0, transform.position.z + SpawnAreaHeight));
        _edgePositions.Add(new Vector3(transform.position.x + SpawnAreaWidth, 0, transform.position.z -SpawnAreaHeight));
    }

    public void ArenaStep(int stepCount, ResetParameters parameters)
	{
        if (_spaceship.IsDone())
        {
            return;
        }

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (!_enemies[i].IsAlive())
            {
                Destroy(_enemies[i].gameObject);
                _enemies.RemoveAt(i);
                continue;
            }

            _enemies[i].UpdateStep();
        }

        if (stepCount > _nextSpawn)
        {
            SpawnEnemy((int)parameters["max_enemy_count"], parameters["min_enemy_distance"]);
            _nextSpawn = stepCount + parameters["spawn_rate"];
        }
	}

    private void SpawnEnemy(int maxCount, float minDistance)
    {
        if (_enemies.Count >= maxCount)
        {
            return;
        }

        Vector3 newPos;
        do
        {
            _spawnStrategy = (_spawnStrategy + 1) % 3;

            switch (_spawnStrategy)
            {
                default:
                case 0:
                    newPos = RandomPositionInArea();
                    break;
                case 1:
                    newPos = RandomPositionNearShip(minDistance);
                    break;
                case 2:
                    newPos = RandomPositionInEdge();
                    break;
            }
        }
        while (Vector3.Distance(newPos, _spaceship.transform.position) < minDistance || !IsValid(newPos));

        var enemy = Instantiate(_enemyPrefabs[(_enemySpawned++ % 30 == 0) ? 1 : 0], newPos, Quaternion.identity) as Enemy;
        enemy.Initialize(_spaceship.transform);

        _enemies.Add(enemy);
    }

    private bool IsValid(Vector3 newPos)
    {
        return Math.Abs(transform.position.x - newPos.x) <= SpawnAreaWidth && Math.Abs(transform.position.z - newPos.z) <= SpawnAreaHeight;
    }

    public void ArenaReset()
    {
        _nextSpawn = 0;

        foreach (Enemy child in _enemies)
        {
            Destroy(child.gameObject);
        }
        _enemies.Clear();

        if (UnityEngine.Random.value < _academy.resetParameters["reset_center_prob"])
        {
            _spaceship.transform.position = transform.position;
        }
        else
        {
            _spaceship.transform.position = RandomPositionInArea();
        }
    }

    private Vector3 RandomPositionNearShip(float minEnemyDist)
    {
        return new Vector3(_spaceship.transform.position.x + UnityEngine.Random.Range(-minEnemyDist, minEnemyDist), 0, _spaceship.transform.position.z + UnityEngine.Random.Range(-minEnemyDist, minEnemyDist));
    }

    private Vector3 RandomPositionInEdge()
    {
        return _edgePositions[UnityEngine.Random.Range(0, _edgePositions.Count)];
    }

    public Vector3 RandomPositionInArea()
    {
        return new Vector3(transform.position.x + UnityEngine.Random.Range(-SpawnAreaWidth, SpawnAreaWidth), 0, transform.position.z + UnityEngine.Random.Range(-SpawnAreaHeight, SpawnAreaHeight));
    }
}
