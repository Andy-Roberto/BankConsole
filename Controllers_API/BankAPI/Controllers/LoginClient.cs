using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.DTOs;
using TestBankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data;
using BankAPI;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using System.Net.Http;

namespace BankAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginClient: ControllerBase{
    private readonly LoginClientService loginService;
    private readonly BankContext _context;
    private IConfiguration config;
    public LoginClient(BankContext _context,LoginClientService loginService, IConfiguration config){
        this.loginService= loginService;
        this._context = _context;
        this.config = config;
    }
    [HttpPost("authenticate/cliente")]
    public async Task<IActionResult> Login(ClientDto clientDto){
        var cliente = await loginService.GetClient(clientDto);
        if(cliente is null)
            return BadRequest(new {message=" Credenciales invalidas."});
        string jwtToken = GenerateToken(cliente);
        return Ok(new {token=jwtToken});
    }
    [HttpGet]
    public IEnumerable<Client> Get(){
        return _context.Clients.ToList();
    }
    [HttpPut]
    public void Crear(Client client){
        
        _context.Clients.Add(client);
        _context.SaveChanges();
        
    }
    private string GenerateToken(Client client){
        var claims = new[]{
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(ClaimTypes.Email,client.Email),
            
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key2").Value));
        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
        var securityToken = new JwtSecurityToken(
            claims:claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials:creds
        );
    string  token = new JwtSecurityTokenHandler().WriteToken(securityToken);
    return token;
    }
}  