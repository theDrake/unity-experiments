using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
  public AudioSource fxSource;
  public AudioSource musicSource;
  public static SoundManager instance = null;
  public float minPitch = 0.95f;
  public float maxPitch = 1.05f;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy (gameObject);
      DontDestroyOnLoad (gameObject);
    }
  }

  public void PlaySingle(AudioClip clip) {
    fxSource.clip = clip;
    fxSource.Play();
  }

  public void RandomizeSfx(params AudioClip[] clips) {
    fxSource.pitch = Random.Range(minPitch, maxPitch);
    fxSource.clip = clips[Random.Range(0, clips.Length)];
    fxSource.Play();
  }
}