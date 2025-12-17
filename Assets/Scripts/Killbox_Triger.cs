using UnityEngine;

public class TestKillbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"В триггер вошел: {other.name}");
        Destroy(other.gameObject);
        
    }
}