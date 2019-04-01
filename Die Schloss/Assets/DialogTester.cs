using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTester : MonoBehaviour
{

    public Button test1, test2, test3, test4;
    private int i1, i2, i3, i4;

    void Start()
    {
        test1.onClick.AddListener(AddMessage);
        test2.onClick.AddListener(AddMedMsg);
        test3.onClick.AddListener(AddHighMsg);
        test4.onClick.AddListener(AddMandMsg);
    }

    void AddMessage()
    {
        DialogManager.Instance.AddMessage(new Message("low msg " + i1++));
    }

    void AddMedMsg()
    {
        DialogManager.Instance.AddMessage(new MedMsg("med msg " + i2++));
    }

    void AddHighMsg()
    {
        DialogManager.Instance.AddMessage(new HighMsg("high msg " + i3++));
    }

    void AddMandMsg()
    {
        DialogManager.Instance.AddMessage(new MandMsg("mand msg " + i4++));
    }
}
