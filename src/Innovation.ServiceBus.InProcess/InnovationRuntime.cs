﻿namespace Innovation.ServiceBus.InProcess
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using System.Runtime.Loader;

    using Settings;
    using Api.Querying;
    using Api.Messaging;
    using Api.Reactions;
    using Api.Commanding;
    using Api.Validation;
    using Api.Interceptors;

    public class InnovationRuntime
    {
        #region Fields

        internal static HashSet<string> ReferenceAssemblies { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Innovation.Api",
            "Innovation.ServiceBus.InProcess"
        };
        private readonly IServiceCollection services;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public InnovationRuntime(IServiceCollection services, ILogger<InnovationRuntime> logger, IServiceProvider serviceProviderInstance)
        {
            this.services = services;
            this.logger = logger;
            this.serviceProvider = serviceProviderInstance;
        }

        #endregion Constructor

        #region Methods

        public void Configure()
        {
            this.RegisterHandlers();
        }

        #endregion Methods

        #region Private Methods

        private void RegisterHandlers()
        {
            var assemblyDictionary = new Dictionary<string, Assembly>();

            var discoveredAssemblies = this.LoadReferencingLibraries();

            foreach (var discoveredAssembly in discoveredAssemblies)
            {
                var assemblyName = discoveredAssembly.GetName();

                if (ReferenceAssemblies.Contains(assemblyName.Name))
                {
                    continue;
                }

                if (!assemblyDictionary.ContainsKey(assemblyName.Name))
                {
                    assemblyDictionary.Add(assemblyName.Name, discoveredAssembly);
                }
            }

            var loadedAssemblies = this.LoadFromLocations();

            if (loadedAssemblies?.Length > 0)
            {
                foreach (var loadedAssembly in loadedAssemblies)
                {
                    assemblyDictionary.Add(loadedAssembly.GetName().Name, loadedAssembly);
                }
            }

            var assembliesToIgnore = new List<string>();

            foreach (var assembly in assemblyDictionary)
            {
                if (assembly.Key.StartsWith("Microsoft.") ||
                    assembly.Key.StartsWith("System.") ||
                    assembly.Key.StartsWith("Newtonsoft.") ||
                    assembly.Key.StartsWith("NuGet.") ||
                    assembly.Key.StartsWith("xunit.") ||
                    assembly.Key.StartsWith("Serilog.") ||
                    assembly.Key.StartsWith("Newtonsoft."))
                {
                    assembliesToIgnore.Add(assembly.Key);
                }
            }

            foreach (var ignoredAssembly in assembliesToIgnore)
            {
                if (assemblyDictionary.ContainsKey(ignoredAssembly))
                {
                    assemblyDictionary.Remove(ignoredAssembly);
                }
            }

            var finalAssemblyList = assemblyDictionary.Select(x => x.Value).ToArray();

            foreach (var assembly in finalAssemblyList)
            {
                this.logger.LogDebug("Processing Assembly: {Assembly}", assembly.GetName());

                TypeInfo[] types = null;

                try
                {
                    types = assembly.DefinedTypes.ToArray();
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex.Message, ex);
                }

                if (types == null)
                {
                    this.logger.LogDebug("No Defined Types Found");

                    continue;
                }

                foreach (var type in types.Where(x => x.ImplementedInterfaces.Any(y => y.GenericTypeArguments.Any())))
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    var isCommandHandler = type.AsType().IsGenericTypeOf(typeof(ICommandHandler<ICommand>));
                    var isMessageHandler = type.AsType().IsGenericTypeOf(typeof(IMessageHandler<IMessage>));
                    var isQueryHandler = type.AsType().IsGenericTypeOf(typeof(IQueryHandler<IQuery, IQueryResult>));
                    var isCommandReactor = type.AsType().IsGenericTypeOf(typeof(ICommandReactor<ICommand>));
                    var isCommandResultReactor = type.AsType().IsGenericTypeOf(typeof(ICommandResultReactor<ICommand>));
                    var isCommandInterceptor = type.AsType().IsGenericTypeOf(typeof(ICommandInterceptor<ICommand>));
                    var isCommandValidator = type.AsType().IsGenericTypeOf(typeof(IValidator<ICommand>));

                    if (isCommandHandler)
                    {
                        var commandHandlerInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(ICommandHandler<ICommand>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Command Handler's", type.Name, commandHandlerInterfaces.Count());

                        foreach (var commandHandlerInterface in commandHandlerInterfaces)
                        {
                            var genericArguments = commandHandlerInterface.GetGenericArguments();

                            if (genericArguments != null && genericArguments.Count() == 1)
                            {
                                this.logger.LogDebug("{TypeName} Handles {CommandName}", type.Name, genericArguments[0].Name);
                            }

                            this.services.TryAddTransient(commandHandlerInterface, type.AsType());
                        }
                    }

                    if (isCommandValidator)
                    {
                        var commandValidatorInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(IValidator<ICommand>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Command Validators's", type.Name, commandValidatorInterfaces.Count());

                        foreach (var commandValidatorInterface in commandValidatorInterfaces)
                        {
                            var genericArguments = commandValidatorInterface.GetGenericArguments();

                            if (genericArguments != null && genericArguments.Count() == 1)
                            {
                                this.logger.LogDebug("{TypeName} Validates {CommandName}", type.Name, genericArguments[0].Name);
                            }

                            this.services.TryAddTransient(commandValidatorInterface, type.AsType());
                        }
                    }

                    if (isMessageHandler)
                    {
                        var queryHandlerInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(IMessageHandler<IMessage>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Message Handler's", type.Name, queryHandlerInterfaces.Count());

                        foreach (var queryHandlerInterface in queryHandlerInterfaces)
                        {
                            this.services.TryAddTransient(queryHandlerInterface, type.AsType());
                        }
                    }

                    if (isQueryHandler)
                    {
                        var queryHandlerInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(IQueryHandler<IQuery, IQueryResult>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Query Handler's", type.Name, queryHandlerInterfaces.Count());

                        foreach (var queryHandlerInterface in queryHandlerInterfaces)
                        {
                            var genericArguments = queryHandlerInterface.GetGenericArguments();

                            if (genericArguments != null && genericArguments.Count() == 2)
                            {
                                this.logger.LogDebug("{TypeName} Handles {QueryName} Returning {QueryResultName}", type.Name, genericArguments[0].Name, genericArguments[1].Name);
                            }

                            this.services.TryAddTransient(queryHandlerInterface, type.AsType());
                        }
                    }

                    if (isCommandReactor)
                    {
                        var queryHandlerInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(ICommandReactor<ICommand>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Command Reactor's", type.Name, queryHandlerInterfaces.Count());

                        foreach (var queryHandlerInterface in queryHandlerInterfaces)
                        {
                            this.services.TryAddTransient(queryHandlerInterface, type.AsType());
                        }
                    }

                    if (isCommandResultReactor)
                    {
                        var queryHandlerInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(ICommandResultReactor<ICommand>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Command Result Reactor's", type.Name, queryHandlerInterfaces.Count());

                        foreach (var queryHandlerInterface in queryHandlerInterfaces)
                        {
                            this.services.TryAddTransient(queryHandlerInterface, type.AsType());
                        }
                    }

                    if (isCommandInterceptor)
                    {
                        var commandInterceptorInterfaces = type.ImplementedInterfaces.Where(x => x.IsGenericTypeOf(typeof(ICommandInterceptor<ICommand>)));

                        this.logger.LogDebug("{TypeName} Contains {Count} Command Interceptor's", type.Name, commandInterceptorInterfaces.Count());

                        foreach (var coomandInterceptorInterface in commandInterceptorInterfaces)
                        {
                            this.services.TryAddTransient(coomandInterceptorInterface, type.AsType());
                        }
                    }
                }
            }
        }

        private Assembly[] LoadFromLocations()
        {
            var loadedAssemblyList = new List<Assembly>();

            var searchLocations = this.serviceProvider.GetService<IOptions<SearchLocations>>().Value;

            if (searchLocations != null && searchLocations.Locations != null)
            {
                foreach (var searchLocation in searchLocations.Locations)
                {
                    if (Directory.Exists(searchLocation))
                    {
                        var files = Directory.GetFiles(searchLocation, "*.dll");

                        foreach (var file in files)
                        {
                            try
                            {
                                var loadedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);

                                loadedAssemblyList.Add(loadedAssembly);
                            }
                            catch (Exception ex)
                            {
                                this.logger.LogError(ex.Message, ex);
                            }
                        }
                    }
                }
            }

            return loadedAssemblyList.ToArray();
        }

        private Assembly[] LoadReferencingLibraries()
        {
            var assemblies = this.GetReferencingLibraries();

            return assemblies == null ? new Assembly[] { } : assemblies.ToArray();
        }

        private IEnumerable<Assembly> GetReferencingLibraries()
        {
            try
            {
                var assembliesReferencingInnovation = new List<Assembly>();

                var runtimeLibraries = DependencyContext.Default.RuntimeLibraries;

                foreach (var runtimeLibrary in runtimeLibraries)
                {
                    if (IsCandidateLibrary(runtimeLibrary))
                    {
                        var assembly = Assembly.Load(new AssemblyName(runtimeLibrary.Name));
                        assembliesReferencingInnovation.Add(assembly);
                    }
                }

                return assembliesReferencingInnovation;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool IsCandidateLibrary(RuntimeLibrary runtimeLibrary)
        {
            return ReferenceAssemblies.Contains(runtimeLibrary.Name) || runtimeLibrary.Dependencies.Any(x => ReferenceAssemblies.Any(y => y.StartsWith(x.Name)));
        }

        #endregion Private Methods
    }
}
