using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Visual
{
    [RequireComponent(typeof(UIElement))]
    public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
    {
        private UIElement target;
        private void Awake()
        {
            target = GetComponent<UIElement>();
        }
        
        public void OnPointerEnter(PointerEventData eventData) => target.AddState("hover", 5).Forget();

        public void OnPointerExit(PointerEventData eventData) => target.RemoveState("hover").Forget();

        public void OnSelect(BaseEventData eventData) => target.AddState("selected", 7).Forget();

        public void OnDeselect(BaseEventData eventData) => target.RemoveState("selected").Forget();
        public void OnPointerClick(PointerEventData eventData)=>target.PlayFeedback("click");
    }
}