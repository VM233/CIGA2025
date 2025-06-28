#if UNITY_EDITOR && ENABLE_SPRITES
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class SingleButtonSpriteUnit : SingleButtonBatchProcessorUnit
    {
        private SpriteDataProviderFactories factory;
        
        private readonly Dictionary<Texture2D, List<Sprite>> spritesLookup = new();
        
        protected override void OnInit()
        {
            base.OnInit();

            factory = new();
            factory.Init();
        }

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is Sprite or Texture2D);
        }

        protected sealed override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            spritesLookup.Clear();
            
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not Sprite sprite)
                {
                    continue;
                }

                var list = spritesLookup.GetValueOrAddNew(sprite.texture);
                list.Add(sprite);
            }

            if (spritesLookup.Count != 0)
            {
                var spriteRectsByName = new Dictionary<string, SpriteRect>();
            
                foreach (var (texture, sprites) in spritesLookup)
                {
                    if (texture.TryGetAssetPath(out var path) == false)
                    {
                        continue;
                    }
                
                    spriteRectsByName.Clear();
                
                    var provider = factory.GetSpriteEditorDataProviderFromObject(texture);
                    provider.InitSpriteEditorDataProvider();

                    var spriteRects = provider.GetSpriteRects();
                
                    foreach (var spriteRect in spriteRects)
                    {
                        spriteRectsByName.Add(spriteRect.name, spriteRect);
                    }

                    int processingIndex = 0;
                    foreach (var sprite in sprites)
                    {
                        if (spriteRectsByName.TryGetValue(sprite.name, out var spriteRect) == false)
                        {
                            continue;
                        }

                        OnProcessSprite(sprite, spriteRect, processingIndex, provider, selectedObjects);
                    
                        processingIndex++;
                    }
                
                    provider.SetSpriteRects(spriteRects);
                
                    provider.Apply();
                
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
            }
            
            int index = 0;
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not Texture2D texture)
                {
                    continue;
                }

                if (texture.TryGetAssetPath(out string path) == false)
                {
                    continue;
                }

                var provider = factory.GetSpriteEditorDataProviderFromObject(texture);
                provider.InitSpriteEditorDataProvider();
                
                OnProcessTexture(texture, provider, index, selectedObjects);
                
                provider.Apply();
                
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                
                index++;
            }
            
            return selectedObjects;
        }

        protected abstract void OnProcessSprite(Sprite sprite, SpriteRect spriteRect, int processingIndex,
            ISpriteEditorDataProvider provider, IReadOnlyList<object> selectedObjects);
        
        protected abstract void OnProcessTexture(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects);
    }
}
#endif