using System.Collections;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenGameFramework
{
    public class SaveAppTask : IAppTask
    {
        #region Fields

        // ---------------------------------------------------------------------------------------------------------
        // Public fields (static)
        // ---------------------------------------------------------------------------------------------------------
        
        public static string LOCAL_DATA_SAVE_KEY = "app_data";

        private bool localSaveOnly;

        #endregion

        #region Object lifecycle

        public SaveAppTask(bool localSaveOnly = false)
        {
            this.localSaveOnly = localSaveOnly;
        }

        #endregion

        #region Methods

        public UniTask Run()
        {
            App.Hooks.OnSaveData.Invoke();
            // PlayerPrefs.SetString(LOCAL_DATA_SAVE_KEY, App.GStore.GetJsonAllData());
            // PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }

        #endregion
    }
}
