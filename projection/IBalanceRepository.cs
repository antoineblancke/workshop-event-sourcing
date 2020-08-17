namespace projection
{
    public interface IBalanceRepository
    {
        void WriteCreditBalance(string bankAccountId, int credit);

        int? ReadBalance(string bankAccountId);
        
        void Clear();
    }
}