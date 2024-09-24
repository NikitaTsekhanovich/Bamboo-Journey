using System.Collections.Generic;
using Abilities;
using GameField;
using GameLogic;
using Player.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BonusSystem
{
    public class BonusesWeekController : MonoBehaviour
    {
        [SerializeField] private List<QuestData> _questsData = new();
        [SerializeField] private List<Image> _icons = new();
        [SerializeField] private List<TMP_Text> _descriptions = new();
        [SerializeField] private List<TMP_Text> _goals = new();
        [SerializeField] private Button _getBonusButton;
        [SerializeField] private GameObject _bonusScreen;
        [SerializeField] private AbilityBamboo _abilityBamboo;
        [SerializeField] private AbilityPanda _abilityPanda;
        [SerializeField] private AbilityRain _abilityRain;

        private List<bool> _progressQuests = new() {false, false, false};

        private void OnEnable()
        {
            GameFieldUpdater.OnCheckQuestType += CheckQuestType;
            ScoreController.OnCheckTypeQuest += CheckQuestType;
        }

        private void OnDisable()
        {
            GameFieldUpdater.OnCheckQuestType -= CheckQuestType;
            ScoreController.OnCheckTypeQuest -= CheckQuestType;
        }

        private void Start()
        {
            UpdateData();
        }

        public void UpdateData()
        {
            CheckGoalProgress();
            CheckQuestsProgress();
            InitWeekQuests();
        }

        private void InitWeekQuests()
        {
            for (var i = 0; i < _questsData.Count; i++)
            {
                _icons[i].sprite = _questsData[i].Icon;
                _descriptions[i].text = _questsData[i].Description;
                _goals[i].text = 
                    $"{PlayerPrefs.GetInt(_questsData[i].KeyProgress)}/{_questsData[i].Goal}";
            }
        }

        private void CheckQuestType(TypeQuest typeQuest)
        {
            for (var i = 0; i < _questsData.Count; i++)
            {
                if (_questsData[i].TypeQuest == typeQuest)
                {
                    var currentProgress = PlayerPrefs.GetInt(_questsData[i].KeyProgress);
                    PlayerPrefs.SetInt(_questsData[i].KeyProgress, currentProgress + 1);
                    _goals[i].text = 
                        $"{currentProgress + 1}/_questsData[i].Goal";
                }
            }
            CheckGoalProgress();
        }

        private void CheckGoalProgress()
        {
            for (var i = 0; i < _questsData.Count; i++)
            {
                var currentProgress = PlayerPrefs.GetInt(_questsData[i].KeyProgress);

                if (currentProgress >= _questsData[i].Goal)
                {
                    _progressQuests[i] = true;
                }
            }
            CheckQuestsProgress();
        }

        private void CheckQuestsProgress()
        {
            var isCompleted = true;

            foreach (var progressQuest in _progressQuests)
            {
                isCompleted &= progressQuest;
            }

            var weekBonusStatus = PlayerPrefs.GetInt(PlayerDataKeys.WeekBonusStatusKey);
            if (isCompleted && weekBonusStatus == 0)
                _getBonusButton.interactable = true;

        }

        public void TakeBonus()
        {
            _getBonusButton.interactable = false;
            var weekBonusStatus = PlayerPrefs.GetInt(PlayerDataKeys.WeekBonusStatusKey);
            PlayerPrefs.SetInt(PlayerDataKeys.WeekBonusStatusKey, weekBonusStatus + 1);

            _bonusScreen.SetActive(true);

            var amountBambooAbility = PlayerPrefs.GetInt(PlayerDataKeys.AmountBambooDataKey);
            PlayerPrefs.SetInt(PlayerDataKeys.AmountBambooDataKey, amountBambooAbility + 10);
            _abilityBamboo.UpdateAmountText(amountBambooAbility + 10);

            var amountRainAbility = PlayerPrefs.GetInt(PlayerDataKeys.AmountRainDataKey);
            PlayerPrefs.SetInt(PlayerDataKeys.AmountRainDataKey, amountRainAbility + 7);
            _abilityRain.UpdateAmountText(amountRainAbility + 7);

            var amountPandaAbility = PlayerPrefs.GetInt(PlayerDataKeys.AmountPandaDataKey);
            PlayerPrefs.SetInt(PlayerDataKeys.AmountPandaDataKey, amountPandaAbility + 8);
            _abilityPanda.UpdateAmountText(amountPandaAbility + 8);
        }
    }
}

