using System.Collections;
using UnityEngine;
using System;

public class Cube : MonoBehaviour
{
    [SerializeField] private Material _material;

    private MeshRenderer _meshRenderer;

    private bool _isSwitched = false;

    public event Action<Cube> Died;

    private void OnEnable()
    {
        _meshRenderer.material = _material;
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Switch()
    {
        if (_isSwitched != true)
        {
            ChangeColor();
            DeleteCube();
        }

        _isSwitched = true;
    }

    private void ChangeColor()
    {
        _meshRenderer.material.color = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    private void DeleteCube()
    {
        StartCoroutine(DeleteWithDelay());
    }

    private IEnumerator DeleteWithDelay()
    {
        int minLifetime = 2;
        int maxLifetime = 5;

        yield return new WaitForSeconds(UnityEngine.Random.Range(minLifetime, maxLifetime));

        Died?.Invoke(gameObject.GetComponent<Cube>());
    }

    public void SwitchOff()
    {
        _isSwitched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isSwitched == false)
        {
            if(collision.gameObject.TryGetComponent<Plane>(out Plane component))
            {
                Switch();
            }
        }
    }
}
