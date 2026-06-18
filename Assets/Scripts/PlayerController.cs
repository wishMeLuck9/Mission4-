using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 150f;
    private InputSystem_Actions controls;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

      void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

   
     void OnEnable()
    {
        controls.Player.Enable();
       
    }

     void Update(){
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float fowardInput = moveInput.x;
        playerRb.AddForce(focalPoint.transform.forward * fowardInput * playerSpeed * Time.deltaTime);
        powerupIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Powerup")) {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }
    IEnumerator PowerupCountdownRoutine(){
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup){
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}
