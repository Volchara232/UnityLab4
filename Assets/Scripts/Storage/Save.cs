using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

namespace Models.Storage
{
    [System.Serializable]
    public class SerializableVector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public SerializableVector2() { }

        public SerializableVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public static SerializableVector2 FromVector2(Vector2 vector)
        {
            return new SerializableVector2(vector);
        }
    }

    [System.Serializable]
    public class BrickSaveData
    {
        public SerializableVector2 position { get; set; }
        public int health { get; set; }
        public string prefabType { get; set; }


        public BrickSaveData()
        {
            position = new SerializableVector2();
        }

        public BrickSaveData(Vector2 pos, int hp)
        {
            position = new SerializableVector2(pos);
            health = hp;
        }
    }

    [System.Serializable]
    public class BallSaveData
    {
        public SerializableVector2 position { get; set; }
        public SerializableVector2 velocity { get; set; }
        public BallSaveData()
        {
            position = new SerializableVector2();
            velocity = new SerializableVector2();
        }

        public BallSaveData(Vector2 pos, Vector2 vel)
        {
            position = new SerializableVector2(pos);
            velocity = new SerializableVector2(vel);
        }
    }

    [System.Serializable]
    public class GameSaveData
    {
        public List<BrickSaveData> bricks { get; set; } = new List<BrickSaveData>();
        public List<BallSaveData> balls { get; set; } = new List<BallSaveData>();
        public List<SerializableVector2> loots { get; set; } = new List<SerializableVector2>();
        public SerializableVector2 platform { get; set; }

        public GameSaveData()
        {
            platform = new SerializableVector2();
        }
    }
}