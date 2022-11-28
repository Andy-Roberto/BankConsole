using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Data.BankModels;
using Microsoft.Extensions.Logging;
using TestBankAPI.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using BankAPI.Data;

namespace BankAPI.Controllers;
    [Authorize(AuthenticationSchemes = "Client,Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _context;
        private readonly ClientService _cliente;
        private readonly BankContext _contexto_aux;
        public AccountController(BankContext _contexto_aux,AccountService context,ClientService cliente)
        {
            _context = context;
            _cliente = cliente;
            this._contexto_aux=_contexto_aux;
        }
        [Authorize(Policy ="SuperAdmin")]
        [HttpGet]
        public async Task<IEnumerable<AccountDtoOut>>Get(){
            return await _context.GetAll();
        }
        [Authorize(Policy ="SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDtoOut>> GetById(int id){
            
            
        var account = await _context.GetDtoId(id);
        if(account is null)
            return NotFound(new {message=$"No existe la cuenta con el id escrito."});
        return account;
        }
        /*eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQW5keSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFuZHJlc0BnbWFpbC5jb20iLCJBZG1pblR5cGUiOiJTdXBlciIsImV4cCI6MTY2OTQ1OTE1NX0.kmLw8EMB4DOtMHiHr_-gqR_i54Y_AVbTtO2AhC2v3_Y5rmBc6WMpMx_o76U-1VdSUuudOqOQaR6uXij0zKVzgg*/
        // [HttpGet("{id}")]
        //  public async Task<ActionResult<AccountDtoOut>> GetDtoId(int id){
            
        // var account = await _context.GetDtoId(id);
        // if(account is null)
        //     return NotFound();
        // return account;
        // }
        [Authorize(Policy ="SuperAdmin")]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(AccountDTOin account){
            var cliente_= await _cliente.GetById((int)account.ClientId);
            if(cliente_ is null){
                return BadRequest();
            }
            var newAccount =await _context.Create(account);
            return CreatedAtAction(nameof(GetById),new{id=newAccount.Id},newAccount);
            
        }
        [Authorize(Policy ="SuperAdmin")]
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Update(int id, AccountDTOin account){
            var numero = account.Id;
            var cuenta_= await _context.GetById(numero);
            if(cuenta_ is null){
                return BadRequest();
            }
            var accountToUpdate = await _context.GetById(id);
            if(accountToUpdate is not null){
                await _context.Update(id,account);
                return NoContent();
            }else{
                return NotFound(new{ message ="No existe la cuenta con el id escrito."});
            }
        }
        [Authorize(Policy ="SuperAdmin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id){
            var existingAccount = await _context.GetById(id);

            if(existingAccount is not null){
                _contexto_aux.BankTransactions.RemoveRange( _contexto_aux.BankTransactions.Where(a=>a.AccountId==id));
                await _context.Delete(id);
                return NoContent();
            }else{
                return NotFound(new{ message ="No existe la cuenta con el id escrito."});
            }
        }
    }
