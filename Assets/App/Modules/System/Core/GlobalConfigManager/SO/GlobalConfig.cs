using System.Collections.Generic;
using UnityEngine;

namespace OpenGameFramework
{
    [CreateAssetMenu(fileName = "Global_Config", menuName = "OpenGameFramework/Global Config")]
    public class GlobalConfig : ScriptableObject
    {
        #region Fields

        // ---------------------------------------------------------------------------------------------------------
        // Serializable fields
        // ---------------------------------------------------------------------------------------------------------

        [SerializeField] private List<ModuleConfig> moduleConfigs;

        // ---------------------------------------------------------------------------------------------------------
        // Public properties
        // ---------------------------------------------------------------------------------------------------------

        public List<ModuleConfig> ModuleConfigs => moduleConfigs;

        #endregion

        #region Methods

        // ---------------------------------------------------------------------------------------------------------
        // Public Methods
        // ---------------------------------------------------------------------------------------------------------

        public T GetConfigOfType<T>() where T : ModuleConfig
        {
            return moduleConfigs.Find(config => config is T) as T;
        }

        #endregion
    }
}
