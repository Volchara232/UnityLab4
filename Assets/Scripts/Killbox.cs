using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"В триггер вошел: {other.name}");
        Destroy(other.gameObject);
    }
}