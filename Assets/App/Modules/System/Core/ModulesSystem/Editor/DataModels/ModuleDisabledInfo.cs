using System;

namespace OpenGameFramework.Editor
{
    [Serializable]
    public class ModuleDisabledInfo
    {
        public string archive_name;

        public string hash;
        public string name;
        public string version;

        public int type;
        public int state;
        public string description;
    }
}