using UnityEngine;

/// <summary>
/// シングルトンのための抽象クラス
/// インスタンスのキャッシュ、重複防止のロジックを含みます
///
/// Abstract class for Singleton pattern.
/// Includes logic for instance caching and duplicate prevention.
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // シーン内から検索
                // Search for the instance in the scene
                instance = Object.FindAnyObjectByType<T>();

                if (instance == null)
                {
                    Debug.LogWarning($"{typeof(T)} instance not found in the scene. Please ensure it is placed in the scene.");
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Awakeにてインスタンスの確立と重複チェックを行います
    /// 継承先でAwakeを使用する場合は base.Awake() を呼ぶか、このメソッドをオーバーライドしてください
    ///
    /// / Establishes the instance and checks for duplicates in Awake.
    /// If you use Awake in the derived class, call base.Awake() or override this method.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Debug.LogWarning($"Multiple instances of {typeof(T)} detected. Destroying the duplicate component on {gameObject.name}.");
            Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        // 自身が唯一のインスタンスだった場合、破棄時に参照をクリアする

        // If this was the unique instance, clear the reference upon destruction.
        if (instance == this)
        {
            instance = null;
        }
    }
}