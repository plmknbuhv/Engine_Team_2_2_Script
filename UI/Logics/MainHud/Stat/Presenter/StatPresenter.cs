using Code.UI.Logics.MainHud.Stat.Model;
using Code.UI.Logics.MainHud.Stat.View;
using EventSystem;

namespace Code.UI.Logics.MainHud.Stat.Presenter
{
    public class StatPresenter : BasePresenter<StatModel, StatView>
    {
        private GameEventChannelSO _costChannel;
        private GameEventChannelSO _levelChannel;
        private GameEventChannelSO _cannonChannel;


        public StatPresenter(StatModel model, StatView view, int maxEnergy, int requiredExp,
            GameEventChannelSO costChannel,
            GameEventChannelSO levelChannel, GameEventChannelSO cannonChannel) : base(
            model, view)
        {
            _costChannel = costChannel;
            _levelChannel = levelChannel;
            _cannonChannel = cannonChannel;

            Model.MaxEnergy = maxEnergy;
            Model.SetMaxExp(requiredExp);
            Model.SetMaxHealth(30);
            Model.CurrentHealth = Model.MaxHealth;

            View.SetEnergy(Model.CurrentEnergy, Model.MaxEnergy);
            _costChannel.AddListener<ChangeCostAmountEvent>(OnChangeCostAmount);
            _levelChannel.AddListener<ChangeExpEvent>(OnChangeExp);
            _levelChannel.AddListener<ChangeRequiredExpEvent>(OnChangeRequiredExp);
            _levelChannel.AddListener<ChangeLevelEvent>(OnChangeLevel);
            _cannonChannel.AddListener<CannonChangeHealthEvent>(HandleCannonChangeHealth);
        }


        public override void Dispose()
        {
            base.Dispose();
            _costChannel.RemoveListener<ChangeCostAmountEvent>(OnChangeCostAmount);
            _levelChannel.RemoveListener<ChangeExpEvent>(OnChangeExp);
            _levelChannel.RemoveListener<ChangeRequiredExpEvent>(OnChangeRequiredExp);
            _levelChannel.RemoveListener<ChangeLevelEvent>(OnChangeLevel);
            _cannonChannel.RemoveListener<CannonChangeHealthEvent>(HandleCannonChangeHealth);
        }

        private void HandleCannonChangeHealth(CannonChangeHealthEvent obj) => Model.SetHealth(obj.health);

        private void OnChangeCostAmount(ChangeCostAmountEvent obj) => Model.SetEnergy(obj.amount);
        private void OnChangeExp(ChangeExpEvent obj) => Model.SetExp(obj.currentExp);

        private void OnChangeRequiredExp(ChangeRequiredExpEvent obj) => Model.SetMaxExp(obj.requiredExp);

        private void OnChangeLevel(ChangeLevelEvent obj) => Model.SetLevel(obj.currentLevel);

        protected override void OnModelChanged()
        {
            View.SetExpFill(Model.CurrentExp, Model.MaxExp);
            View.SetLevel(Model.CurrentLevel);
            View.SetHealth(Model.CurrentHealth, Model.MaxHealth);
            View.SetEnergy(Model.CurrentEnergy, Model.MaxEnergy);
        }
    }
}