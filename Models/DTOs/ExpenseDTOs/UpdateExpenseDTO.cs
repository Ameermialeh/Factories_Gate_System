namespace FactoriesGateSystem.Models.DTOs.ExpenseDTOs
{
    public class UpdateExpenseDTO
    {
        public class UpdateExpenseAmountDTO
        {
            public int id;
            public int newAmount;
        }

        public class UpdateExpenseDescription
        {
            public int id;
            public string? newDescription;
        }
    }
}
