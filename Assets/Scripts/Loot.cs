using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Models
{
   public class Loot : MonoBehaviour
    {
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Player") || other.CompareTag("Player"))
        {
            Debug.Log("Лут пойман!");
            Destroy(gameObject);
        }
    }
    } 
}

