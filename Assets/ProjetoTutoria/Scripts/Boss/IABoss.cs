using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IABoss : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float attackInterval;
    [SerializeField] private float minAttackInterval = 1f;
    [SerializeField] private float attackIntervalReduction = .2f;
    [SerializeField] private float rotationSpeed;
    [SerializeField, Tooltip("minion, shockwave, laser, explosion, bullet")] private GameObject[] attackPrefabs = new GameObject[5]; 
    [SerializeField, Tooltip("minion, shockwave, laser, explosion, bullet")] private sbyte[] maxAttackAmounts = new sbyte[5];
    [SerializeField] private Transform[] minionsSpawnPoint;
    [SerializeField] private Transform[] laserSpawnPoint;
    [SerializeField] private Transform bulletSpawnPoint;
    private NavMeshAgent agent;
    private Animator anim;
    private SkinnedMeshRenderer render;
    private Coroutine currentAttack = null;
    private readonly List<IAStarFPS> minionsList = new List<IAStarFPS>();
    private readonly List<ShockWave> shockwaveList = new List<ShockWave>();
    private readonly List<Laser> laserList = new List<Laser>();
    private readonly List<Explosion> explosionList = new List<Explosion>();
    private readonly List<Bullet> bulletList = new List<Bullet>();
    private Vector3 currentMovepoint = Vector3.zero;

    public enum AttackPaterns {
        sumon,
        shockwave,
        laser,
        explosion,
        shoot,
        movement
    };
    public AttackPaterns AttackPatern;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        render = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    private void Start() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.sumon]; i++) {
            IAStarFPS enemy = Instantiate(attackPrefabs[(int)AttackPaterns.sumon], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<IAStarFPS>();
            minionsList.Add(enemy);
            StartCoroutine(enemy.Activate(false, 0));
        }
        for(int i = 0;i < maxAttackAmounts[(int)AttackPaterns.shockwave];i++) {
            ShockWave attack = Instantiate(attackPrefabs[(int)AttackPaterns.shockwave], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponentInChildren<ShockWave>(); 
            shockwaveList.Add(attack);
            attack.Activate(false);
        }
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.laser]; i++) {
            Laser attack = Instantiate(attackPrefabs[(int)AttackPaterns.laser], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<Laser>();
            laserList.Add(attack);
            attack.Activate(false);
        }
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.explosion]; i++) {
            Explosion attack = Instantiate(attackPrefabs[(int)AttackPaterns.explosion], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<Explosion>();
            explosionList.Add(attack);
            attack.Activate(false, target.transform.position);
        }
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.shoot]; i++) {
            Bullet attack = Instantiate(attackPrefabs[(int)AttackPaterns.shoot], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<Bullet>();
            bulletList.Add(attack);
            attack.Activate(false, target.transform.position, target.transform);
        }
    }
    private void Update() {
        StateMachine();
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("PlayerProjectile")) {
            if (attackInterval > minAttackInterval) {
                StartCoroutine(ReturnDamage());
                attackInterval -= attackIntervalReduction;
            }
            else {
                StopCoroutine(currentAttack);
                currentAttack = null;
                anim.SetBool("Attack", false);
                anim.SetBool("Dead", true);
                anim.SetBool("Damage", false);
                Invoke(nameof(Destroy), 1);
            }
        }
    }
    IEnumerator ReturnDamage() {
        for (int i = 0; i < 4; i++) {
            render.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
            render.material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
        }

    }
    void StateMachine() {
        if (currentAttack == null) {
            switch (AttackPatern) {
                case AttackPaterns.sumon:
                    currentAttack = StartCoroutine(SumonMinionsAttack());
                    break;
                case AttackPaterns.shockwave:
                    currentAttack = StartCoroutine(ShockWaveAttack());
                    break;
                case AttackPaterns.laser:
                    currentAttack = StartCoroutine(Laser());
                    break;
                case AttackPaterns.explosion:
                    currentAttack = StartCoroutine(Explosion());
                    break;
                case AttackPaterns.shoot:
                    currentAttack = StartCoroutine(Shoot());
                    break;
                case AttackPaterns.movement:
                    Movement();
                    break;
            }
        }
        if (currentMovepoint != Vector3.zero) LookAtTarget();
    }
    void ChangeState(bool starMovment) {
        if (!starMovment) {
            switch (Random.Range(0, (int)AttackPaterns.movement)) {
                case 0:
                    AttackPatern = AttackPaterns.sumon;
                    break;
                case 1:
                    AttackPatern = AttackPaterns.shockwave;
                    break;
                case 2:
                    AttackPatern = AttackPaterns.laser;
                    break;
                case 3:
                    AttackPatern = AttackPaterns.explosion;
                    break;
                case 4:
                    AttackPatern = AttackPaterns.shoot;
                    break;
            }
        }
        else AttackPatern = AttackPaterns.movement;
        currentAttack = null;
    }
    void Movement() {
        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
        if (currentMovepoint == Vector3.zero) {
            agent.isStopped = false;
            int index = Random.Range(0, movePoints.Length);
            if (Vector3.Distance(transform.position, movePoints[index].position) <= 1f) {
                if (index == movePoints.Length - 1) index = 0;
                else index++;
            }
            currentMovepoint = movePoints[index].position;
            agent.destination = currentMovepoint;
        }
        else {
            if (Vector3.Distance(transform.position, currentMovepoint) <= agent.height/2 * transform.localScale.magnitude) {
                currentMovepoint = Vector3.zero;
                ChangeState(false);
            }
        }
    }

    void LookAtTarget() {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * rotationSpeed);
    }

    //IEnumerator ManipulateObject(AttackPaterns attakType, List<T> attackPollingList, Transform[] spawnPoints) {
    //    for (int i = 0; i < maxAttackAmounts[(int)attakType]; i++) {
    //        bool spawnSomething = false;
    //        for (int j = 0; j < attackPollingList.Count; j++) {
    //            if (!attackPollingList[j])
    //        }
    //        foreach (IAStarFPS enemy in minionsList) {
    //            if (!enemy.isActive) {
    //                anim.SetBool("Attack", true);
    //                anim.SetBool("Damage", false);
    //                enemy.transform.position = minionsSpawnPoint[Random.Range(0, spawnPoints.Length)].position;
    //                StartCoroutine(enemy.Activate(true, 0));
    //                spawnSomething = true;
    //                break;
    //            }
    //        }
    //        if (!spawnSomething) break;
    //        yield return new WaitForSeconds(attackInterval);
    //    }
    //    ChangeState(true);
    //}
    IEnumerator SumonMinionsAttack() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.sumon]; i++) {
            bool spawnSomething = false;
            foreach (IAStarFPS enemy in minionsList) {
                if (!enemy.isActive) {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Damage", false);
                    enemy.transform.position = minionsSpawnPoint[Random.Range(0, minionsSpawnPoint.Length)].position;
                    StartCoroutine(enemy.Activate(true, 0, target));
                    spawnSomething = true;
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
    IEnumerator ShockWaveAttack() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.shockwave]; i++) {
            bool spawnSomething = false;
            anim.SetBool("Attack", true);
            anim.SetBool("Damage", false);
            yield return new WaitForSeconds(1f);
            foreach (ShockWave attack in shockwaveList) {
                if (!attack.isActive) {
                    spawnSomething = true;
                    attack.transform.position = transform.position - new Vector3(0, agent.height / 2 * transform.localScale.magnitude, 0);
                    attack.Activate(true);
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
    IEnumerator Laser() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.laser]; i++) {
            bool spawnSomething = false;
            foreach (Laser attack in laserList) {
                if (!attack.isActive) {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Damage", false);
                    int index = Random.Range(0, laserSpawnPoint.Length);
                    attack.transform.SetPositionAndRotation(laserSpawnPoint[index].position, laserSpawnPoint[index].rotation);
                    attack.Activate(true);
                    spawnSomething = true;
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
    IEnumerator Explosion() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.explosion]; i++) {
            bool spawnSomething = false;
            anim.SetBool("Attack", true);
            anim.SetBool("Damage", false);
            yield return new WaitForSeconds(1f);
            foreach (Explosion attack in explosionList) {
                if (!attack.isActive) {
                    spawnSomething = true;
                    attack.transform.position = bulletSpawnPoint.position;
                    attack.Activate(true, (target.transform.position - bulletSpawnPoint.position).normalized);
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
    IEnumerator Shoot() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.shoot]; i++) {
            bool spawnSomething = false;
            anim.SetBool("Attack", true);
            anim.SetBool("Damage", false);
            yield return new WaitForSeconds(1f);
            foreach (Bullet attack in bulletList) {
                if (!attack.isActive) {
                    spawnSomething = true;
                    attack.transform.position = bulletSpawnPoint.position;
                    attack.Activate(true, (target.transform.position - bulletSpawnPoint.position).normalized, target.transform);
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
}
