using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoomPuzzle
{
    public class SwitchOnInteract : MonoBehaviour
    {
        [Required]
        public Sprite offSprite;
        
        [Required]
        public Sprite onSprite;
        
        [Required]
        public SpriteRenderer spriteRenderer;

        protected IStageElement stageElement;

        protected virtual void Awake()
        {
            stageElement = GetComponent<IStageElement>();
            stageElement.OnInteract += OnInteract;
            stageElement.OnReset += OnReset;
        }

        protected virtual void OnReset()
        {
            spriteRenderer.sprite = offSprite;
        }

        protected virtual void OnInteract(IStageElement element, IStageElement from, InteractHint hint)
        {
            spriteRenderer.sprite = onSprite;
        }
    }
}