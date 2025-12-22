using System;
using System.Collections.Generic;
using System.Text;

namespace DKLicensePlateLookup.Models
{
    public class VehicleInfo
    {
        public string RegNumber { get; }
        public string VIN { get; }
        public string MakeAndModel { get; }
        public string TypeUse { get; }
        public string FirstReg { get; }
        public string Insurance { get; set; } = "Ikke indlæst";


        public VehicleInfo(string regNumber, string vin, string makeAndModel, string typeUse, string firstReg)
        {
            RegNumber = regNumber;
            VIN = vin;
            MakeAndModel = makeAndModel;
            TypeUse = typeUse;
            FirstReg = firstReg;
        }
    }
    
}
