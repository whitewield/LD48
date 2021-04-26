//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Hang {
	namespace AiryAudio {

		[System.Serializable]
		public struct AiryAudioSnapshot {
			public string myName;
			public AudioMixerSnapshot mySnapshot;
		}
//		public struct AiryAudioClip {
//			public AiryAudioClip (AudioClip g_audioClip, float g_baseVolume) : this() {
//				this.myAudioClip = g_audioClip;
//				this.myBaseVolume = g_baseVolume;
//
//			}
//			public AudioClip myAudioClip;
//			public float myBaseVolume;
//		}

		public static class AiryAudioActions {
			public static void Play (AiryAudioSource g_airyAudioSource) {
				if (g_airyAudioSource == null)
					return;
				
				g_airyAudioSource.SetPosition (Vector3.zero);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Vector3 g_position) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetPosition (g_position);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Vector2 g_position) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetPosition (g_position);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Transform g_parent) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetParent (g_parent);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void PlayScheduled (AiryAudioSource g_airyAudioSource, double g_startTime) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}

				g_airyAudioSource.AudioSource.PlayScheduled (g_startTime);
			}

			public static void SetVolume (AiryAudioSource g_airyAudioSource, float g_volume) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetVolume (g_volume * g_airyAudioSource.GetVolume ());
			}

			public static void SetRandomVolume (AiryAudioSource g_airyAudioSource, float g_minVolume, float g_maxVolume) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetVolume (Random.Range (g_minVolume, g_maxVolume) * g_airyAudioSource.GetVolume ());
			}

			public static void SetPitch (AiryAudioSource g_airyAudioSource, float g_pitch) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetPitch (g_pitch);
			}

			public static void SetRandomPitch (AiryAudioSource g_airyAudioSource, float g_minPitch, float g_maxPitch) {
				if (g_airyAudioSource == null) {
					Debug.LogError ("AiryAudioSource doesn't exist!");
					return;
				}
				
				g_airyAudioSource.SetPitch (Random.Range (g_minPitch, g_maxPitch));
			}

			//Edit based on Matt Boch's code
			public static float RemapRange (float g_input, float g_inputFrom, float g_inputTo, float g_outputFrom, float g_outputTo) {
				//need to test

				//make sure the value between g_inputFrom and g_inputTo;
				float t_input = g_input;
				if (g_inputFrom < g_inputTo)
					t_input = Mathf.Clamp (g_input, g_inputFrom, g_inputTo);
				else 
					t_input = Mathf.Clamp (g_input, g_inputTo, g_inputFrom);

				float t_inputRange = (g_inputTo - g_inputFrom);
				float t_outputRange = (g_outputTo - g_outputFrom);
				return (((t_input - g_inputFrom) * t_outputRange) / t_inputRange) + g_outputFrom;
			}
		}

		public class AiryAudioManager : MonoBehaviour {

			private static AiryAudioManager instance = null;
			public static AiryAudioManager Instance { get { return instance; } }

			[SerializeField] AiryAudioBank myAiryAudioBank;
			private Dictionary<string, AiryAudioData> myBank = new Dictionary<string, AiryAudioData> ();
			private Dictionary<string, AudioMixerSnapshot> mySnapshots = new Dictionary<string, AudioMixerSnapshot> ();

			[SerializeField] GameObject myPool_Prefab;
			[SerializeField] int myPool_Size = 50;
			private List<AiryAudioSource> myPool = new List<AiryAudioSource> ();

			[SerializeField] AudioMixer myAudioMixer;
			[SerializeField] float myAudioMixerTransitionTime = 0.5f;

			void Awake () {
				if (instance != null && instance != this) {
					Destroy(this.gameObject);
					return;
				} else {
					instance = this;
				}

				if (this.transform.parent == null) {
					DontDestroyOnLoad (this.gameObject);
				}

				// init bank
				foreach (AiryAudioData t_data in myAiryAudioBank.GetMyBank()) {
					if (myBank.ContainsKey (t_data.myName)) {
						Debug.LogError ("already have " + t_data.myName + " in my bank!");
						continue;
					}
						
					myBank.Add (t_data.myName, t_data);
				}

				// init snapshot
				foreach (AiryAudioSnapshot t_data in myAiryAudioBank.GetMySnapshots()) {
					if (myBank.ContainsKey (t_data.myName)) {
						Debug.LogError ("already have " + t_data.myName + " in my snapshots!");
						break;
					}

					mySnapshots.Add (t_data.myName, t_data.mySnapshot);
				}

				// init pool
				for (int i = 0; i < myPool_Size; i++) {
					myPool.Add (Instantiate (myPool_Prefab, this.transform).GetComponent<AiryAudioSource> ());
				}
			}

			public AiryAudioData GetAudioData (string g_dataName) {
				return myBank [g_dataName];
			}

			public AiryAudioSource InitAudioSource (AiryAudioData[] g_datas) {
				return InitAudioSource (g_datas [Random.Range (0, g_datas.Length)]);
			}

			public AiryAudioSource InitAudioSource (AiryAudioData g_data) {
				AiryAudioSource t_audioSource = null;
				for (int i = 0; i < myPool.Count; i++) {
					if (myPool [i].AudioSource.isPlaying == false)
						t_audioSource = myPool [i];
				}
				if (t_audioSource == null) {
					Debug.Log ("Can not find idle audio source!, creating a new one");
					t_audioSource = Instantiate (myPool_Prefab, this.transform).GetComponent<AiryAudioSource> ();
					myPool.Add (t_audioSource);
				}

				t_audioSource.SetAudioData (g_data);

				return t_audioSource;
			}

			public AiryAudioSource InitAudioSource (string[] g_dataNames) {
				return InitAudioSource (g_dataNames [Random.Range (0, g_dataNames.Length)]);
			}

			public AiryAudioSource InitAudioSource (string g_dataName) {
				if (myBank.ContainsKey (g_dataName))
					return InitAudioSource (myBank [g_dataName]);

				Debug.Log ("Can not find " + g_dataName + " in audio bank!");
				return null;
			}

			public void SetSnapshot (string g_name) {
				if (mySnapshots.ContainsKey (g_name) == false) {
					Debug.Log ("Can not find " + g_name + " in snapshots!");
					return;
				}

				AudioMixerSnapshot[] t_snapshots = { mySnapshots [g_name] };
				float[] t_weights = { 1 };
				myAudioMixer.TransitionToSnapshots (t_snapshots, t_weights, myAudioMixerTransitionTime);
			}

		}

	}
}
