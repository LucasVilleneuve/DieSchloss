using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{

    public GameObject Explosion;
    private MonsterBrain mb;
    public PlayerStateMachine psm;
    // Start is called before the first frame update
    public MonsterStateMachine msm;

    void Start()
    {
        mb = GetComponent<MonsterBrain>();
        msm = GetComponent<MonsterStateMachine>();

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Atack()
    {
        Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y , -1), Quaternion.identity);
        StartCoroutine("BeDead");
        yield return StartCoroutine(psm.TakeDammage(1));
        msm.EndTurn();

    }

    private IEnumerator BeDead()
    {

        yield return new WaitForSeconds(0.3f);
        mb.ActDead(true);
    }
}
