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
        if(Input.GetButtonDown("Fire1") && !CrabMovement.attackable && attackTimer == 0)
        {
            normalAttack.fillAmount = 1;
        } 
        else if(!CrabMovement.attackable && attackTimer > 0)
        {
            normalAttack.fillAmount -= 1 / normalAttackCDInSeconds * Time.deltaTime;
        }

        
        if(CrabMovement.attackable && attackTimer > normalAttackCDInSeconds)
        {
            normalAttack.fillAmount = 0;
            attackTimer = 0;
        }
        
    }
    
    private void HeavyAttack()
    {
        if(Input.GetButtonDown("Fire2") && CrabMovement.heavyAttackable)
        {
            heavyAttack.fillAmount = 1;
        }

        if (!CrabMovement.heavyAttackable)
        {
            heavyAttack.fillAmount -= 1 /  ((heavyAttackCDInSeconds * Time.deltaTime) + CrabMovement.actionInterval);
        }
        
        if(heavyAttack.fillAmount <= 0)
        {
            heavyAttack.fillAmount = 0;
        }
        
    }
    
    private void Dash()
    {
        if(Input.GetButtonDown("Shift") && CrabMovement.dashable)
        {
            dash.fillAmount = 1;
        }

        if (!CrabMovement.dashable)
        {
            dash.fillAmount -= 1 / ((Time.deltaTime * dashCDInSeconds) + CrabMovement.actionInterval);
        }
        
        if(dash.fillAmount <= 0)
        {
            dash.fillAmount = 0;
        }
        
    }
    
}
