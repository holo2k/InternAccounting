namespace InternAccounting.BusinessLayer.DataTransferObjects.Intern
{
    public class Email
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            if (!IsValid(address))
                throw new ArgumentException("Некорректный email", nameof(address));

            Address = address;
        }

        public static bool IsValid(string email) =>
            !string.IsNullOrWhiteSpace(email) && email.Contains("@");

        public override string ToString() => Address;
    }
}
