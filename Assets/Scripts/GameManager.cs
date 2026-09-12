using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    private string message = "";
    public TextMeshProUGUI peatonField;
    public TextMeshProUGUI robberField;
    private int initialPeatones;
    private int initialRobbers;
    private bool gameStarted = false;
    private void Start()
    {
        gameOver = false;
        gameStarted = false;
    }

    void Update()
    {
        if (gameOver) return;

        Peaton[] peatones = FindObjectsOfType<Peaton>();
        Ladron[] ladrones = FindObjectsOfType<Ladron>();

        if (!gameStarted)
        {
            gameStarted = true;
            initialPeatones = peatones.Length;
            initialRobbers = ladrones.Length;
        }

        peatonField.text = peatones.Length + "/" + initialPeatones;
        robberField.text = ladrones.Length + "/" + initialRobbers;

        if (peatones.Length == 0)
        {
            gameOver = true;
            message = "¡Perdiste!";
            //Time.timeScale = 0f;
        }
        else if (ladrones.Length == 0)
        {
            gameOver = true;
            message = "¡Ganaste!";
            //Time.timeScale = 0f;
        }
    }

    void OnGUI()
    {
        if (gameOver && message == "¡Perdiste!")
        {
            /*
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = message == "¡Ganaste!" ? Color.green : Color.red;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), message, style);
            */
            SceneLoader.LoadSceneByIndex(3);
        }else if (gameOver && message == "¡Ganaste!")
        {
            SceneLoader.LoadSceneByIndex(2);
        }
    }
}
