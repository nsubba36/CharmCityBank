using System.Security.Cryptography;

namespace CharmCityBank.Api.Utility;

public static class Utilities
{
    public static string GenRandomAlphanumeric(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return RandomNumberGenerator.GetString(chars, length);
    }
}