using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hang {
	namespace AiryAudio {
		public class AiryAudioSnapper : MonoBehaviour {

			[SerializeField] string myName;

			// Use this for initialization
			void Start () {
				AiryAudioManager.Instance.SetSnapshot (myName);
			}

		}
	}
}
