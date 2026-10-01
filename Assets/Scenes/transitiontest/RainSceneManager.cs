using UnityEngine;

public class RainSceneManager : MonoBehaviour
{
    public void PlantScene()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.PlantScene, setActive: true)
            .Unload(SceneDatabase.Slots.Rain)
            .Unload(SceneDatabase.Scenes.RainScene)
            .WithOverlay()
            .Perform();
    }

    public void FireScene()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.FireScene, setActive: true)
            .Unload(SceneDatabase.Slots.Rain)
            .Unload(SceneDatabase.Scenes.RainScene)
            .WithOverlay()
            .Perform();
    }
}
