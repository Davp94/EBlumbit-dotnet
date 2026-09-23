using System;

namespace EBlumbit.Models;

public class Cliente
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; }

    public string NroIdentificacion { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public string Telefono { get; set; }

    public string Correo { get; set; }

    public bool Estado { get; set; }

}
