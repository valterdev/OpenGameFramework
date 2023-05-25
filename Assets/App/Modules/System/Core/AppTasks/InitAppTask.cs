using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OpenGameFramework
{
    public class InitAppTask : IAppTask
    {
        #region Methods

        // ---------------------------------------------------------------------------------------------------------
        // Public Methods (override)
        // ---------------------------------------------------------------------------------------------------------

        public async UniTask Run()
        {
            App.Hooks.OnBeforeStart.Invoke();
            await ImportUI("UI", "/UILayer");
            App.Hooks.OnStart.Invoke();
            App.Instance.MainLoop.NextInit();
        }
        
        private async UniTask ImportUI(string sceneName, string rootName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var root = GameObject.Find(rootName);
            
            root.GetComponent<Canvas>().worldCamera = Camera.main;
            root.transform.SetParent(GameObject.Find("/App/UI").transform);

            await SceneManager.UnloadSceneAsync(sceneName);
        }

        #endregion
    }
}