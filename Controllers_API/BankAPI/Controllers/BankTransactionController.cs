using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
// using BankAPI.Data;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.Extensions.Logging;
using TestBankAPI.Data.DTOs;
using BankAPI.Data.DTOs;

namespace BankAPI.Controllers;

[Authorize(AuthenticationSchemes = "Client,Administrador")]
[ApiController]
[Route("api/[controller]")]
public class BankTransactionController: ControllerBase{
    private readonly BankContext _context;
    private readonly BankTransactionService _service;
    private readonly AccountService _account;
    private readonly ClientService _client;
    public BankTransactionController(BankContext _context,AccountService _account,BankTransactionService  _service){
        this._context = _context;
        this._service = _service;
        this._account = _account;
    }

    [HttpGet("todo")]
    public async Task<IEnumerable<BankTransaction>> Get(){
        var email = this.User.FindFirstValue(ClaimTypes.Email
        );
        // Console.WriteLine("Capacitacion");
        // Console.WriteLine(userId);
        return await _service.GetAllByEmail(email,null);
    }
    
    [HttpPut("depositar")]
    public async Task<IActionResult> Depositar(BankTransactionDtoIn transaccion){

        // Console.WriteLine($"id:{id} cantidad:{cantidad}");
        
        var account = await _account.GetById(transaccion.AccountId);
        if(account is null)
            return NotFound(new {message="No se encontro"});
        account.Balance+=transaccion.Amount;
        var nuevo = new AccountDTOin();
        nuevo.AccountType = account.AccountType;
        nuevo.Balance = account.Balance;
        nuevo.ClientId = account.ClientId;
        nuevo.Id = account.Id;
        await _account.Update(transaccion.AccountId,nuevo);
        var tr = await _service.DoTransaction(account,transaccion,null,1);
        return CreatedAtAction(nameof(GetById),new{id=tr.Id},tr);
        
    }
    [HttpPut("retirar")]
    public async Task<IActionResult> Retirar(BankTransactionIn_ret transaccion){
        var email = this.User.FindFirstValue(ClaimTypes.Email
        );
        var account2 = await _service.GetCuenta(email,transaccion.AccountId);
        if(account2 is null)
            return NotFound(new {message="El cliente no tiene una cuenta con ese id."});
        if(transaccion.ExternalAccount is not null){
            int auxiliar = transaccion.ExternalAccount.GetValueOrDefault();
            var externo = await _account.GetById(auxiliar);
            if(externo is null)
            {
                return NotFound(new {message="No existe esta cuenta"});
            }
        }
    
        var account = await _account.GetById(transaccion.AccountId);
        account.Balance-=transaccion.Amount;
        if(account.Balance<0){
            return NotFound(new {message="El resultado del retiro tiene numeros negativos."});
        }
        var nuevo = new AccountDTOin();
        nuevo.AccountType = account.AccountType;
        nuevo.Balance = account.Balance;
        nuevo.ClientId = account.ClientId;
        nuevo.Id = account.Id;
        await _account.Update(transaccion.AccountId,nuevo);
        BankTransactionDtoIn transaccion_aux = new BankTransactionDtoIn();
        transaccion_aux.AccountId = transaccion.AccountId;
        transaccion_aux.Amount = transaccion.Amount;
        var tr = await _service.DoTransaction(account,transaccion_aux,transaccion.ExternalAccount,2);
        return CreatedAtAction(nameof(GetById),new{id=tr.Id},tr);
        
    }
    [HttpGet("Cuentas")]
    public async Task<IEnumerable<Account?>> getAccounts(){
        var email = this.User.FindFirstValue(ClaimTypes.Email
        );
        // Console.WriteLine("Capacitacion");
        // Console.WriteLine(userId);
        return await _service.GetCuentas(email);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<BankTransaction>> GetById(int id){
        var tr = await _service.GetById(id);
        if(tr is null)
            return NotFound(new {message=$"No existe la trasaccion con el id escrito."});
        return tr;
        }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var email = this.User.FindFirstValue(ClaimTypes.Email
        );
        var account2 =  await _service.GetCuenta(email,id);
        if(account2 is null){
            return NotFound(new {message="El cliente no cuenta con esta cuenta"});
        }
        if(account2.Balance>0)
        {
            return NotFound(new {message="No se puede eliminar la cuenta ya que esta aun cuenta con dinero"});
        }
            var cuenta = await _account.GetById(id);
            if(cuenta is not null){
                await _service.Delete(id);
                await _account.Delete(id);
                return NoContent();
            }else{
                return NotFound(new{ message ="No existe la cuenta con el id escrito."});
            }
        }
}