using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveSystem : MonoBehaviour
{
    /*
    //====Cambiar de estados
    private bool _AirControl = true;
    private bool _Ground = false;
    //=====

    //private const float MaxSpeed = 30f;
    [SerializeField] private float _JumpForce = 30f;

    //[Range(0, 3f)] [SerializeField] private float _MovementSmoothing = .95f;
    //private const float _MinjumpForce = 2f;
    //private const float _MaxJumpForce = 30f;
    private Rigidbody2D _rigidboy2D;
    private Vector3 _velocity = Vector3.zero;
    private bool _FacingRight = true;
    [SerializeField] private Transform _GroundCheck;

    [SerializeField] private Transform _CeilingCheck;
    [SerializeField] private LayerMask _WhatisGround;
     private LayerMask _PlayerLayer;
     private LayerMask _platformLayer;
    private LayerMask _floor;



    int playerLayer, plataformLayer;
    bool jumpOffCoroutineIsRunning = false;

    const float _CillingRadius = .5f;
    const float _GroundedRadius = .5f;

    //private const int STATE_STAND= 0;
    //private const int STATE_MOVE = 1;
    //private const int STATE_JUMP = 2;
    //private const int STATE_RUN = 3;
    //private const int STATE_FALL = 4;
    //private const int STATE_COUCH = 5;

    // Start is called before the first frame update


    private void Start()
    {
        _rigidboy2D = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        plataformLayer = LayerMask.NameToLayer("Plataform");

        _floor = LayerMask.NameToLayer("Floor");

        #region zona de saludos
        _GroundCheck = transform.Find("Suelo");
        _CeilingCheck = transform.Find("Techo");
        #endregion
    }

    // Update is called once per frame

    #region Collision Controller
    private void FixedUpdate()
    {


        
        _Ground = false;
      

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_GroundCheck.position, _GroundedRadius, _WhatisGround);


        for (int i = 0; i < colliders.Length; i++)
        {
            _Ground = true;
            if (colliders[i].gameObject != gameObject)
            {
                //if (colliders[i].gameObject != )
                  
                if (colliders[i].gameObject.layer == _platformLayer)
                {


                }


            }
            
            _Ground = true;

            if (colliders[i].gameObject != gameObject)
            {
              
                if (colliders[i].gameObject.layer == plataformLayer)
                {
                    Debug.Log("Toque La Plataform" + plataformLayer);
                }

               
                
                if (_rigidboy2D.velocity.y <= 0)
                {
                    // Debug.Log("Esta en el suelo " + jumpOffCoroutineIsRunning + "isFloor " + _Ground);
                    Debug.Log("Bajando");
                    Physics2D.IgnoreLayerCollision(playerLayer, 11, false);

                }
               

            }
            
            if (_rigidboy2D.velocity.y > 0)

            {
                Debug.Log("Subiendo");
                Physics2D.IgnoreLayerCollision(playerLayer, 11, true);
            }
            else if (_rigidboy2D.velocity.y <= 0 && !jumpOffCoroutineIsRunning)
            {
                // Debug.Log("Esta en el suelo " + jumpOffCoroutineIsRunning + "isFloor " + _Ground);
                Debug.Log("Bajando");
                Physics2D.IgnoreLayerCollision(playerLayer, 11, false);

            }
            
        }




    }


    #endregion




    public void MovePlayer(float move, bool jump)
    {
        if (_Ground)
        {


            //===========================================MoveFunction
            #region Move Function
            Vector3 targetVelocity = new Vector3(move*10, _rigidboy2D.velocity.y);
            #endregion
            _rigidboy2D.velocity = new Vector2(targetVelocity.x, _rigidboy2D.velocity.y);
            //  _rigidboy2D.velocity = Vector3.SmoothDamp(_rigidboy2D.velocity, targetVelocity, ref _velocity, 0f);//_MovementSmoothing);
            //_rigidboy2D.velocity = new Vector3(targetVelocity.x, _rigidboy2D.velocity.y);
            //=======================================
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                _rigidboy2D.velocity = Vector2.zero;
            }

            //==============================Flip condition
            if (move > 0 && !_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && _FacingRight)
            {
                Flip();
            }
            //=====================================

            //=============================Jump Part
        }

        
        if (_Ground && jump)
        {
            
            _Ground = false;
            _rigidboy2D.AddForce(new Vector2(0f, _JumpForce));
            
            
        }
        
        
        //=============================
    }
    //public void incrementVelDown(float DownForz)
    //{
    //    this.transform.position = new Vector2(Vector2.zero.x, this.transform.position.y - (DownForz * Time.deltaTime));
    //}
    //public void incrementGravDown(float DownForce)
    //{
    //    rigidboy2D.gravityScale=DownForce;
    //}
    //public void DashDown(float dashForce)
    //{
    //    rigidboy2D.AddForce(new Vector2(0f, _JumpForce));
    //}

    //FLIP del personaje basico ajustar despues, para crear 
    #region flip
        
    private void Flip()
    {
        _FacingRight = !_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    //==============================================
    #endregion


    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_GroundCheck.position, _GroundedRadius);
            Gizmos.DrawWireSphere(_CeilingCheck.position, _GroundedRadius);
        }
        
        
    IEnumerator jumpOff()
    {
        jumpOffCoroutineIsRunning = true;
        Physics2D.IgnoreLayerCollision(_PlayerLayer, _platformLayer, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(_PlayerLayer, _platformLayer, false);
        jumpOffCoroutineIsRunning = false;

    }
    
    private bool getIsFlip()
    {
        return _FacingRight;
    }
  
}


    
        private void Update()
        {
            Debug.Log(_Ground);
        }
       
        private void OnCollisionEnter2D(Collision2D gamecolision)
        {
            if(gamecolision.gameObject.tag == "Floor")
            {
                Debug.Log("Entra");
                _Ground = true;
                _AirControl = false;
            }
        }
        */
}
 



