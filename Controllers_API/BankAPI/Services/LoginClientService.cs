using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;
namespace TestBankAPI.Services;
public class LoginClientService{
    private readonly BankContext _context;
    public LoginClientService(BankContext context)
    {
        _context = context;
    }
    public async Task<Client?> GetClient(ClientDto cliente){
        return await _context.Clients.SingleOrDefaultAsync(x=> x.Email == cliente.Email && x.Pwd == cliente.Pwd);
    }
}
