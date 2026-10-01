using UnityEngine;

public class CoreManager: MonoBehaviour
{
    void Start()
    {

        // core setup for the game
        //loads everything

        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Rain, SceneDatabase.Scenes.RainScene)
            .WithOverlay()
            .Perform();
    }
}
