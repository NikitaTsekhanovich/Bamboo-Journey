using Player.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuControllers
{
    public class CollectionController : MonoBehaviour
    {
        [SerializeField] private Image _pandaLocked;
        [SerializeField] private Image _alligatorLocked;
        [SerializeField] private Image _tigerLocked;
        [SerializeField] private Image _dolphinLocked;
        [SerializeField] private Image _salamanderLocked;
        [SerializeField] private Image _bearLocked;
        [SerializeField] private Image _monkeyLocked;

        [SerializeField] private TMP_Text _pandaAmountText;
        [SerializeField] private TMP_Text _alligatorAmountText;
        [SerializeField] private TMP_Text _tigerAmountText;
        [SerializeField] private TMP_Text _dolphinAmountText;
        [SerializeField] private TMP_Text _salamanderAmountText;
        [SerializeField] private TMP_Text _bearAmountText;
        [SerializeField] private TMP_Text _monkeyAmountText;


        [SerializeField] private Sprite _pandaOpen;
        [SerializeField] private Sprite _alligatorOpen;
        [SerializeField] private Sprite _tigerOpen;
        [SerializeField] private Sprite _dolphinOpen;
        [SerializeField] private Sprite _salamanderOpen;
        [SerializeField] private Sprite _bearOpen;
        [SerializeField] private Sprite _monkeyOpen;

        private void Start()
        {
            InitCollection();
        }

        private void InitCollection()
        {
            InitItemCollection(
                PlayerDataKeys.AmountPandaCollectonDataKey,
                _pandaLocked,
                _pandaOpen,
                _pandaAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountAlligatorCollectonDataKey,
                _alligatorLocked,
                _alligatorOpen,
                _alligatorAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountTigerCollectonDataKey,
                _tigerLocked,
                _tigerOpen,
                _tigerAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountDolphinCollectonDataKey,
                _dolphinLocked,
                _dolphinOpen,
                _dolphinAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountSalamanderCollectonDataKey,
                _salamanderLocked,
                _salamanderOpen,
                _salamanderAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountBearCollectonDataKey,
                _bearLocked,
                _bearOpen,
                _bearAmountText);
            InitItemCollection(
                PlayerDataKeys.AmountMonkeyCollectonDataKey,
                _monkeyLocked,
                _monkeyOpen,
                _monkeyAmountText);
        }

        private void InitItemCollection(string key, Image imageLocked, Sprite imageOpen, TMP_Text amountText)
        {
            var amountItem = PlayerPrefs.GetInt(key);

            if (amountItem > 0)
            {   
                imageLocked.sprite = imageOpen;
                amountText.text = $"x{amountItem}";
            }
        }
    }
}

