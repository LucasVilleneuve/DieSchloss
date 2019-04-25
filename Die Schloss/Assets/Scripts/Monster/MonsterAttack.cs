using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{

    public GameObject Explosion;
    private MonsterBrain mb;
    // Start is called before the first frame update
    void Start()
    {
        mb = GetComponent<MonsterBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Atack()
    {
        Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y , -1), Quaternion.identity);
        StartCoroutine("BeDead");
    }

    private IEnumerator BeDead()
    {

        yield return new WaitForSeconds(0.3f);
        mb.ActDead(true);
    }
}
