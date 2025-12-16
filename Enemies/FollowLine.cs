using System;
using System.Collections.Generic;
using System.Linq;
using Code.Entities;
using Code.Units.UnitAnimals.Turtles;
using Code.Units.UnitAnimals.Whales;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Enemies
{
    public class FollowLine : MonoBehaviour, IEntityComponent
    {
        private SplineAnimate _splineAnimate;
        public SplineAnimate SplineAnimate => _splineAnimate;

        public float NormalizedTime => _splineAnimate.NormalizedTime;
        
        private float _splineOffsetX;

        private bool _isMoving;
        public bool IsMoving => _isMoving;
        private bool _isReverse;

        public void Initialize(Entity _)
        {
            _splineAnimate = GetComponentInParent<SplineAnimate>();
        }

        public void SetUpFollowLine(SplineContainer spline, float splineOffsetX = 0)
        {
            _splineAnimate.Container = spline;
            _splineOffsetX = splineOffsetX;
        }

        public void SetAlign(SplineAnimate.AlignmentMode alignmentMode)
        {
            _splineAnimate.Alignment = alignmentMode;
        }
        
        //현재 스플라인 시점에서의 회전값을 반환
        public Quaternion GetLookLineRotation()
        {
            Vector3 tangent = _splineAnimate.Container.EvaluateTangent(_splineAnimate.NormalizedTime);
            Vector3 upVector = _splineAnimate.Container.EvaluateUpVector(_splineAnimate.NormalizedTime);
            return Quaternion.LookRotation(tangent, upVector);
        }

        public void SetDuration(float duration)
        {
            if (duration == 0)
                Debug.LogWarning("duration 값이 0입니다.");

            float normalizedTime = SplineAnimate.NormalizedTime;
            
            _splineAnimate.MaxSpeed = duration;
            
            SetNormalizedTime(normalizedTime);
        }

        //매프레임 마다 스플라인을 따라 움직여 OffsetX를 한 번만 설정하면 값이 덮이므로
        //스플라인 위치 + OffsetX 매프레임 마다 더하여 값이 덮이지 않도록 하였다.
        private void Update()
        {
            if (_splineAnimate == null || _isMoving == false) return;

            if (_isReverse)
            {
                int splineCount = _splineAnimate.Container.Spline.Count;
                float speedDelta = _splineAnimate.MaxSpeed * Time.deltaTime;
                float deltaNormalized = speedDelta / splineCount * (_isReverse ? -1f : 1f);
                _splineAnimate.NormalizedTime = Mathf.Clamp01(_splineAnimate.NormalizedTime + deltaNormalized);
            }
            
            //_splineAnimate.transform.position += _splineAnimate.transform.right * _splineOffsetX;
        }

        public void SetNormalizedTime(float normalizedTime)
        {
            _splineAnimate.NormalizedTime = normalizedTime;
        }
        
        public void PlayMove(bool isReverse, float normalizedTime = -1)
        {
            if (_splineAnimate.Container == null)
                return;

            _isReverse = isReverse;
            
            if(Mathf.Approximately(normalizedTime, -1))
                _splineAnimate.NormalizedTime = isReverse ? 1 : 0;
            else
                _splineAnimate.NormalizedTime = normalizedTime;
            
            _splineAnimate.Loop = isReverse ? SplineAnimate.LoopMode.PingPong : SplineAnimate.LoopMode.Loop;

            _splineAnimate.Play();
            _isMoving = true;
        }

        public void PlayMove(int splineIndex, bool isReverse, float normalizedTime)
        {
            if (_splineAnimate.Container == null)
                return;

            SplineContainer container = _splineAnimate.Container;
            IReadOnlyList<Spline> splines = container.Splines;

            float4x4 localToWorld = container.transform.localToWorldMatrix;

            float totalLength = 0f;
            for (int i = 0; i < splines.Count; i++)
                totalLength += splines[i].CalculateLength(localToWorld);

            float previousLength = 0f;
            for (int i = 0; i < splineIndex; i++)
                previousLength += splines[i].CalculateLength(localToWorld);

            float currentLength = splines[splineIndex].CalculateLength(localToWorld);

            float globalNormalizedTime = (previousLength + currentLength * normalizedTime) / totalLength;

            PlayMove(isReverse, globalNormalizedTime);
        }

        [ContextMenu("Play Move")]
        public void PlayMove()
        {
            if (_splineAnimate.Container == null)
                return;
            
            _splineAnimate.Play();
            _isMoving = true;
        }

        [ContextMenu("Pause Move")]
        public void PauseMove()
        {
            if (_splineAnimate.Container == null)
                return;

            _splineAnimate.Pause();
            _isMoving = false;
        }

        public (Vector3, float) GetNearestInfo(Vector3 targetPos)
        {
            Spline spline = _splineAnimate.Container.Spline;
            Vector3 localPos = _splineAnimate.Container.transform.InverseTransformPoint(targetPos);

            SplineUtility.GetNearestPoint(spline, localPos, out var nearest, out float t);
            
            _splineOffsetX = nearest.x - localPos.x;
            
            SplineContainer container = _splineAnimate.Container;
            container.Evaluate(t, out float3 position, out float3 tangent, out float3 up);

            Vector3 right = Vector3.Cross(up, tangent).normalized;

            Vector3 targetLocalPos = nearest;
            targetLocalPos += new Vector3(right.x * _splineOffsetX, 0, 0);
            
            Vector3 nearestPos = _splineAnimate.Container.transform.TransformPoint(targetLocalPos);

            return (nearestPos, t);
        }
        
        public void SetSplineOffsetX(float offsetX) => _splineOffsetX = offsetX;
    }
}