using System;
using System.Collections.Generic;

namespace OpenGameFramework.Editor
{
    [Serializable]
    public class ProjectInfo
    {
        #region Fields

        // Public Fields
        public string hash;
        public string name;

        public string config_for_generate;

        public List<string> plugins = new List<string>();
        public List<UtilityInfo> utils = new List<UtilityInfo>();
        public List<ModuleDisabledInfo> not_active_modules = new List<ModuleDisabledInfo>();
        public List<DisabledHooksInfo> disabled_hooks = new List<DisabledHooksInfo>();

        #endregion
    }
}