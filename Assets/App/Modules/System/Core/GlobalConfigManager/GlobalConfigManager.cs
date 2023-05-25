using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OpenGameFramework
{
    public class GlobalConfigManager : SingletonCrossScene<GlobalConfigManager>
    {
        // Constants
        public const string GlobalConfigFilePath = "Assets/App/Modules/System/Config/Global_Config.asset";

        #region App lifecycle

        /// <summary>
        /// Pre-initialization function
        /// </summary>
        public void PreInit()
        {
            RegisterStaticObject();
        }


        /// <summary>
        /// Registers (creates and initializes) a global static variable so that we can access the manager from any part of the code.
        /// The singleton pattern (thread-safe) is implemented.
        /// </summary>
        private void RegisterStaticObject()
        {
            App.GlobalConfig = App.AssetsManager.GetAssetByPath<GlobalConfig>("Assets/App/Modules/System/Config/Global_Config.asset");
        }

        #endregion
    }
}
