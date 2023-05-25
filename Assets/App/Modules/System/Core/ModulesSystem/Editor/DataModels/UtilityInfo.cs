using System.Collections.Generic;
using System;

namespace OpenGameFramework.Editor
{
    [Serializable]
    public class UtilityInfo
    {
        #region Fields

        // Public Fields
        public string hash;
        public bool is_editor;

        public string name;
        public string description;
        public List<string> files;

        #endregion
    }
}