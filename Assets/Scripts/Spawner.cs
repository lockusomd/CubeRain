using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Vector3 _spawnPositionMin = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _spawnPositionMax = new Vector3(0, 0, 0);
    [SerializeField] private float _scale = 0.25f;
    [SerializeField] private float _repeatRate = 0.1f;
    [SerializeField] private int _poolCapacity = 1000;
    [SerializeField] private int _poolMaxSize = 2000;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => CreateCube(),
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

    private GameObject CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = GetPosition();
        cube.transform.rotation = GetRotation();
        cube.transform.localScale *= _scale;
        cube.AddComponent<Cube>();
        cube.GetComponent<Cube>().SetPool(_pool);
        cube.AddComponent<BoxCollider>();
        cube.GetComponent<Renderer>().material = _material;
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        cube.SetActive(false);

        return cube;
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
}
