using UnityEngine;
using System.Collections;

public class EnemyActionDash : MonoBehaviour {
    [SerializeField] Transform target;              // Playerの位置を指定
    [SerializeField] float dashSpeed = 10f;         // 一定の突進速度
    [SerializeField] float dashDuration = 1f;       // 突進の効果時間
    [SerializeField] float dashCooldown = 2f;       // 突進後の待機時間

    private bool isDashing = false;       // 突進中かどうか
    private Rigidbody rb;                 // Rigidbodyコンポーネント
    private Vector3 dashDirection;        // 突進の方向

    //攻撃判定（近接武器）用のコライダー
    [SerializeField]
    Collider attackCollider;

    void Start()
    {
        // Rigidbodyコンポーネントの取得
        rb = GetComponent<Rigidbody>();

        // 追尾対象の取得（Playerを想定）
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void StartDash()
    {
        // 突進中の場合は再実行せず、警告メッセージを出す
        if (isDashing)
        {
            Debug.LogWarning("Dash already in progress.");
            return;
        }

        // 状態をリセットしてからコルーチンを開始する
        ResetState();
        StartCoroutine(EnemyDashRoutine());
    }

    private IEnumerator EnemyDashRoutine()
    {

        // 突進中フラグを立てる
        isDashing = true;

        // 突進方向を計算（プレイヤーの方向）
        dashDirection = (target.position - transform.position).normalized;

        // 攻撃判定用のコライダーを有効化
        AttackColliderOn();

        // 突進終了時間を計算
        float dashEndTime = Time.time + dashDuration;
        // 突進中の処理（dash中であれば、指定した時間だけ進む）
        while (Time.time < dashEndTime && isDashing)
        {
            rb.velocity = dashDirection * dashSpeed; // 突進方向に一定速度で進む
            yield return null; // 次のフレームへ
        }

        // 突進終了後、速度を0にする
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(dashCooldown); // 待機時間を設ける

        // 突進終了時の処理をリセット
        ResetState();
    }

    private void ResetState()
    {
        // 突進関連の状態をリセットする共通処理
        isDashing = false;                 // 突進中フラグをリセット
        //rb.velocity = Vector3.zero;       // 突進の速度を0に戻す
        AttackColliderOff();              // 攻撃判定用コライダーを無効化
    }

    public void CancelDash()
    {
        // 突進中でない場合は何もしない
        if (!isDashing)
            return;

        // 状態をリセットし、デバッグログを出力
        ResetState();
        Debug.Log("Dash canceled!");
    }

    // 近接攻撃用のコライダーを有効にする関数
    void AttackColliderOn()
    {
        attackCollider.enabled = true;
        Debug.Log("Attack c on");

    }

    // 近接攻撃用のコライダーを無効にする関数
    void AttackColliderOff()
    {
        attackCollider.enabled = false;
        Debug.Log("Attack c off");

    }

    public bool IsDashing()
    {
        // 突進中かどうかを外部から確認できるようにする
        return isDashing;
    }
}
