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
public class LoginController: ControllerBase{
    private readonly LoginService loginService;
    private readonly BankContext _context;
    private IConfiguration config;
    public LoginController(BankContext _context,LoginService loginService, IConfiguration config){
        this.loginService= loginService;
        this._context = _context;
        this.config = config;
    }
    [HttpPost("authenticate")]
    public async Task<IActionResult> Login(AdminDto adminDto){
        var admin = await loginService.GetAdmin(adminDto);
        if(admin is null)
            return BadRequest(new {message=" Credenciales invalidas."});
        string jwtToken = GenerateToken(admin);
        return Ok(new {token=jwtToken});
    }
    [HttpGet]
    public IEnumerable<Administrator> Get(){
        return _context.Administrators.ToList();
    }
    [HttpPut]
    public void Crear(Administrator admin){
        
        _context.Administrators.Add(admin);
        _context.SaveChanges();
        
    }
    private string GenerateToken(Administrator admin){
        var claims = new[]{
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email,admin.Email),
            new Claim("AdminType",admin.AdminType)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
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