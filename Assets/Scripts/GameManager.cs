using UnityEngine;

namespace Models
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public bool IsPaused { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePause();
            }
        }
        
        public void TogglePause()
        {
            IsPaused = !IsPaused;
            
            if (IsPaused)
            {
                Time.timeScale = 0f;
                Debug.Log("Игра на паузе");
            }
            else
            {
                Time.timeScale = 1f;
                Debug.Log("Игра продолжается");
            }
        }
        
        public void SetPause(bool pause)
        {
            IsPaused = pause;
            Time.timeScale = pause ? 0f : 1f;
        }
    }
}