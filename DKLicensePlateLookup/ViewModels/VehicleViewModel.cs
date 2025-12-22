using DKLicensePlateLookup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DKLicensePlateLookup.ViewModels
{
    public class VehicleViewModel : SuperClassViewModel
    {
        private VehicleInfo _vehicleInfo;

        private string _regNumber = string.Empty;
        public string RegNumber
        {
            get => _regNumber;
            set
            {
                if (_regNumber == value) return;
                _regNumber = value;
                OnPropertyChanged();
            }
        }

        private string _vin = string.Empty;
        public string VIN
        {
            get => _vin;
            set
            {
                if (_vin == value) return;
                _vin = value;
                OnPropertyChanged();
            }
        }

        private string _makeAndModel = string.Empty;
        public string MakeAndModel
        {
            get => _makeAndModel;
            set
            {
                if (_makeAndModel == value) return;
                _makeAndModel = value;
                OnPropertyChanged();
            }
        }

        private string _typeUse = string.Empty;
        public string TypeUse
        {
            get => _typeUse;
            set
            {
                if (_typeUse == value) return;
                _typeUse = value;
                OnPropertyChanged();
            }
        }

        private string _firstReg = string.Empty;
        public string FirstReg
        {
            get => _firstReg;
            set
            {
                if (_firstReg == value) return;
                _firstReg = value;
                OnPropertyChanged();
            }
        }

        private string _insurance = string.Empty;
        public string Insurance
        {
            get => _insurance;
            set
            {
                if (_insurance == value) return;
                _insurance = value;
                OnPropertyChanged();
            }
        }

        public VehicleViewModel(VehicleInfo vehicleInfo)
        {
            this._vehicleInfo = vehicleInfo;

            RegNumber = _vehicleInfo.RegNumber;
            VIN = _vehicleInfo.VIN;
            MakeAndModel = _vehicleInfo.MakeAndModel;
            TypeUse = _vehicleInfo.TypeUse;
            FirstReg = _vehicleInfo.FirstReg;
            Insurance = _vehicleInfo.Insurance;
        }
    }
}
