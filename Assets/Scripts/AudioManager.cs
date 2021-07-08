using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _foregroundSoundContainer;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayLaserSound()
    {
        AudioSource.PlayClipAtPoint(_laserSoundClip, transform.position);
    }

    public void PlayExplosionSound()
    {
        AudioSource.PlayClipAtPoint(_explosionSoundClip, transform.position);
    }

}
