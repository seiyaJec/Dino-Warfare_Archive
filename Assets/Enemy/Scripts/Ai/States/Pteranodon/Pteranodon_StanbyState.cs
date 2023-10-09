using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pteranodon_StanbyState : AiState
{
    private float readyTime_sec;         //�ҋ@����(�b)
    private float turningTime_sec;       //���񎞊�(�b)
    private float turningRadius;         //���񔼌a
    private float turningOneRapTime_sec; //���񎞁A1������̂ɂ����鎞��(�b)
    private float turningSpeed;          //���񑬓x
    private Vector3 turningCenterPos;               // ���񎞂̒��S�_ �����̍��W�����ɋ��߂邽��Enter���ɃL���b�V������K�v����
    private const float turningModelAngleY = 90.0f; // ���񎞂̃��f���̊p�x ���񎞂͉E�����邽�߁A90�x�ɐݒ�
    private AiStateId nextState;

    private float initModelAngleY;

    public Pteranodon_StanbyState(float readyTime, float turningTime, float turningRadius, float turningOneRapTime, AiStateId nextState)
    {
        this.readyTime_sec = readyTime;
        this.turningTime_sec = turningTime;
        this.turningRadius = turningRadius;
        this.turningOneRapTime_sec = turningOneRapTime;
        this.turningSpeed = 360.0f / this.turningOneRapTime_sec;
        this.nextState = nextState;
    }

    private enum StanbyState
    {
        Turning,
        Ready,
        SetNextAction
    }
    private StanbyState stanbyState;

    private float timer;

    private int actionCount = 0;

    private bool isAttackPosSet = false;


    public AiStateId GetId()
    {
        return AiStateId.Stanby;
    }

    public void Init(AiAgent agent)
    {

    }

    public void Enter(AiAgent agent)
    {
        timer = 0.0f;

        initModelAngleY = agent.model.transform.localEulerAngles.y;
        agent.navMeshAgent.isStopped = true;
        agent.navMeshAgent.velocity = Vector3.zero;
        if (!isAttackPosSet)
        {
            isAttackPosSet = true;
            agent.config.attackPosition = agent.transform.position;
        }

        stanbyState = StanbyState.Turning;

        //����̒��S�_�����߂�
        Vector3 targetDirection = (agent.targetEntity.transform.position - agent.transform.position).normalized;
        turningCenterPos = agent.transform.position - targetDirection * turningRadius;

        //�Ώۂ̕�������(XZ���̂�)
        LookTargetXZ(agent);
    }

    //XZ���̂݃^�[�Q�b�g�̕���������
    private void LookTargetXZ(AiAgent agent)
    {
        Vector3 targetPos = agent.targetEntity.transform.position;

        agent.transform.LookAt(
            new Vector3(
                targetPos.x,
                agent.transform.position.y,
                targetPos.z));

    }

    //Y���̂݃��[�J���ŉ�]
    private void LocalRotateY(Transform transform, float rotateAngle)
    {
        Vector3 angle = transform.localEulerAngles;
        angle.y += rotateAngle;
        transform.localEulerAngles = angle;
    }

    public void Update(AiAgent agent)
    {
        Think();
        Move(agent);

        timer += Time.deltaTime;

        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);
    }

    private void Think()
    {
        StanbyState nextState = stanbyState;

        switch(nextState)
        {
            case StanbyState.Turning:
                if (timer > turningTime_sec)
                    nextState = StanbyState.Ready;
                break;

            case StanbyState.Ready:
                if (timer > readyTime_sec)
                    nextState = StanbyState.SetNextAction;
                break;

            case StanbyState.SetNextAction:
                break;
        }

        UpdateStanbyState(nextState);
    }

    private void UpdateStanbyState(in StanbyState state)
    {
        if (stanbyState == state)
            return;

        stanbyState = state;
        timer = 0.0f;
    }

    private void Move(AiAgent agent)
    {
        switch (stanbyState)
        {
            case StanbyState.Turning:
                Turning(agent);
                break;

            case StanbyState.Ready:

                if (nextState == AiStateId.Rush && timer == 0.0f)
                {
                    agent.animator.SetTrigger("Attack");
                }
                //Y���̌����𒼂�
                if (agent.model.transform.localEulerAngles.y > initModelAngleY + 3.0f ||
                    agent.model.transform.localEulerAngles.y < initModelAngleY - 3.0f)
                {
                    Vector3 angle = agent.model.transform.localEulerAngles;
                    angle.y -= turningModelAngleY * Time.deltaTime * 2;
                    agent.model.transform.localEulerAngles = angle;
                }
                else
                {
                    Vector3 localEulerAngle = agent.model.transform.localEulerAngles;
                    localEulerAngle.y = initModelAngleY;
                    agent.model.transform.localEulerAngles = localEulerAngle;

                    //�^�[�Q�b�g�̕���������
                    LookTargetXZ(agent);
                }
                break;

            case StanbyState.SetNextAction:
                SetNextAction(agent);
                break;
        }
    }

    //����
    private void Turning(AiAgent agent)
    {
        if (agent.model.transform.localEulerAngles.y < initModelAngleY + turningModelAngleY)
        {
            Vector3 angle = agent.model.transform.localEulerAngles;
            angle.y += turningModelAngleY * Time.deltaTime;
            agent.model.transform.localEulerAngles = angle;
        }

        agent.transform.RotateAround(
            turningCenterPos,
            Vector3.up,
            turningSpeed * Time.deltaTime);
    }

    private void SetNextAction(AiAgent agent)
    {
        if (actionCount >= 4)
        {
            actionCount = 0;
            agent.stateMachine.ChangeState(nextState);
            return;
        }
        ++actionCount;
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}
