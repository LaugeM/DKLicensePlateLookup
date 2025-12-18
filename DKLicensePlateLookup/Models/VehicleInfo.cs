using System;
using System.Collections.Generic;
using System.Text;

namespace DKLicensePlateLookup.Models
{
    public sealed record VehicleInfo(
        string? Registreringsnr,
        string? Stelnummer,
        string? MaerkeModel,
        string? ArtAnvend,
        string? FoersteReg);
}
