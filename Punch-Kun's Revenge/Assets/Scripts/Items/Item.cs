using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            OnPickup(other.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(GameObject player);
}