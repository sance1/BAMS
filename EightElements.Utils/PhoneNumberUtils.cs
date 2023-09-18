using PhoneNumbers;

namespace EightElements.Utils
{
    public static class PhoneNumberUtils
    {
        public static PhoneNumberResult Verify(string phone, string code)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumber = phoneNumberUtil.Parse(phone, code);
                var valid = phoneNumberUtil.IsValidNumberForRegion(phoneNumber, code);
                return new PhoneNumberResult()
                {
                    CountryCode = phoneNumber.CountryCode.ToString(),
                    Number = phoneNumber.NationalNumber.ToString(),
                    IsError = !valid
                };
            }
            catch (NumberParseException e)
            {
                return new PhoneNumberResult()
                {
                    CountryCode = "",
                    Number = "",
                    IsError = true
                };
            }
        }

        public static PhoneNumberResult Split(string phone)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumber = phoneNumberUtil.Parse("+" + phone, "");
                return new PhoneNumberResult()
                {
                    CountryCode = phoneNumber.CountryCode.ToString(),
                    Number = phoneNumber.NationalNumber.ToString(),
                    IsError = false
                };
            }
            catch (NumberParseException e)
            {
                return new PhoneNumberResult()
                {
                    CountryCode = "",
                    Number = "",
                    IsError = true
                };
            }
        }

        public class PhoneNumberResult
        {
            public string CountryCode { get; set; }
            public string Number { get; set; }

            public bool IsError { get; set; }
        }
    }
}