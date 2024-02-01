using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gml.Core.Constants;
using Gml.Core.Services.Storage;
using Gml.Models.Auth;
using Gml.WebApi.Models.Enums.Auth;
using GmlCore.Interfaces.Auth;
using GmlCore.Interfaces.Integrations;
using NotImplementedException = System.NotImplementedException;

namespace Gml.Core.Integrations
{
    public class ServicesIntegrationProcedures : IServicesIntegrationProcedures
    {
        private readonly IStorageService _storage;
        private IEnumerable<IAuthServiceInfo>? _authServices;

        public ServicesIntegrationProcedures(IStorageService storage)
        {
            _storage = storage;
        }

        public Task<AuthType> GetAuthType() => _storage.GetAsync<AuthType>(StorageConstants.AuthType);

        public Task<IEnumerable<IAuthServiceInfo>> GetAuthServices()
        {
            return Task.FromResult(new List<IAuthServiceInfo>
            {
                new AuthServiceInfo("Undefined", AuthType.Undefined),
                new AuthServiceInfo("DataLifeEngine", AuthType.DataLifeEngine)
            }.AsEnumerable());
        }

        public async Task<IAuthServiceInfo?> GetActiveAuthService()
        {
            if (_authServices == null || _authServices.Count() == 0)
                _authServices = await GetAuthServices();

            return await _storage.GetAsync<AuthServiceInfo>(StorageConstants.ActiveAuthService);
        }

        public async Task<IAuthServiceInfo?> GetAuthService(AuthType authType)
        {
            if (_authServices == null || _authServices.Count() == 0)
                _authServices = await GetAuthServices();

            return _authServices.FirstOrDefault(c => c.AuthType == authType);
        }

        public async Task SetActiveAuthService(IAuthServiceInfo service)
        {
            await _storage.SetAsync(StorageConstants.AuthType, service.AuthType);
            await _storage.SetAsync(StorageConstants.ActiveAuthService, service);
        }
    }
}
