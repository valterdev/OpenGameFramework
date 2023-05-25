using UnityEngine;
using OpenGameFramework.DI;

namespace OpenGameFramework.API
{
    [Inject(typeof(API.Domain.IMatch3BoardService))]
    public class Match3BoardService : API.Domain.IMatch3BoardService
    {
        public Match3BoardService()
        {
            
        }
        
        /// <summary>
        /// Pre-initialization function
        /// </summary>
        public void PreInit()
        {
            //App.GlobalConfig.GetConfigOfType<matchcon>()
        }

        /// <summary>
        /// Initialization function
        /// </summary>
        public void Init()
        {
            
        }

        public void Test()
        {
            Debug.Log("Run test");
        }
    }
}