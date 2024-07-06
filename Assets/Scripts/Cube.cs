using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    private ObjectPool<GameObject> _pool;

    private bool _isSwitched = false;

    public bool IsSwitched => _isSwitched;

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
        GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }

    private void DeleteCube()
    {
        StartCoroutine("DeleteWithDelay");
    }

    private IEnumerator DeleteWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(2,5));

        _pool.Release(gameObject);
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    public void SwitchOff()
    {
        _isSwitched = false;
    }
}
