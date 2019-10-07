using System;
using System.Text.RegularExpressions;

namespace Framework.Utility
{
    public static class FormatPhoneNumberHelper
    {
        public static string RemoveFormatPhone(this string maskPhoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(maskPhoneNumber))
            {
                return maskPhoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "");
            }
            return maskPhoneNumber;
        }
        public static string ApplyFormatPhone(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return "";
            }

            phoneNumber = RemoveFormat(phoneNumber);

            if (string.IsNullOrWhiteSpace(phoneNumber) || (phoneNumber.Length != 10))
            {
                return phoneNumber;
            }

            return String.Format("{0:(000) 000-0000}", double.Parse(phoneNumber));
        }

        public static string ApplyFormatSsn(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return "";
            }

            number = RemoveFormat(number);

            if (string.IsNullOrWhiteSpace(number) || number.Length != 9)
            {
                return number;
            }

            return String.Format("{0:000-00-0000}", double.Parse(number));
        }

        public static string RemoveFormat(this string number)
        {
            return string.IsNullOrWhiteSpace(number) ? number : Regex.Replace(number, "[^.0-9]+", "");
        }


        public static string RemoveNpiFormat(this string number)
        {
            return number.Replace("_", "");
        }

        public static bool IsFormatPhone(this string phoneNumber)
        {
            var match = Regex.Match(phoneNumber, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
            return match.Success;
        }

        public static int ParseToInt(this string number)
        {
            int.TryParse(number, out var result);
            return result;
        }

        public static bool IsFormatEmail(this string email)
        {
            var match = Regex.Match(email, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return match.Success;
        }
    }
}