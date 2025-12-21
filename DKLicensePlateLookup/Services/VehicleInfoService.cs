using DKLicensePlateLookup.Models;
using DKLicensePlateLookup.Services.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKLicensePlateLookup.Services
{
    public class VehicleInfoService
    {
        private readonly HttpRequestService _httpRequestService;

        public VehicleInfoService(HttpRequestService httpRequestService)
        {
            _httpRequestService = httpRequestService;
        }

        //public async Task<VehicleInfo> LookupVehicleAsync(string registrationNumber)
        //{
        //    // Call HttpRequestService, parse result, map to VehicleInfo, etc.
        //}
    }
}
