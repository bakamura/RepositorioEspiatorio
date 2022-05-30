using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAStarFPS : MonoBehaviour {
    public GameObject target;
    public NavMeshAgent agent;
    public Animator anim;
    public SkinnedMeshRenderer render;
    public enum States {
        pursuit,
        atacking,
        stoped,
        dead,
        damage,
    }

    public enum EnemyTypes {
        agressive,
        kamikaze,
        bully,
        wandering,
        passive
    }
    public EnemyTypes enemyType;
    public States state;
    public float knockbackForce;
    [SerializeField] private float _actionRange;
    [HideInInspector] public bool _isAgressive;
    [HideInInspector] public bool isActive;
    [SerializeField, Tooltip("y = z, x = x")] private Vector2 _wanderingRange;
    [SerializeField] private GameObject _explosionParticle;
    private Vector3 _currentTargetPoint;
    private Coroutine deactivating = null;


    private void Awake() {
        UpdateTargetPoint();
        if (enemyType != EnemyTypes.passive && enemyType != EnemyTypes.wandering) _isAgressive = true;
    }

    // Update is called once per frame
    void Update() {
        StateMachine();
        anim.SetFloat("Velocidade", agent.velocity.magnitude);

    }

    void StateMachine() {
        switch (state) {
            case States.pursuit:
                PursuitState();
                break;
            case States.atacking:
                AttackState();
                break;
            case States.stoped:
                StoppedState();
                break;
            case States.dead:
                DeadState();
                break;
            case States.damage:
                DamageState();
                break;
        }
    }
    private void UpdateTargetPoint() {
        _currentTargetPoint = transform.position + new Vector3(Random.Range(-_wanderingRange.x, _wanderingRange.x), 0, Random.Range(-_wanderingRange.y, _wanderingRange.y));
    }

    void ReturnPursuit() {
        state = States.pursuit;

    }
    public void Damage() {
        state = States.damage;
        Invoke("ReturnPursuit", 1);
        StartCoroutine(ReturnDamage());
    }
    IEnumerator ReturnDamage() {
        for (int i = 0; i < 4; i++) {
            render.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
            render.material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
        }

    }

    public void Dead() {
        state = States.dead;
        PlayerData.UpdateTPPowerUps(1);
    }


    void PursuitState() {
        agent.isStopped = false;
        if (_isAgressive) agent.destination = target.transform.position;
        else agent.destination = _currentTargetPoint;
        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
        if (!_isAgressive && Vector3.Distance(transform.position, _currentTargetPoint) < 1f) UpdateTargetPoint();
        if (Vector3.Distance(transform.position, target.transform.position) < _actionRange) {
            switch (enemyType) {
                case EnemyTypes.agressive:
                    state = States.atacking;
                    break;
                case EnemyTypes.kamikaze:
                    //MenuScript.instance.GameOverScreen();
                    GameObject part = Instantiate(_explosionParticle, transform.position, Quaternion.identity);
                    part.transform.parent = null;
                    this.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    void AttackState() {
        agent.isStopped = true;
        anim.SetBool("Attack", true);
        anim.SetBool("Damage", false);
        if (Vector3.Distance(transform.position, target.transform.position) > 4) {
            state = States.pursuit;
        }
    }

    void StoppedState() {
        agent.isStopped = true;
        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
    }

    void DeadState() {
        agent.isStopped = true;
        anim.SetBool("Attack", false);
        anim.SetBool("Dead", true);
        anim.SetBool("Damage", false);
        if (deactivating == null) deactivating = StartCoroutine(Activate(false, 1));
    }

    void DamageState() {
        agent.isStopped = true;
        anim.SetBool("Damage", true);
    }

    public IEnumerator Activate(bool state, float delay) {
        yield return new WaitForSeconds(delay);
        isActive = state;
        this.gameObject.SetActive(state);
    }

    private void OnDrawGizmosSelected() {
        if (UnityEditor.EditorApplication.isPlaying) {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(_currentTargetPoint, new Vector3(.1f, .1f, .1f));
        }
    }
}
