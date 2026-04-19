using Code.Upgrades;
using Code.Upgrades.Data;

namespace Code.UI.Logics.Upgrade.Model
{
    public class UpgradeModel : BaseModel
    {
        private UpgradeDataSO[] _cards;
        public UpgradeDataSO[] Cards
        {
            get => _cards;
            set
            {
                if (_cards == value) return;
                _cards = value;
                NotifyChanged();
            }
        }
        public int RemainingUpgrades { get; set; }
    }
}