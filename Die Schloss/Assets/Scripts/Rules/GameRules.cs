using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    private enum GameState
    {
    }

    [SerializeField] private GameObject player;

    private PlayerTurn playerScript;

    private void Awake()
    {
        playerScript = player.GetComponent<PlayerTurn>();
    }

    private void Start()
    {
        //StartCoroutine(ExecuteTurn());
    }

    private void Update()
    {
        ExecuteTurn();
    }

    private IEnumerator ExecuteTurn()
    {
        // ExecuteGameTurn();
        playerScript.ExecuteTurn();
        // enemy.ExecuteTurn();
        return null;
    }
}