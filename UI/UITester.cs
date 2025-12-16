using Code.UI.Logics;
using EventSystem;
using UnityEngine;

namespace Code.UI
{
    public class UITester : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private UIStateType testStateType;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(testStateType));
            }
        }
    }
}