using UnityEngine;

public class PlantScene: MonoBehaviour
{
    public void FireScene()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, SceneDatabase.Scenes.FireScene, setActive: true)
            .Unload(SceneDatabase.Scenes.PlantScene)
            .WithOverlay()
            .Perform();
    }
    public void RainScene()
    {
        SceneController.Instance
          .NewTransition()
          .Load(SceneDatabase.Slots.Rain, SceneDatabase.Scenes.RainScene)
          .Unload(SceneDatabase.Scenes.PlantScene)
          .WithOverlay()
          .Perform();
    }


}
