using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
   Vector2 checkPoint;

   private void Start() 
   {
    checkPoint = transform.position;
   }
   
   private void Update()
   {
        if (Input.GetButtonDown("Last Checkpoint")){
            Respawn();
        }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
        if (collision.CompareTag("Obstacle")) {
            Die();
        }
   }

   public void updateCheckpoint(Vector2 pos)
   {
        checkPoint = pos;
   }

   void Die()
   {
    Respawn();
   }

    void Respawn()
    {
        transform.position = checkPoint;
    }

    public void RestartScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
