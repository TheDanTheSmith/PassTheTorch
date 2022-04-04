using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterObject character;

    public float moveSpeed = 3f;

    public SpriteAnimator animator;
    List<int> freezeFrames = new List<int> { 3, 4, 5, 6 };
    List<int> slowFrames = new List<int> { 2 };


    public bool isCarryingTorch;
    public GameObject torch;

    public List<Transform> torchPositions = new List<Transform>();
    public Transform currentTorchPosition;

    int lastFrame = -1;


    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterObject>();
        torch = GameObject.Find("Torch");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetActive(true);
        if (isCarryingTorch)
        {
            ChooseTorchPos();
            torch.transform.position = currentTorchPosition.position + Vector3.back * 0.5f;

            if (lastFrame != animator.currentFrame)
            {
                torch.transform.position += Vector3.right * Random.Range(-0.02f, 0.02f) + Vector3.up * Random.Range(-0.02f, 0.02f);
            }
        }


        lastFrame = animator.currentFrame;
        if (freezeFrames.Contains(animator.currentFrame))
            return;

        if (Manager.instance.ShouldMoveForward(character))
        {
            animator.SetActive(true);
            Vector3 amount = moveSpeed * Vector3.right;

            if (isCarryingTorch)
            {
                amount = moveSpeed * Input.GetAxis("Horizontal") * Vector3.right * 1.1f;
                if (amount.magnitude == 0f)
                    animator.SetActive(false);
            }

            if (amount.x < -0.01f)
                SetFacing(false);
            else if (amount.x > 0.01f)
                SetFacing(true);

            if (slowFrames.Contains(animator.currentFrame) || Manager.instance.ShouldAllowOvertake(character))
                amount *= 0.5f;
            if (Manager.instance.ShouldAllowOvertake(character))
                amount *= 0.5f;

            transform.position += amount * Time.fixedDeltaTime;


        }
        else
            animator.SetActive(false);
    }

    void ChooseTorchPos()
    {
        currentTorchPosition = null;
        foreach (Transform t in torchPositions)
        {
            if (t.name.Contains("'" + animator.currentFrame.ToString() + "'"))
            {
                currentTorchPosition = t;
                break;
            }
        }

        //Debug.Log("Frame: " + animator.currentFrame.ToString() + " ---- " + currentTorchPosition.name);
    }

    void SetFacing(bool forward)
    {
        if (forward)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(new Vector3(Manager.instance.GetTargetX(character), 1f, 3f), 0.5f);
    }
}
