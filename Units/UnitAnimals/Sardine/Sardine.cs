using System;
using System.Collections.Generic;
using System.Linq;
using Code.Units.UnitStat;
using Code.Util;
using UnityEngine;

namespace Code.Units.UnitAnimals.Sardine
{
    public class Sardine : WaveRideableUnit
    {
        public int LinkCount { get; private set; }

        public int MaxLinkCount
        {
            get
            {
                int maxLinkCnt = Math.Clamp(_maxLinkCnt, _sardineData.defaultLinkCnt, _sardineData.maxLinkCount);
                return maxLinkCnt;
            }
        }

        [SerializeField] private UnitDetector unitDetector;

        private HashSet<Sardine> _nearbySardines = new HashSet<Sardine>();
        private readonly HashSet<Sardine> _visitedSardines = new HashSet<Sardine>();
        private readonly HashSet<Sardine> _linkedSardines = new HashSet<Sardine>();

        private SardineDataSO _sardineData;
        private UnitStatCompo _statCompo;
        private ColorMaskComponent _colorMaskCompo;
        private Vector3 _originScale;
        private bool _isSetUp;
        private int _maxLinkCnt;

        protected override void Awake()
        {
            base.Awake();

            _originScale = transform.localScale;

            _sardineData = UnitData as SardineDataSO;
            _maxLinkCnt = _sardineData!.defaultLinkCnt;

            _statCompo = GetCompo<UnitStatCompo>();
            _colorMaskCompo = GetCompo<ColorMaskComponent>();

            OnDeadEvent.AddListener(HandleDead);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnDeadEvent.RemoveListener(HandleDead);
        }

        public override void SetUpUnit()
        {
            base.SetUpUnit();

            _isSetUp = true;
        }

        protected override void Update()
        {
            base.Update();

            if (IsDead || _isSetUp == false || IsRideWave) return;

            unitDetector.SetTarget(UnitTeamType.Friendly);

            if (unitDetector.TryGetUnits(out HashSet<Sardine> sardines, _sardineData.sardineDetectRange))
            {
                _nearbySardines = sardines;

                int beforeLinkCount = LinkCount;

                FindTotalSardines();

                if (beforeLinkCount != LinkCount)
                {
                    ChangeStat(LinkCount);
                }
            }
            else if (LinkCount != 0)
            {
                ChangeStat(0);
            }
        }

        private void ChangeStat(int linkCnt)
        {
            foreach (StatAdder statAdder in _sardineData.statAdders)
            {
                if (_statCompo.TryGetStat(statAdder.statType, out Stat stat))
                {
                    stat.RemoveModifier(this);
                    
                    float value = statAdder.value * linkCnt;
                    
                    stat.AddModifier(this, value);
                }
            }

            transform.localScale = _originScale + _sardineData.scaleAdder * linkCnt;

            int colorIdx = Mathf.Clamp(linkCnt - 1, 0, MaxLinkCount - 1);

            _colorMaskCompo.SetColor(_sardineData.linkColors[colorIdx]);
        }

        private void HandleDead()
        {
            _nearbySardines.Clear();
            _isSetUp = false;

            ChangeStat(0);
        }

        private void FindTotalSardines()
        {
            LinkCount = 0;
            _visitedSardines.Clear();

            Stack<Sardine> stack = new Stack<Sardine>();

            foreach (Sardine sardine in _nearbySardines)
            {
                if (sardine != null && !sardine.IsDead && _visitedSardines.Add(sardine))
                {
                    stack.Push(sardine);
                }
            }

            while (stack.Count > 0)
            {
                Sardine target = stack.Pop();
                LinkCount = Mathf.Clamp(LinkCount + 1, 0, MaxLinkCount);

                foreach (Sardine sardine in target._nearbySardines)
                {
                    if (sardine != null && !sardine.IsDead && _visitedSardines.Add(sardine))
                    {
                        stack.Push(sardine);
                    }
                }
            }

            if (_linkedSardines.SetEquals(_visitedSardines) == false)
            {
                _linkedSardines.Clear();

                foreach (Sardine linkedSardine in _visitedSardines)
                    _linkedSardines.Add(linkedSardine);
            }
        }

        public void GroupAttack()
        {
            foreach (Sardine linkedSardine in _linkedSardines)
            {
                Unit target = linkedSardine.TargetUnit;
                bool isCombat = linkedSardine.IsInBattle || (target != null && target.IsDead == false);

                if (isCombat || linkedSardine.IsDead) continue;

                linkedSardine.TargetUnit = TargetUnit;
                linkedSardine.ChangeState("CHASE");
            }
        }
    }
}