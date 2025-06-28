using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class PopupManager : UniqueMonoBehaviour<PopupManager>
    {
        [Required]
        public Popup popupPrefab;

        protected readonly Stack<Popup> popupPool = new();

    }
}