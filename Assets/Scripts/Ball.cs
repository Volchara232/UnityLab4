using UnityEngine;
namespace Models

{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float maxRandomAngle = 5f; 
        
        private Rigidbody2D rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb.linearVelocity.magnitude < 0.1f) 
            {
                rb.linearVelocity = Vector2.up * speed;
            }

        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Length > 0)
            {
        
                float randomAngle = Random.Range(-maxRandomAngle, maxRandomAngle);
                Vector2 newDirection = Quaternion.Euler(0, 0, randomAngle) * rb.linearVelocity.normalized;
                rb.linearVelocity = newDirection * speed;
            }
        }
    } 
}
