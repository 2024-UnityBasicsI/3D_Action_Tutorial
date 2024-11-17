using TMPro;  // TextMeshProを使用するための名前空間
using UnityEngine;  // Unity機能を使用するための名前空間
using System.Collections;  // コルーチンを使うための名前空間

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageText; // ダメージ数値を表示するTextMeshProUGUI
    [SerializeField] Color startColor; // テキストの初期色

    // ダメージを表示する関数
    public void ShowDamage(int damage)
    {
        damageText.text = damage.ToString();  // ダメージ数値を文字列に変換して設定
        damageText.color = new Color(startColor.r, startColor.g, startColor.b, 1f); // 完全に表示

        StartCoroutine(FadeOut());  // フェードアウト処理をコルーチンで開始
    }

    // ダメージテキストをフェードアウトさせるコルーチン
    private IEnumerator FadeOut()
    {
        float time = 0;  // 経過時間の初期化

        // 1秒間でフェードアウト
        while (time < 1f)
        {
            time += Time.deltaTime;  // 時間を加算
            float alpha = Mathf.Lerp(1f, 0f, time);  // アルファを1から0に補完
            damageText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);  // 新しい色を設定
            yield return null;  // 次のフレームまで待機
        }

        Destroy(gameObject);  // テキストが消えたらオブジェクトを削除
    }
}
