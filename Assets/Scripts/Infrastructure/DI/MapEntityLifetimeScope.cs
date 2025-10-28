using FissuredDawn.Core.Interfaces.GameManagers;
using FissuredDawn.Infrastructure.Startup;
using FissuredDawn.Scene.Data;
using FissuredDawn.Scene.Spawner;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.DI
{
    public class MapEntityLifetimeScope : LifetimeScope
    {
        [SerializeField] private CharacterManager _characterManager;

        [SerializeField] private Transform _characterTransform;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseComponents(components =>
            {
                components.AddInNewPrefab(_characterManager, Lifetime.Scoped)
                    .UnderTransform(gameObject.transform)
                    .As<IEntityManager<MapCharacter>>()
                    .AsSelf();
            });
            builder.RegisterEntryPoint<MapEntityStartupConfiguration>();
        }
    }

}