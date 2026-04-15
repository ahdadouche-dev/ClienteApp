using ClientesApp.Desktop.Services;
using ClientesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientesApp.Desktop
{
    public partial class MainForm : Form
    {
        private readonly ClienteService _service;
        private List<Cliente> _clientes;
        private string _dniEnEdicion = null;

        public MainForm()
        {
            InitializeComponent();
            _service = new ClienteService();
            _clientes = new List<Cliente>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                cboPais.DataSource = Enum.GetValues(typeof(ClientesApp.Domain.Models.Pais));
                cboPais.SelectedIndex = 0;

                _clientes = _service.CargarClientes();
                RefrescarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dniEnEdicion != null)
                {
                    var actualizado = new Cliente
                    {
                        Dni = _dniEnEdicion,
                        Nombre = txtNombre.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        FechaNacimiento = dtpFecha.Value,
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Pais = (Pais)cboPais.SelectedItem
                    };

                    var (ok, errores) = _service.ActualizarCliente(_clientes, _dniEnEdicion, actualizado);
                    if (!ok)
                    {
                        MessageBox.Show(string.Join("\n", errores), "Errores de validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    RefrescarGrid();
                    LimpiarFormulario();
                    SalirModoEdicion();
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    var nuevo = new Cliente
                    {
                        Dni = txtDni.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        FechaNacimiento = dtpFecha.Value,
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim().ToLower(),
                        Pais = (Pais)cboPais.SelectedItem
                    };

                    var (ok, errores) = _service.AgregarCliente(_clientes, nuevo);

                    if (!ok)
                    {
                        MessageBox.Show(string.Join("\n", errores), "Errores de validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    RefrescarGrid();
                    LimpiarFormulario();
                    SalirModoEdicion();
                    MessageBox.Show(_dniEnEdicion != null ? "Cliente actualizado correctamente." : "Cliente agregado correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecciona un cliente de la tabla para eliminar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dni = dgvClientes.SelectedRows[0].Cells["Dni"].Value?.ToString();
                var confirm = MessageBox.Show($"¿Eliminar cliente con DNI {dni}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                var eliminado = _service.EliminarCliente(_clientes, dni);

                if (!eliminado)
                {
                    MessageBox.Show("No se encontró el cliente.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefrescarGrid();
                MessageBox.Show("Cliente eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "CSV|*.csv|JSON|*.json";
                    dialog.Title = "Selecciona un fichero de clientes";

                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    var ruta = dialog.FileName;
                    var extension = Path.GetExtension(ruta).ToLower();

                    progressBar.Value = 0;
                    btnImportar.Enabled = false;

                    List<Cliente> importados = null;

                    await Task.Run(() =>
                    {
                        importados = extension == ".csv"
                            ? _service.ImportarDesdeCsv(ruta)
                            : _service.ImportarDesdeJson(ruta);
                    });

                    progressBar.Value = 50;

                    int agregados = 0;
                    int duplicados = 0;

                    foreach (var cliente in importados)
                    {
                        var (ok, _) = _service.AgregarCliente(_clientes, cliente);
                        if (ok) agregados++;
                        else duplicados++;
                    }

                    progressBar.Value = 100;
                    RefrescarGrid();

                    MessageBox.Show(
                        $"Importación completada.\nAgregados: {agregados}\nDuplicados omitidos: {duplicados}",
                        "Importación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al importar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnImportar.Enabled = true;
                progressBar.Value = 0;
            }
        }

        private void RefrescarGrid()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = new BindingList<Cliente>(_clientes);
        }

        private void LimpiarFormulario()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            dtpFecha.Value = DateTime.Now;
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0) return;

            var row = dgvClientes.SelectedRows[0];
            txtDni.Text = row.Cells["Dni"].Value?.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtApellidos.Text = row.Cells["Apellidos"].Value?.ToString();
            txtTelefono.Text = row.Cells["Telefono"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["FechaNacimiento"].Value?.ToString(), out var fecha))
                dtpFecha.Value = fecha;

            if (Enum.TryParse(row.Cells["Pais"].Value?.ToString(), out Pais pais))
                cboPais.SelectedItem = pais;

            // Activar modo edición
            _dniEnEdicion = txtDni.Text;
            txtDni.Enabled = false;
            btnAgregar.Text = "Guardar cambios";
            btnCancelar.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            SalirModoEdicion();
            LimpiarFormulario();
        }

        private void SalirModoEdicion()
        {
            _dniEnEdicion = null;
            txtDni.Enabled = true;
            btnAgregar.Text = "Agregar";
            btnCancelar.Visible = false;
        }
    }
}
