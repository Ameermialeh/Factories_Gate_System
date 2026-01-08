namespace FactoriesGateSystem.DTOs.ExpenseDTOs
{
    public class UpdateExpenseDTO
    {
        public class UpdateExpenseAmount
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
