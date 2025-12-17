using UnityEngine;
namespace Models
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject lootPrefab;
        [SerializeField] [Range(0f, 1f)] private float spawnChance = 1f;
        public void TrySpawnLoot(Vector2 spawnPosition)
        {
            if (lootPrefab == null)
            {
                Debug.LogWarning("LootPrefab не назначен в LootSpawn!");
                return;
            }

            float roll = Random.Range(0f, 1f);
            if (roll <= spawnChance)
            {
                Instantiate(lootPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"Лут заспавнен с шансом {spawnChance * 100}%");
            }
        }
    }
}