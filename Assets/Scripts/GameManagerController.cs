using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{


    private GameRepository gameRepository;
    private GameData gameData;

    public Text enemigosMuertosText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameRepository = GameRepository.GetInstance();
        // enemigosMuertosText = GameObject.Find("EnemigoMuertosText").GetComponent<Text>();
        gameData = gameRepository.GetData();
    }

    // Update is called once per frame
    void Update()
    {
        enemigosMuertosText.text = $"ENEMIGOS: {gameData.EnemigosMuertos}";
    }
}
