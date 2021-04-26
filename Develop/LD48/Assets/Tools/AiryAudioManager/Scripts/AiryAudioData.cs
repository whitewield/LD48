using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hang {
	namespace AiryAudio {
		[CreateAssetMenu(fileName = "AiryAudioData", menuName = "Hang/AiryAudio/AiryAudioData", order = 2)]
		public class AiryAudioData : ScriptableObject {
			public string myName;
			public AudioClip[] myAudioClips;

			[Header ("Volume")]
			public float myVolume = 1;
			public bool isRandomVolume = false;
			public Vector2 myVolumeRange = Vector2.one;

			[Header ("Pitch")]
			public float myPitch = 1;
			public bool isRandomPitch = false;
			public Vector2 myPitchRange = Vector2.one;

			public AudioClip GetMyAudioClip () {
				return myAudioClips [Random.Range (0, myAudioClips.Length)];
			}

			public void Play () {
				if (AiryAudioManager.Instance == null) {
					Debug.LogError ("AiryAudioManager is missing!");
					return;
				}

				AiryAudioActions.Play (AiryAudioManager.Instance.InitAudioSource (this));
			}

			public void Play (Transform g_parent) {
				if (AiryAudioManager.Instance == null) {
					Debug.LogError ("AiryAudioManager is missing!");
					return;
				}

				AiryAudioActions.Play (AiryAudioManager.Instance.InitAudioSource (this), g_parent);
			}

			public void Play (Vector3 g_position) {
				if (AiryAudioManager.Instance == null) {
					Debug.LogError ("AiryAudioManager is missing!");
					return;
				}

				AiryAudioActions.Play (AiryAudioManager.Instance.InitAudioSource (this), g_position);
			}

			public void Play (Vector2 g_position) {
				if (AiryAudioManager.Instance == null) {
					Debug.LogError ("AiryAudioManager is missing!");
					return;
				}

				AiryAudioActions.Play (AiryAudioManager.Instance.InitAudioSource (this), g_position);
			}

			public void PlayScheduled (double g_startTime) {
				if (AiryAudioManager.Instance == null) {
					Debug.LogError ("AiryAudioManager is missing!");
					return;
				}

				AiryAudioActions.PlayScheduled (AiryAudioManager.Instance.InitAudioSource (this), g_startTime);
			}
		}
	}
}