using UnityEngine;

public class FireScene: MonoBehaviour
{
    public void PlantScene()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.PlantScene, setActive: true)
            .Unload(SceneDatabase.Scenes.BurnScene)
            .WithOverlay()
            .WithClearUnusedAssets()
            .Perform();
    }

    public void RainScene()
    {
        SceneController.Instance
          .NewTransition()
          .Load(SceneDatabase.Slots.Rain, SceneDatabase.Scenes.RainScene)
          .Unload(SceneDatabase.Scenes.BurnScene)
          .WithOverlay()
          .WithClearUnusedAssets()
          .Perform();
    }



}
