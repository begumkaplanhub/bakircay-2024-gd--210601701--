using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match.View
{
    public class UIController : MonoBehaviour
    {
        public TMP_Text scoreText;
        public Image objectFillImage;

        public Button windSkillButton;
        public Button scoreMultiplierButton;
        public Button instantMatchButton;
        public Button temporaryScoreIncreaseButton;
        public Button objectEnlargementButton;
        public Button fireSkillButton;
        public Button resetSkillButton;

        private string _scoreTextFormat = "Score: {0}";
        private ItemSpawner _itemSpawner;

        private int _score = 0;
        private float _currentMultiplier = 1f; // Default multiplier is 1x

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            _score = 0;
            _currentMultiplier = 1f; // Reset multiplier during initialization
            SetInitialValues();

            GameEvents.OnItemMatched += OnItemMatched;
            GameEvents.OnItemsSpawned += SetInitialValues;

            GameEvents.OnScoreMultiplierActive += OnScoreMultiplierActive;
            GameEvents.OnScoreMultiplierDeactive += OnScoreMultiplierDeactive;

            GameEvents.OnTemporaryScoreIncreaseActive += OnTemporaryScoreIncreaseActive;
            GameEvents.OnTemporaryScoreIncreaseDeactive += OnTemporaryScoreIncreaseDeactive;
        }

        private void OnDestroy()
        {
            GameEvents.OnItemMatched -= OnItemMatched;
            GameEvents.OnItemsSpawned -= SetInitialValues;

            GameEvents.OnScoreMultiplierActive -= OnScoreMultiplierActive;
            GameEvents.OnScoreMultiplierDeactive -= OnScoreMultiplierDeactive;

            GameEvents.OnTemporaryScoreIncreaseActive -= OnTemporaryScoreIncreaseActive;
            GameEvents.OnTemporaryScoreIncreaseDeactive -= OnTemporaryScoreIncreaseDeactive;
        }

        private void SetInitialValues()
        {
            SetScoreUI();
            objectFillImage.fillAmount = 0;

            windSkillButton.interactable = true;
            scoreMultiplierButton.interactable = true;
            instantMatchButton.interactable = true;
            temporaryScoreIncreaseButton.interactable = true;
            objectEnlargementButton.interactable = true;
            fireSkillButton.interactable = true;
            resetSkillButton.interactable = true;
        }

        private void OnItemMatched(ItemData data)
        {
            const int pointsPerMatch = 20;
            int pointsToAdd = Mathf.RoundToInt(pointsPerMatch * _currentMultiplier);
            _score += pointsToAdd;

            Debug.Log($"Matched Item: {data.itemName}, Added Score: {pointsToAdd}, Multiplier: {_currentMultiplier}, Total Score: {_score}");

            SetScoreUI();
            SetFillUI();
        }

        private void SetScoreUI()
        {
            scoreText.text = string.Format(_scoreTextFormat, _score);
        }

        private void SetFillUI()
        {
            objectFillImage.fillAmount = 1 - (_itemSpawner.CurrentItemCount / (float)_itemSpawner.SpawnedItemCount);
        }

        public void OnWindSkillButtonClick()
        {
            windSkillButton.interactable = false;
            GameEvents.OnWindSkillUsed?.Invoke();
        }

        public void OnScoreMultiplierButtonClick()
        {
            scoreMultiplierButton.interactable = false;
            GameEvents.OnScoreMultiplierUsed?.Invoke();
        }

        public void OnInstantMatchButtonClick()
        {
            instantMatchButton.interactable = false;
            GameEvents.OnInstantMatchUsed?.Invoke();
        }

        public void OnTemporaryScoreIncreaseButtonClick()
        {
            temporaryScoreIncreaseButton.interactable = false;
            GameEvents.OnTemporaryScoreIncreaseUsed?.Invoke();

            // Add 20 points directly when the button is pressed
            const int extraPoints = 20;
            _score += extraPoints;

            Debug.Log($"Temporary Score Increase: Added {extraPoints} points. Total Score: {_score}");

            SetScoreUI();
        }

        public void OnObjectEnlargementButtonClick()
        {
            objectEnlargementButton.interactable = false;
            GameEvents.OnObjectEnlargementUsed?.Invoke();
        }

        public void OnFireSkillButtonClick()
        {
            fireSkillButton.interactable = false;
            GameEvents.OnFireSkillUsed?.Invoke();
        }

        public void OnResetSkillButtonClick()
        {
            if (GameManager.Instance.resetSkill == null)
            {
                Debug.LogError("ResetSkill is not initialized in GameManager!");
                return;
            }
            GameManager.Instance.resetSkill.TriggerReset();
        }

        private void OnScoreMultiplierActive(float multiplier)
        {
            _currentMultiplier = multiplier;
            scoreText.color = Color.red;
        }

        private void OnScoreMultiplierDeactive()
        {
            _currentMultiplier = 1f;
            scoreText.color = Color.black;
        }

        private void OnTemporaryScoreIncreaseActive(int extraPoints)
        {
            scoreText.color = Color.blue;
        }

        private void OnTemporaryScoreIncreaseDeactive()
        {
            scoreText.color = Color.black;
        }
    }
}
