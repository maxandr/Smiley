using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
		private Animator m_Anim;            // Reference to the player's animator component.
		public  GameObject upper_part;            // Reference to the player's animator component.
		public  GameObject upper_part_center;            // Reference to the player's animator component.
        private Animator m_Anim_Upper;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

		//public GameObject projectile;
		public float fireRate = 0.0F;
		private float nextFire = 0.0F;

		public  GameObject gunpoint;            // Reference to the player's animator component.
		public  GameObject bullet_instantiate;            // Reference to the player's animator component.
		public  GameObject bullet;            // Reference to the player's animator component.
		public  float bulletSpeed=50.0f;            // Reference to the player's animator component.
		private bool aimed = false;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
			m_Anim_Upper = upper_part.GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			nextFire = Time.time;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
			m_Anim.SetBool("Ground", m_Grounded);
			m_Anim_Upper.SetBool("Ground", m_Grounded);
            // Set the vertical animation
			m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
			m_Anim_Upper.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        }

		public void Fire(Quaternion pAngle,float move) {
			//m_Anim_Upper.SetBool("FireBool",true);
			//aimed = true;			
			Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			RaycastHit2D hit = Physics2D.Raycast(new Vector2 (bullet_instantiate.transform.position.x,bullet_instantiate.transform.position.y), mousePos - new Vector2 (bullet_instantiate.transform.position.x,bullet_instantiate.transform.position.y),100);
			//Debug.DrawLine(bullet_instantiate.transform.position, (new Vector3(mousePos.x,mousePos.y) - bullet_instantiate.transform.position) * 100, Color.cyan);
			GameObject clone ;
			clone = Instantiate (bullet, bullet_instantiate.transform.position,pAngle/* bullet_instantiate.transform.rotation*/) as GameObject;
			clone.GetComponent<Rigidbody2D>().velocity = new Vector2((mousePos.x - gunpoint.transform.position.x) * bulletSpeed, (mousePos.y- gunpoint.transform.position.y) * bulletSpeed);
		}

		public void Move(float move, bool crouch, bool jump,bool isMoving)
        {
			
			m_Anim.SetBool("isMoving", isMoving);

            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }
            // Set whether or not the character is crouching in the animator
			//m_Anim.SetBool("Crouch", crouch);
			//m_Anim_Upper.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
				m_Anim.SetFloat("Speed", Mathf.Abs(move));
				m_Anim_Upper.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                { 
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
				m_Anim.SetBool("Ground", false);
				m_Anim_Upper.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
			var pos = Camera.main.WorldToScreenPoint (gunpoint.transform.position);
			var pos_smiley = Camera.main.WorldToScreenPoint (transform.position);
			var dir = Input.mousePosition - pos;
			var dir_smiley = Input.mousePosition - pos_smiley;
			//Shooting
			if (Input.GetButton ("Fire2")) {
				aimed = true;			
			} else {
				aimed = false;			
			}
			int tX = 1;
			if (Input.GetButton ("Fire1") && nextFire<=Time.time) {
				nextFire = Time.time + fireRate;
				var angle = Mathf.Atan2 (dir.y, tX * dir.x) * Mathf.Rad2Deg;
				if (move < 0) {
					Quaternion tQ = Quaternion.Euler (new Vector3 (0, 0, angle));
					tQ.x *= -1;
					Fire (tQ,move);
				} else {
					Fire (Quaternion.Euler (new Vector3 (0, 0, angle)),move);
				}
				m_Anim_Upper.SetBool("FireBool",true);
			} else {
				m_Anim_Upper.SetBool("FireBool",false);
			}
			if(Input.GetButton ("Fire1")) {
				aimed = true;	
			}
			//Rotating


			if (aimed) {
				if (dir_smiley.x < 0 && m_FacingRight) {
					Flip ();
				} else if (dir_smiley.x > 0 && !m_FacingRight) {
					Flip ();
				}
				if (!m_FacingRight) {
					tX = -1;
				}

				var angle = Mathf.Atan2 (dir.y, tX * dir.x) * Mathf.Rad2Deg;
				upper_part_center.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
				m_Anim_Upper.SetFloat("ShootAngle", angle);
			} else {
				var angle = Mathf.Atan2 (dir.y, tX * dir.x) * Mathf.Rad2Deg;
				upper_part_center.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				m_Anim_Upper.SetFloat("ShootAngle", angle);
			}


        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
