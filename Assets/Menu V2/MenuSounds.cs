using UnityEngine;
using System.Collections;

public class MenuSounds : MonoBehaviour 
{
	private AudioSource _aSourceMusic;
	private AudioSource _aSourceSFX;
	public AudioClip MusicClip;
	public AudioClip SelectionSound;

	void Start () 
	{
		_aSourceMusic = gameObject.AddComponent<AudioSource>();
		_aSourceMusic.clip = MusicClip;
		_aSourceMusic.loop = true;
		if (MusicClip != null) _aSourceMusic.Play();

		_aSourceSFX = gameObject.AddComponent<AudioSource>();
		_aSourceSFX.playOnAwake = false;
	}

	public void PlaySoundEffect(AudioClip clip)
	{
		_aSourceSFX.PlayOneShot(clip);
	}

}
