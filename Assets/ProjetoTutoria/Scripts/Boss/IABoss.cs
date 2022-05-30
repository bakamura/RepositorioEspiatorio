using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IABoss : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float attackInterval;
    [SerializeField, Tooltip("minion, shockwave, laser, explosion, bullet")] private GameObject[] attackPrefabs = new GameObject[5]; 
    [SerializeField, Tooltip("minion, shockwave, laser, explosion, bullet")] private sbyte[] maxAttackAmounts = new sbyte[5];
    [SerializeField] private Transform[] minionsSpawnPoint;
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
    private Vector3 currentMovepoint = Vector3.negativeInfinity;
    public int a { get; private set; }

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
        anim = GetComponent<Animator>();
        render = GetComponent<SkinnedMeshRenderer>();
    }
    private void Start() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.sumon]; i++) {
            IAStarFPS enemy = Instantiate(attackPrefabs[(int)AttackPaterns.sumon], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<IAStarFPS>();
            minionsList.Add(enemy);
            StartCoroutine(enemy.Activate(false, 0));
        }
        for(int i = 0;i < maxAttackAmounts[(int)AttackPaterns.shockwave];i++) {
            ShockWave attack = Instantiate(attackPrefabs[(int)AttackPaterns.shockwave], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<ShockWave>(); 
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
            //attack.Activate(false);
        }
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.shoot]; i++) {
            Bullet attack = Instantiate(attackPrefabs[(int)AttackPaterns.shoot], transform.position + new Vector3(0, -1000, 0), Quaternion.identity).GetComponent<Bullet>();
            bulletList.Add(attack);
            //attack.Activate(false);
        }
    }
    private void Update() {
        StateMachine();
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
                    //currentAttack = StartCoroutine(Laser());
                    break;
                case AttackPaterns.explosion:
                    //currentAttack = StartCoroutine(Explosion());
                    break;
                case AttackPaterns.shoot:
                    currentAttack = StartCoroutine(Shoot());
                    break;
                case AttackPaterns.movement:
                    Movement();
                    break;
            }
        }
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
        if (currentMovepoint == Vector3.negativeInfinity) {
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
            if (Vector3.Distance(transform.position, currentMovepoint) <= 1f) {
                currentMovepoint = Vector3.negativeInfinity;
                ChangeState(false);
            }
        }
    }
    IEnumerator SumonMinionsAttack() {
        for (int i = 0; i < maxAttackAmounts[(int)AttackPaterns.sumon]; i++) {
            bool spawnSomething = false;
            foreach (IAStarFPS enemy in minionsList) {
                if (!enemy.isActive) {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Damage", false);
                    enemy.transform.position = minionsSpawnPoint[Random.Range(0, minionsSpawnPoint.Length)].position;
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
                    attack.transform.position = transform.position - new Vector3(0, agent.height / 2, 0);
                    attack.Activate(true);
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
    //IEnumerator Laser() {

    //}
    //IEnumerator Explosion() {

    //}
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
                    attack.Activate(true, (target.transform.position - bulletSpawnPoint.position).normalized);
                    break;
                }
            }
            if (!spawnSomething) break;
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(true);
    }
}
