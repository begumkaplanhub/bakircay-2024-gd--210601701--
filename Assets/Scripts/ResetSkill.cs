using Match.View;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

namespace Match.Skills
{
    public class ResetSkill : MonoBehaviour
    {
        private ItemSpawner _itemSpawner;
        private UIController _uiController;

        public GameObject resetEffect; // Optional visual effect during reset
        private float _resetEffectDuration = 2f; // Duration of the reset effect

        // Initialize with references
        public void Initialize(ItemSpawner itemSpawner, UIController uiController)
        {
            _itemSpawner = itemSpawner;
            _uiController = uiController;

            // Ensure the reset effect is inactive at the start
            if (resetEffect != null)
                resetEffect.SetActive(false);
        }

        // Trigger the reset process
        public void TriggerReset()
        {
            if (resetEffect != null)
            {
                resetEffect.SetActive(true); // Activate the reset effect
            }

            // Perform the reset after a delay
            Invoke(nameof(ResetGame), _resetEffectDuration);
        }

        private void ResetGame()
        {
            // Reload the current active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Scene reloaded, game reset!");
        }

        internal void Initialize(ItemSpawner itemSpawner)
        {
            throw new NotImplementedException();
        }
    }
}

