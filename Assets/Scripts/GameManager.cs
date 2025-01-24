using Match.Skills;
using Match.View;
using UnityEngine;

namespace Match
{
    public class GameManager : MonoBehaviour
    {
        // Singleton instance
        public static GameManager Instance;

        // Core components
        public ItemSpawner itemSpawner;
        public UIController uiController;

        // Skills
        public WindSkill windSkill;
        public ScoreMultiplierSkill scoreMultiplierSkill;
        public InstantMatchSkill instantMatchSkill;
        public TemporaryScoreIncreaseSkill temporaryScoreIncreaseSkill;
        public ObjectEnlargementSkill objectEnlargementSkill;
        public FireSkill fireSkill;
        public ResetSkill resetSkill; // Yeni eklenen ResetSkill

        private void Awake()
        {
            // Ensure only one instance of GameManager exists
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            // Initialize core components
            InitializeUI();
            InitializeSpawner();

            // Initialize all skills
            InitializeSkills();
        }

        private void InitializeUI()
        {
            if (uiController != null)
            {
                uiController.Initialize(itemSpawner);
                Debug.Log("UIController initialized.");
            }
            else
            {
                Debug.LogError("UIController is not assigned in GameManager.");
            }
        }

        private void InitializeSpawner()
        {
            if (itemSpawner != null)
            {
                itemSpawner.SpawnObjects();
                Debug.Log("ItemSpawner initialized.");
            }
            else
            {
                Debug.LogError("ItemSpawner is not assigned in GameManager.");
            }
        }

        private void InitializeSkills()
        {
            if (windSkill != null)
            {
                windSkill.Initialize(itemSpawner);
                Debug.Log("WindSkill initialized.");
            }
            else
            {
                Debug.LogWarning("WindSkill is not assigned in GameManager.");
            }

            if (instantMatchSkill != null)
            {
                instantMatchSkill.Initialize(itemSpawner);
                Debug.Log("InstantMatchSkill initialized.");
            }
            else
            {
                Debug.LogWarning("InstantMatchSkill is not assigned in GameManager.");
            }

            if (objectEnlargementSkill != null)
            {
                objectEnlargementSkill.Initialize(itemSpawner);
                Debug.Log("ObjectEnlargementSkill initialized.");
            }
            else
            {
                Debug.LogWarning("ObjectEnlargementSkill is not assigned in GameManager.");
            }

            if (fireSkill != null)
            {
                fireSkill.Initialize(itemSpawner);
                Debug.Log("FireSkill initialized.");
            }
            else
            {
                Debug.LogWarning("FireSkill is not assigned in GameManager.");
            }

            if (resetSkill != null) 
            {
                resetSkill.Initialize(itemSpawner);
                Debug.Log("ResetSkill initialized.");
            }
            else
            {
                Debug.LogWarning("ResetSkill is not assigned in GameManager.");
            }
        }
    }
}

