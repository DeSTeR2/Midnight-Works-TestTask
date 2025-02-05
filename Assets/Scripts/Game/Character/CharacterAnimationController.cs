using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Character
{
    public class CharacterAnimationController
    {
        Animator animator;

        Character character;
        CharacterState state;

        bool isPaused = false;

        public bool IsPause { get => isPaused; }
        public CharacterState State { get => state; }

        public CharacterAnimationController(Animator animator, Character character)
        {
            this.animator = animator;
            this.character = character;
            state = CharacterState.Idle;
        }

        public void Move(Vector3 moveVector)
        {
            if (isPaused)
            {
                animator.SetBool("Moving", false);
                animator.SetFloat("Velocity", 0);
                return;
            } 

            float x = moveVector.x;
            float z = moveVector.z;

            animator.SetFloat("VelocityX", -x);
            animator.SetFloat("VelocityY", z);

            if (x != 0 || z != 0)
            {
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }

            float movement = character.UpdateMovement();
            animator.SetFloat("Velocity", movement);
        }

        public void PickUp(bool isOnFloor)
        {
            state = CharacterState.Carring;
            PickUpAnimation(isOnFloor);
        }

        private void PickUpAnimation(bool floor)
        {
            if (floor)
            {
                SetAnimationTrigger(CharacterAnimations.PickObjectFromFloor);
            }
            else
            {
                SetAnimationTrigger(CharacterAnimations.PickObjectFromTable);
            }
        }

        public void PutDown(bool isOnFloor)
        {
            state = CharacterState.Idle;
            PutDownAnimation(isOnFloor);
        }

        private void PutDownAnimation(bool floor) { 
            if (floor)
            {
                SetAnimationTrigger(CharacterAnimations.PutDownObjectToFloor);
            } else
            {
                SetAnimationTrigger(CharacterAnimations.PutDownObjectToTable);
            }
        }

        private void SetAnimationTrigger(string animName)
        {
            animator.SetTrigger(animName);
            PauseAnimations(1.2f);
        }

        public void WorkAnimation(string animName, bool start)
        {
            isPaused = start;
            SetAnimationBool(animName, start);
            SetAnimationBool(CharacterAnimations.IsWorking, start);
        }

        private void SetAnimationBool(string animName, bool state)
        {
            animator.SetBool(animName, state);
        }

        async void PauseAnimations(float pauseTime)
        {
            isPaused = true;
            await Task.Delay((int)(pauseTime * 1000));
            isPaused = false;
        }
    }

    public static class CharacterAnimations
    {
        public static string PickObjectFromFloor = "CarryPickupTrigger";
        public static string PickObjectFromTable = "CarryRecieveTrigger";
        public static string PutDownObjectToFloor = "CarryPutdownTrigger";
        public static string PutDownObjectToTable = "CarryHandoffTrigger";
        public static string MachineWorking = "MachineWorking";
        public static string CoppingTree = "CoppingTree";
        public static string IsWorking = "IsWorking";
    }
}