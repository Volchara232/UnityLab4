using UnityEngine;
namespace Models

{
    public class Ball : MonoBehaviour
    {
        public float speed = 10f;
        public float maxRandomAngle = 5f; 
        
        private Rigidbody2D rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.up * speed;
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Length > 0)
            {
        
                float randomAngle = Random.Range(-maxRandomAngle, maxRandomAngle);
                Vector2 newDirection = Quaternion.Euler(0, 0, randomAngle) * rb.velocity.normalized;
                rb.velocity = newDirection * speed;
            }
        }
    } 
}
