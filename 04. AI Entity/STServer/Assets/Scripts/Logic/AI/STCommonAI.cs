using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCommonAI : behaviac.Agent
{
    private STMovement mSTMovement;
    private Vector3 mEntityPos;

    public bool InitAI(string strAI)
    {
        mEntityPos = transform.parent.position;
        mSTMovement = transform.parent.GetComponent<STMovement>();

        bool bRet = this.btload(strAI);
        if (bRet)
        {
            this.btsetcurrent(strAI);
        }
        return bRet;
    }

    public void MoveLeft()
    {
        mSTMovement.SetMovePosition(new Vector3(mEntityPos.x - 0.5f, mEntityPos.y, mEntityPos.z));
    }

    public void MoveRight()
    {
        mSTMovement.SetMovePosition(new Vector3(mEntityPos.x + 0.5f, mEntityPos.y, mEntityPos.z));
    }

    public void MoveBack()
    {
        mSTMovement.SetMovePosition(new Vector3(mEntityPos.x, mEntityPos.y, mEntityPos.z - 0.5f));
    }

    public void MoveForward()
    {
        mSTMovement.SetMovePosition(new Vector3(mEntityPos.x, mEntityPos.y, mEntityPos.z + 0.5f));
    }
}
