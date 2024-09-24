using System.Text;
using Player;
using Player.Data;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private readonly StringBuilder _code = new();
    private int _counter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _code.Append("Q");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _code.Append("W");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _code.Append("E");

            if (_code.ToString() == "QWE")
            {
                Debug.Log("Cheater!");
                var currentCoins = PlayerPrefs.GetInt($"{PlayerDataKeys.CoinsDataKey}");
                PlayerPrefs.SetInt($"{PlayerDataKeys.CoinsDataKey}", currentCoins + 200);
            }
            else
            {
                _code.Clear();
            }
        }
    }

    public void ClickLogo()
    {
        _counter++;
        if (_counter % 10 == 0)
        {
            Debug.Log("Cheater!");
            var currentCoins = PlayerPrefs.GetInt($"{PlayerDataKeys.CoinsDataKey}");
            PlayerPrefs.SetInt($"{PlayerDataKeys.CoinsDataKey}", currentCoins + 200);
        }
    }
}