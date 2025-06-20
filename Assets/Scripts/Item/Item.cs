using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class Item : MonoBehaviour
    {
        [IsNotNullOrEmpty]
        public string itemID;
        
        [IsNotNullOrEmpty]
        public string itemName;

        [PreviewField(ObjectFieldAlignment.Center, Height = 50)]
        [Required]
        public Sprite icon;

        public int Count { get; set; } = 1;
    }
}