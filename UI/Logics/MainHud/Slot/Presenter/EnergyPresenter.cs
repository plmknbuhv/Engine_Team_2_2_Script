using Code.UI.Logics.MainHud.Slot.Model;
using Code.UI.Logics.MainHud.Slot.View;
using EventSystem;

namespace Code.UI.Logics.MainHud.Slot.Presenter
{
    public class EnergyPresenter : BasePresenter<EnergyModel, EnergyView>
    {
        private GameEventChannelSO _costEventChannel;
        private int _currentCostAmount = 0;
        public EnergyPresenter(EnergyModel model, EnergyView view, int maxValue, GameEventChannelSO costEventChannel) : base(model, view)
        {
            Model.SetEnergy(0, maxValue);
            View.InitializeView(maxValue);
            _costEventChannel = costEventChannel;
            _costEventChannel.AddListener<ChangeCostAmountEvent>(HandleCostAmountChange);
        }

        private void HandleCostAmountChange(ChangeCostAmountEvent obj)
        {
            var changedAmount = obj.amount - _currentCostAmount;
            _currentCostAmount = obj.amount;
            
            if (changedAmount > 0)
                AddEnergy(changedAmount);
            else if (changedAmount < 0)
                ConsumeEnergy(-changedAmount);
        }

        protected override void OnInit()
        {
            base.OnInit();
            Model.OnConsumed += OnConsumed;
        }

        public override void Dispose()
        {
            base.Dispose();
            Model.OnConsumed -= OnConsumed;
            _costEventChannel.RemoveListener<ChangeCostAmountEvent>(HandleCostAmountChange);
        }
        
        public void AddEnergy(int amount)
        {
            Model.AddEnergy(amount);
        }
        
        public void ConsumeEnergy(int amount)
        {
            Model.ConsumeEnergy(amount);
        }

        protected override void OnModelChanged()
        {
            View.SetValue(Model.CurrentEnergy);
        }

        private void OnConsumed()
        {
            View.ConsumeEnergy();
        }
    }
}