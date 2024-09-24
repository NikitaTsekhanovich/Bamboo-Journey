using System.Collections.Generic;
using GameField.Scrolls;
using UnityEngine;
using UnityEngine.UI;

namespace GameField.Scrolls
{
    public class ContainerVerticalScrolls : MonoBehaviour
    {
        [SerializeField] private List<ScrollRect> _verticalScrolls;
        public List<ScrollRect> VerticalScrolls => _verticalScrolls;
        // private static List<VerticalScroll> verticalScrolls;

        // public static List<VerticalScroll> VerticalScrolls => verticalScrolls;

        // private void Awake()
        // {
        //     verticalScrolls = new(_verticalScrolls);
        // }
    }
}

