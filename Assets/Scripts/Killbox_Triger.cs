using UnityEngine;

public class TestKillbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"В триггер вошел: {other.name}");
        
        if (other.name.Contains("Ball"))
        {
            Debug.Log("МЯЧ УПАЛ! УНИЧТОЖАЕМ...");
            Destroy(other.gameObject);
        }
    }
}