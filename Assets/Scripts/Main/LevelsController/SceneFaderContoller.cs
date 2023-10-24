using UnityEngine;
using UnityEngine.UI;


    [RequireComponent(typeof(CanvasGroup))]
    public class SceneFaderContoller : MonoBehaviour
    {
        [SerializeField]
        private BaseSceneFaderAnimation iosFader;
        [SerializeField]
        private BaseSceneFaderAnimation androidFader;

        private ISceneFaderAnimation fader;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_ANDROID
            fader = androidFader.GetComponent<ISceneFaderAnimation>();
#elif UNITY_IOS
            fader = iosFader.GetComponent<ISceneFaderAnimation>();
#else
            fader = GetComponentInChildren<ISceneFaderAnimation>(true);
#endif
        }

        private void Start()
        {
            ShowImmediately();
            GameController.Instance.OnShowFadeUI += GameC_OnLevelStartLoading;
            GameController.Instance.OnLevelLoaded += GameC_OnLevelLoaded;
        }

        private void GameC_OnLevelStartLoading(int levelNumber)
        {
            Show(levelNumber);
        }

        private void GameC_OnLevelLoaded(int levelNumber)
        {
            Hide();
        }

        public void Show(int levelNumber)
        {
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.Show(Help);
            canvasGroup.blocksRaycasts = true;
            void Help()
            {
                GameController.Instance.InvokeOnStartLevelLoading(levelNumber);
            }
        }

        public void Hide()
        {
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.Hide(Help);
            void Help()
            {
                canvasGroup.blocksRaycasts = false;
            }
        }

        public void ShowImmediately()
        {
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.ShowImmediately();
            canvasGroup.blocksRaycasts = true;
        }

        public void HideImmediately()
        {
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.HideImmediately();
            canvasGroup.blocksRaycasts = false;
        }
    }

