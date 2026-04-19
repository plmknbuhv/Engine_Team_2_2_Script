using System;
using UnityEngine;
using UnityEngine.Splines;

namespace Code.Tester
{
    public class SplineAnimateTimeTester : MonoBehaviour
    {
        [SerializeField] private SplineAnimate spline;
        [SerializeField] private Transform target;

        private void Start()
        {
            spline.NormalizedTime = 1f;
            
            Vector3 worldPos = target.position;
            var localPos = spline.Container.transform.InverseTransformPoint(worldPos);
            
            float a = SplineUtility.GetNearestPoint(spline.Container.Spline, transform.position, out var werewr, out var normalizedTime);
            print(normalizedTime + " " + a);
            Vector3 worldNearest = spline.Container.transform.TransformPoint(werewr);
            spline.NormalizedTime = normalizedTime;

            target.position = worldNearest;  
            
            spline.Play();
        }
    }
}