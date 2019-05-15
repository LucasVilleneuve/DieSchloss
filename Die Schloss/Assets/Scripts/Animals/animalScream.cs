using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalScream : MonoBehaviour
{
    public Transform playerTransform;

    public float minDist = 4f;

    public bool wasFound;

    
    // Start is called before the first frame update
    void Start()
    {

    }


    public virtual void Scare()
    {
        gameObject.GetComponent<Animator>().SetBool("wasFound", true);
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 1.84f);
    }

    public void CheckTrigger()
    {
        if (!wasFound && Vector2.Distance(playerTransform.position, transform.position) <= minDist)
        {
            wasFound = true;
            Scare();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckTrigger();
    }
}
