using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hang {
	namespace AiryAudio {
		[CreateAssetMenu(fileName = "AiryAudioBank", menuName = "Hang/AiryAudio/AiryAudioBank", order = 1)]
		public class AiryAudioBank : ScriptableObject {

			[SerializeField] List<AiryAudioSnapshot> mySnapshots;

			[SerializeField] List<AiryAudioData> myBank;

			public List<AiryAudioSnapshot> GetMySnapshots () {
				return mySnapshots;
			}

			public List<AiryAudioData> GetMyBank () {
				return myBank;
			}
		}
	}
}