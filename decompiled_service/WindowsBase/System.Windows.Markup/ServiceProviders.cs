using System.ComponentModel;

namespace System.Windows.Markup;

/// <summary>Provides an implementation for the <see cref="T:System.IServiceProvider" /> interface with methods that enable adding services.</summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class ServiceProviders : IServiceProvider
{
	/// <summary>Gets the service object of the specified type.</summary>
	/// <param name="serviceType">The type of service object to get.</param>
	/// <returns>A service implementation for the type <paramref name="serviceType" />. May be <see langword="null" /> if there is no service stored for type <paramref name="serviceType" />.</returns>
	public object GetService(Type serviceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds a service to the list. </summary>
	/// <param name="serviceType">Service type of the new service.</param>
	/// <param name="service">The service implementation class.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="serviceType" /> or <paramref name="service" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">Attempted to add a service that already exists in the dictionary.</exception>
	public void AddService(Type serviceType, object service)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.ServiceProviders" /> class. </summary>
	public ServiceProviders()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
