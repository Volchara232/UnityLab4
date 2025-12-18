using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using System.IO;
using System;

namespace Models.Storage
{
    public class SaveManager : MonoBehaviour
    {
        [Header("Префабы")]
        [SerializeField] private GameObject brickPrefab;
        [SerializeField] private GameObject brickPrefab2;
        [SerializeField] private GameObject brickPrefab3;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject lootPrefab;
        [SerializeField] private GameObject platformPrefab;

        [Header("Настройки")]
        [SerializeField] private string saveFileName = "savegame.json";

        private Dictionary<string, GameObject> brickPrefabs = new Dictionary<string, GameObject>();
        public static SaveManager Instance { get; private set; }

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
            InitializeBrickPrefabs();
        }
        private void InitializeBrickPrefabs()
        {
            // Добавляем префабы в словарь
            if (brickPrefab != null)
                brickPrefabs["default"] = brickPrefab;

            if (brickPrefab2 != null)
                brickPrefabs["type2"] = brickPrefab2;

            if (brickPrefab3 != null)
                brickPrefabs["type3"] = brickPrefab3;

            Debug.Log($"Инициализировано {brickPrefabs.Count} префабов кирпичей");
        }
        public void SaveGame()
        {
            try
            {
                GameSaveData saveData = new GameSaveData();
                Brick[] allBricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);
                foreach (Brick brick in allBricks)
                {
                    var brickData = new BrickSaveData
                    {
                        position = SerializableVector2.FromVector2(brick.transform.position),
                        health = GetBrickHP(brick),
                        prefabType = GetBrickType(brick) 
                    };
                    saveData.bricks.Add(brickData);
                }

                Ball[] allBalls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
                foreach (Ball ball in allBalls)
                {
                    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        var ballData = new BallSaveData
                        {
                            position = SerializableVector2.FromVector2(ball.transform.position),
                            velocity = SerializableVector2.FromVector2(rb.linearVelocity) // Используем linearVelocity
                        };
                        saveData.balls.Add(ballData);
                    }
                }

                Loot[] allLoot = FindObjectsByType<Loot>(FindObjectsSortMode.None);
                foreach (Loot loot in allLoot)
                {
                    saveData.loots.Add(SerializableVector2.FromVector2(loot.transform.position));
                }

                Platform platform = FindFirstObjectByType<Platform>();
                if (platform != null)
                {
                    saveData.platform = SerializableVector2.FromVector2(platform.transform.position);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IncludeFields = false
                };

                string jsonString = JsonSerializer.Serialize(saveData, options);
                string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
                File.WriteAllText(filePath, jsonString);

                Debug.Log($"Игра сохранена! Файл: {filePath}");
                Debug.Log($"Статистика: {saveData.bricks.Count} кирпичей, " +
                         $"{saveData.balls.Count} шаров, " +
                         $"{saveData.loots.Count} лута");

