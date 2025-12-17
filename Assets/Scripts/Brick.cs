using UnityEngine;
namespace Models
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private int HP = 1;
        [SerializeField] private LootSpawner lootSpawner;
        
        private void Start()
        {
            // Если не назначили через инспектор - ищем на этом же объекте
            if (lootSpawner == null)
            {
                lootSpawner = GetComponent<LootSpawner>();
            }
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("ПОПАДАНИЕ ПО КИРПИЧУ!");
            HP--;
            
            if (HP <= 0)
            {

                lootSpawner.TrySpawnLoot(transform.position);
                Destroy(gameObject);
                Debug.Log("Кирпич сломан!");
            }   
        }
    }
}