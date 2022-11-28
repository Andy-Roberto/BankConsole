using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using TestBankAPI.Data.DTOs;

namespace BankAPI.Services;
public class AccountService
{
    private readonly BankContext _context;
    public AccountService(BankContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<AccountDtoOut>> GetAll(){
        return  await _context.Accounts.Select(a => new AccountDtoOut{
            Id = a.Id,
            AccountName = a.AccountTypeNavigation.Name,
            ClientName = a.Client != null ? a.Client.Name: "",
            Balance = a.Balance,
            RegDate = a.RegDate
        }).ToListAsync();
    }
    public async Task<AccountDtoOut?> GetDtoId(int id){
        return  await _context.Accounts.Where(
            a=> a.Id ==id
        ).Select(a => new AccountDtoOut{
            Id = a.Id,
            AccountName = a.AccountTypeNavigation.Name,
            ClientName = a.Client != null ? a.Client.Name: "",
            Balance = a.Balance,
            RegDate = a.RegDate
        }).SingleOrDefaultAsync();
    }
    public async Task<Account?> GetById(int id){
        var account = await _context.Accounts.FindAsync(id);
        return account;
    }
    public async Task<Account> Create(AccountDTOin newaccount){
        var nuevo = new Account();
        nuevo.AccountType = newaccount.AccountType;
        nuevo.Balance=newaccount.Balance;
        nuevo.ClientId=newaccount.ClientId;
        _context.Accounts.Add(nuevo);
        await _context.SaveChangesAsync();
        return nuevo;
    }
    public async Task Update(int id, AccountDTOin account){
        var existingAccount = await _context.Accounts.FindAsync(id);
        if(existingAccount is not null)
        {
            existingAccount.AccountType = account.AccountType;
            existingAccount.ClientId = account.ClientId;
            existingAccount.Balance = account.Balance;
            await _context.SaveChangesAsync();
        }
    }
    public async Task Delete(int id){
        var existingAccount =await GetById(id);
        if(existingAccount is not null){
            _context.Accounts.Remove(existingAccount);
            await _context.SaveChangesAsync();
        }
        
    }
}