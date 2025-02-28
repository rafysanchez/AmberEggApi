﻿using Autofac;
using AutoMapper;
using AmberEggApi.ApplicationService.Mappings;

namespace AmberEggApi.ApplicationService.InjectionModules
{
    public class IoCModuleAutoMapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(
                    context => new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new DomainToViewModelMapping());
                        cfg.AddProfile(new QueryModelToViewModelMapping());
                    }))
                .AsSelf().SingleInstance();

            builder.Register(
                    c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}