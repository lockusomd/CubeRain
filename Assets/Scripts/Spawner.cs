using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPositionMin = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _spawnPositionMax = new Vector3(0, 0, 0);
    [SerializeField] private float _repeatRate = 0.1f;
    [SerializeField] private int _poolCapacity = 1000;
    [SerializeField] private int _poolMaxSize = 2000;
    [SerializeField] private Cube _prefab;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => SetParameters(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Delete(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetCube());
    }

    private IEnumerator GetCube()
    {
        bool isWork = true;

        while (isWork)
        {
            _pool.Get();

            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void SetParameters(Cube cube)
    {
        cube.Died += SendToPool;

        cube.transform.position = GetRandomPosition();
        cube.transform.rotation = GetRandomRotation();
        cube.SwitchOff();
        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        float positionX = Random.Range(_spawnPositionMin.x, _spawnPositionMax.x);
        float positionY = Random.Range(_spawnPositionMin.y, _spawnPositionMax.y);
        float positionZ = Random.Range(_spawnPositionMin.z, _spawnPositionMax.z);

        return new Vector3(positionX, positionY, positionZ);
    }

    private Quaternion GetRandomRotation()
    {
        float minDegrees = 0f;
        float maxDegrees = 360f;

        return Quaternion.Euler(Random.Range(minDegrees, maxDegrees), Random.Range(minDegrees, maxDegrees), Random.Range(minDegrees, maxDegrees));
    }

    private void SendToPool(Cube cube)
    {
        cube.Died -= SendToPool;

        _pool.Release(cube);
    }

    private void Delete(Cube cube)
    {
        cube.Died -= SendToPool;

        Destroy(cube);
    }
}
