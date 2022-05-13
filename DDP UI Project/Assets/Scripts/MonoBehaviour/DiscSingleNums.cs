using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscSingleNums : ShowDiscInfo
{
    public List<TextMeshPro> numTexts;

    public override void SetDiscNum(int num, int sortingOrder)
    {
        string sNum = num.ToString();

        if (2 < sNum.Length)
            sNum = "no";

        foreach (TextMeshPro numText in numTexts)
        {
            numText.text = sNum;
            numText.sortingOrder = sortingOrder;
        }
    }
}
