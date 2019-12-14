using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;


        Vector3 movement;
        Animator anim;
        Rigidbody playerRigidbody;
		#if !MOBILE_INPUT
            int floorMask;
            float camRayLength = 100f;
		#endif

        void Awake ()
        {
			#if !MOBILE_INPUT
                floorMask = LayerMask.GetMask ("Floor");
			#endif
            
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void FixedUpdate ()
        {
			float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
			float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
            Move (h, v);
            Turning ();
            Animating (h, v);
        }


        void Move (float h, float v)
        {
            movement.Set (h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;
            playerRigidbody.MovePosition (transform.position + movement);
        }


        void Turning ()
        {
			#if !MOBILE_INPUT
			{
	            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
	            RaycastHit floorHit;
	            if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
	            {
	                Vector3 playerToMouse = floorHit.point - transform.position;
	                playerToMouse.y = 0f;
	                Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);
	                playerRigidbody.MoveRotation (newRotatation);
	            }
			}
			#else
			{
	            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal2") , 0f , CrossPlatformInputManager.GetAxisRaw("Vertical2"));

	            if (turnDir != Vector3.zero)
	            {
	                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;
	                playerToMouse.y = 0f;
	                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
	                playerRigidbody.MoveRotation(newRotatation);
	            }
			}
            #endif
        }


        void Animating (float h, float v)
        {
            bool walking = h != 0f || v != 0f;
            anim.SetBool ("IsWalking", walking);
        }
    }
}