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
public class Player : MonoBehaviour
{
    public const float LEFT_MAX = -37.2f;
    public const float RIGHT_MAX = 37.2f;
    private enum State
    {
        Idle,
        Move,
        Fight,
        Die // 죽으면 30초 뒤 부활. 죽은 상태에서 게임 종료 시 카운트다운 종료 시점과 동일
    }
    public float attackDelay;
    public CommonStatus commonStatus;
    public Animator animator;
    [SerializeField] private State state;
    [SerializeField] private Monster currentTarget;
    [SerializeField] private Monster attackTarget;
    private Vector3 moveDirection;
    private Vector3 rayDirection;
    private int layer;

    private void Awake()
    {
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
                    Attack();
                    if (attackTarget.commonStatus.currentHp <= 0)
                    {
                        yield return new WaitForSeconds(attackDelay);
                        state = State.Idle;
                        currentTarget = null;
                        break;
                    }
                    yield return new WaitForSeconds(attackDelay);
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    state = State.Idle;
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
            if (state == State.Fight)
            {
                continue;
            }
            if (GameManager.Instance.monsterList.Count > 0)
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
    private void Attack()
    {
        animator.SetTrigger("Attack");
        attackTarget.DecreaseHp(Random.Range(commonStatus.minAttack, commonStatus.maxAttack));
    }
    private void MovePlayer(Vector3 dir, bool isLeft)
    {
        moveDirection = dir;
        rayDirection = dir;
        transform.rotation = ReversalObjectY(isLeft);
        state = State.Move;
    }
    private Quaternion ReversalObjectY(bool left)
    {
        Quaternion q = left? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0,180,0);
        return q;
    }
    private Monster FindNearestMonster()
    {
        Monster monster = GameManager.Instance.monsterList.OrderBy(x =>
        {
            return Vector3.Distance(transform.position, x.transform.position);
        }
        ).FirstOrDefault();
        print(monster);
        return monster;
    }
    private void LateUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(currentTarget && state != State.Fight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 2f, layer);
            if (hit.collider)
            {
                attackTarget = hit.collider.GetComponent<Monster>();
                state = State.Fight;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, moveDirection * 2f, Color.red);
    }
}
