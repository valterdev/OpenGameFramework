using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OpenGameFramework
{
    [CreateAssetMenu(fileName = "Module_Config", menuName = "OpenGameFramework/Module Config")]
    public class ModuleConfig : ScriptableObject
    {
        // Serializable fields
        [SerializeField] private List<AssetReferenceById> uiElementReferencesById;
        [SerializeField] private List<AssetReferenceById> assetsReferencesById;

        // Public properties
        public IReadOnlyList<AssetReferenceById> UIElementReferences => uiElementReferencesById;
        public IReadOnlyList<AssetReferenceById> AssetsReferences => assetsReferencesById;
    }

    [Serializable]
    public class AssetReferenceById
    {
        // Public fields
        public string Id;
        public AssetReference AssetReference;
    }
}
