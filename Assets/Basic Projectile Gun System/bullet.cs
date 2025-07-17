using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class bullet : MonoBehaviour
{
    public IEnumerator Initialise(float range)
    {
        yield return new WaitForSeconds(range);
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        if (ObjectPooler.instance != null) ObjectPooler.instance.ReturnObject(gameObject, 1);
    }
}
