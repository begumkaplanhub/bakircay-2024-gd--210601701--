using System.Collections;
using UnityEngine;
using TMPro;

namespace Match.Skills
{
    public class ScoreMultiplierSkill : MonoBehaviour
    {
        public float multiplierDuration = 10f;
        private float _multiplierValue = 3f;
        private bool _isMultiplierActive = false;
        private Coroutine _currentCoroutine;

        public TMP_Text countdownText;

        public void Initialize()
        {
            if (countdownText != null)
                countdownText.text = string.Empty;
        }

        private void OnEnable()
        {
            GameEvents.OnScoreMultiplierUsed += OnScoreMultiplierUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreMultiplierUsed -= OnScoreMultiplierUsed;
        }

        private void OnScoreMultiplierUsed()
        {
            if (_isMultiplierActive && _currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            _currentCoroutine = StartCoroutine(ActivateScoreMultiplier());
        }

        private IEnumerator ActivateScoreMultiplier()
        {
            _isMultiplierActive = true;

            GameEvents.OnScoreMultiplierActive?.Invoke(_multiplierValue);

            float remainingTime = multiplierDuration;

            while (remainingTime > 0)
            {
                if (countdownText != null)
                {
                    countdownText.text = $"Multiplier: x{_multiplierValue} ({remainingTime:F1}s)";
                }

                yield return new WaitForSeconds(0.1f);
                remainingTime -= 0.1f;
            }

            if (countdownText != null)
            {
                countdownText.text = string.Empty;
            }

            GameEvents.OnScoreMultiplierDeactive?.Invoke();

            _isMultiplierActive = false;
        }
    }
}
