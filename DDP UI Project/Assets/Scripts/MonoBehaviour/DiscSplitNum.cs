using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscSplitNum : ShowDiscInfo
{
    [Header("References")]
    public TextMeshPro leftNum;
    public TextMeshPro rightNum;

    public override void SetDiscNum(int num, int sortingOrder)
    {
        string sNum = num.ToString();

        if (sNum.Length == 1)
        {
            leftNum.text = "0";
            rightNum.text = sNum;
        }
        else if (sNum.Length == 2)
        {
            leftNum.text = sNum.Substring(0, 1);
            rightNum.text = sNum.Substring(1);
        }
        else
        {
            string l = "n";
            string r = "o";

            leftNum.text = l;
            rightNum.text = r;
        }

        leftNum.sortingOrder = sortingOrder;
        rightNum.sortingOrder = sortingOrder;
    }
}
