using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string SceneToLoad = default;
    public Vector2 PlayerNewCoordinates = default;
    [SerializeField] FadePanelSignal FadeToSignal = default;
    [SerializeField] FadePanelSignal FadeFromSignal = default;
    [SerializeField] PlayerLocation PlayerLocation = default;
    GameObject player;

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (player == null)
                player = collidedObject.gameObject;
            StartCoroutine(FadeCo());
        }
    }
    public IEnumerator FadeCo()
    {
        if (FadeToSignal)
        {
            player.GetComponent<PlayerMovement>().Freeze();
            FadeToSignal.Raise();
            yield return new WaitForSeconds(FadeToSignal.Duration);
        }
        PlayerLocation.FacingDirection = player.GetComponent<PlayerInput>().CurrentFacingDirection;
        PlayerLocation.FadeFromSignal = FadeFromSignal;
        PlayerLocation.Location = PlayerNewCoordinates;
        PlayerLocation.UseThis = true;
        SceneManager.LoadScene(SceneToLoad);
        yield return null;
    }
}
