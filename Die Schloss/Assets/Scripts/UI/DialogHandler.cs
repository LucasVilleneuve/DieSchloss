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
        setVisible(false);
        message.text = null;
    }


    void setVisible(bool val) { gameObject.SetActive(val); }
     

    public void setMessage(string msg)
    {
        setVisible(!(msg == null || msg == ""));
        message.text = msg;
    }

}
