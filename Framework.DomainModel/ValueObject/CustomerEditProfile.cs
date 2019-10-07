using System;
using System.Globalization;

namespace Framework.DomainModel.ValueObject
{

    public class CustomerEditProfile : DtoBase
    {
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string BirthDate { get; set; }
        public DateTime? BirthDateValue
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BirthDate))
                {
                    return null;
                }
                DateTime.TryParseExact(BirthDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
                return result;
            }
        }
        public string Description { get; set; }
        public string Email { get; set; }
        public int? LanguageId { get; set; }
        public string Avatar { get; set; }
    }
}