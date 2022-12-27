using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Normal Attack")] public Image normalAttack;
    public float normalAttackCDInSeconds;
    private float attackTimer = 0f;

    [Header("Heavy Attack")] public Image heavyAttack;
    public float heavyAttackCDInSeconds;
    private float heavyAttackTimer = 0f;

    [Header("Dash")] public Image dash;
    public float dashCDInSeconds;
    private float dashTimer = 0f;
    
    [Header("Crab Amount")] 
    [SerializeField] public int crabAmount;
    [SerializeField] private TextMeshProUGUI crabAmountText;
    // public Text crabAmountText;
    
    // Instance for other classes to access
    public static InGameUI Instance;

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
        attackTimer += Time.deltaTime;
        heavyAttackTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;

        NormalAttack();
        HeavyAttack();
        Dash();
        
        
        crabAmountText.text = "x " + Instance.crabAmount.ToString();
    }
    

    private void NormalAttack()
    {
        bool isCoolDown = attackTimer < normalAttackCDInSeconds;

        if (Input.GetButtonDown("Fire1") && !isCoolDown && CrabMovement.attackable)
        {
            normalAttack.fillAmount = 1;
            attackTimer = 0;
        }

        if (!CrabMovement.attackable && isCoolDown)
        {
            normalAttack.fillAmount -= Time.deltaTime / normalAttackCDInSeconds;
        }

        if (normalAttack.fillAmount <= 0.01f)
        {
            normalAttack.fillAmount = 0;
        }

    }

    private void HeavyAttack()
    {
        bool isHeavyAttackCoolDown = heavyAttackTimer < heavyAttackCDInSeconds;
        if (Input.GetButtonDown("Fire2") && !isHeavyAttackCoolDown && CrabMovement.heavyAttackable)
        {
            heavyAttack.fillAmount = 1;
            heavyAttackTimer = 0;
        }

        if (!CrabMovement.heavyAttackable && isHeavyAttackCoolDown)
        {
            heavyAttack.fillAmount -= Time.deltaTime / heavyAttackCDInSeconds;
        }

        if (heavyAttack.fillAmount <= 0.01f)
        {
            heavyAttack.fillAmount = 0;
        }
    }

    private void Dash()
    {
        bool isDashCoolDown = dashTimer < dashCDInSeconds;
        if (Input.GetButtonDown("Shift") && CrabMovement.dashable && !isDashCoolDown)
        {
            dash.fillAmount = 1;
            dashTimer = 0;

        }

        if (!CrabMovement.dashable && isDashCoolDown)
        {
            dash.fillAmount -= 1 / dashCDInSeconds * Time.deltaTime;
            if (dash.fillAmount <= 0.01f)
            {
                dash.fillAmount = 0;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        crabAmountText = transform.Find("ScoreboardPanelUI").Find("ObjectiveUI").GetComponent<TextMeshProUGUI>();
        
    }
}
