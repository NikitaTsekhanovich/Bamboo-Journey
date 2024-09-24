using DG.Tweening;
using UnityEngine;

namespace GameField.Items
{
    public class Fruit : MonoBehaviour
    {
        public void StartFeed(Item panda)
        {
            BlockClickHandler.Instance.OnBlockPanel();

            DOTween.Sequence()
                .Append(transform.DOMove(panda.CurrentLevelImage.transform.position, 0.5f))
                .AppendCallback(() => BlockClickHandler.Instance.OffBlockPanel())
                .AppendCallback(() => Destroy(gameObject));    
        }
    }
}

