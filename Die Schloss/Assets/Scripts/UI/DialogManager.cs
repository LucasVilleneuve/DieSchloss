using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    // It is not recommended to simply enqueue messages. Use AddMessage() for Evaluations
    private Queue<Message> messages = new Queue<Message>();
    private IEnumerator callback = null;
    private IEnumerator randcallback = null;

    private Message current = null;
    private string display = null;

    public DialogHandler handler = null;

    private List<string> RandomMessages;


    DialogManager()
    {
        RandomMessages = new List<string>();
        RandomMessages.Add("See the devil in I!");
        RandomMessages.Add("*grab a brush and put a little makeup*");
        RandomMessages.Add("People they just ain't no good.");
        RandomMessages.Add("If they were right, I'd agree, but it's them you know, not me!");
        RandomMessages.Add("So close, no matter how far...");
        RandomMessages.Add("I tried so hard and got so far, but in the end, it doesn't even matter.");
        RandomMessages.Add("More wood for the fires, loud neighbors");
        RandomMessages.Add("Silence like a cancer grows.");
        RandomMessages.Add("Last thing I remember, I was running for the door. I had to find a passage back to the place I was before.");
        RandomMessages.Add("I see a red door and I want it painted black");
        RandomMessages.Add("Oh my dear Heaven is a big band now!");
        RandomMessages.Add("Now hush little baby, don't you cry.");
        RandomMessages.Add("I've become comfortably numb.");
        RandomMessages.Add("So you think you can tell Heaven from Hell?");
        RandomMessages.Add("It's getting dark, too dark to see...");
        RandomMessages.Add("There's a feeling I get when I look to the west and my spirit is crying for leaving.");
        RandomMessages.Add("Everybody needs somebody, you're not the only one.");
        RandomMessages.Add("Don't you cry tonight...");
        RandomMessages.Add("Welcome to the jungle!");
        RandomMessages.Add("Can anybody finds me somebody to love <3");
        RandomMessages.Add("I sometimes wish I've never been born at all.");
        RandomMessages.Add("I don't feel so good...");
        RandomMessages.Add("There is so much dust in here...");
        RandomMessages.Add("¯\\_(ツ)_/¯");
    }

    // Called to add a new message for display.
    // Depending of its level, it will be dropped, it will replace the current message or it will be added to the queue for later display.
    // If it is added to the queue but not displayed within the given time, it will be dropped as well.
    // (Message will always be dropped and MandMsg will always replace)
    public void AddMessage(Message msg)
    {
        Debug.Log("Adding new message: [" + msg.msg + "].");
        Message.Action tmp;

        if (current == null) {
            Debug.Log("default displaying");
        }
        else {
            tmp = msg.Evaluate(current.level);
            switch (tmp)
            {
                case Message.Action.Drop:
                    Debug.Log("dropping");
                    return;
                case Message.Action.Display:
                    Debug.Log("displaying");
                    break;
                case Message.Action.Wait:
                    Debug.Log("queuing");
                    messages.Enqueue(msg);
                    return;
            }
        }
        tryStopCB();
        current = msg;
        updateDisplay();
        startCB();
    }


    private void tryStopCB()
    {
        if (callback != null)
        {
            StopCoroutine(callback);
            callback = null;
        }
        if (randcallback != null)
        {
            StopCoroutine(randcallback);
            randcallback = null;
        }
    }

    private void startCB()
    {
        callback = UpdateMessage(3);
        StartCoroutine(callback);
    }

    private IEnumerator KeepDelayedMessage(float sec, Message msg)
    {
        yield return new WaitForSeconds(sec);
        AddMessage(msg);
    }

    private void SendDelayedMessage()
    {
        string message = RandomMessages[UnityEngine.Random.Range(0, RandomMessages.Count)];
        randcallback = KeepDelayedMessage(20, new Message(message));
        StartCoroutine(randcallback);
    }

    // Called whenever the current message expires.
    // If the queue is empty, the display turns off.
    // If not, the next element in the queue is evaluated (to see if it hasn't expired) and displayed.
    public IEnumerator UpdateMessage(float sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("changing message.");
        if (messages.Count == 0)
        {
            current = null;
            this.updateDisplay("");
            // SendDelayedMessage();
        }
        else
        {
            current = getLastValidMessage();
            this.updateDisplay();
            this.startCB();
        }
    }


    // This function will recursively search for a not expired message to display.
    private Message getLastValidMessage()
    {
        if (messages.Count == 0)
            return null;
        Message tmp = messages.Dequeue();
        if (tmp.HasExpired())
            return getLastValidMessage();
        return tmp;
    }


    // called to update the variable responsible of the display.
    // If no parameter is given, it will use the current message.
    public void updateDisplay(string tmp = null)
    {
        if (tmp == null && current != null) { display = current.msg; }
        else { display = tmp; }

        if (handler) { handler.setMessage(display); }
    }
}