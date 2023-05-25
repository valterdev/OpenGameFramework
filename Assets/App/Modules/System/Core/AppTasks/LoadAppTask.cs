using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace OpenGameFramework
{
    public class LoadAppTask : IAppTask
    {
        public UniTask Run()
        {
            // From Local Save (Sample)
            // var jsonData = PlayerPrefs.GetString(SaveAppTask.LOCAL_DATA_SAVE_KEY);
            // if (!string.IsNullOrEmpty(jsonData))
            // {
            //     try
            //     {
            //         App.GStore.ImportFromJson(jsonData);
            //         App.GStore.ProcessLoadedData();
            //     }
            //     catch (Exception e)
            //     {
            //         App.LogException(this, e);
            //     }
            // }
            
            return UniTask.CompletedTask;
        }
    }
}