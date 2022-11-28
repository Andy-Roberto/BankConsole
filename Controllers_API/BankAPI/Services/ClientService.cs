using BankAPI.Data;
using BankAPI.Data.BankModels;
namespace BankAPI.Services;
using Microsoft.EntityFrameworkCore;
public class ClientService{
    private readonly BankContext _context;
    
    public ClientService(BankContext context){
        _context = context;
    }
    public async Task<IEnumerable<Client>> GetAll(){
        return await _context.Clients.ToListAsync();
        
    }
    public async Task<Client?> GetById(int id){
        var client = await _context.Clients.FindAsync(id);
        
        return client;
    }

    public async Task<Client> Create(Client newclient){
        _context.Clients.Add(newclient);
        await _context.SaveChangesAsync();
        return newclient;
    }
    public async Task Update(int id, Client client){
        
        var existingClient = await _context.Clients.FindAsync(id);
        if(existingClient is not null)
        {
            existingClient.Name = client.Name;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.Email = client.Email;
            existingClient.Pwd = client.Pwd;
            await _context.SaveChangesAsync();
        }
    }
    public async Task Delete(int id){
        var existingClient =await GetById(id);
        if(existingClient is not null){
            _context.Clients.Remove(existingClient);
            await _context.SaveChangesAsync();
        }
        
    }
}