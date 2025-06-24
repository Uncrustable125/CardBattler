using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }


    //Changes the HP bar in a smooth manner
    public IEnumerator SetHPSmooth(float newHp)
    {
        //grabs curHp from healthbar
        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;
        while(curHp - newHp > Mathf.Epsilon){
            curHp -= changeAmt = Time.deltaTime;
            health.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHp, 1f);

    }
}
