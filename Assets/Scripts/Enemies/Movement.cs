using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private StickmanEnemy enemy;
    public void Init(StickmanEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void MoveAtDestination(Vector3 position, float deltaTime, out bool reached)
    {
        Vector3 toTargetDirection = position - transform.position;
        if(toTargetDirection.magnitude < 0.01f)
        {
            reached = true;
            return;
        }
        toTargetDirection.y = 0f;
        //rotation
        Quaternion currentRot = enemy.transform.rotation;
        float rotAngle = Quaternion.Angle(currentRot, Quaternion.LookRotation(toTargetDirection));
        float rotDuration = rotAngle / enemy.rotSpeed;

        enemy.transform.rotation = Quaternion.Lerp(currentRot, Quaternion.LookRotation(toTargetDirection), deltaTime/rotDuration);
        //move
        transform.Translate(new Vector3(0,0, enemy.runSpeed * deltaTime));
        reached = false;
    }
}
