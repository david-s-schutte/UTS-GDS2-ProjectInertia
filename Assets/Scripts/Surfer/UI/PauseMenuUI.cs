using System;
using FMODUnity;
using Managers;
using Surfer.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer.UI
{
    public class PauseMenuUI : MonoUI, IInteractable
    {
        [Header("Menus")] [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _settingsMenu;

        [Header("Settings Componetns")] [SerializeField]
        private Slider _soundSlider;

        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;


        private FMOD.Studio.Bus _masterBus;
        private FMOD.Studio.Bus _sfxBus;
        private FMOD.Studio.Bus _musicBus;

        public PlayerControls Controls { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            _uiManager.RegisterUI(this, true);

            _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
            _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        }

        protected override void OnInitialised()
        {
        }

        public override void OnFront()
        {
        }


        public override void OnRegistered()
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        public override void OnUnregistered()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            Destroy(this.gameObject);
        }

        #region Button Management (not ideally coded but u know

        public void Continue()
        {
           _uiManager.UnRegisterUI(this);
        }

        public void OpenSettingsMenu()
        {
            _pauseMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        }

        public void BackToPauseMenu()
        {
            _pauseMenu.SetActive(true);
            _settingsMenu.SetActive(false);
        }

        public void UpdateMasterVolume() => _masterBus.setVolume(_soundSlider.value);
        public void UpdateMusicVolume () => _musicBus.setVolume(_musicSlider.value);
        public void UpdateSFXVolume () => _sfxBus.setVolume(_sfxSlider.value);


        public void ExitGame()
        {
            Application.Quit();
        }

        #endregion

        #region IInteractable

        public void EnableControls()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }

        public void DisableControls()
        {
            Controls.Disable();
        }

        #endregion
    }
}