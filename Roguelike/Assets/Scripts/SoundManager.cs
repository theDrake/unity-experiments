using UnityEngine;

public class SoundManager : MonoBehaviour {
  public static SoundManager Instance { get; private set; }

  private const float _minPitch = 0.95f;
  private const float _maxPitch = 1.05f;

  public AudioSource FxSource;
  public AudioSource MusicSource;

  private void Awake() {
    if (!Instance) {
      Instance = this;
    } else {
      Destroy (gameObject);
    }
  }

  public void PlayClip(AudioClip clip) {
    FxSource.clip = clip;
    FxSource.Play();
  }

  public void PlayRandomClip(params AudioClip[] clips) {
    FxSource.pitch = Random.Range(_minPitch, _maxPitch);
    FxSource.clip = clips[Random.Range(0, clips.Length)];
    FxSource.Play();
  }
}
