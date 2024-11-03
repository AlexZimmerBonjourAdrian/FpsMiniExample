using UnityEngine;
using System.Collections;
using Vector2 = UnityEngine.Vector2;

//[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
 {

// 	public float jumpHeight = 4;
// 	public float timeToJumpApex = .4f;
// 	float accelerationTimeAirborne = .2f;
// 	float accelerationTimeGrounded = .1f;
// 	float moveSpeed = 6;

// 	float gravity;
// 	float jumpVelocity;
// 	UnityEngine.Vector3 velocity;
// 	float velocityXSmoothing;

// 	Controller2D controller;
//  public float interactionDistance = 2f;
// 	void Start() {
// 		controller = GetComponent<Controller2D> ();

// 		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
// 		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
// 		print ("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
// 	}

// 	void Update() {

// 		if (controller.collisions.above || controller.collisions.below) {
// 			velocity.y = 0;
// 		}

// 		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

// 		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
// 			velocity.y = jumpVelocity;
// 		}

// 		float targetVelocityX = input.x * moveSpeed;
// 		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
// 		velocity.y += gravity * Time.deltaTime;
// 		controller.Move (velocity * Time.deltaTime);

// 	 	  if (Input.GetKeyDown(KeyCode.E)) // O la tecla que prefieras
//         {
			
//             Interact();
//         }
// 	}
		
// void Interact()
// {
//     // Verificar si el controlador ha detectado una colisión
   
//         // Encontrar objetos interactivos en la dirección de la cara
//         Debug.Log("Buscando objetos interactivos...");
//         RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(controller.collisions.left ? -1 : 1, 0), interactionDistance);

//         foreach (RaycastHit2D hit in hits)
//         {
//             // Buscar el componente Iinteract en el objeto colisionado
//             Iinteract interactable = hit.collider.GetComponent<Iinteract>();

//             // Si se encuentra un objeto interactivo, ejecutar la interacción
//             if (interactable != null)
//             {
//                 Debug.Log("Interactuando con: " + hit.collider.gameObject.name);
//                 interactable.Oninteract();
//                 break; // Solo interactuar con el primer objeto encontrado
//             }
//         }
    
// }

	
	
}
