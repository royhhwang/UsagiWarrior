using UnityEngine;
using System.Collections;
using BreadcrumbAi;

[System.Serializable]
public class EnemySounds
{
    //i wanted to take a clip of taylor's voice saying "clap once if you can hear me"
    //public AudioClip
}

public class EnemyControls : MonoBehaviour
{

    //public EnemySounds 
    public enum EnemyType { Melee, Ranged, Boss };
    public EnemyType enemyType;
    public Rigidbody bossProjectilePrefab;
    private Transform player;

    private Ai ai;

    private bool _removeBody, _isHit, _animAttack;

    private float rangedAttackNext = 0.0f;
    private float rangedAttackRate = 2.0f;
    private float meleeAttackNext = 0.0f;
    private float meleeAttackRate = 1.0f;

    private Animator anim;
    private string animRun = "Run";
    private string animDeath1 = "Death1";
    private string animAttack = "Attack";

    void Start()
    {
        ai = GetComponent<Ai>();
        anim = GetComponent<Animator>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.transform;
        }
    }

    void Update()
    {
        CheckHealth();
        //CheckDeath();
    }

    void FixedUpdate()
    {
        Animation();
        Attack();
    }

    //private void CheckDeath() { }

    private void Animation()
    {
        if (ai.lifeState == Ai.LIFE_STATE.IsAlive)
        {
            if (ai.moveState != Ai.MOVEMENT_STATE.IsIdle)
            {
                anim.SetBool(animRun, true);
            }
            else
            {
                anim.SetBool(animRun, false);
            }
            if (_animAttack)
            {
                anim.SetBool(animAttack, true);
            }
            else
            {
                anim.SetBool(animAttack, false);
            }
        }
        else if (ai.lifeState == Ai.LIFE_STATE.IsDead)
        {
            anim.SetBool(animDeath1, true);
        }
    }

    private void Attack()
    {
        if (player)
        {
            if (ai.lifeState == ai.LIFE_STATE.IsAlive)
            {
                if (enemyType != EnemyType.Ranged)
                {
                    if (ai.attackState == Ai.ATTACK_STATE.CanAttackPlayer && Time.time > meleeAttackNext)
                    {
                        meleeAttackNext = Time.time + meleeAttackRate;
                        float rand = Random.value;
                        //if(rand <= 0.4f){
                        //	audioSource.clip = audioClips.audio_melee_attack_1;
                        //} else {
                        //	audioSource.clip = audioClips.audio_melee_attack_2;
                        //}
                        //audioSource.PlayOneShot(audioSource.clip);
                        //player.GetComponent<UnityChanControlScriptWithRgidBody>()._isHit = true;
                        //player.GetComponent<UnityChanControlScriptWithRgidBody>().Bleed(transform.rotation);
                        _animAttack = true;
                    }
                    else
                    {
                        _animAttack = false;
                    }
                }
                else
                {
                    if (ai.attackState == Ai.ATTACK_STATE.CanAttackPlayer && Time.time > rangedAttackNext)
                    {
                        rangedAttackNext = Time.time + rangedAttackRate;
                        Rigidbody spit = (Rigidbody)Instantiate(rangedProjectilePrefab, transform.position + transform.forward + transform.up, transform.rotation) as Rigidbody;
                        spit.AddForce(transform.forward * 500);
                        _animAttack = true;
                    }
                    else
                    {
                        _animAttack = false;
                    }
                }
            }
        }
    }

    private void CheckHealth()
    {
        if (_isHit && this != null)
        {
            float rand = Random.value;
            if (ai.Health > 0)
            {
                //audioSource.clip = audioClips.hurt_noise_1;
                //audioSource.PlayHurtNoise(audioSource.clip);
            }
            if (ai.Health <= 0)
            {
                //audioSource.clip = audioClips.death_noise_1;
                //audioSource.PlayDeathNoise(audioSource.clip);
            }
            _isHit = false;
        }
        if(ai.lifeState == Ai.LIFE_STATE.IsDead)
        {
            //point system?
        }
    }

    //IEnumerator DespawnEnemy()
    //{
        //if(enemyType == EnemyType.Boss)
        //{
            //Destroy(specialPrefab);
        //}
        //yield return new WaitForRespawn(5);
        //Invoke("UpdateEnemyCount", 7);
        //_removeBody= true;
    //}

    void UpdateEnemyCount()
    {
        if (enemyType == EnemyType.Boss)
        {
            //while boss is alive, have enemies constantly spawning
            //OR -- have an enemy running around spawning enemies at a set rate
        }
    }
}