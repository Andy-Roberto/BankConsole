namespace BankAPI.Data.DTOs;
public class BankTransactionIn_ret{


    public int AccountId { get; set; }


    public int? ExternalAccount { get; set; }

    public decimal Amount { get; set; }
}