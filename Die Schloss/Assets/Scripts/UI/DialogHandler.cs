using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    public Text message;

    void Start()
    {
        // setting self as the handler to the manager
        // necessary for setMessage callback
        DialogManager.Instance.handler = this;

        // init
        gameObject.SetActive(false);
        message.text = null;
    }


    public void setMessage(string msg)
    {
        Debug.Log("ouioui nouveau message [" + msg + "]");
        gameObject.SetActive(!(msg == null || msg == ""));
        message.text = msg;
    }

}
