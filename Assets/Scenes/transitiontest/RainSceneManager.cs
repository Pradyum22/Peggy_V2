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
            .WithClearUnusedAssets()
            .Perform();
    }

    public void FireScene()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.BurnScene, setActive: true)
            .Unload(SceneDatabase.Slots.Rain)
            .Unload(SceneDatabase.Scenes.RainScene)
            .WithOverlay()
            .WithClearUnusedAssets()
            .Perform();
    }
}