                Debug.Log("Содержимое JSON:\n" + jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка сохранения: {e.Message}\n{e.StackTrace}");
            }
        }

        public void LoadGame()
        {
            try
            {
                string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"Файл сохранения не найден: {filePath}");
                    return;
                }

                string jsonString = File.ReadAllText(filePath);
                Debug.Log($"Читаем JSON из: {filePath}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = false
                };

                GameSaveData saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, options);

                if (saveData == null)
                {
                    Debug.LogError("Не удалось десериализовать данные!");
                    return;
                }

                Debug.Log($"Загружаем игру...");
                Debug.Log($"Загружено: {saveData.bricks.Count} кирпичей, " +
                         $"{saveData.balls.Count} шаров, " +
                         $"{saveData.loots.Count} лута");

             
                ClearCurrentLevel();

                foreach (BrickSaveData brickData in saveData.bricks)
                {
                    if (brickData.position != null)
                    {
                        Vector2 position = new Vector2(brickData.position.x, brickData.position.y);

                        GameObject brickPrefabToUse = GetBrickPrefabByType(brickData.prefabType);

                        if (brickPrefabToUse != null)
                        {
                            GameObject newBrick = Instantiate(brickPrefabToUse, position, Quaternion.identity);

                            Brick brickComponent = newBrick.GetComponent<Brick>();
                            if (brickComponent != null)
                            {
                                SetBrickHP(brickComponent, brickData.health);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"⚠️ Не найден префаб для типа: {brickData.prefabType}. Использую дефолтный.");
                            if (brickPrefabs.ContainsKey("default"))
                            {
                                GameObject newBrick = Instantiate(brickPrefabs["default"], position, Quaternion.identity);
                                Brick brickComponent = newBrick.GetComponent<Brick>();
                                if (brickComponent != null)
                                {
                                    SetBrickHP(brickComponent, brickData.health);
                                }
                            }
                        }
                    }
                }


                if (ballPrefab != null)
                {
                    foreach (BallSaveData ballData in saveData.balls)
                    {
                        if (ballData.position != null)
                        {
                            Vector2 position = new Vector2(ballData.position.x, ballData.position.y);
                            GameObject newBall = Instantiate(ballPrefab, position, Quaternion.identity);


                            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
                            if (rb != null && ballData.velocity != null)
                            {
                                Vector2 velocity = new Vector2(ballData.velocity.x, ballData.velocity.y);
                                rb.linearVelocity = velocity; 
                            }
                        }
                    }
                }


                if (lootPrefab != null)
                {
                    foreach (SerializableVector2 lootPos in saveData.loots)
                    {
                        if (lootPos != null)
                        {
                            Vector2 position = new Vector2(lootPos.x, lootPos.y);
                            Instantiate(lootPrefab, position, Quaternion.identity);
                        }
                    }
                }


                if (saveData.platform != null && platformPrefab != null)
                {
                    Vector2 position = new Vector2(saveData.platform.x, saveData.platform.y);
                    Instantiate(platformPrefab, position, Quaternion.identity);
                }

                Debug.Log(" Игра успешно загружена!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка загрузки: {e.Message}\n{e.StackTrace}");
            }
        }

        private string GetBrickType(Brick brick)
        {
            try
            {
                var typeProperty = brick.GetType().GetProperty("BrickType");
                if (typeProperty != null && typeProperty.CanRead)
                {
                    return (string)typeProperty.GetValue(brick) ?? "default";
                }

                var typeField = brick.GetType().GetField("brickType",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (typeField != null)
                {
                    return (string)typeField.GetValue(brick) ?? "default";
                }

                return "default";
            }
            catch
            {
                return "default";
            }
        }

        private GameObject GetBrickPrefabByType(string type)
        {
            if (string.IsNullOrEmpty(type))
                type = "default";

            if (brickPrefabs.ContainsKey(type))
                return brickPrefabs[type];

            if (brickPrefabs.ContainsKey("default"))
                return brickPrefabs["default"];

            return brickPrefab; 
        }

        private int GetBrickHP(Brick brick)
        {
            try
            {
                
                var hpProperty = brick.GetType().GetProperty("HP");
                if (hpProperty != null && hpProperty.CanRead)
                {
                    return (int)hpProperty.GetValue(brick);
                }

             
                var getHPMethod = brick.GetType().GetMethod("GetHP");
                if (getHPMethod != null)
                {
                    return (int)getHPMethod.Invoke(brick, null);
                }

                var hpField = brick.GetType().GetField("hp",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (hpField != null)
                {
                    return (int)hpField.GetValue(brick);
                }

                Debug.LogWarning("Не удалось получить HP кирпича. Используется значение по умолчанию: 1");
                return 1;
            }
            catch
            {
                return 1;
            }
        }

        private void SetBrickHP(Brick brick, int hp)
        {
            try
            {
         
                var setHPMethod = brick.GetType().GetMethod("SetHP");
                if (setHPMethod != null)
                {
                    setHPMethod.Invoke(brick, new object[] { hp });
                    return;
                }

                var hpProperty = brick.GetType().GetProperty("HP");
                if (hpProperty != null && hpProperty.CanWrite)
                {
                    hpProperty.SetValue(brick, hp);
                    return;
                }

                var hpField = brick.GetType().GetField("hp",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (hpField != null)
                {
                    hpField.SetValue(brick, hp);
                    return;
                }

                Debug.LogWarning($"⚠️ Не удалось установить HP для кирпича. Используется значение: {hp}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка установки HP: {e.Message}");
            }
        }

        private void ClearCurrentLevel()
        {
            try
            {
                ClearObjects<Brick>();
                ClearObjects<Ball>();
                ClearObjects<Loot>();

                Platform platform = FindFirstObjectByType<Platform>();
                if (platform != null)
                {
                    Destroy(platform.gameObject);
                }

                Debug.Log("🧹 Сцена очищена");
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при очистке сцены: {e.Message}");
            }
        }

        private void ClearObjects<T>() where T : MonoBehaviour
        {
            T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (T obj in objects)
            {
                if (obj != null && obj.gameObject != null)
                {
                    Destroy(obj.gameObject);
                }
            }
        }


        public bool SaveExists()
        {
            string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

            if (!File.Exists(filePath))
                return false;

            try
            {
                string jsonString = File.ReadAllText(filePath);
                GameSaveData testData = JsonSerializer.Deserialize<GameSaveData>(jsonString);
                return testData != null;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteSave()
        {
            string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Debug.Log("Сохранение удалено");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка удаления: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Сохранение не найдено для удаления");
            }
        }

        public string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, saveFileName);
        }

        void Start()
        {
            Debug.Log($"SaveManager запущен");
            Debug.Log($"Путь для сохранений: {Application.persistentDataPath}");

            if (SaveExists())
            {
                Debug.Log($"Найдено сохранение: {GetSavePath()}");
            }
            else
            {
                Debug.Log($"Сохранений не найдено");
            }
        }
    }
}