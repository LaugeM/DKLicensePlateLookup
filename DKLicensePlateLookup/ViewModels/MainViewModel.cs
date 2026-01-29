using DKLicensePlateLookup.Commands;
using DKLicensePlateLookup.Services;
using DKLicensePlateLookup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DKLicensePlateLookup.ViewModels
{
    public class MainViewModel : SuperClassViewModel
    {
        VehicleInfoService vehicleInfoService;

        private string _inputRegNumber = string.Empty;
        public string InputRegNumber
        {
            get => _inputRegNumber;
            set
            {
                if (_inputRegNumber == value) return;
                _inputRegNumber = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            vehicleInfoService = new();
        }

        public ICommand LookupRegistrationCommand { get; } = new LookupRegistrationCommand();
        public ICommand LookupInsuranceCommand { get; } = new LookupInsuranceCommand();

        private VehicleViewModel _currentVehicle;
        public VehicleViewModel CurrentVehicle
        {
            get
            {
                return _currentVehicle;
            }
            set
            {
                if (_currentVehicle == value) return;
                _currentVehicle = value;
                OnPropertyChanged();
            }
        }

        public async Task LookupVehicle()
        {
            VehicleInfo newVehicle = await vehicleInfoService.LookupVehicleAsync(InputRegNumber);
            CurrentVehicle = new VehicleViewModel(newVehicle);
        }

        public async Task LookupInsuranceCompany()
        {
            var insuranceCompany = await vehicleInfoService.LookupVehicleInsuranceAsync();
            CurrentVehicle.Insurance = insuranceCompany;
        }

    }
}
