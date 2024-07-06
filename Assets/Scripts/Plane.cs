using UnityEngine;

public class Plane : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Cube>(out Cube component);

        component.Switch();
    }
}
