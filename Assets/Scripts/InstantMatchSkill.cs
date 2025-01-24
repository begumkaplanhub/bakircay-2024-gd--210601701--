using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Match.Skills
{
    public class InstantMatchSkill : MonoBehaviour
    {
        private ItemSpawner _itemSpawner;
        public GameObject matchEffect; // Eşleşme sırasında oluşacak görsel efekt

        private float _shuffleDuration = 1.5f; // Karıştırma süresi
        private float _matchAnimationDuration = 1f; // Eşleşme animasyonu süresi

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            if (matchEffect != null)
                matchEffect.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.OnInstantMatchUsed += OnInstantMatchUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnInstantMatchUsed -= OnInstantMatchUsed;
        }

        private void OnInstantMatchUsed()
        {
            var items = _itemSpawner.GetItems();
            if (items == null || items.Count < 2)
            {
                Debug.LogWarning("Eşleşme için yeterli nesne yok.");
                return;
            }

            // Karıştırma animasyonu
            StartCoroutine(ShuffleItemsCoroutine(items, () =>
            {
                // Karıştırma sonrası eşleşme işlemini başlat
                var pair = items
                    .GroupBy(x => x.matchID)
                    .FirstOrDefault(g => g.Count() > 1);

                if (pair != null)
                {
                    var firstItem = pair.First();
                    var secondItem = pair.Last();

                    // Eşleşme işlemini başlat
                    StartCoroutine(MatchItemsCoroutine(firstItem, secondItem));
                }
                else
                {
                    Debug.LogWarning("Eşleşecek bir çift bulunamadı.");
                }
            }));
        }

        private IEnumerator ShuffleItemsCoroutine(List<Item> items, System.Action onComplete)
        {
            foreach (var item in items)
            {
                if (item == null) continue;

                // Rastgele pozisyon ve döndürme animasyonu
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-1f, 1f)
                );

                item.transform.DOShakePosition(_shuffleDuration, randomOffset, 10, 90, false, true);
                item.transform.DORotate(Vector3.up * 360, _shuffleDuration, RotateMode.FastBeyond360);
            }

            yield return new WaitForSeconds(_shuffleDuration);

            onComplete?.Invoke(); // Karıştırma tamamlandıktan sonra eşleşme işlemini çağır
        }

        private IEnumerator MatchItemsCoroutine(Item firstItem, Item secondItem)
        {
            if (firstItem == null || secondItem == null) yield break;

            float openDuration = 0.5f;
            float closeDuration = 0.5f;

            // Eşleşme efekti başlat
            if (matchEffect != null)
            {
                matchEffect.SetActive(true);
                matchEffect.transform.position = (firstItem.transform.position + secondItem.transform.position) / 2;
            }

            // Objeleri eşleştirme pozisyonlarına taşı
            Vector3 centerPosition = (firstItem.transform.position + secondItem.transform.position) / 2f;

            firstItem.transform.DOMove(centerPosition + Vector3.left * 0.5f, _matchAnimationDuration);
            secondItem.transform.DOMove(centerPosition + Vector3.right * 0.5f, _matchAnimationDuration);

            yield return new WaitForSeconds(_matchAnimationDuration);

            // Objeleri merkeze al
            firstItem.transform.DOMove(centerPosition, openDuration);
            secondItem.transform.DOMove(centerPosition, openDuration);

            yield return new WaitForSeconds(openDuration);

            // Objeleri aşağıya kaydır ve sahneden çıkar
            Vector3 dropPosition = centerPosition + Vector3.down * 2f;
            firstItem.transform.DOMove(dropPosition, closeDuration);
            secondItem.transform.DOMove(dropPosition, closeDuration);

            yield return new WaitForSeconds(closeDuration);

            // Objeleri devre dışı bırak
            firstItem.gameObject.SetActive(false);
            secondItem.gameObject.SetActive(false);

            // Temporary Score ekle (opsiyonel)
            GameEvents.OnTemporaryScoreIncreaseActive?.Invoke(10);

            // Eşleşme tamamlandı, efekti kapat
            if (matchEffect != null)
                matchEffect.SetActive(false);

            // Skor güncelleme event'i tetikle
            GameEvents.OnItemMatched?.Invoke(firstItem.itemData);
        }
    }
}
