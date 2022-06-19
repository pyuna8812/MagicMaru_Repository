using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CommonStatus
{
    public float maxHp;
    public float currentHp;
    public float minAttack;
    public float maxAttack;
    public float moveSpeed;
}
public enum State
{
    Idle,
    Move,
    Fight,
    Die // 죽으면 30초 뒤 부활. 죽은 상태에서 게임 종료 시 카운트다운 종료 시점과 동일
}
public class Player : MonoBehaviour
{
    public static Player instance;
    public const float LEFT_MAX = -37.2f;
    public const float RIGHT_MAX = 37.2f;
    public float attackDelay;
    public CommonStatus commonStatus;
    public Animator animator;
    [SerializeField] private State state;
    [SerializeField] private Monster currentTarget;
    [SerializeField] private Monster attackTarget;
    private Vector3 moveDirection;
    private Vector3 rayDirection;
    private int layer;
    public float resurrectionCount = 30f;
    public Transform rig;
    public bool isDie;
    public Sprite dieSprite;
    public Sprite normalSprite;
    public SpriteRenderer headSprite;
    public State State { get => state; set => state = value; }

    private void Awake()
    {
        instance = this;
        layer = (-1) - (1 << LayerMask.NameToLayer("Player"));
        commonStatus.currentHp = commonStatus.maxHp;
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Co_AnimTransition());
        StartCoroutine(Co_SelectBehavior());
    }
    private IEnumerator Co_AnimTransition()
    {
        float damage = 0;
        while (true)
        {
            yield return null;
            switch (state)
            {
                case State.Idle:
                    animator.SetBool("IsMove", false);
                    break;
                case State.Move:
                    if (!animator.GetBool("IsMove"))
                    {
                        animator.SetBool("IsMove", true);
                    }
                    if(transform.position.x <= LEFT_MAX)
                    {
                        MovePlayer(Vector3.right, false);
                    }
                    else if (transform.position.x >= RIGHT_MAX)
                    {
                        MovePlayer(Vector3.left, true);
                    }
                    transform.position += moveDirection * commonStatus.moveSpeed * Time.deltaTime;
                    break;
                case State.Fight:
                    if (animator.GetBool("IsMove"))
                    {
                        animator.SetBool("IsMove", false);
                    }
                    animator.SetTrigger("Attack");
                    yield return new WaitForSeconds(0.5f);
                    Attack(damage);
                    if (attackTarget.commonStatus.currentHp <= 0)
                    {
                        attackTarget = null;
                        yield return new WaitForSeconds(attackDelay);
                        state = State.Idle;
                        currentTarget = null;
                        break;
                    }
                    yield return new WaitForSeconds(attackDelay);
                    break;
                case State.Die:
                    //어택 딜레이에 걸려서 외부 코루틴으로 작성
                    break;
                default:
                    break;
            } 
        }
    }
    private IEnumerator Co_SelectBehavior()
    {
        while (true)
        {
            yield return null;
            if (state == State.Fight || isDie)
            {
                continue;
            }
            if (GameManager.Instance.monsterList.Count > 0 || GameManager.Instance.bossList.Count > 0)
            {
                if (!currentTarget)
                {
                    currentTarget = FindNearestMonster();
                }
                if (transform.position.x - currentTarget.transform.position.x > 0)
                {
                    MovePlayer(Vector3.left, true);
                }
                else
                {
                    MovePlayer(Vector3.right, false);
                }
                continue;
            }
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    MovePlayer(Vector3.left, true);
                    break;
                case 1:
                    MovePlayer(Vector3.right, false);
                    break;
                case 2:
                    moveDirection = Vector3.zero;
                    state = State.Idle;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(Random.Range(0f, 5f));
        }
    }
    private void Attack(float damage)
    {
        damage = Random.Range(commonStatus.minAttack, commonStatus.maxAttack);
        attackTarget.DecreaseHp(damage);
        MainUIManager.instance.ShowDamageUI(attackTarget.transform.position, Mathf.Ceil(damage).ToString(), true);
    }
    private void MovePlayer(Vector3 dir, bool isLeft)
    {
        moveDirection = dir;
        rayDirection = dir;
        transform.rotation = GameManager.Instance.ReversalObjectY(isLeft);
        state = State.Move;
    }
    private Monster FindNearestMonster()
    {
        Monster monster = GameManager.Instance.monsterList.OrderBy(x =>
        {
            return Vector3.Distance(transform.position, x.transform.position);
        }
        ).FirstOrDefault();
        Monster boss = GameManager.Instance.bossList.OrderBy(x =>
        {
            return Vector3.Distance(transform.position, x.transform.position);
        }).FirstOrDefault();
        if(monster == null)
        {
            return boss;
        }
        else if (boss == null)
        {
            return monster;
        }
        if(Vector3.Distance(transform.position,monster.transform.position) >= Vector3.Distance(transform.position, boss.transform.position))
        {
            return boss;
        }
        else
        {
            return monster;
        }
    }
    private void LateUpdate()
    {
        rig.localRotation = Quaternion.Euler(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentTarget && state != State.Fight && state != State.Die)
        {
            if (isDie)
            {
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1.5f, layer);
            if (hit.collider)
            {
                attackTarget = hit.collider.GetComponent<Monster>();
                state = State.Fight;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, moveDirection * 1.5f, Color.red);
    }
    public void DecreaseHp(float value)
    {
        value = Mathf.Ceil(value);
        commonStatus.currentHp -= value;
        if(commonStatus.currentHp <= 0)
        {
            if (isDie)
            {
                return;
            }
            StartCoroutine(Co_Die());
        }
    }
    private IEnumerator Co_Die()
    {
        state = State.Die;
        animator.SetTrigger("Die");
        isDie = true;
        headSprite.sprite = dieSprite;
        yield return new WaitForSeconds(resurrectionCount);
        headSprite.sprite = normalSprite;
        commonStatus.currentHp = commonStatus.maxHp;
        animator.SetTrigger("Resurrection");
        state = State.Idle;
        yield return new WaitForSeconds(2f);
        isDie = false;
    }
}
