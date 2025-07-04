#if FISHNET
using VMFramework.Core;

namespace VMFramework.Network
{
    public partial class UUIDCoreManager
    {
        /// <summary>
        /// 检查一致性
        /// </summary>
        /// <returns></returns>
        public bool CheckConsistency<TUUIDOwner>(TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (owner == null)
            {
                Debugger.LogWarning($"检查一致性失败，{nameof(owner)}为空");
                return false;
            }
            
            if (TryGetOwnerWithWarning(owner.UUID, out TUUIDOwner existedOwner) ==
                false)
            {
                Debugger.LogWarning($"不存在此{owner.UUID}对应的{typeof(TUUIDOwner)}");
                return false;
            }

            if (existedOwner.Equals(owner) == false)
            {
                Debugger.LogWarning($"此{owner}的UUID对应的是{existedOwner}，一致性检查失败");
                return false;
            }
            
            return true;
        }
    }
}
#endif