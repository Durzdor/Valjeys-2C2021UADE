using UnityEngine;

public class BGM : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private AudioClip loopClip;
    [SerializeField] private AudioClip reverbClip;
#pragma warning restore 649

    #endregion

    private AudioSource _audioSource;
    private bool WasLooping => _audioSource.clip == loopClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.clip = WasLooping ? reverbClip : loopClip;
        _audioSource.Play();
    }
}
