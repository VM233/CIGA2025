#if UNITY_EDITOR && ENABLE_SPRITES
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class SpritesPivotSetUnit : SingleButtonSpriteUnit
    {
        protected override string ProcessButtonName => "Set Pivot";

        [HorizontalGroup]
        [SerializeField]
        private SpriteAlignment alignment = SpriteAlignment.Center;

        [HorizontalGroup]
        [SerializeField]
        [EnableIf(nameof(alignment), SpriteAlignment.Custom)]
        private Vector2 pivot = new(0.5f, 0.5f);

        protected override void OnProcessSprite(Sprite sprite, SpriteRect spriteRect, int processingIndex,
            ISpriteEditorDataProvider provider, IReadOnlyList<object> selectedObjects)
        {
            spriteRect.alignment = alignment;
            spriteRect.pivot = pivot;
        }

        protected override void OnProcessTexture(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects)
        {
            var spriteRects = provider.GetSpriteRects();

            foreach (var spriteRect in spriteRects)
            {
                spriteRect.alignment = alignment;
                spriteRect.pivot = pivot;
            }

            provider.SetSpriteRects(spriteRects);
        }
    }
}
#endif