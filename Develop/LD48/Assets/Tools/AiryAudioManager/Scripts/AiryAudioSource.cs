using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hang {
	namespace AiryAudio {

		public enum AiryAudioSourceAction {
			Play,
			Stop,
			Pause,
			UnPause,
		}

		public class AiryAudioSource : MonoBehaviour {

			[SerializeField] AudioSource myAudioSource;
			public AudioSource AudioSource { get { return myAudioSource; } }
			private Transform myParent;
			// Use this for initialization
//			void Start () {
//
//			}

			// Update is called once per frame
			void Update () {
				if (myParent != null) {
					if (myAudioSource.isPlaying)
						this.transform.position = myParent.position;
					else
						myParent = null;
				}
			}

			public void SetParent (Transform g_parent) {
				myParent = g_parent;
				this.transform.position = myParent.position;
			}

			public void SetAudioData (AiryAudioData g_data) {
				SetAudioClip (g_data.GetMyAudioClip ());

				if (g_data.isRandomVolume)
					SetVolume (Random.Range (g_data.myVolumeRange.x, g_data.myVolumeRange.y));
				else
					SetVolume (g_data.myVolume);

				if (g_data.isRandomPitch)
					SetPitch (Random.Range (g_data.myPitchRange.x, g_data.myPitchRange.y));
				else
					SetPitch (g_data.myPitch);
			}

			public void SetAudioClip (AudioClip g_audioClip) {
				myAudioSource.clip = g_audioClip;
			}

			public float GetVolume () {
				return myAudioSource.volume;
			}

			public void SetVolume (float g_volume) {
				myAudioSource.volume = g_volume;
			}

			public void SetPitch (float g_pitch) {
				myAudioSource.pitch = g_pitch;
			}

			public void SetPosition (Vector3 g_position) {
				this.transform.position = g_position;
			}

			public void SetPosition (Vector2 g_position) {
				SetPosition ((Vector3)g_position);
			}

			public float GetClipLength () {
				if (myAudioSource == null || myAudioSource.clip == null)
					return 0;
				return myAudioSource.clip.length;
			}

			public void Action (AiryAudioSourceAction g_action) {
				switch (g_action) {
				case AiryAudioSourceAction.Play:
					myAudioSource.Play ();
					break;
				case AiryAudioSourceAction.Stop:
					myAudioSource.Stop ();
					break;
				case AiryAudioSourceAction.Pause:
					myAudioSource.Pause ();
					break;
				case AiryAudioSourceAction.UnPause:
					myAudioSource.UnPause ();
					break;
				}
			}
		}
	}
}
