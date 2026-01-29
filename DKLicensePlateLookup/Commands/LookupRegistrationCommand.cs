using DKLicensePlateLookup.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DKLicensePlateLookup.Commands
{
    public class LookupRegistrationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool result = true;

            if (parameter is MainViewModel mvm)
            {
                
            }

            return result;
        }

        public void Execute(object parameter)
        {
            if (parameter is MainViewModel mvm)
            {
                mvm.LookupVehicle();
            }
            else
            {
                throw new ArgumentException("Illegal parameter type");
            }
        }
    }
}
