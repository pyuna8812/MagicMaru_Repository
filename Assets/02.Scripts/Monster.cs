using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private enum MonsterType
    {
        Normal,
        Boss
    }
    [SerializeField]private MonsterType monsterType;
    public CommonStatus commonStatus;
    public float attackDelay;
    [SerializeField] private State state;
    private Vector3 moveDirection;
    public Animator animator;
    public Transform rig;
    private BoxCollider2D box;
    private Vector3 rayDirection;
    private Vector3 rayPoint;
    private int layer;
    private void Awake()
    {
        layer = (-1) - (1 << LayerMask.NameToLayer("Monster"));
        box = GetComponent<BoxCollider2D>();
    }
    private float CheckDirection()
    {
        float d = Player.instance.transform.position.x - transform.position.x;
        return d;
    }
    private void OnEnable()
    {
        box.enabled = true;
        StartCoroutine(Co_AnimTransition());
        if(monsterType == MonsterType.Normal)
        {
            StartCoroutine(Co_SelectBehavior());
            return;
        }
        if(CheckDirection() < 0)
        {
            MoveMonster(Vector3.left, true);
            rayDirection = Vector3.left;
        }
        else
        {
            MoveMonster(Vector3.right, false);
            rayDirection = Vector3.right;
        }
    }
    public void DecreaseHp(float value)
    {
        if(state != State.Fight)
        {
            print(Vector3.Distance(Player.instance.transform.position, transform.position));
            if(CheckDirection() < 0)
            {
                transform.rotation = GameManager.Instance.ReversalObjectY(true);
            }
            else
            {
                transform.rotation = GameManager.Instance.ReversalObjectY(false);
            }
            state = State.Fight;
        }
        commonStatus.currentHp -= value;
        if(commonStatus.currentHp <= 0)
        {
            state = State.Die;
            box.enabled = false;
            GameManager.Instance.UpdateGold(monsterType == MonsterType.Normal?GameManager.Instance.tapGold * 100 : GameManager.Instance.tapGold * 1000);
            if(monsterType == MonsterType.Normal)
            {
                GameManager.Instance.monsterList.Remove(this);
            }
            else
            {
                GameManager.Instance.bossList.Remove(this);
            }
        }
    }
    public void ResetStatus()
    {
        commonStatus.currentHp = commonStatus.maxHp;
    }
    private IEnumerator Co_AnimTransition()
    {
        float damage;
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
                    if (transform.position.x <= Player.LEFT_MAX)
                    {
                        MoveMonster(Vector3.right, false);
                    }
                    else if (transform.position.x >= Player.RIGHT_MAX)
                    {
                        MoveMonster(Vector3.left, true);
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
                    damage = Random.Range(commonStatus.minAttack, commonStatus.maxAttack);
                    MainUIManager.instance.ShowDamageUI(Player.instance.transform.position, Mathf.Ceil(damage).ToString(), false);
                    Player.instance.DecreaseHp(damage);
                    if(commonStatus.currentHp > 0)
                    {
                        yield return new WaitForSeconds(attackDelay);
                    }
                    break;
                case State.Die:
                    animator.SetTrigger("Die");
                    yield return new WaitForSeconds(3f);
                    transform.gameObject.SetActive(false);
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
            if (state == State.Fight || state == State.Die)
            {
                continue;
            }
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    MoveMonster(Vector3.left, true);
                    break;
                case 1:
                    MoveMonster(Vector3.right, false);
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
    private void MoveMonster(Vector3 dir, bool isLeft)
    {
        moveDirection = dir;
        transform.rotation = GameManager.Instance.ReversalObjectY(isLeft);
        state = State.Move;
    }
    // Update is called once per frame
    private void OnDisable()
    {
        rig.localRotation = Quaternion.Euler(0, 0, 0);
    }
    private void Update()
    {
        if(monsterType == MonsterType.Normal)
        {
            return;
        }
        rayPoint = transform.position + new Vector3(0, 2);
        if (state != State.Fight && state != State.Die)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayPoint, rayDirection, 1.5f, layer);
            if (hit.collider)
            {
                state = State.Fight;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (monsterType == MonsterType.Normal)
        {
            return;
        }
        Debug.DrawRay(rayPoint, moveDirection * 1.5f, Color.red);
    }
}
