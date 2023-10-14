using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Sploosh.GameEngine;
using Sploosh.Modules.Game.Views;

namespace Sploosh.Modules.Game
{
    public class GameModule : IModule
    {
        public GameModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(GameUI));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGameState, GameState>();
        }
    }
}
