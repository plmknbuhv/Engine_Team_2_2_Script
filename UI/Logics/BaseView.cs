using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Logics
{
    public class BaseView : MonoBehaviour
    {
        public virtual void Initialize() { }

        public virtual UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}