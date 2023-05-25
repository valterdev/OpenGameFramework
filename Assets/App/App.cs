using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using OpenGameFramework.API;
using OpenGameFramework.DI;
using UnityEngine;

namespace OpenGameFramework
{
    public partial class App : MonoBehaviour
    {
        #region Fields

        private EasyDiContainer _diContainer;

        #endregion
        
        
        #region System API

        // ---------------------------------------------------------------------------------------------------------
        // public properties (static)
        // ---------------------------------------------------------------------------------------------------------

        public static App Instance { get; private set; }

        public static APIHooksContainer Hooks { get; private set; }

        // ---------------------------------------------------------------------------------------------------------
        // public properties 
        // ---------------------------------------------------------------------------------------------------------

        public MainLoop MainLoop { get; }

        // ---------------------------------------------------------------------------------------------------------
        // Methods
        // ---------------------------------------------------------------------------------------------------------

        public static UniTask Do(IAppTask appTask)
        {
            return appTask.Run();
        }


        public static T Do<T>() where T : IAppTask
        {
            return (T)Activator.CreateInstance(typeof(T));
        }


        private static void CleanUpMemory()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        #endregion

        #region Object lifecycle

        private App()
        {
            Instance = this;
            Hooks = new APIHooksContainer();
            MainLoop = new MainLoop();

            _diContainer = new EasyDiContainer();
        }

        public void CoreInit()
        {
            AssetsManager.Instance().PreInit();
            GlobalConfigManager.Instance().PreInit();
            
            _diContainer.PreInit();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
             DontDestroyOnLoad(this);

             App.Instance.CoreInit();
             App.Hooks.OnStart += _diContainer.Init;

             try
             {
                 Do(new PreInitAppTask());
             }
             catch (Exception e)
             {
                 OnErrorPre(e);
                 throw;
             }
        }

        private IEnumerator Start()
        {
            while (MainLoop.CurState < MainLoop.State.Init)
            {
                yield return new WaitForEndOfFrame();
            }

            try
            {
                Do(new InitAppTask());
            }
            catch (Exception e)
            {
                OnError(e);
                throw;
            }
            
        }

        #endregion

        private void OnErrorPre(Exception e)
        {
            LogException(typeof(PreInitAppTask), e);
        }

        private void OnError(Exception e)
        {
            LogException(typeof(InitAppTask), e);
        }
    }
}