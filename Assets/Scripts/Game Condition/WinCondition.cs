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
        _gameUI = InGameUI.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameUI.crabAmount <= minCrabAmount)
        {
            // You Lose!
            Debug.Log("You Lose!");
        }
    }
}
