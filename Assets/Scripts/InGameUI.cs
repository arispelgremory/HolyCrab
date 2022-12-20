using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Normal Attack")]
    public Image normalAttack;
    public float normalAttackCDInSeconds;
    private float attackTimer = 0f;

    [Header("Heavy Attack")]
    public Image heavyAttack;
    public float heavyAttackCDInSeconds;
    private float heavyAttackTimer = 0f;

    [Header("Dash")]
    public Image dash;
    public float dashCDInSeconds;
    private float dashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        normalAttack.fillAmount = 0;
        heavyAttack.fillAmount = 0;
        dash.fillAmount = 0;

        normalAttackCDInSeconds = CrabMovement.attackCoolDownTime;
        heavyAttackCDInSeconds = CrabMovement.heavyAttackCoolDownTime;
        dashCDInSeconds = CrabMovement.dashCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        heavyAttackTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        
        NormalAttack();
        HeavyAttack();
        Dash();
    }

    private void NormalAttack()
    {
        bool isCoolDown = attackTimer < normalAttackCDInSeconds;
        if(Input.GetButtonDown("Fire1") && !isCoolDown && CrabMovement.attackable)
        {
            normalAttack.fillAmount = 1;
            attackTimer = 0;
        } 
        
        if(!CrabMovement.attackable && isCoolDown)
        {
            normalAttack.fillAmount -= 1 / normalAttackCDInSeconds * Time.deltaTime;
            if(normalAttack.fillAmount <= 0)
            {
                normalAttack.fillAmount = 0;
            }
        }
        
    }
    
    private void HeavyAttack()
    {
        bool isCoolDown = heavyAttackTimer < heavyAttackCDInSeconds;
        if(Input.GetButtonDown("Fire2") && CrabMovement.heavyAttackable && !isCoolDown)
        {
            heavyAttack.fillAmount = 1;
            heavyAttackTimer = 0;
        }

        if (!CrabMovement.heavyAttackable && isCoolDown)
        {
            heavyAttack.fillAmount -= 1 /  ((heavyAttackCDInSeconds * Time.deltaTime));
        }
        
        if(heavyAttack.fillAmount <= 0 && (CrabMovement.heavyAttackable && !isCoolDown))
        {
            heavyAttack.fillAmount = 0;
        }
        
        
        
    }
    
    private void Dash()
    {
        bool isCoolDown = dashTimer < dashCDInSeconds;
        if(Input.GetButtonDown("Shift") && CrabMovement.dashable && !isCoolDown)
        {
            dash.fillAmount = 1;
            dashTimer = 0;
        }

        if (!CrabMovement.dashable && isCoolDown)
        {
            dash.fillAmount -= 1 / ((Time.deltaTime * dashCDInSeconds));
            if(dash.fillAmount <= 0)
            {
                dash.fillAmount = 0;
            }
        }
    }
    
}
