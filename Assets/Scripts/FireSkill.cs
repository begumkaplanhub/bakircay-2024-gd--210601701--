using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match.Skills
{
    public class FireSkill : MonoBehaviour
    {
        public GameObject fireEffectPrefab;
        private float fireEffectDuration = 10f;

        private ItemSpawner _itemSpawner;

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
        }

        private void OnEnable()
        {
            GameEvents.OnFireSkillUsed += OnFireSkillUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnFireSkillUsed -= OnFireSkillUsed;
        }

        private void OnFireSkillUsed()
        {
            var items = _itemSpawner.GetItems();
            if (items == null || items.Count == 0) return;

            var matchingPair = items
                .GroupBy(x => x.matchID)
                .FirstOrDefault(group => group.Count() > 1);

            if (matchingPair != null)
            {
                var firstItem = matchingPair.First();
                var secondItem = matchingPair.Last();

                StartCoroutine(ActivateFireEffectAndDestroy(firstItem, secondItem));
            }
        }

        private IEnumerator ActivateFireEffectAndDestroy(Item firstItem, Item secondItem)
        {
            if (firstItem == null || secondItem == null) yield break;

            var fireEffect1 = Instantiate(fireEffectPrefab, firstItem.transform.position, Quaternion.identity);
            var fireEffect2 = Instantiate(fireEffectPrefab, secondItem.transform.position, Quaternion.identity);

            Destroy(fireEffect1, fireEffectDuration);
            Destroy(fireEffect2, fireEffectDuration);

            yield return new WaitForSeconds(fireEffectDuration);

            if (firstItem.gameObject.activeSelf)
            {
                firstItem.gameObject.SetActive(false);
            }

            if (secondItem.gameObject.activeSelf)
            {
                secondItem.gameObject.SetActive(false);
            }

            GameEvents.OnItemMatched?.Invoke(firstItem.itemData);
        }
    }
}
