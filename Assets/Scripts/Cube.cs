using System.Collections;
using UnityEngine;
using System;

public class Cube : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private bool _isSwitched = false;

    public bool IsSwitched => _isSwitched;

    public static event Action<GameObject> Died;

    private void Start()
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
        yield return new WaitForSeconds(UnityEngine.Random.Range(2,5));

        Died?.Invoke(gameObject);
    }

    public void SwitchOff()
    {
        _isSwitched = false;
    }
}
