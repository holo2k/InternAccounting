namespace InternAccounting.BusinessLayer.DataTransferObjects.Intern
{
    using System.Text.RegularExpressions;

    public class PhoneNumber
    {
        public string Number { get; private set; }

        private static readonly Regex PhoneRegex = new Regex(@"^\+7\d{10}$", RegexOptions.Compiled);

        public PhoneNumber(string number)
        {
            number = number.Replace(" ", "");
            if (!IsValid(number))
                throw new ArgumentException("Некорректный номер телефона. Должен быть формата '+7xxxxxxxxxx'", nameof(number));

            Number = number;
        }

        public static bool IsValid(string number) => PhoneRegex.IsMatch(number);

        public override string ToString() => Number;
    }
}
