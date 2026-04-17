using UnityEngine;

public class OffLevelDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
            GameManager.Instance.GameOver();
    }
}