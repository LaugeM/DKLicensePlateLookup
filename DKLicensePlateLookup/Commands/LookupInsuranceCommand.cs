using DKLicensePlateLookup.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DKLicensePlateLookup.Commands
{
    public class LookupInsuranceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool result = false;

            if (parameter is MainViewModel mvm)
            {
                if (mvm.CurrentVehicle != null)
                {
                    result = mvm.CurrentVehicle.CheckVinExists();
                }
            }

            return result;
        }

        public void Execute(object parameter)
        {
            if (parameter is MainViewModel mvm)
            {
                mvm.LookupInsuranceCompany();
            }
            else
            {
                throw new ArgumentException("Illegal parameter type");
            }
        }
    }
}
