using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BatchManager : MonoBehaviour
{
    private Button mButton_BatchOpen;
    private Button mButton_BatchClose;
    private Button mButton_Reset;

    private Image mImage_BatchOpen;
    private Image mImage_BatchClose;

    public Color mColor_ActiveOpenColor;
    public Color mColor_DeactiveOpenColor;

    public Color mColor_ActiveCloseColor;
    public Color mColor_DeactiveCloseColor;

    private float duration = 0.3f;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        mButton_BatchOpen = Global.FindChild<Button>(transform, "BatchOpen");
        mButton_BatchOpen.onClick.AddListener(OnButtonBatchOpen);

        mButton_BatchClose = Global.FindChild<Button>(transform, "BatchClose");
        mButton_BatchClose.onClick.AddListener(OnButtonBatchClose);

        mButton_Reset = Global.FindChild<Button>(transform, "Reset");
        mButton_Reset.onClick.AddListener(OnButtonReset);

        mImage_BatchOpen = mButton_BatchOpen.GetComponent<Image>();
        mImage_BatchClose = mButton_BatchClose.GetComponent<Image>();
    }

    private void OnButtonBatchOpen()
    {
        NetCenter.Instance.SendRequest(NetCode.BATCH_OPEN_SRES);

        if (mImage_BatchOpen.color == mColor_ActiveOpenColor)
        {
            mImage_BatchOpen.DOColor(mColor_DeactiveOpenColor, duration);
        }

        if (mImage_BatchClose.color == mColor_DeactiveCloseColor)
        {
            mImage_BatchClose.DOColor(mColor_ActiveCloseColor, duration);
        }

        mButton_BatchOpen.interactable = false;
        mButton_BatchClose.interactable = true;
    }

    private void OnButtonBatchClose()
    {
        NetCenter.Instance.SendRequest(NetCode.BATCH_CLOSE_SRES);

        if (mImage_BatchClose.color == mColor_ActiveCloseColor)
        {
            mImage_BatchClose.DOColor(mColor_DeactiveCloseColor, duration);
        }

        if (mImage_BatchOpen.color == mColor_DeactiveOpenColor)
        {
            mImage_BatchOpen.DOColor(mColor_ActiveOpenColor, duration);
        }

        mButton_BatchClose.interactable = false;
        mButton_BatchOpen.interactable = true;
    }

    private void OnButtonReset()
    {
        mButton_BatchOpen.interactable = true;
        mButton_BatchClose.interactable = true;

        if (mImage_BatchOpen.color == mColor_DeactiveOpenColor)
        {
            mImage_BatchOpen.DOColor(mColor_ActiveOpenColor, duration);
        }

        if (mImage_BatchClose.color == mColor_DeactiveCloseColor)
        {
            mImage_BatchClose.DOColor(mColor_ActiveCloseColor, duration);
        }

    }
}
