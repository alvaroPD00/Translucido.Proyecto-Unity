using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   GameController gameController;

   private void Awake()
   {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
        if(collision.CompareTag("Player")) {
            gameController.updateCheckpoint(transform.position);
        }
   }
}
 