using UnityEngine;

public class BallControllerFixed : MonoBehaviour
{
    [Header("Настройки скорости")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float minBounceAngle = 10f; // Минимальный угол отскока
    
    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    
    void Awake()
    {
        // Получаем Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        
        // Настраиваем Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            Debug.LogError("Rigidbody2D не найден на мяче!");
        }
    }
    
    void Start()
    {
        // Запускаем мяч
        LaunchBall();
    }
    
    void Update()
    {
        // Запоминаем скорость для корректного отражения
        if (rb != null)
        {
            lastVelocity = rb.velocity;
        }
        
        // Перезапуск по R
        if (Input.GetKeyDown(KeyCode.R))
        {
            LaunchBall();
        }
    }
    
    void FixedUpdate()
    {
        if (rb == null) return;
        
        // Поддерживаем постоянную скорость
        if (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        else if (rb.velocity.magnitude < 0.1f && rb.velocity.magnitude > 0)
        {
            // Если скорость слишком мала - перезапускаем
            LaunchBall();
        }
    }
    
    void LaunchBall()
    {
        if (rb == null) return;
        
        // Запускаем мяч вверх с небольшим случайным смещением
        float randomX = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(randomX, 1f).normalized;
        
        rb.velocity = direction * speed;
        lastVelocity = rb.velocity;
        
        Debug.Log("Мяч запущен! Направление: " + direction);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb == null) return;
        
        Debug.Log("Столкновение с: " + collision.gameObject.name);
        
        // Получаем нормаль столкновения
        if (collision.contacts.Length > 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            
            // Отражение вектора скорости
            Vector2 reflectedVelocity = Vector2.Reflect(lastVelocity.normalized, normal).normalized;
            
            // Проверяем, чтобы угол отскока не был слишком маленьким
            float angle = Vector2.Angle(reflectedVelocity, Vector2.up);
            
            if (Mathf.Abs(angle) < minBounceAngle || Mathf.Abs(angle) > 180 - minBounceAngle)
            {
                // Если угол слишком маленький - корректируем его
                float correctedAngle = Mathf.Sign(reflectedVelocity.x) * minBounceAngle;
                reflectedVelocity = new Vector2(
                    Mathf.Sin(correctedAngle * Mathf.Deg2Rad),
                    Mathf.Cos(correctedAngle * Mathf.Deg2Rad)
                );
            }
            
            // Применяем новую скорость
            rb.velocity = reflectedVelocity * speed;
            
            Debug.Log($"Отскок! Угол: {angle}°, Направление: {reflectedVelocity}");
        }
    }
    
    // Для визуализации направления
    void OnDrawGizmos()
    {
        if (Application.isPlaying && rb != null && rb.velocity.magnitude > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, rb.velocity.normalized * 2);
            
            // Линия к следующей позиции
            Gizmos.color = Color.yellow;
            Vector3 nextPos = transform.position + (Vector3)rb.velocity * Time.fixedDeltaTime;
            Gizmos.DrawLine(transform.position, nextPos);
        }
    }
}