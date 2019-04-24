using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BeDead");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BeDead()
    {

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
