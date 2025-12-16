using Code.UI.Logics.StateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.Logics.Result
{
    public class ResultController : MonoBehaviour, IUIState
    {
        [field: SerializeField] public UIStateType StateType { get; private set; } = UIStateType.Result;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private GameEventChannelSO waveChannel;
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private int mainGameSceneIndex;
        [Header("Effects")] [SerializeField] private UIElement canvasElement;
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private Transform showPosition;
        [SerializeField] private Transform hidePosition;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.OutBack;

        [Header("Result Objects")] [SerializeField]
        private GameObject[] loseObjects;

        [SerializeField] private GameObject[] winObjects;
        [Header("Reference")] [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private string winText;
        [SerializeField] private string loseText;
        [SerializeField] private TextMeshProUGUI subResultText;
        [SerializeField] private string winSubText;
        [SerializeField] private string loseSubText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private const string winKey = "win";
        private bool _isWin;

        private void Awake()
        {
            restartButton.onClick.AddListener(HandleRestart);
            exitButton.onClick.AddListener(HandleExit);
            waveChannel.AddListener<AllWaveCompleteEvent>(HandleAllWaveComplete);
            cannonChannel.AddListener<CannonDeathEvent>(HandleCannonDeath);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(HandleRestart);
            exitButton.onClick.RemoveListener(HandleExit);
            waveChannel.RemoveListener<AllWaveCompleteEvent>(HandleAllWaveComplete);
            cannonChannel.RemoveListener<CannonDeathEvent>(HandleCannonDeath);
        }

        private void HandleCannonDeath(CannonDeathEvent obj)
        {
            _isWin = false;
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Result));
        }

        private void HandleAllWaveComplete(AllWaveCompleteEvent obj)
        {
            Debug.Log("All waves complete - player wins!");
            _isWin = true;
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Result));
        }

        private void HandleRestart()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(false));
            uiChannel.AddListener<UIFadeCompleteEvent>(HandleFadeCompleteRestart);
        }

        private async void HandleFadeCompleteRestart(UIFadeCompleteEvent obj)
        {
            uiChannel.RemoveListener<UIFadeCompleteEvent>(HandleFadeCompleteRestart);
            graphicRaycaster.enabled = false;
            await SceneManager.LoadSceneAsync(mainGameSceneIndex);
        }

        private void HandleExit() => Application.Quit();

        public async UniTask OnEnter()
        {
            Time.timeScale = 0f;
            foreach (var obj in winObjects)
            {
                resultText.text = _isWin ? winText : loseText;
                subResultText.text = _isWin ? winSubText : loseSubText;
                obj.SetActive(_isWin);
                if (_isWin)
                    canvasElement.AddState(winKey).Forget();
                else
                    canvasElement.RemoveState(winKey, false).Forget(); 
            }

            foreach (var obj in loseObjects)
            {
                obj.SetActive(!_isWin);
            }

            canvas.enabled = true;
            await MoveCanvas(showPosition.position);
            graphicRaycaster.enabled = true;
        }

        public async UniTask OnExit()
        {
            Time.timeScale = 1f;
            graphicRaycaster.enabled = false;
            await MoveCanvas(hidePosition.position);
            canvas.enabled = false;
        }

        private async UniTask MoveCanvas(Vector3 targetPos)
        {
            canvasTransform.DOMove(targetPos, moveDuration).SetEase(moveEase).SetUpdate(true);
            await UniTask.WaitForSeconds(moveDuration, true);
        }
    }
}