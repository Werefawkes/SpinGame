using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
	[SelfFill(true)] public AudioSource source;


	/*	Variables
	 *	1. Clip
	 *	2. Pitch
	 *	3. Volume
	 */
	public void PlaySound(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}

	public void PlaySound(AudioClip clip, float pitch)
	{
		source.pitch = pitch;
		source.PlayOneShot(clip);
	}

	public void PlaySound(AudioClip clip, float pitch, float volume)
	{
		source.pitch = pitch;
		source.PlayOneShot(clip, volume);
	}

	public void PlayRandom(AudioClip[] clips)
	{
		AudioClip clip = clips[Random.Range(0, clips.Length)];

		PlaySound(clip);
	}

	public void PlayRandom(AudioClip[] clips, Vector2 pitchRange)
	{
		AudioClip clip = clips[Random.Range(0, clips.Length)];
		float pitch = Random.Range(pitchRange.x, pitchRange.y);

		PlaySound(clip, pitch);
	}

	public void PlayRandom(AudioClip[] clips, Vector2 pitchRange, Vector2 volumeRange)
	{
		AudioClip clip = clips[Random.Range(0, clips.Length)];
		float pitch = Random.Range(pitchRange.x, pitchRange.y);
		float volume = Random.Range(volumeRange.x, volumeRange.y);

		PlaySound(clip, pitch, volume);
	}
}
