using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hang {
	namespace AiryAudio {
		public class AiryAudioPlayer : MonoBehaviour {

			[SerializeField] AiryAudioData[] myAudioDatas;
			[SerializeField] bool playOnStart;


			// Use this for initialization
			void Start () {
				if (playOnStart && myAudioDatas != null) {
					AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioDatas);
					AiryAudioActions.Play (t_source, this.transform);
				}
			}

			public void PlayIndex (int t_index) {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioDatas [t_index]);
				AiryAudioActions.Play (t_source, this.transform);
			}

			public void PlayName (string t_name) {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (t_name);
				AiryAudioActions.Play (t_source, this.transform);
			}

			public void Play () {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioDatas);
				AiryAudioActions.Play (t_source, this.transform);
			}
		}
	}
}
