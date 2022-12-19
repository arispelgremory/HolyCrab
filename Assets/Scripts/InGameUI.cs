using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Normal Attack")]
    public Image normalAttack;
    public float normalAttackCDInSeconds;

    [Header("Heavy Attack")]
    public Image heavyAttack;
    public float heavyAttackCDInSeconds;

    [Header("Dash")]
    public Image dash;
    public float dashCDInSeconds;

    // Start is called before the first frame update
    void Start()
    {
        normalAttack.fillAmount = 0;
        heavyAttack.fillAmount = 0;
        dash.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        NormalAttack();
        HeavyAttack();
        Dash();
    }

    private void NormalAttack()
    {
        if(Input.GetButtonDown("Fire1") && CrabMovement.attackable)
        {
            normalAttack.fillAmount = 1;
        }

        if (!CrabMovement.attackable)
        {
            normalAttack.fillAmount -= 1 / (normalAttackCDInSeconds) * Time.deltaTime;
        }
        
        if(normalAttack.fillAmount <= 0)
        {
            normalAttack.fillAmount = 0;
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
            heavyAttack.fillAmount -= 1 /  (heavyAttackCDInSeconds) * Time.deltaTime;
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
            dash.fillAmount -= 1 / (dashCDInSeconds) * Time.deltaTime;
        }
        
        if(dash.fillAmount <= 0)
        {
            dash.fillAmount = 0;
        }
        
    }
    
}
