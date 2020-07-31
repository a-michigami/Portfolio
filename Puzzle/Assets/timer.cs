using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    // メッセージ表示用のUIを指定します。
    [SerializeField]
    private Text title = null;
    // 制限時間表示用のUIを指定します。
    [SerializeField]
    private Text time = null;
    // スコア表示用のUIを指定します。
    [SerializeField]
    private Text score = null;
    // 制限時間の残り
    float nowTime = 60;
    // ゲーム開始前の場合はtrue、ゲーム中はfalse
    static public bool isWait = true;
    // 現在のスコア
    static public int nowScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        time.text = "00s";
        title.text = "wait...";
        score.text = "000000";
        nowTime = 60;
        StartCoroutine(BallCheck());
    }

    // Update is called once per frame
    void Update()
    {
        nowTime -= Time.deltaTime;
        // タイムアップ判定
        if (nowTime < 0)
        {
            isWait = true;
            title.text = "Time Up";
            title.enabled = true;
            nowTime = 0;
        }
        time.text = string.Format("{0:f2}s", nowTime);
        score.text = nowScore.ToString("d6");
    }

    IEnumerator BallCheck()
    {
        isWait = true;
        // １秒待機
        yield return new WaitForSeconds(1);
        // すべての初期配置ピースが制止するまでループ
        while (true)
        {
            // 現在シーンに存在するすべてのピースを取得
            var balls = GameObject.FindGameObjectsWithTag("Piece");
            bool isMove = false;
            foreach (var ball in balls)
            {
                // まだ動いているピースがあるかどうかを判定
                if (ball.GetComponent<Rigidbody2D>().velocity.magnitude > 0.3f)
                {
                    isMove = true;
                    break;
                }
            }
            // すべての初期配置ピースが制止した場合
            if (!isMove)
            {
                break;
            }
            yield return null;
        }
        // ゲーム開始
        isWait = false;
        title.text = "GO!!";
        // １秒後にメッセージ表示を消す
        yield return new WaitForSeconds(1);
        title.enabled = false;
    }
}
