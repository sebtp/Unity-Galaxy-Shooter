using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(_clip, transform.position);
        Destroy(gameObject, 3.0f);
    }

}
