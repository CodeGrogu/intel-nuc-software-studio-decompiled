using UniwillSdkClient.UniwillService;

namespace UniwillSdkClient.Interfaces;

public interface IUniwillClient
{
	UniwillServiceClient Client { get; }

	void Initialize();

	RGBKeyboardCapabilities GetKeyboardType();
}
