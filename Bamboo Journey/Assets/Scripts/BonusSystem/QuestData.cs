using UnityEngine;

namespace BonusSystem
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Quest data/ Quest")]
    public class QuestData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;
        [SerializeField] private int _goal;
        [SerializeField] private string _keyProgress;
        [SerializeField] private TypeQuest _typeQuest;

        public Sprite Icon => _icon;
        public string Description => _description;
        public int Goal => _goal;
        public string KeyProgress => _keyProgress;
        public TypeQuest TypeQuest => _typeQuest;
    }
}

