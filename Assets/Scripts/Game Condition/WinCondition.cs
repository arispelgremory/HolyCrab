using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [Header("Game Conditions")]
    [SerializeField] private int minCrabAmount;
    // Crab Amount
    private InGameUI _gameUI;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        _gameUI = InGameUI.Instance;
        _gameUI.requirementCount = minCrabAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameUI.crabAmount >= minCrabAmount && _gameUI.timeCount <= 0)
        {
            //  You Win!
            _gameUI.HasWin();
        }

        if(_gameUI.crabAmount <= minCrabAmount)
        {
            // You Lose!
            _gameUI.HasLost();
        }
    }
}
