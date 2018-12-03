﻿using System.Configuration;
using SimpleInjector;
using System.Web.Http;
using DataAccess.NHibernate;
using DataAccess.Repositories;
using FileManagement;
using Frontend.App_Data;
using SimpleInjector.Integration.WebApi;
using UserManagement.Application;
using UserManagement.Domain;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;
using System;

namespace Frontend.App_Start
{
    public class Bootstrapper
    {
        public Container Setup()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            RegisterSettings(container);
            SetupDependencies(container);
            container.Verify();

            return container;
        }

        private void SetupDependencies(Container container)
        {
            container.Register<IUserRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<IUserManager>(() => container.GetInstance<UserManager>(), Lifestyle.Singleton);
            container.Register<ISessionProvider>(() => container.GetInstance<SessionProvider>(), Lifestyle.Singleton);          
            container.Register<IFileManager>(() => container.GetInstance<FileManager>(), Lifestyle.Singleton);
            container.Register<IAuthorizer>(() => new Authorizer(
                        TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["Authorizer.TokenLifeTimeInSeconds"])),
                        container.GetInstance<IUserRepository>()), Lifestyle.Singleton);
        }

        private static void RegisterSettings(Container container)
        {
            var settings = ConfigurationManager.AppSettings;
            container.Register(() => SettingsReader.ReadFileStorage(settings), Lifestyle.Singleton);

        }
    }
}