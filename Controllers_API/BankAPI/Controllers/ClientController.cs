using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// using BankAPI.Data;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.Extensions.Logging;

namespace BankAPI.Controllers;

// [Authorize(AuthenticationSchemes = "Client,Administrador")]
[ApiController]
[Route("api/[controller]")]
public class ClientController: ControllerBase{
    private readonly ClientService _context;
    public ClientController(ClientService context){
        _context = context;
    }
    [HttpGet]
    public async Task<IEnumerable<Client>> Get(){
        return await _context.GetAll();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id){
        var client = await _context.GetById(id);
        if(client is null)
            return NotFound(new {message=$"El cliente con ID = {id} no existe."});
        return client;
    }
    [HttpPost]
    public async Task<IActionResult> Create(Client client){
        var newClient =await _context.Create(client);
        return CreatedAtAction(nameof(GetById),new{id=client.Id},newClient);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client client){
        if(id!=client.Id)
            return BadRequest(new {message=$"La id no coincide."});
        var clientToUpdate = await _context.GetById(id);
        
        if(clientToUpdate is not null){
            await _context.Update(id,client);
            return NoContent();
        }else{
            return NotFound(new {message=$"El cliente con ID = {id} no existe."});
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var existingClient = await _context.GetById(id);   

        if(existingClient is not null){
            await _context.Delete(id);
            return NoContent();
        }else{
            return NotFound();
        }
    }
    
}