using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsByType<T>(FindObjectsSortMode.None);
                    _instance = objs.Length > 0 ? objs[0] : null;
                    
                    if (_instance == null)
                        Debug.LogWarning($"{typeof(T)} is not existing");
                    else if(objs.Length > 1)
                        Debug.LogWarning($"{typeof(T)} is already existing");
                }

                return _instance;
            }
        }
    }
}