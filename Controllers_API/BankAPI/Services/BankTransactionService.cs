using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using TestBankAPI.Data.DTOs;
using BankAPI.Data.DTOs;

namespace BankAPI.Services;
public class BankTransactionService
{
    private readonly BankContext _context;
    private readonly AccountService _account;
    private readonly ClientService _client;
    public BankTransactionService(BankContext context,AccountService _account,ClientService _client)
    {
        _context = context;
        this._account = _account;
        this._client = _client;
    }
    public async Task<IEnumerable<BankTransaction>> Getall(int id){
        
        return  await _context.BankTransactions.Where(a=>a.AccountId==id).ToListAsync();
    }
    public async Task<IEnumerable<BankTransaction?>> GetAllByEmail(string email,int? id){
        var cliente = await _context.Clients.Where(a=>a.Email==email).SingleOrDefaultAsync();
        
        if(id is null){
            return  await _context.BankTransactions.Where(a=>a.AccountId==cliente.Id).ToListAsync();
        }else{

            
            return  await _context.BankTransactions.Where(a=>a.AccountId==cliente.Id&&a.Id==id).ToListAsync();
        }
    }
    public async Task<IEnumerable<Account?>> GetCuentas(string email){
        var cliente = await _context.Clients.Where(a=>a.Email==email).SingleOrDefaultAsync();
            return  await _context.Accounts.Where(a=>a.ClientId==cliente.Id).ToListAsync();
    }
    public async Task<Account?> GetCuenta(string email,int id){
        var cliente = await _context.Clients.Where(a=>a.Email==email).SingleOrDefaultAsync();
            return  await _context.Accounts.Where(a=>a.ClientId==cliente.Id&&a.Id==id).SingleOrDefaultAsync();
    }
    public async Task<BankTransaction?> GetById(int id){
        return await _context.BankTransactions.FindAsync(id);
    }
    public async Task<BankTransaction> DoTransaction(Account account,BankTransactionDtoIn tr,int? ExternalAccount,int TransactionType){
        var transaccion = new BankTransaction();
        transaccion.AccountId=account.Id;
        transaccion.Amount= tr.Amount;
        transaccion.TransactionType = TransactionType;
        
        

        if(ExternalAccount is not null){
            transaccion.ExternalAccount = ExternalAccount;
            var account_ = new AccountDTOin();
            int buscar = ExternalAccount.GetValueOrDefault();
            var account2_ = await _account.GetById(buscar);
            account_.AccountType = account2_.AccountType;
            account_.Balance +=tr.Amount+account2_.Balance;
            account_.ClientId = account2_.ClientId;
            account_.Id = account2_.Id;
            await _account.Update(buscar,account_);
        }
        _context.BankTransactions.Add(transaccion);
        await _context.SaveChangesAsync();
        return transaccion;

    }
    public async Task Delete(int id){
        var existingAccount = await _context.BankTransactions.Where(a=>a.AccountId==id).ToListAsync();
        if(existingAccount is not null)
        {
            _context.BankTransactions.RemoveRange( _context.BankTransactions.Where(a=>a.AccountId==id));
            await _account.Delete(id);
            await _context.SaveChangesAsync();
        }
    }
}