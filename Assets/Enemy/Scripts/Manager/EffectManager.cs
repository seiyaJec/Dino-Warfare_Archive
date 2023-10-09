using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    //Singleton
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    private List<Effect> effectPool = new List<Effect>();

    //
    public enum EffectType
    {
        Common,
        Hit,
        WeaponHit,
        Explosion,
        Shot
    }

    [SerializeField] private Transform effectPoolTr;

    [SerializeField] private ParticleSystem commonHitEffect;
    [SerializeField] private ParticleSystem enemyHitEffect;
    [SerializeField] private ParticleSystem metalicHitEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject shotEffect;

    private Transform cursorTransform;

    private void Awake()
    {
        cursorTransform = GameObject.FindWithTag("Cursor").transform;
    }

    //Emitを使うParticleの処理専用の処理
    public void PlayEffect(Vector3 pos, Vector3 normal,
        EffectType effectType = EffectType.Common)
    {

        var targetEffect = commonHitEffect;

        if (effectType == EffectType.Hit)
        {
            targetEffect = enemyHitEffect;
        }
        else if (effectType == EffectType.WeaponHit)
        {
            targetEffect = metalicHitEffect;
        }

        targetEffect.transform.position = pos;
        targetEffect.transform.rotation = Quaternion.LookRotation(normal);
        targetEffect.Emit(1);

        targetEffect.transform.SetParent(effectPoolTr);
    }

    //EffectPoolとして管理するエフェクト(インスタンス)
    public void PlayEffect(Vector3 pos, EffectType effectType)
    {
        var targetEffect = GetEffect(effectType);
        if (targetEffect)
        {
            if (pos == Vector3.zero)
            {
                targetEffect.transform.localPosition = Vector3.zero;
            }
            else
            {
                targetEffect.transform.position = pos;
            }
            targetEffect.gameObject.SetActive(true);
        }
        else
        {
            CreateNewEffect(pos, effectType);
        }
    }

    private Effect GetEffect(EffectType effectType)
    {
        foreach (var effect in effectPool)
        {
            if (!effect.gameObject.activeSelf)
            {
                        //エフェクトの種類区分
                if (effect.type == effectType)
                {
                    return effect;
                }
            }
        }
        return null;
    }

    private void CreateNewEffect(Vector3 pos, EffectType effectType)
    {
        GameObject effectobj;
        Effect effect;
        switch (effectType)
        {
            case EffectType.Explosion:
                effectobj = Instantiate(explosionEffect, pos, Quaternion.identity, effectPoolTr);
                effect = effectobj.GetComponent<Effect>();
                if (effect) { effectPool.Add(effect); }
                break;
            case EffectType.Shot:
                effectobj = Instantiate(shotEffect, pos, Quaternion.identity, cursorTransform);
                effect = effectobj.GetComponent<Effect>();
                if (effect) { effectPool.Add(effect); }
                break;
            default:
                break;
        }
    }
}