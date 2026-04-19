using Code.UI.Visual;
using Code.Units.UnitDatas;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.MainHud.Slot
{
    public class DescriptionController : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private UIElement gradeElement;

        private string _costTextFormat;
        private string _gradeStyle;

        private void Awake()
        {
            _costTextFormat = costText.text;
        }

        public void SetUpDescription(UnitDataSO data)
        {
            gradeText.text = $"[{data.classType.ToString()}]";
            var newGrade = data.classType.ToString().ToLower();
            if (_gradeStyle != newGrade)
            {
                if (!string.IsNullOrEmpty(_gradeStyle))
                    gradeElement.RemoveState(_gradeStyle).Forget();
                _gradeStyle = data.classType.ToString().ToLower();
                gradeElement.AddState(_gradeStyle, 1).Forget();
            }
            icon.sprite = data.icon;
            costText.text = string.Format(_costTextFormat, data.requiredCost);
            nameText.text = data.unitName;
            descriptionText.text = data.description;
        }
    }
}