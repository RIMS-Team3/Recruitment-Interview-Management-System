namespace RecruitmentInterviewManagementSystem.Domain.ValueObjects
{
    public class Money
    {
        public decimal Value { get; set; }

        public Money(decimal value)
        {
            if (value < 0) value = 0;

            Value = value;
        }
    }
}
