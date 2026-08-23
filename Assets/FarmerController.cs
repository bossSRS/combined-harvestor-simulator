using CHS.Tutorial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerController : MonoBehaviour
{
    public List<Transform> Target;
    public GameObject CurrentTarget;
    public NavMeshAgent agent;
    public Vector3 DefPosition;
    public bool OnTarget;
    public Animator animator;
    public TutorialManager tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        DefPosition = transform.position;
        animator = GetComponent<Animator>();
        tutorialManager = FindAnyObjectByType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(CurrentTarget.transform.position,transform.position) <= 2f)
        {
            animator.SetBool("move", false);
            OnTarget = false;
            Invoke("ResetDirecxtion", 1f);
        }
        else
        {
            agent.SetDestination(CurrentTarget.transform.position);
            animator.SetBool("move",true);
        }
    }
    public void ResetDirecxtion()
    {
        OnTarget = true;
        var temp = Target[UnityEngine.Random.Range(0, Target.Count)].gameObject;
        if(CurrentTarget == temp)
        {
            CurrentTarget = Target[UnityEngine.Random.Range(0, Target.Count)].gameObject;
        }
        else
        {
            CurrentTarget = temp;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.gameObject.tag == "CHS")
            {
                if(tutorialManager != null)
                {
                    tutorialManager.HittingPeopleWarning();
                }
                gameObject.SetActive(false);
            }
        }
    }
}
