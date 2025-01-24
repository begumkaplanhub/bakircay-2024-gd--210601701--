using System.Collections;
using UnityEngine;

namespace Match.Skills
{
    public class TemporaryScoreIncreaseSkill : MonoBehaviour
    {
        public int extraPoints = 20; // Fixed points to add when the button is pressed

        private void OnEnable()
        {
            GameEvents.OnTemporaryScoreIncreaseUsed += OnTemporaryScoreIncreaseUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnTemporaryScoreIncreaseUsed -= OnTemporaryScoreIncreaseUsed;
        }

        private void OnTemporaryScoreIncreaseUsed()
        {
            AddPoints(); // Add points directly when the skill is used
        }

        private void AddPoints()
        {
            // Notify that the extra points have been added
            GameEvents.OnTemporaryScoreIncreaseActive?.Invoke(extraPoints);

            Debug.Log($"Added {extraPoints} points!");
        }

        // Commented out spawning functionality
        /*
        private IEnumerator ActivateTemporaryScoreIncrease()
        {
            // Notify that the extra points effect is active
            GameEvents.OnTemporaryScoreIncreaseActive?.Invoke(extraPoints);

            // Spawn an extra pair of items
            SpawnExtraItems();

            // Wait for the skill duration
            yield return new WaitForSeconds(extraPointsDuration);

            // Notify that the extra points effect is no longer active
            GameEvents.OnTemporaryScoreIncreaseDeactive?.Invoke();
        }

        private void SpawnExtraItems()
        {
            if (_itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned in TemporaryScoreIncreaseSkill!");
                return;
            }

            // Fetch random item data
            var randomItemData = _itemSpawner.itemRepository.GetRandomItems(1);
            if (randomItemData.Count == 0)
            {
                Debug.LogWarning("No items available in the repository to spawn!");
                return;
            }

            // Spawn two items as a pair
            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPosition = _itemSpawner.GetValidSpawnPosition();

                // Instantiate the item
                var itemPrefab = randomItemData[0].itemPrefab;
                var instance = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

                // Assign the item data to the newly spawned item
                var itemComponent = instance.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.itemData = randomItemData[0];
                    itemComponent.matchID = _itemSpawner.spawnedObjects.Count / 2; // Set a unique matchID
                    _itemSpawner.spawnedObjects.Add(instance.transform);

                    Debug.Log($"Spawned Extra Item: {itemComponent.itemData.itemName}");
                }
                else
                {
                    Debug.LogError("Spawned object does not have an Item component!");
                }
            }
        }
        */
    }
}

