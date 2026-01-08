using LiSoft.Application.Models;
using LiSoft.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LiSoft.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] ContactDto contactDto)
    {
        _logger.LogDebug("Recebida requisição para criar contato: {Name}, {Email}", 
            contactDto.Name, contactDto.Email);
        
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            _logger.LogDebug("Criando contato no banco de dados...");
            var contact = await _contactService.CreateContactAsync(contactDto);
            
            _logger.LogInformation("Novo contato criado com sucesso. ID: {Id}, Email: {Email}", 
                contact.Id, contact.Email);

            return Ok(new { message = "Contato criado com sucesso", id = contact.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar contato. Nome: {Name}, Email: {Email}", 
                contactDto.Name, contactDto.Email);
            return StatusCode(500, new { message = "Erro ao processar a solicitação" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        _logger.LogDebug("Recebida requisição para listar todos os contatos");
        
        try
        {
            var contacts = await _contactService.GetAllContactsAsync();
            _logger.LogInformation("Retornados {Count} contatos", contacts.Count());
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar contatos");
            return StatusCode(500, new { message = "Erro ao processar a solicitação" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(string id)
    {
        _logger.LogDebug("Recebida requisição para buscar contato por ID: {Id}", id);
        
        try
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            
            if (contact == null)
            {
                _logger.LogWarning("Contato não encontrado. ID: {Id}", id);
                return NotFound(new { message = "Contato não encontrado" });
            }

            _logger.LogDebug("Contato encontrado: {Name}, {Email}", contact.Name, contact.Email);
            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar contato por ID: {Id}", id);
            return StatusCode(500, new { message = "Erro ao processar a solicitação" });
        }
    }
}
