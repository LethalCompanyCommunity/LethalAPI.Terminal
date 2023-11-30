// -----------------------------------------------------------------------
// <copyright file="ServiceCollection.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A light-weight temporal service collection to provide services for command execution.
/// </summary>
public struct ServiceCollection
{
    /// <summary>
    /// Type -> Service Instance mapping for services registered to this container.
    /// </summary>
    private readonly Dictionary<Type, object> services = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCollection"/> struct.
    /// Creates a new container with the specified services.
    /// </summary>
    /// <param name="services">The services to register.</param>
    public ServiceCollection(params object[] services)
    {
        WithServices(services);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCollection"/> struct.
    /// Creates a new instance of the service collection with no services.
    /// </summary>
    public ServiceCollection()
    {
    }

    /// <summary>
    /// Tries to get a service from the container.
    /// </summary>
    /// <param name="t">Type of the service to fetch.</param>
    /// <param name="service">Service instance, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the service could be fetched from the container.</returns>
    public bool TryGetService(Type t, [NotNullWhen(true)] out object? service)
    {
        if (services == null)
        {
            service = null;
            return false;
        }

        return services.TryGetValue(t, out service);
    }

    /// <summary>
    /// Adds a service to the container, overriding any existing service of the same type.
    /// </summary>
    /// <typeparam name="T">Type of the service to register.</typeparam>
    /// <param name="instance">Service instance to register.</param>
    public void WithService<T>(T instance)
    {
        if (services == null)
        {
            return;
        }

        if (instance is null)
        {
            return;
        }

        services[typeof(T)] = instance;
    }

    /// <summary>
    /// Copies an array of services into the container, overriding any existing services of the same time.
    /// </summary>
    /// <param name="servicesToRegister">Services to register.</param>
    public void WithServices(params object[] servicesToRegister)
    {
        if (this.services == null)
        {
            return;
        }

        foreach (object? service in servicesToRegister)
        {
            this.services.Add(service.GetType(), service);
        }
    }
}