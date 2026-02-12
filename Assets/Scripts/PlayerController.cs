using UnityEngine;//絶対に必要


public class PlayerController : MonoBehaviour　//絶対に継承する
{
    Rigidbody2D rbody;//Rigidbody2D型の変数
    float axisH = 0.0f;//入力
    public float speed = 3.0f;//移動速度
    public float jump = 9.0f;//ジャンプ力
    public LayerMask groundLayer;//着地できるレイヤー
    bool goJump = false;//ジャンプ開始フラグ
    bool onGround = false;//地面フラグ

    //アニメーション対応
    Animator animator;
    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle";
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; //スコア

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigidbody2Dを取ってくる
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

    }

    // Update is called once per frame
    // PCの性能差でフレーム差が生まれてしまう
    void Update()
    {
        if (GameManager.gameState != GameState.InGame)
        {
            return;
        }

        //地上判定
        onGround = Physics2D.CircleCast(transform.position,//発射位置
                0.2f,//円の半径
                Vector2.down,//発射方向
                0.0f,//発射距離
                groundLayer);//検出するレイヤー
        if (Input.GetButtonDown("Jump"))//キャラクターをジャンプさせる
        {
            goJump = true;//ジャンプフラグを立てる
        }
        axisH = Input.GetAxisRaw("Horizontal");//方向キーの入力をcheckするHorizontal＝左右
                                               //inputmanager-ただしシステム上古いとされている-の設定を変えることで利用可能

        if (axisH > 0.0f)                           // 向きの調整
        {
            //Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左移動");

            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }
        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
                //Debug.Log(nowAnime);

            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.gameState != GameState.InGame)
        {
            return;
        }
        if (onGround || axisH != 0)//地面の上or速度が０ではない
        {
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
            //値を代入する。対象を押し出す
        }
        if (onGround && goJump)//地面の上でジャンプキーが押された
        {
            Vector2 jumpPw = new Vector2(0, jump);//ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);//瞬間的な力を加える
            goJump = false;//ジャンプフラグを切る
                           //Debug.Log("ジャンプ");

        }
    }
    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdata.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
    }

    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear;
        GameStop();
    }

    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();             // ゲーム停止

        // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;      // 当たりを消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に少し跳ね上げる

        Destroy(gameObject, 2.0f); // 2秒後にヒエラルキーからオブジェクトを抹消
    }
    void GameStop()
    {

        rbody.linearVelocity = new Vector2(0, 0);           // 速度を0にして強制停止
    }
    public float GetAxisH()
    {
        return axisH;
    }
}
