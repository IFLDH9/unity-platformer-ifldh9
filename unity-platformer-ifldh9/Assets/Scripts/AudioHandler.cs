using UnityEngine;
using UnityEngine.Networking;

public class AudioHandler : NetworkBehaviour
{

    [SerializeField] private AudioSource breakBlock;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource placeBlock;

    public void PlayJump()
    {
        jumpSound.Play();
    }

    public void PlayHitSound()
    {
        hit.Play();
    }

    public void PlayBreakBlock()
    {
        breakBlock.Play();
    }

    public void PlayPlaceBlock()
    {
        placeBlock.Play();
    }

}
