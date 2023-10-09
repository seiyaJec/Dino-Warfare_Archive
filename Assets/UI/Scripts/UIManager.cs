using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }

    //?�ン?�ネ?�ト
    private Animator animator;

    [SerializeField] private UnityEngine.UI.Image[] lifeImage;

    [SerializeField] private GameObject lockPref;
    [SerializeField] private GameObject cSignalPref;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject gameclearPanel;
    [SerializeField] private GameObject deadImage;
    [SerializeField] private GameObject checkContinuePanel;
    [SerializeField] private UnityEngine.UI.Image shotImageBase;
    [SerializeField] private GameObject shotImagePrefab;
    [HideInInspector] private List<UnityEngine.UI.Image> shotImages;
    [HideInInspector] private int shotsAll;
    [HideInInspector] private int shotsNow;
    [Header("?�の?�向?�調?�る")]
    [SerializeField] private Transform forwardSphere;    //?�レ?�ヤ?�の"正面"?�指?�ベ??��??
    [SerializeField] private Transform rightSphere;      //?�レ?�ヤ?�の"???�指?�ベ??��??
    [SerializeField]private Transform leftSphere;         //?�レ?�ヤ?�の"�??�指?�ベ??��??
    [Header("会話")]

    [SerializeField] public TalkPanel talkPanel;
    //UI?�ブ?�ェ??��?�リ?�ト?��???
    private List<LockOnTarget> lockTargets; //弱点?�位?�UI
    private List<CircleSignal> cSignals; //?�の?�撃????�UI

    //?�ブ?�ェ??��?�ー?�　Parent Transform
    private Transform lockOnPool;
    private Transform cSignalPool;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lockTargets = new List<LockOnTarget>();
        cSignals = new List<CircleSignal>();
        lockOnPool = transform.Find("LockOnPool").transform;
        cSignalPool = transform.Find("SignalPool").transform;
    }

    //体力?�ー?�UI?�ッ?�デ?�ト
    public void UpdateHealth(float currentHealth)
    {
        float maxLife = lifeImage.Length;
        if (maxLife < currentHealth)
            return;

        for (int i = 0; i < maxLife; i++)
        {
            lifeImage[i].enabled = i < currentHealth ? true : false;
        }
    }

    //弱点?�位?�UI表示
    public void EnableLockImage(Transform target, float size = 1.0f)
    {
        //?�ス?�で使っ?�い?�い??��?�を検索?�て使う
        foreach (var lockTarget in lockTargets)
        {
            if (!lockTarget.enabled)
            {
                lockTarget.enabled = true;
                lockTarget.SetTarget(target, size);
                return;
            }
        }
        //見つ?�ら?�買?�た?�新?�いUI?�ブ?�ェ??��?�成
        CreateNewLockObj(target, size);
    }
    //?�表�?
    public void DisableLockImage(Transform target)
    {
        foreach (var lockTarget in lockTargets)
        {
            if (lockTarget.target == target)
            {
                lockTarget.enabled = false;
            }
        }
    }
    public void DisableAllLockImage()
    {
        foreach (var lockTarget in lockTargets)
        {
            lockTarget.enabled = false;
        }
    }
    private void CreateNewLockObj(Transform target, float size)
    {
        var obj = Instantiate(lockPref, lockOnPool);
        var newLock = obj.GetComponent<LockOnTarget>();
        lockTargets.Add(newLock);

        newLock.enabled = true;
        newLock.SetTarget(target, size);
    }

    public void EnableSignalImage(Transform target, float duration)
    {
        foreach (var cSignal in cSignals)
        {
            if (!cSignal.enabled)
            {
                cSignal.enabled = true;
                cSignal.Init(target, duration);
                return;
            }
        }
        CreateNewSignalObj(target, duration);
    }
    public void DisableSignalImage(Transform target)
    {
        foreach (var cSignal in cSignals)
        {
            if (cSignal.target == target)
            {
                cSignal.enabled = false;
            }
        }
    }
    private void CreateNewSignalObj(Transform target, float duration)
    {
        var obj = Instantiate(cSignalPref, cSignalPool);
        var newCircleSignal = obj.GetComponent<CircleSignal>();
        cSignals.Add(newCircleSignal);

        newCircleSignal.enabled = true;
        newCircleSignal.Init(target, duration);
    }

    //ダメージを受けた方向によって画面が赤く光る
    //https://tsubakit1.hateblo.jp/entry/2018/02/05/235634
    public void PlayHitUIAnimation(Transform playerT,Transform enemyT,bool dead)
    {
        var diff = enemyT.transform.position - playerT.transform.position; //プレイヤー→敵
        
        if(dead)
        {
            Debug.Log("deadmu");
            deadImage.SetActive(true);
            return;
        }
        if(CheckCenter(playerT,diff))
        {
            Debug.Log("centermu");
            animator.SetTrigger("HitC");
            return;
        }
        if(CheckLeft(playerT,diff))
        {
            Debug.Log("im leftmu");
            animator.SetTrigger("HitL");
        }
        else
        {
            Debug.Log("im rightmu");
            animator.SetTrigger("HitR");
        } 
    }
    #region CheckDamageAngle
    //真ん中でダメージを受けたらtrue
    bool CheckCenter(Transform playerT,Vector3 diff)
    {
        //右判定
        var rightV = rightSphere.position - playerT.transform.position; //横方向のベクトル
        var axisR = Vector3.Cross(rightV, diff );
        var angleR = Vector3.Angle(rightV, diff ) * (axisR.y < 0 ? -1 : 1) ;
        Debug.Log("RightSphere:"+angleR);

        //左判定
        var leftV = leftSphere.position - playerT.transform.position; //横方向のベクトル
        var axisL = Vector3.Cross(leftV, diff );
        var angleL = Vector3.Angle(leftV, diff ) * (axisL.y < 0 ? -1 : 1) ;
        Debug.Log("LeftSphere:"+angleL);

        return angleR * angleL > 0;
    }
    //左でダメージを受けたらtrue
    bool CheckLeft(Transform playerT,Vector3 diff)
    {
        var forwardV = forwardSphere.position - playerT.transform.position; //正面のベクトル

        var axis = Vector3.Cross(forwardV, diff );//左右を求める
        var angle = Vector3.Angle(forwardV, diff ) * (axis.y < 0 ? -1 : 1) ;//角度を求める
        Debug.Log("ForwardSphere:"+angle);
        return angle < 0;
    }
    #endregion

    public void SetActiveGameoverPanel()
    {
        animator.SetTrigger("Gameover");
    }

    public void SetActiveGameClearPanel()
    {
        animator.SetTrigger("Gameclear");
    }

    public bool IsActiveGameClearPanel()
    {
        return gameclearPanel.activeInHierarchy;
    }
    
    public void OnButton_Result()
    {
        LoadingSceneController.LoadScene("Scene_Title");
    }

    public void OnButton_Title()
    {
        LoadingSceneController.LoadScene("Scene_Title");
    }


    //--------------------------------------------------------
    //ci0329
    //?�ン?�ィ?�ュ??
    public void SetActiveCheckContinuePanel()
    {
        checkContinuePanel.SetActive(true);
        animator.SetTrigger("CheckContinue");
    }

    public bool IsActiveContinuePanel()
    {
        return checkContinuePanel.activeInHierarchy;
    }

    public void OnButton_Continue()
    {
        GameData.state = GameData.GameState.DEFAULT;
        
        deadImage.SetActive(false);
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerHealth>().Revival();

        var cursor = checkContinuePanel.GetComponentInChildren<CursorMove>();
        animator.SetTrigger("ContinueApply");
        cursor.ResetPos();
    }

    //死亡時の赤背景を表示/非表示
    public void OnButton_DeadBG()
    {
        deadImage.SetActive(!deadImage.activeSelf);
    }

    //残弾?�初?�化
    public void InitializeShot(uint magazineSize)
    {
        //?�で?�生?�さ?�て?�た?�終�?
        if(shotImages != null) { return; }


        shotImages = new List<UnityEngine.UI.Image>();
        for(int i = 0; i < magazineSize; ++i)
        {
            var newShotBar = Instantiate(shotImagePrefab, shotImageBase.transform.parent);
            shotImages.Add(newShotBar.GetComponent<UnityEngine.UI.Image>());
            Vector3 newPosition = shotImageBase.rectTransform.localPosition;
            newPosition.y += shotImageBase.rectTransform.sizeDelta.y * i;
            shotImages[i].rectTransform.localPosition = newPosition;
        }
        shotImageBase.gameObject.SetActive(false);
        shotsAll = (int)magazineSize;
    }


    //残弾?�更??
    public void UpDateShot(uint shotsNow, int reloadNum)
    {
        if (shotsNow > 0 || reloadNum > 0)
        {
            for (int i = 0; i < shotImages.Count; ++i)
            {
                if (i < shotsNow)
                {
                    shotImages[i].color = new Color(1f, 1f, 1f, 1f);
                }
                else if(i < shotsNow + reloadNum)
                {
                    shotImages[i].color = new Color(0.5f, 1f, 0.5f, 0.2f);
                }
                else
                {
                    shotImages[i].color = new Color(1f, 1f, 1f, 0.2f);
                }
            }
        }
        else
        {
            for (int i = 0; i < shotImages.Count; ++i)
            {
                shotImages[i].color = new Color(1f, 0.5f, 0.5f, 0.2f);
            }
        }
    }

    //--------------------------------------------------------
}