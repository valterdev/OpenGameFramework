using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

namespace OpenGameFramework
{
    public class AssetsManager : SingletonCrossScene<AssetsManager>
    {
        #region Fields
        // ---------------------------------------------------------------------------------------------------------
        // Protected fields
        // ---------------------------------------------------------------------------------------------------------
        protected Dictionary<string, AssetReference> availableAssets = new();

        // ---------------------------------------------------------------------------------------------------------
        // Private fields
        // ---------------------------------------------------------------------------------------------------------
        private Dictionary<string, GameObject> loadedAssets = new();
        private Transform appRoot;
        #endregion

        /// <summary>
        /// Pre-initialization function
        /// </summary>
        public void PreInit()
        {
            RegisterStaticObject();
            App.Hooks.OnLoadData += LoadInfoAboutAssets;
        }

        /// <summary>
        /// Registers (creates and initializes) a global static variable so that we can access the manager from any part of the code.
        /// The singleton pattern (thread-safe) is implemented.
        /// </summary>
        public void RegisterStaticObject()
        {
            App.AssetsManager = AssetsManager.Instance();
        }

        #region Methods
        // ---------------------------------------------------------------------------------------------------------
        // Public Methods
        // ---------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Synchronous retrieval of a file via a path (Addresable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Addresable Path</param>
        /// <returns></returns>
        public T GetAssetByPath<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        }

        /// <summary>
        /// Synchronous retrieval of a file via a id (in config files)
        /// </summary>
        /// <param name="id">id in config of module</param>
        /// <returns>Instantiated Gameobject on the scene</returns>
        /// <exception cref="Exception"></exception>
        public GameObject Get(string id)
        {
            if (loadedAssets.ContainsKey(id) == false)
            {
                if (availableAssets.TryGetValue(id, out AssetReference assetReference))
                {
                    if (appRoot == null)
                    {
                        appRoot = GameObject.Find("/App").transform;
                    }

                    GameObject assetPrefab = assetReference.LoadAssetAsync<GameObject>().WaitForCompletion();
                    GameObject assetObject = Instantiate(assetPrefab, appRoot);

                    loadedAssets.Add(id, assetObject);
                } else
                {
                    throw new Exception("Asset '" + id + "' doesn't found");
                }
            }

            return loadedAssets[id];
        }

        // ---------------------------------------------------------------------------------------------------------
        // Private Methods
        // ---------------------------------------------------------------------------------------------------------

        private void LoadInfoAboutAssets()
        {
            foreach (ModuleConfig moduleConfig in App.GlobalConfig.ModuleConfigs)
            {
                if (moduleConfig.AssetsReferences != null)
                {
                    foreach (var referenceById in moduleConfig.AssetsReferences)
                    {
                        availableAssets.Add(referenceById.Id, referenceById.AssetReference);
                    }
                }
            }
        }
        #endregion
    }
}