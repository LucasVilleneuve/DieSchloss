using UnityEngine;

public class catScream : animalScream
{

    public float speed = 6;

    void Start()
    {
    }

    public override void Scare()
    {
        wasFound = true;
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<Animator>().SetBool("wasFound", true);
        Destroy(gameObject, 2.2f);
    }


    void Update()
    {
        CheckTrigger();
        if (wasFound)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

}
