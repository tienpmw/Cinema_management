using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace CinemaWebAPI.Utilities
{
    public static class Util
    {
        public static string GetRandomString(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        public static void WriteFile(string fileName, object data)

        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fullPath, json);
        }

        public static string ReadFile(string fileName)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string text = File.ReadAllText(fullPath);
            return text;
        }

		public static string GetHexDeviceId()
		{
			// asciiBytes will have your MAC address in decimal
			byte[] unicodeBytes = Encoding.Unicode.GetBytes(GetDeviceId());
			byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
			// show the MAC address in hex
			return BitConverter.ToString(asciiBytes);
		}
		private static string GetDeviceId()
		{
			string macAddresses = string.Empty;

			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (nic.OperationalStatus == OperationalStatus.Up)
				{
					macAddresses += nic.GetPhysicalAddress().ToString();
					break;
				}
			}

			return macAddresses;
		}
	}
}
