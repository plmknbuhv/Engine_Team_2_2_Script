using Cannons.StatSystem;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.CostSystem
{
    [Provide]
    public class CostManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private GameEventChannelSO costChannel;
        [SerializeField] private int maxCostCount = 15;
        [SerializeField] private int addCostCount = 1;

        private int _cost;
        public int Cost
        {
            get => _cost;
            set
            {
                _cost = Mathf.Clamp(value, 0, maxCostCount);
                var evt = CostEvents.ChangeCostAmountEvent.Initializer(_cost);
                costChannel.RaiseEvent(evt);
            }
        }

        private float _nextAddTime;

        [Inject] private CannonStatCompo _statCompo;

        private void Start()
        {
            Cost = 0;
            _nextAddTime = Time.time + _statCompo.GetStatValue(CannonStatType.ADD_COST_SPEED);
        }

        private void Update()
        {
            if (Time.time > _nextAddTime)
            {
                AddCost(addCostCount);
                _nextAddTime += _statCompo.GetStatValue(CannonStatType.ADD_COST_SPEED);
            }
        }

        public void AddCost(int amount)
        {
            Cost += amount;
        }
        
        public void SubCost(int amount)
        {
            if (!CheckUseCost(amount))
            {
                return;
            }
            
            Cost -= amount;
        }

        /// <summary>
        /// 필요한 코스트만큼 현재 코스트가 있는지 확인
        /// </summary>
        /// <param name="amount">필요한 코스트</param>
        public bool CheckUseCost(int amount)
        {
            bool canUseCost = Cost >= amount;

            if (!canUseCost)
            {
                var evt = CostEvents.FailUseCostEvent.Initializer(amount);
                costChannel.RaiseEvent(evt);
            }
            
            return canUseCost;
        }
        
        
    }
}