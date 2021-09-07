using UnityEngine;
using UnityEngine.UI;

public class BgMusic : MonoBehaviour
{
    AudioSource m_AudioSource;

    public Toggle m_Toggle;

    void Start()
    {
        //Fetch the AudioSource component of the GameObject (make sure there is one in the Inspector)
        m_AudioSource = GetComponent<AudioSource>();
        //Stop the Audio playing
        m_AudioSource.Stop();
        m_AudioSource.Play();
    }

    void Update()
    {
        m_AudioSource.loop = m_Toggle.isOn;
    }
}
