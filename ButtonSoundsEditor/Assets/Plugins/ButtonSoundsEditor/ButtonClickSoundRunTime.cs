using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using System;

namespace Assets.Plugins.ButtonSoundsEditor
{
    // Put this on your AudioSource for the buttons
    public class ButtonClickSoundRunTime : MonoBehaviour
    {
        #region SINGLETON
        public static ButtonClickSoundRunTime instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        #endregion

        private List<GameObject> _candidates = new List<GameObject>();
        [SerializeField] private AudioSource audioSourceRunTime;
        [SerializeField] private AudioClip audioClipRunTime;

        private void Start()
        {
            if (audioSourceRunTime == null)
            {
                audioSourceRunTime.GetComponent<AudioSource>();
                if (audioSourceRunTime == null)
                    Debug.LogError("AudioSource null. Please put the ButtonClickSoundRunTime component on your AudioSource GameObject.");
            }
            if (audioClipRunTime == null)
                Debug.LogError("AudioClip null. Set it in the inspector.");

            RefreshButtons();
        }

        #region EVENTS
        public event Action<GameObject> onAddButton;
        public event Action<GameObject, bool> onRemoveButton;
        public event Action onRefreshButtons;

        /// <summary>
        /// Add a GameObject with a Button, Toggle or EventSystem component to the list of GameObject with click sounds.
        /// </summary>
        public void AddButton(GameObject gameObjectToAdd)
        {
            if (gameObjectToAdd.GetComponent<Button>() || gameObjectToAdd.GetComponent<Toggle>() || gameObjectToAdd.GetComponent<EventTrigger>())
            {
                if (onAddButton != null)
                    onAddButton(gameObjectToAdd);

                Debug.Log("Button added");
            }
            else
                Debug.LogError("GameObject not valid. Must have a Button, Toggle or EventSystem component on it.");
        }

        /// <summary>
        /// Remove a GameObject with a Button, Toggle or EventSystem component from the list of GameObject with click sounds.
        /// </summary>
        public void RemoveButton(GameObject gameObjectToRemove, bool removeButtonClickSoundComponent)
        {
            if (!_candidates.Contains(gameObjectToRemove))
            {
                Debug.LogError("GameObject not present in the List. Can't be removed.");
                return;
            }

            if (onRemoveButton != null)
                onRemoveButton(gameObjectToRemove, removeButtonClickSoundComponent);

            Debug.Log("Button removed");
        }

        /// <summary>
        /// Refresh the list of GameObjects with click sounds. Might take some time depending on the scene element number.
        /// </summary>
        public void RefreshButtons()
        {
            if (onRefreshButtons != null)
                onRefreshButtons();

            Debug.Log("Buttons refreshed");
        }

        private void OnEnable()
        {
            onAddButton += AddButtonToList;
            onRemoveButton += RemoveButtonFromList;
            onRefreshButtons += RefreshButtonsList;
        }

        private void OnDisable()
        {
            onAddButton -= AddButtonToList;
            onRemoveButton -= RemoveButtonFromList;
            onRefreshButtons -= RefreshButtonsList;
        }
        #endregion

        #region RUN-TIME FUNCTIONS
        private void AddButtonToList(GameObject gameObjectToAdd)
        {
            SetDatasButtonClickSoundComponent(GetButtonClickSoundComponent(gameObjectToAdd));
            _candidates.Add(gameObjectToAdd);
        }

        private void RemoveButtonFromList(GameObject gameObjectToRemove, bool removeButtonClickSoundComponent)
        {
            if (removeButtonClickSoundComponent == true)
                Destroy(gameObjectToRemove.GetComponent<ButtonClickSound>());

            _candidates.Remove(gameObjectToRemove);
        }

        private void RefreshButtonsList()
        {
            _candidates.Clear();

            var buttons = FindObjectsOfType<Button>().Select(_ => _.gameObject).ToList();
            var eventTriggers = FindObjectsOfType<EventTrigger>().Where(_ => _.triggers.Any(e => e.eventID == EventTriggerType.PointerClick)).Select(_ => _.gameObject).ToList();
            var toggles = FindObjectsOfType<Toggle>().Select(_ => _.gameObject).ToList();

            if (buttons.Count > 0)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    SetDatasButtonClickSoundComponent(GetButtonClickSoundComponent(buttons[i]));
                    _candidates.Add(buttons[i]);
                }
            }
            
            if (eventTriggers.Count > 0)
            {
                for (int i = 0; i < eventTriggers.Count; i++)
                {
                    SetDatasButtonClickSoundComponent(GetButtonClickSoundComponent(eventTriggers[i]));
                    _candidates.Add(eventTriggers[i]);
                }
            }

            if (toggles.Count > 0)
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    SetDatasButtonClickSoundComponent(GetButtonClickSoundComponent(toggles[i]));
                    _candidates.Add(toggles[i]);
                }
            }
        }
        #endregion

        #region BUTTON CLICK SOUND COMPONENT
        private bool HasButtonClickSoundComponent(GameObject gameObjectToTest)
        {
            if (gameObjectToTest.GetComponent<ButtonClickSound>())
                return true;
            return false;
        }

        private ButtonClickSound GetButtonClickSoundComponent(GameObject gameObjectWithComponent)
        {
            ButtonClickSound tempComponent;

            if (HasButtonClickSoundComponent(gameObjectWithComponent))
                tempComponent = gameObjectWithComponent.GetComponent<ButtonClickSound>();
            else
                tempComponent = gameObjectWithComponent.AddComponent<ButtonClickSound>();

            return tempComponent;
        }

        private void SetDatasButtonClickSoundComponent(ButtonClickSound buttonClickSoundComponent)
        {
            buttonClickSoundComponent.AudioSource = audioSourceRunTime;
            buttonClickSoundComponent.ClickSound = audioClipRunTime;
        }
        #endregion

#if UNITY_EDITOR
        public void RefreshList()
        {
            RefreshButtonsList();
        }
#endif
    }
}
