using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator; 

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;

	//Variables that manage the array of sprites and the current index
    public Sprite[] sprites;  // Assign multiple sprites in the Inspector
	private SpriteRenderer spriteRenderer;
	private int currentSpriteIndex = 0;

	//Variables that define the size of the sprite and the original size.
	public Vector3 newSize = new Vector3(0.7f, 0.7f, 1f); // New size for the sprite
    public Vector3 originalSize;

	//rigid body variables
	private Rigidbody2D rigidBody;
	private float originalGravityScale;  // Store the original gravity scale

	//Collider variables
	private CapsuleCollider2D capsuleCollider;
    private Vector2 originalOffset;
    private Vector2 capsuleOriginalSize;

	//CONSTANTS 1 = Sprite Default / 0 = tiny sprite / 2 = Big Sprite / ------------------------------------------------------
	public static readonly Vector3 ORIGINAL_SIZE_SPRITES_0_1 = new Vector3(0.5f, 0.5f, 1f);
	public static readonly Vector3 BIG_SIZE_SPRITE_2 = new Vector3(0.7f, 0.7f, 1f);

	//COLLIDER SIZE AND OFFSET FOR SPRITE 1
	public static readonly Vector2 COLLIDER_CAPSULE_SIZE_1 = new Vector2(1.0282f, 4.3126f);
	public static readonly Vector2 COLLIDER_CAPSULE_OFFSET_1 = new Vector2(0.0234f, 1.8878f);
	
	//COLLIDER SIZE AND OFFSET FOR SPRITE 2
	public static readonly Vector2 COLLIDER_CAPSULE_SIZE_2 = new Vector2(3.7940f, 3.7940f);
	public static readonly Vector2 COLLIDER_CAPSULE_OFFSET_2 = new Vector2(-0.0758f, 2.0734f);

	//COLLIDER SIZE AND OFFSET FOR SPRITE 0
	public static readonly Vector2 COLLIDER_CAPSULE_SIZE_0 = new Vector2(2.3883f, 1.3793f);
	public static readonly Vector2 COLLIDER_CAPSULE_OFFSET_0 = new Vector2(0.0767f, 0.7510f);




	void Start()
    {
        // Save the original size of the sprite
        originalSize = new Vector3(0.5f, 0.5f, 1f);
		newSize = new Vector3(0.7f, 0.7f, 1f); // Overwrite Inspector value

		//Get Renderer Component for re-rendering other sprites
		spriteRenderer = GetComponent<SpriteRenderer>();

		rigidBody = GetComponent<Rigidbody2D>();
		originalGravityScale = rigidBody.gravityScale;
		
		capsuleCollider = GetComponent<CapsuleCollider2D>();

		originalOffset = capsuleCollider.offset;
        capsuleOriginalSize = capsuleCollider.size;

    }
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetMouseButtonDown(0)) // Press right arrow to switch
        {
            if (currentSpriteIndex < 2)
            {
                currentSpriteIndex++;  // Increase index up to max 2
				if (currentSpriteIndex == 2) {
					animator.enabled = false;
					OverrideSprite(sprites[currentSpriteIndex]);
				} else {
					OverrideSprite(sprites[currentSpriteIndex]);
				}
                // spriteRenderer.sprite = sprites[currentSpriteIndex];
				// animator.enabled = true;
				
				

				//IF the sprite is the big one (2) apply negative gravity scale. 
				if (currentSpriteIndex == 2) {
					animator.SetBool("isFloating", true);
					animator.SetBool("isChato", false);
					capsuleCollider.size = COLLIDER_CAPSULE_SIZE_2; 
					capsuleCollider.offset = COLLIDER_CAPSULE_OFFSET_2;
					transform.localScale = newSize;
					animator.enabled = true;
					FloatSprite();
					// animator.enabled = true;
					
				//If the sprite is another one, apply original size and possitive gravity scale.
				} else {
					animator.enabled = false;
					animator.SetBool("isFloating", false);
					animator.SetBool("isChato", false);
					transform.localScale = originalSize;
					capsuleCollider.size = COLLIDER_CAPSULE_SIZE_1;
					capsuleCollider.offset = COLLIDER_CAPSULE_OFFSET_1;
					capsuleCollider.direction = CapsuleDirection2D.Vertical;
					// capsuleCollider.enabled = enabled;
					DefloatSprite();
				}
                Debug.Log("Index increased: " + currentSpriteIndex);
				
            }
        }

		if (Input.GetMouseButtonDown(1)) // Press right arrow to switch
        {
            if (currentSpriteIndex > 0)
            {
				// animator.enabled = false;
                currentSpriteIndex--;  // Decrease index down to min 0
                OverrideSprite(sprites[currentSpriteIndex]);
				// animator.enabled = true;
				if (currentSpriteIndex == 1) {
					animator.SetBool("isFloating", false);
					animator.SetBool("isChato", false);
					capsuleCollider.direction = CapsuleDirection2D.Vertical;
					capsuleCollider.size = COLLIDER_CAPSULE_SIZE_1;
					capsuleCollider.offset = COLLIDER_CAPSULE_OFFSET_2;
					transform.localScale = originalSize;

					//If the gravity is different from the original AND the sprite is the default, set it to original (to fall)
					if (rigidBody.gravityScale != originalGravityScale) {
						DefloatSprite();
					}


				} else {
					animator.SetBool("isChato", true);
					transform.localScale = originalSize;
					capsuleCollider.direction = CapsuleDirection2D.Horizontal;
					capsuleCollider.size = COLLIDER_CAPSULE_SIZE_0;
					capsuleCollider.offset = COLLIDER_CAPSULE_OFFSET_0;
					jump = false;

				}
                Debug.Log("Index decreased: " + currentSpriteIndex);
            }
        }

		if (Input.GetButtonDown("Jump")){
			if (currentSpriteIndex == 0 || currentSpriteIndex == 2) {
				jump = false;
			} else {
				jump = true;
				animator.SetBool("isJumping", true);
				Debug.Log("Jump button pressed");
				
			}
		}

	}

	void OverrideSprite(Sprite newSprite)
	{
		spriteRenderer.sprite = newSprite;
		if (!controller.IsFacingRight) {
			spriteRenderer.flipX = true;
		}
		else {
			spriteRenderer.flipX = false;
		}
		// if (!controller.IsFacingRight)
		// {
	
		// }
	}

	void FlipSprite()
	{
		
	}


	public void onLanding() 
	{
		animator.SetBool("isJumping", false);
		animator.enabled = true;
	}

	 void FloatSprite()
    {
        rigidBody.gravityScale = -0.7f;
    }

	void DefloatSprite()
    {
        rigidBody.gravityScale = originalGravityScale;
    }

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
