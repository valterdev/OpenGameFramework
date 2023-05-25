using Cysharp.Threading.Tasks;
using UnityEngine;

namespace OpenGameFramework
{
    public class PreInitAppTask : IAppTask
    {
        public async UniTask Run()
        {
            // some code for pre init
            await App.Do<LoadAppTask>().Run();
            App.Instance.MainLoop.NextInit();
        }
    }
}