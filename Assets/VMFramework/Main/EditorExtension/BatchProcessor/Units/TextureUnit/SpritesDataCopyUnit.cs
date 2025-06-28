#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class SpritesDataCopyUnit : SingleButtonTextureSpritesUnit
    {
        protected override string ProcessButtonName => "Copy Sprites Data";

        [SerializeField]
        private Texture2D source;
        
        private SpriteDataProviderFactories factory;
        
        protected override void OnInit()
        {
            base.OnInit();

            factory = new();
            factory.Init();
        }

        protected override void OnProcess(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects)
        {
            if (texture.TryGetImporter(out var importer, out _))
            {
                if (importer.spriteImportMode != SpriteImportMode.Multiple)
                {
                    Debugger.LogError($"Cannot copy sprites data to a non-multiple sprite texture: {texture.name}.");
                }
            }
            
            var xScale = texture.width.F() / source.width;
            var yScale = texture.height.F() / source.height;
            
            var sourceProvider = factory.GetSpriteEditorDataProviderFromObject(source);
            sourceProvider.InitSpriteEditorDataProvider();

            var sourceSpriteRects = sourceProvider.GetSpriteRects();
            var spriteRects = provider.GetSpriteRects();

            for (int i = 0; i < sourceSpriteRects.Length; i++)
            {
                var sourceSpriteRect = sourceSpriteRects[i].Copy();
                var rect = sourceSpriteRect.rect;
                rect.x *= xScale;
                rect.y *= yScale;
                rect.width *= xScale;
                rect.height *= yScale;
                sourceSpriteRect.rect = rect;

                sourceSpriteRect.name = $"{texture.name}_{i}";

                if (i < spriteRects.Length)
                {
                    sourceSpriteRect.spriteID = spriteRects[i].spriteID;
                }
                
                sourceSpriteRects[i] = sourceSpriteRect;
            }
            
            provider.SetSpriteRects(sourceSpriteRects);
        }
    }
}
#endif