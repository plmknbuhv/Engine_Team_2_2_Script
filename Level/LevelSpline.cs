using System.Collections.Generic;
using GondrLib.Dependencies;
using UnityEngine;
using UnityEngine.Splines;

namespace Level
{
    [Provide]
    public class LevelSpline : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<SplineContainer> splines;
        public List<SplineContainer> Splines
        {
            get
            {
                if (splines.Count == 0)
                {
                    Debug.LogWarning("Splines 리스트가 비었습니다.");
                    return null;
                }
                
                return splines;
            }
        }
    }
}