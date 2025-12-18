using UnityEngine;
namespace Models
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private int hp = 1; 
        [SerializeField] private LootSpawner lootSpawner;
        [SerializeField] private string brickType = "default" ;
        public int HP => hp;
        public string BrickType => brickType;
        public void SetHP(int newHP)
        {
            hp = newHP;

        }
        public void SetBrickType(string type)
        {
            brickType = type;
        }
        private void Start()
        {
            if (lootSpawner == null)
            {
                lootSpawner = GetComponent<LootSpawner>();
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("ПОПАДАНИЕ ПО КИРПИЧУ!");
            hp--;

            if (hp <= 0)
            {
                lootSpawner.TrySpawnLoot(transform.position);
                Destroy(gameObject);
                Debug.Log("Кирпич сломан!");
            }
        }
    }
}