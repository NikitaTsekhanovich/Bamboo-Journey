using TMPro;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _amountText;
        protected int _amount;

        protected void Start()
        {
            UpdateAmountText(_amount);
        }

        public void UpdateAmountText(int amount)
        {
            _amountText.text = $"{amount}";
        }

        protected void UseAbility()
        {
            _amount -= 1;
            SetAmountAbilities();
            UpdateAmountText(_amount);
        }

        protected abstract void SetAmountAbilities();
    }
}

