using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Vector3 _spawnPositionMin = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _spawnPositionMax = new Vector3(0, 0, 0);
    [SerializeField] private float _repeatRate = 0.1f;
    [SerializeField] private int _poolCapacity = 1000;
    [SerializeField] private int _poolMaxSize = 2000;
    [SerializeField] private Cube _prefab;

    private ObjectPool<GameObject> _pool;

    private void OnEnable()
    {
        Cube.Died += SendToPool;
    }

    private void OnDisable()
    {
        Cube.Died -= SendToPool;
    }

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab.gameObject),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetPosition();
        obj.transform.rotation = GetRotation();
        obj.GetComponent<Renderer>().material = _material;
        obj.GetComponent<Cube>().SwitchOff();
        obj.SetActive(true);
    }

    private Vector3 GetPosition()
    {
        float positionX = Random.Range(_spawnPositionMin.x, _spawnPositionMax.x);
        float positionY = Random.Range(_spawnPositionMin.y, _spawnPositionMax.y);
        float positionZ = Random.Range(_spawnPositionMin.z, _spawnPositionMax.z);
        Vector3 position = new Vector3(positionX, positionY, positionZ);

        return position;
    }

    private Quaternion GetRotation()
    {
        Quaternion rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

        return rotation;
    }

    private void SendToPool(GameObject obj)
    {
        _pool.Release(obj);
    }
}
