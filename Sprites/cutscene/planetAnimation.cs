using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class planetAnimation : MonoBehaviour
{
    private Animator anim;
    private bool done = false;
    private int count = 0;
    public int threshold;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform.DOScale(1.5f, 4f);
    }
    public void OnAnimationFinished()
    {
        Debug.Log(count);
        count++;
        if (count >= threshold)
        {
            if (!done) 
            { 
                anim.SetTrigger("shake"); 
                done = true;
                AudioManager.Instance.Play("rumble");
            }
            //else { Destroy(gameObject); }
            count = 0;
        }
    }
}
