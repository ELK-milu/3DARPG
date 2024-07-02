using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class SphereLiquid:Liquid
{
    [Header("震动幅度")]
    public float ShakeStrength = 2f;
    [Header("震动幅度")]
    public bool AllowShake = true;
    public override void DOFill (int addition)
    {
        if (addition > 0)
        {
            ShakeStrength= Mathf.Clamp(addition/1000f, 0, 1);
        }
        else
        {
            ShakeStrength = 0f;
        }
        // 圆形是-13~112，映射到100~0
        float source = (float)(fillAmount * -0.8f) + 89.6f;
        float desAddition = (float)(source - addition - 89.6) / (-0.8f);
        float des = Mathf.Clamp(desAddition, -13, 112);
        Debug.Log($"消耗值{addition},总值{source},映射值{des},晃动{ShakeStrength}");
        if (FillTween != null)
        {
            DOTween.Kill(FillTween);
            FillTween = null;
            DOFill(addition);
        }
        else
        {
            if (AllowShake)
            {
                transform.parent.DOShakePosition(2f, ShakeStrength);
            }

            FillTween =DOTween.To(() => fillAmount, x => fillAmount = x, des, 1).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    FillTween = null;
                });
        }
    }
}
