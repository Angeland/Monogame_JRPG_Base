using RPG.States.Characters.Individuals;
using RPG.States.World;

namespace RPG.States.Scene.SceneRigging
{
    public class CityOne : SceneRenderer
    {
        public CityOne(SceneCharacter sceneCharacter) : base(sceneCharacter, SceneMaps.AllScenes[Scenes.CITY_ONE])
        {
        }
    }
}
