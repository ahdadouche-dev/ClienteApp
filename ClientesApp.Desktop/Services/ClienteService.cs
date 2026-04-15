using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ClientesApp.Domain.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace ClientesApp.Desktop.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService()
        {
            var apiUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            };
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiUrl) };
        }

        public List<Cliente> CargarClientes()
        {
            try
            {
                var response = _httpClient.GetAsync("/clientes").Result;
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Cliente>>(json) ?? new List<Cliente>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }

        public (bool ok, List<string> errores) AgregarCliente(List<Cliente> clientes, Cliente nuevo)
        {
            try
            {
                var json = JsonConvert.SerializeObject(nuevo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync("/clientes", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    clientes.Add(nuevo);
                    return (true, new List<string>());
                }

                var error = response.Content.ReadAsStringAsync().Result;
                return (false, new List<string> { error });
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public bool EliminarCliente(List<Cliente> clientes, string dni)
        {
            try
            {
                var response = _httpClient.DeleteAsync($"/clientes/{dni}").Result;
                if (!response.IsSuccessStatusCode) return false;

                var cliente = clientes.Find(c =>
                    c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));
                if (cliente != null) clientes.Remove(cliente);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return false;
            }
        }

        public void GuardarClientes(List<Cliente> clientes) { } // La API gestiona la persistencia

        public List<Cliente> ImportarDesdeCsv(string rutaOrigen)
        {
            var clientes = new List<Cliente>();
            var lineas = System.IO.File.ReadAllLines(rutaOrigen);

            foreach (var linea in System.Linq.Enumerable.Skip(lineas, 1))
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

        public List<Cliente> ImportarDesdeJson(string rutaOrigen)
        {
            var json = System.IO.File.ReadAllText(rutaOrigen);
            return JsonConvert.DeserializeObject<List<Cliente>>(json) ?? new List<Cliente>();
        }
    }
}