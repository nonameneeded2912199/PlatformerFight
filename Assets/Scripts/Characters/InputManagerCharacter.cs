using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerCharacter : MonoBehaviour
{
    public int horizontal;

    public int vertical;

    public int buttonUp;

    public int buttonDown;

    public int buttonLeft;

    public int buttonRight;

    public int buttonATK;

    public int buttonJump;

    public int buttonDash;

    public int buttonSkill;

    public int buttonSkill2;

    public int buttonSkill3;

    public int buttonSkill4;

    public int normalATK;

    public int skill1;

    public int skill2;

    public int skill3;

    public int skill4;

    public int jump;

    public int dash;

    private void Start()
    {
        horizontal = 0;
        vertical = 0;
        buttonUp = 0;
        buttonDown = 0;
        buttonLeft = 0;
        buttonRight = 0;
        buttonSkill = 0;
        buttonSkill2 = 0;
        buttonSkill3 = 0;
        buttonSkill4 = 0;
        buttonATK = 0;
        buttonJump = 0;
        buttonDash = 0;
        normalATK = 0;
        skill1 = 0;
        skill2 = 0;
        skill3 = 0;
        skill4 = 0;
        jump = 0;
        dash = 0;
    }

    public void DoButtonUp(int a)
    {
        if (a == 1)
            buttonUp = 1;
        else
            buttonUp = 0;
    }

    public void DoButtonDown(int a)
    {
        if (a == 1)
            buttonDown = 1;
        else
            buttonDown = 0;
    }

    public void DoButtonLeft(int a)
    {
        if (a == 1)
            buttonLeft = 1;
        else
            buttonLeft = 0;
    }

    public void DoButtonRight(int a)
    {
        if (a == 1)
            buttonRight = 1;
        else
            buttonRight = 0;
    }

    public void DoButtonATK(int a)
    {
        if (a == 1)
            buttonATK = 1;
        else
            buttonATK = 0;
    }

    public void DoButtonJump(int a)
    {
        if (a == 1)
            buttonJump = 1;
        else
            buttonJump = 0;
    }

    public void DoButtonDash(int a)
    {
        if (a == 1)
            buttonDash = 1;
        else
            buttonDash = 0;
    }

    public void DoButtonSkill(int a)
    {
        if (a == 1)
            buttonSkill = 1;
        else
            buttonSkill = 0;
    }

    public void DoButtonSkill2(int a)
    {
        if (a == 1)
            buttonSkill2 = 1;
        else
            buttonSkill2 = 0;
    }

    public void DoButtonSkill3(int a)
    {
        if (a == 1)
            buttonSkill3 = 1;
        else
            buttonSkill3 = 0;
    }

    public void DoButtonSkill4(int a)
    {
        if (a == 1)
            buttonSkill4 = 1;
        else
            buttonSkill4 = 0;
    }

    private void HandleInput()
    {
        // Horizontal 
        if ((int)Input.GetAxis("Horizontal") == 1 || buttonRight == 1)
        {
            horizontal = 1;
        }
        else if ((int)Input.GetAxis("Horizontal") == -1 || buttonLeft == 1)
        {
            horizontal = -1;
        }
        else
        {
            horizontal = 0;
        }

        // Vertical
        if ((int)Input.GetAxis("Vertical") == 1 || buttonUp == 1)
        {
            vertical = 1;
        }
        else if ((int)Input.GetAxis("Vertical") == -1 || buttonDown == 1)
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }

        // Jump
        if (Input.GetButton("Jump") || buttonJump == 1)
        {
            jump = 1;
        }
        else
        {
            jump = 0;
        }

        // ATK
        if (Input.GetButton("NormalATK") || buttonATK == 1)
        {
            normalATK = 1;
        }
        else
        {
            normalATK = 0;
        }

        // Dash
        if (Input.GetButton("Dash") || buttonDash == 1)
        {
            dash = 1;
        }
        else
        {
            dash = 0;
        }

        // Skill
        if (Input.GetButton("Skill") || buttonSkill == 1)
        {
            skill1 = 1;
        }
        else
        {
            skill1 = 0;
        }

        // Skill2
        if (Input.GetButton("Skill2") || buttonSkill2 == 1)
        {
            skill2 = 1;
        }
        else
        {
            skill2 = 0;
        }

        // Skill3
        if (Input.GetButton("Skill3") || buttonSkill3 == 1)
        {
            skill3 = 1;
        }
        else
        {
            skill3 = 0;
        }
        
        // Skill4
        if (Input.GetButton("Skill4") || buttonSkill4 == 1)
        {
            skill4 = 1;
        }
        else
        {
            skill4 = 0;
        }
    }

    private void Update()
    {
        HandleInput();
    }

}
