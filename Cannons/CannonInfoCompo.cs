using System;
using System.Collections.Generic;
using GondrLib.Dependencies;
using UnityEngine;

namespace Cannons
{
    [Provide]
    public class CannonInfoCompo : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<CannonPartData> partDataList;
        
        private Dictionary<string, Transform> cannonPartDict;

        private void Awake()
        {
             cannonPartDict = new Dictionary<string, Transform>();
             partDataList.ForEach(part =>
             {
                 if (!cannonPartDict.TryAdd(part.id, part.part))
                 {
                     Debug.LogWarning($"{part.id} has already been added");
                 }
             });
        }

        public Transform GetPart(string id)
        {
            if (!cannonPartDict.TryGetValue(id, out var part))
            {
                Debug.LogWarning($"{id} has not been added");
            }

            return part;
        }
    }

    [Serializable]
    public struct CannonPartData
    {
        public string id;
        public Transform part;
    }
}