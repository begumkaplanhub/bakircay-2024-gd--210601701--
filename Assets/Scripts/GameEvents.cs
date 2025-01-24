using System;

namespace Match
{
    public static class GameEvents
    {
        // Item-related events
        public static Action<ItemData> OnItemMatched;
        public static Action OnItemsSpawned;

        // Skill-related events
        public static Action OnWindSkillUsed;
        public static Action OnScoreMultiplierUsed;
        public static Action OnInstantMatchUsed;
        public static Action OnTemporaryScoreIncreaseUsed;
        public static Action OnObjectEnlargementUsed;
        public static Action OnFireSkillUsed;

        // Skill effects events
        public static Action<float> OnScoreMultiplierActive;
        public static Action OnScoreMultiplierDeactive;

        public static Action<int> OnTemporaryScoreIncreaseActive;
        public static Action OnTemporaryScoreIncreaseDeactive;

        // Reset Skill event
        public static Action OnResetSkillUsed; 
    }
}

