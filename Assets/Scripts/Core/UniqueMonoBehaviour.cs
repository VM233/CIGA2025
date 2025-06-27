using UnityEngine;

namespace RoomPuzzle
{
    public class UniqueMonoBehaviour<T> : MonoBehaviour
        where T : UniqueMonoBehaviour<T>
    {
        public static T Instance { get; set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
        }
    }
}