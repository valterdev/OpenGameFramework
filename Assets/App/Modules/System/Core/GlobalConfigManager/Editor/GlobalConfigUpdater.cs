using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace OpenGameFramework
{
    [InitializeOnLoadAttribute]
    public class GlobalConfigUpdater : MonoBehaviour, IPreprocessBuildWithReport
    {
        // Public fields
        public int callbackOrder => 0;



        #region Editor lifecycle

        static GlobalConfigUpdater()
        {
            EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
        }


        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateGlobalConfig();
        }

        #endregion



        #region Private Methods

        private static void UpdateGlobalConfig()
        {
            GlobalConfig globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>(GlobalConfigManager.GlobalConfigFilePath);
            globalConfig.ModuleConfigs.Clear();

            string[] configGuids = AssetDatabase.FindAssets($"t: {nameof(ModuleConfig)}");

            foreach (string configGuid in configGuids)
            {
                ModuleConfig config = AssetDatabase.LoadAssetAtPath<ModuleConfig>(AssetDatabase.GUIDToAssetPath(configGuid));
                globalConfig.ModuleConfigs.Add(config);
            }

            AssetDatabase.SaveAssets();
        }

        #endregion



        #region Event handlers

        private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange == PlayModeStateChange.ExitingEditMode)
            {
                UpdateGlobalConfig();
            }
        }

        #endregion
    }
}
