using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DKLicensePlateLookup.Services.Data
{
    class LocalDataStore
    {
        public void save(string input, string fileName)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    streamWriter.WriteLine(input);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"Exeption {e.Message}");
            }

        }
    }
}
