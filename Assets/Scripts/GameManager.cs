using UnityEngine;
using UnityEngine.SceneManagement;

//↓型を自作している　列挙型
public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState;
    public string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;
    }

    // Update is called once per frame
    void Update()
    {

    }
        //リスタート
    public void Restart()
    {
        //Debug.Log("リスタート");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
    