using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D {
	[RequireComponent(typeof (PlatformerCharacter2D))]
	public class WeaponSwapper : MonoBehaviour {
		private PlatformerCharacter2D m_Character;
		public enum gunTypes {
			AUTOGUN=0,
			PLAZMAGUN
		}
		public RuntimeAnimatorController Weapon1;
		public RuntimeAnimatorController Weapon2;

		public  GameObject Bullet1_prefab; 
		public  GameObject Bullet2_prefab;

		public  GameObject MainPart; 

		public gunTypes Current;
		// Update is called once per frame
		private void Awake () {
			m_Character = MainPart.GetComponent<PlatformerCharacter2D>();
			Current = gunTypes.AUTOGUN;
		}
		void Update () {
			if (Input.GetKeyDown (KeyCode.Q)) {
				SwapWeapons ();
			}
		}
		void SwapWeapons() {
			if (Current == gunTypes.AUTOGUN) {
				GetComponent<Animator> ().
				GetComponent<Animator> ().runtimeAnimatorController = Weapon2;
				Current = gunTypes.PLAZMAGUN;
				m_Character.bullet = Bullet2_prefab;
			} else {
				GetComponent<Animator> ().runtimeAnimatorController = Weapon1;
				Current = gunTypes.AUTOGUN;
				m_Character.bullet = Bullet1_prefab;
			}
		}
	}
}