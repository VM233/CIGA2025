#if UNITY_EDITOR && ENABLE_SPRITES
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RenameSpritesToSequentialUnit : SingleButtonSpriteUnit
    {
        protected override string ProcessButtonName => "Rename Sprites To Sequential";

        [SerializeField]
        private string newName;

        protected override void OnProcessTexture(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects)
        {
            var spriteRects = provider.GetSpriteRects();
            
            var newName = this.newName;

            if (newName.IsNullOrEmpty())
            {
                newName = texture.name;
            }

            for (int i = 0; i < spriteRects.Length; i++)
            {
                var spriteRect = spriteRects[i];

                spriteRect.name = $"{newName}_{i}";
            }
            
            provider.SetSpriteRects(spriteRects);
        }

        protected override void OnProcessSprite(Sprite sprite, SpriteRect spriteRect, int processingIndex,
            ISpriteEditorDataProvider provider, IReadOnlyList<object> selectedObjects)
        {
            spriteRect.name = $"{newName}_{processingIndex}";
        }
    }
}
#endif