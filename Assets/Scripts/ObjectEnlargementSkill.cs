using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match.Skills
{
    public class ObjectEnlargementSkill : MonoBehaviour
    {
        public float enlargementDuration = 5f;
        private float enlargementScale = 1.5f;
        private ItemSpawner _itemSpawner;

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
        }

        private void OnEnable()
        {
            GameEvents.OnObjectEnlargementUsed += OnObjectEnlargementUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnObjectEnlargementUsed -= OnObjectEnlargementUsed;
        }

        private void OnObjectEnlargementUsed()
        {
            var items = _itemSpawner.GetItems();
            if (items == null || items.Count == 0) return;

            StartCoroutine(EnlargeObjects(items));
        }

        private IEnumerator EnlargeObjects(List<Item> items)
        {
            foreach (var item in items)
            {
                if (item == null || !item.gameObject.activeSelf) continue;
                item.transform.localScale *= enlargementScale;
            }

            yield return new WaitForSeconds(enlargementDuration);

            foreach (var item in items)
            {
                if (item == null || !item.gameObject.activeSelf) continue;
                item.transform.localScale = Vector3.one;
            }
        }
    }
}
