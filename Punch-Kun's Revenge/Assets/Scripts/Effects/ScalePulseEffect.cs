using UnityEngine;

public class ScalePulseEffect : MonoBehaviour
{
	[Header("設定")]
	[Tooltip("サイズが変化するスピード（周波数）")]
	[SerializeField] private float pulseSpeed = 2f;

	[Tooltip("サイズが変化する幅（元のスケールに対する割合、0.1で±10%）")]
	[SerializeField] private float pulseAmount = 0.1f;

	[Tooltip("ポーズ中（Time.timeScale = 0）でも動かすかどうか")]
	[SerializeField] private bool useUnscaledTime = false;

	private Vector3 originalScale;

	protected void Start()
	{
		// 初期状態のスケールを保持
		originalScale = transform.localScale;
	}

	protected void Update()
	{
		// 使用する時間を取得
		float time = useUnscaledTime ? Time.unscaledTime : Time.time;

		// サイン波を用いて -1.0 〜 1.0 の値を計算
		// pulseSpeed倍することでアニメーションの速さを制御
		float sinValue = Mathf.Sin(time * pulseSpeed * Mathf.PI * 2f);

		// スケールの倍率を計算 (1.0 - pulseAmount 〜 1.0 + pulseAmount)
		float currentScaleFactor = 1f + (sinValue * pulseAmount);

		// スケールを適用
		transform.localScale = originalScale * currentScaleFactor;
	}
}
