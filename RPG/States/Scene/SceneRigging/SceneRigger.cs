using RPG.States.Characters.Individuals;
using RPG.States.World;

namespace RPG.States.Scene.SceneRigging
{
    class SceneRigger
    {
        internal void BuildScene(Scenes ActiveScene)
        {
            switch (ActiveScene)
            {
                case Scenes.CITY_ONE: buildCityOne(); break;
                case Scenes.WORLD: returnToWorld(); break;
            }

        }

        private void buildCityOne()
        {
            SceneCharacter sc = new SceneCharacter("Ola");

            GSS.ActiveScene = new CityOne(sc);
            GSS.RenderState = RenderState.Scene;
        }

        private void returnToWorld()
        {
            GSS.RenderState = RenderState.World;
        }
    }
}
