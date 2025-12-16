using System.Collections;
using EventSystem;
using Settings.InputSystem;
using TMPro;
using UnityEngine;

namespace Code.Tutorials
{
    public class TutorialViewer : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO tutorialChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private TutorialStep[] tutorialParts;
        [SerializeField] private UseInputType useInputType;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        private int _currentPartIndex;
        private TutorialStep _currentStep;
        private Coroutine _writeTextCoroutine;
        private bool _canUseNextStep;

        private void Awake()
        {
            tutorialChannel.AddListener<NextStepTutorialEvent>(HandleNextStep);
            tutorialChannel.AddListener<ViewTutorialMessageEvent>(HandleMessageView);
        }

        private void OnDestroy()
        {
            tutorialChannel.RemoveListener<NextStepTutorialEvent>(HandleNextStep);
            tutorialChannel.RemoveListener<ViewTutorialMessageEvent>(HandleMessageView);
        }

        private void Start()
        {
            playerInput.SetInputEnabled(false);
            playerInput.SetEnableInputAction(false, useInputType);
            
            _currentStep = null;
            _currentPartIndex = 0;

            _canUseNextStep = true;
            NextTutorial();
        }

        private void Update()
        {
            _currentStep?.UpdateStep();
        }

        private void HandleMessageView(ViewTutorialMessageEvent evt)
        {
            if (_writeTextCoroutine != null)
            {
                StopCoroutine(_writeTextCoroutine);
                _writeTextCoroutine = null;
            }
            
            _writeTextCoroutine = StartCoroutine(WriteTextCoroutine(evt.message));
        }

        private void HandleNextStep(NextStepTutorialEvent evt)
        {
            NextTutorial();
        }

        private void NextTutorial()
        {
            if (!_canUseNextStep) return;
            
            _currentStep?.End();
            
            if (_currentPartIndex >= tutorialParts.Length)
                return;
            
            _currentStep = tutorialParts[_currentPartIndex++]; 
            _currentStep.Enter();

            _canUseNextStep = false;
        }

        IEnumerator WriteTextCoroutine(string message)
        {
            int maxCount = message.Length;
            
            descriptionText.SetText("");
            
            for (int i = 0; i < maxCount; i++)
            {
                descriptionText.SetText($"{descriptionText.text}{message[i]}");
                
                yield return new WaitForSeconds(0.025f);
            }

            descriptionText.SetText(message);

            _canUseNextStep = true;
        }
    }
}