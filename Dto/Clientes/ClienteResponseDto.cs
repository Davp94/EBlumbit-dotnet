using System;

namespace EBlumbit.Dto.Clientes;

public class ClienteResponseDto
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; }

    public string NroIdentificacion { get; set; }

    public string FechaNacimiento { get; set; }

    public string Telefono { get; set; }

    public string Correo { get; set; }

    public bool Estado { get; set; }
}
