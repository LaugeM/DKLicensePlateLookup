using DKLicensePlateLookup.Models;
using DKLicensePlateLookup.Services.Network;
using DKLicensePlateLookup.Services.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKLicensePlateLookup.Services
{
    public class VehicleInfoService
    {
        private readonly HttpRequestService _httpRequestService;
        private readonly HtmlDmrParser _htmlDmrParser;

        public VehicleInfoService()
        {
            _httpRequestService = new HttpRequestService();
            _htmlDmrParser = new HtmlDmrParser();
        }

        public async Task<VehicleInfo> LookupVehicleAsync(string registrationNumber)
        {
            var html = await _httpRequestService.GetInfo(registrationNumber);

            string MakeAndModel = _htmlDmrParser.GetField(html, "Mærke, model, variant");

            string[] MakeAndModelArray = _htmlDmrParser.SplitMakeAndModel(MakeAndModel);

            string RegNumber = _htmlDmrParser.GetField(html, "Registreringsnr.");
            string VIN = _htmlDmrParser.GetField(html, "Stelnummer");
            string Make = MakeAndModelArray[0];
            string Model = MakeAndModelArray[1];
            string TypeUse = _htmlDmrParser.GetField(html, "Art, anvendelse");
            string FirstReg = _htmlDmrParser.GetField(html, "1. registreringsdato");

            VehicleInfo newVehicle = new(RegNumber, VIN, Make, Model, TypeUse, FirstReg);
            return newVehicle;


            // Call HttpRequestService, parse result, map to VehicleInfo, etc.
        }
    }
}
