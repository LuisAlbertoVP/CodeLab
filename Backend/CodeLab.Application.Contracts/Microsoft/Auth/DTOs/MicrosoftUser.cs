using System;

namespace CodeLab.Application.Contracts.Microsoft.Auth.DTOs;

public class MicrosoftUser
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string FotoBase64 { get; set; }
    public string RolesActiveDirectory { get; set; }
}
