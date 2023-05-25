using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenGameFramework
{
    public partial class App
    {
        #region Unity lifecycle

        void OnApplicationPause(bool state)
		{
			// if (state)
			// {
			// 	if (!App.GStore.Get<bool>("NoSave"))
			// 	{
			// 		App.Do(new SaveAppTask());
			// 	}
			// 	
			// }
		}

		void OnApplicationQuit()
		{
			// if (!App.GStore.Get<bool>("NoSave"))
			// {
			// 	App.Do(new SaveAppTask());
			// }
		}

        #endregion
    }
}
