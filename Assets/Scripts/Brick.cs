using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hp = 1;

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ПОПАДАНИЕ ПО КИРПИЧУ!");
        hp--;
            
        if (hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Кирпич сломан!");
        }   
    }
}