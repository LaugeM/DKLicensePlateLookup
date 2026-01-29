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
            var infoHtml = await _httpRequestService.GetInfo(registrationNumber);

            string MakeAndModel = _htmlDmrParser.GetField(infoHtml, "Mærke, model, variant");

            string[] MakeAndModelArray = _htmlDmrParser.SplitMakeAndModel(MakeAndModel);

            string RegNumber = _htmlDmrParser.GetField(infoHtml, "Registreringsnr.");
            string VIN = _htmlDmrParser.GetField(infoHtml, "Stelnummer");
            string Make = MakeAndModelArray[0];
            string Model = MakeAndModelArray[1];
            string TypeUse = _htmlDmrParser.GetField(infoHtml, "Art, anvendelse");
            string FirstReg = _htmlDmrParser.GetField(infoHtml, "1. registreringsdato");

            VehicleInfo newVehicle = new(RegNumber, VIN, Make, Model, TypeUse, FirstReg);
            
            return newVehicle;
        }

        public async Task<string> LookupVehicleInsuranceAsync()
        {
            var insuranceHtml = await _httpRequestService.GetInsuranceInfo();

            // Convenient method

            string insuranceCompany = _htmlDmrParser.GetInsuranceCompany(insuranceHtml);

            // Alternate method, useful to get other information than just insurance company name

            // string insurance = _htmlDmrParser.GetFieldFromSection(insuranceHtml, "Forsikring", "Selskab");

            return insuranceCompany;
        }
    }
}
