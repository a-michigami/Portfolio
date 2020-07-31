using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set : MonoBehaviour
{
    // ボールのプレハブを指定します。
    [SerializeField]
    private GameObject[] balls = null;
    // ボールが消滅する際のサウンドを指定します。
    [SerializeField]
    private AudioClip popSound = default;

    // 選択されているボールを保存する配列
    List<GameObject> picBalls = new List<GameObject>();
    // 現在選択されているボール種類を保存しておく変数
    int pieceId = -1;

    // Start is called before the first frame update
    void Start()
    {
        // 45個の初期ボールを生成
        StartCoroutine(setBall(45));
    }

    // 指定された個数のボールを生成
    private IEnumerator setBall(int ball_n)
    {
        for (int counter = 0; counter < ball_n; counter++)
        {
            // 生成位置を決定
            var position = transform.position;
            position.x += Random.Range(-1.7f, 1.7f);
            position.y += 6.5f;
            // ボールの種類を決定
            var ballId = Random.Range(0, balls.Length);
            var ballPrefab = balls[ballId];
            // ボールを生成
            Instantiate(ballPrefab, position, ballPrefab.transform.rotation);
            // 0.1秒待機
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム開始している場合のみ
        if (!timer.isWait)
        {
            // タッチ開始を検出
            if (Input.GetMouseButtonDown(0))
            {
                OnTap();
            }
            // ドラッグ中を検出
            else if (Input.GetMouseButton(0))
            {
                IsDrag();
            }
            // タッチ終了を検出
            else if (Input.GetMouseButtonUp(0))
            {
                if (picBalls.Count > 0)
                {
                    StartCoroutine(TapOff());
                }
            }
        }
    }

    // タッチ開始の際に呼び出されます。
    private void OnTap()
    {
        pieceId = -1;
        // 指定した開始点からのRayと交差するオブジェクトを検出
        var hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            // 前回選択されていたボール配列をクリアー
            picBalls.Clear();

            var ball = hit.collider.gameObject;
            // ヒットしたボールのタグを保存
            pieceId = ball.GetComponent<Ball_action>().PieceId;
            // 今回ヒットしたボールを選択配列に追加
            picBalls.Add(ball);
            Debug.Log(picBalls[0]);
            // ボールをハイライト表示
            ball.GetComponent<Ball_action>().Highlight();
            //StartCoroutine(TapOff(ball));
        }
    }

    // ドラッグ中に呼び出されます。
    void IsDrag()
    {
        // 指定した開始点からのRayと交差するオブジェクトを検出
        var hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            var ball = hit.collider.gameObject;
            // picBalls配列にまだ追加されていない場合
            if (!picBalls.Contains(ball))
            {
                // 今回ヒットしたボールが選択中のボール種類である場合
                if (ball.GetComponent<Ball_action>().PieceId == pieceId)
                {
                    // 今回ヒットしたボールを選択配列に追加
                    picBalls.Add(ball);
                    // ボールをハイライト表示
                    ball.GetComponent<Ball_action>().Highlight();
                }
                // ことなるボール種類をタップしてしまった場合
                else
                {
                    ClearBall();
                }
            }
        }
    }

    // 選択配列をクリアーします。
    void ClearBall()
    {
        foreach (var ball in picBalls)
        {
            ball.GetComponent<Ball_action>().NormalCol();
        }
        picBalls.Clear();
    }

    // タッチ終了の際に呼び出されます。
    IEnumerator TapOff()
    {
        // 選択されているピースが３個以上の場合
        if (picBalls.Count >= 3)
        {
            int add_ball = 0;
            // 選択中のすべてのピースに対する繰り返し処理
            foreach (var ball in picBalls)
            {
                // 消滅サウンドを再生してボールを削除
                AudioSource.PlayClipAtPoint(popSound, transform.position);
                Destroy(ball);
                add_ball++;
                timer.nowScore += add_ball * 200;
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(setBall(picBalls.Count));
            picBalls.Clear();
        }
        else
        {
            ClearBall();
        }
    }
}
