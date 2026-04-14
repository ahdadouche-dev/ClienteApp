using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientesApp.Domain.Models;
using ClientesApp.Domain.Validators;
using Newtonsoft.Json;

namespace ClientesApp.Desktop.Services
{
    public class ClienteService
    {
        private readonly string _rutaFichero;

        public ClienteService(string rutaFichero = "clientes_store.json")
        {
            _rutaFichero = rutaFichero;
        }

        public List<Cliente> CargarClientes()
        {
            try
            {
                if (!File.Exists(_rutaFichero)) return new List<Cliente>();
                var json = File.ReadAllText(_rutaFichero);
                return JsonConvert.DeserializeObject<List<Cliente>>(json) ?? new List<Cliente>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }

        public void GuardarClientes(List<Cliente> clientes)
        {
            try
            {
                var json = JsonConvert.SerializeObject(clientes, Formatting.Indented);
                File.WriteAllText(_rutaFichero, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar clientes: {ex.Message}");
                throw;
            }
        }

        public (bool ok, List<string> errores) AgregarCliente(List<Cliente> clientes, Cliente nuevo)
        {
            var (esValido, errores) = ClientValidator.Validar(nuevo);
            if (!esValido) return (false, errores);

            if (clientes.Any(c => c.Dni.Equals(nuevo.Dni, StringComparison.OrdinalIgnoreCase)))
                return (false, new List<string> { "Ya existe un cliente con ese DNI." });

            clientes.Add(nuevo);
            GuardarClientes(clientes);
            return (true, new List<string>());
        }

        public bool EliminarCliente(List<Cliente> clientes, string dni)
        {
            var cliente = clientes.FirstOrDefault(c =>
                c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));

            if (cliente == null) return false;

            clientes.Remove(cliente);
            GuardarClientes(clientes);
            return true;
        }

        public List<Cliente> ImportarDesdeJson(string rutaOrigen)
        {
            var json = File.ReadAllText(rutaOrigen);
            return JsonConvert.DeserializeObject<List<Cliente>>(json) ?? new List<Cliente>();
        }

        public List<Cliente> ImportarDesdeCsv(string rutaOrigen)
        {
            var clientes = new List<Cliente>();
            var lineas = File.ReadAllLines(rutaOrigen);

            foreach (var linea in lineas.Skip(1)) // saltar cabecera
            {
                var campos = linea.Split(',');
                if (campos.Length < 6) continue;

                clientes.Add(new Cliente
                {
                    Dni = campos[0].Trim(),
                    Nombre = campos[1].Trim(),
                    Apellidos = campos[2].Trim(),
                    FechaNacimiento = DateTime.TryParse(campos[3].Trim(), out var fecha) ? fecha : DateTime.MinValue,
                    Telefono = campos[4].Trim(),
                    Email = campos[5].Trim()
                });
            }

            return clientes;
        }
    }
}