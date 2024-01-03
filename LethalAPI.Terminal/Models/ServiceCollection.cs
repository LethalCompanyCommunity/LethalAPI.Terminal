using System;
using System.Collections.Generic;

namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// A light-weight temporal service collection to provide services for command execution
	/// </summary>
	public struct ServiceCollection
	{
		/// <summary>
		/// Type -> Service Instance mapping for services registered to this container
		/// </summary>
		private Dictionary<Type, object> m_Services = new Dictionary<Type, object>();

		/// <summary>
		/// Creates a new container with the specified services
		/// </summary>
		/// <param name="services"></param>
		public ServiceCollection(params object[] services)
		{
			WithServices(services);
		}

		/// <summary>
		/// Creates a new instance of the service collection with no services
		/// </summary>
		public ServiceCollection()
		{
		}

		/// <summary>
		/// Tries to get a service from the container
		/// </summary>
		/// <param name="t">Type of the service to fetch</param>
		/// <param name="service">Service instance, or <see langword="null"/></param>
		/// <returns><see langword="true"/> if the service could be fetched from the container</returns>
		public bool TryGetService(Type t, out object service)
		{
			if (m_Services == null)
			{
				service = null;
				return false;
			}

			return m_Services.TryGetValue(t, out service);
		}

		/// <summary>
		/// Adds a service to the container, overriding any existing service of the same type
		/// </summary>
		/// <typeparam name="T">Type of the service to register</typeparam>
		/// <param name="instance">Service instance to register</param>
		public void WithService<T>(T instance)
		{
			if (m_Services == null)
			{
				return;
			}

			m_Services[typeof(T)] = instance;
		}

		/// <summary>
		/// Copies an array of services into the container, overriding any existing services of the same time
		/// </summary>
		/// <param name="services">Services to register</param>
		public void WithServices(params object[] services)
		{
			if (m_Services == null)
			{
				return;
			}

			foreach (var service in services)
			{
				if (service == null)
				{
					continue;
				}

				m_Services.Add(service.GetType(), service);
			}
		}
	}
}
