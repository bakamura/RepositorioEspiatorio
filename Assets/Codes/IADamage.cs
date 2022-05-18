using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IADamage : MonoBehaviour {
    public int lives = 10;
    public IAStarFPS iastar;
    // Update is called once per frame
    void Update() {
        if (lives < 0 && iastar.state != IAStarFPS.States.dead) {
            iastar.Dead();
            Destroy(gameObject, 4);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (iastar.state != IAStarFPS.States.dead) {
            if (collision.gameObject.CompareTag("PlayerProjectile")) {
                if (iastar.enemyType == IAStarFPS.EnemyTypes.passive) {
                    iastar._isAgressive = true;
                    iastar.enemyType = IAStarFPS.EnemyTypes.agressive;
                }

                lives--;
                iastar.Damage();
            }
            else if (collision.gameObject.CompareTag("Player")) {
                switch (iastar.enemyType) {
                    case IAStarFPS.EnemyTypes.bully:
                        collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * iastar.knockbackForce, ForceMode.Impulse);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ExplosionDamage() {
        lives = -1;
    }
}
