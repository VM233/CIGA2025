using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Animations
{
    public sealed partial class GameObjectAnimation : GamePrefab
    {
        public override string IDSuffix => "game_object_animation";

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        public List<IGameObjectAnimationClip> clips = new();

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowInInspector]
        public float TotalDuration => GetTotalDuration();
        
        private float GetTotalDuration()
        {
            if (clips.IsNullOrEmpty())
            {
                return 0;
            }

            return clips.Max(clip => clip != null ? clip.GetStartTime() + clip.GetDuration() : 0);
        }
        
        public void Run(Transform target, CancellationToken token = default)
        {
            if (IsEmpty())
            {
                Debugger.LogWarning($"{this} has no clips to play.");
                return;
            }
            
            foreach (var clip in clips)
            {
                if (clip.IsRequirementSatisfied(target) == false)
                {
                    continue;
                }
                
                clip.Start(target);
                
                if (target != null && target.gameObject.activeSelf)
                {
                    clip.Run(target, token);
                }
            }
        }
        
        public async UniTask RunAndAwait(Transform target, CancellationToken token = default)
        {
            if (IsEmpty())
            {
                Debugger.LogWarning($"{this} has no clips to play.");
                return;
            }
            
            var awaitList = new List<UniTask>();

            foreach (var clip in clips)
            {
                if (clip.IsRequirementSatisfied(target) == false)
                {
                    continue;
                }
                
                clip.Start(target);
                
                if (target != null && target.gameObject.activeSelf)
                {
                    awaitList.Add(clip.Run(target, token));
                }
            }

            if (awaitList.Count == 0)
            {
                Debugger.LogWarning($"{this} has no clips that satisfy the requirements to play.");
                return;
            }
            
            await UniTask.WhenAll(awaitList);
        }

        public void Kill(Transform target)
        {
            if (target == null)
            {
                return;
            }

            foreach (var clip in clips)
            {
                clip.Kill(target);
            }
        }
        
        public bool IsEmpty()
        {
            return clips == null || clips.Count == 0;
        }

        #region Init & Check

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            clips.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            clips.Init();
        }

        #endregion
    }
}