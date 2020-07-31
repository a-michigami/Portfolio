using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_action : MonoBehaviour
{
    // ハイライト色を指定します。
    [SerializeField]
    private Color overCol = default;
    // 選択された際のサウンドを指定します。
    [SerializeField]
    private AudioClip picSound = null;
    // ハイライトがキャンセルされた際のサウンドを指定します。
    [SerializeField]
    private AudioClip cancelSound = default;
    // このピースの種類を識別するIDを指定します。
    [SerializeField]
    private int pieceId = 0;
    // このピースの種類を識別するIDを取得します。
    public int PieceId
    {
        get { return pieceId; }
    }

    // 通常状態のカラー
    Color col;

    // Start is called before the first frame update
    void Start()
    {
        // 起動直後の通常カラーを保存しておく
        col = GetComponent<Renderer>().material.color;
    }

    // ボールを通常表示に設定します。
    public void NormalCol()
    {
        GetComponent<Renderer>().material.color = col;
        AudioSource.PlayClipAtPoint(cancelSound, transform.position);
    }

    // ボールをハイライト表示に設定します。
    public void Highlight()
    {
        GetComponent<Renderer>().material.color = overCol;
        AudioSource.PlayClipAtPoint(picSound, transform.position);
    }
}
