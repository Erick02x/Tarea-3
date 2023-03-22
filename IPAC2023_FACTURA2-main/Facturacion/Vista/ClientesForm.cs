using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClientesForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ClientesForm()
        {
            InitializeComponent();
        }

        string tipoOperacion;
        Cliente cliente;
        ClienteDB clienteDB = new ClienteDB();

        DataTable dt = new DataTable();
        Cliente user = new Cliente();

        private void NuevoButton_Click_1(object sender, EventArgs e)
        {
            IdentidadTextBox.Focus();
            tipoOperacion = "Nuevo";
            HabilitarControles();
        }
        private void HabilitarControles()
        {
            IdentidadTextBox.Enabled = true;
            NombreCTextBox.Enabled = true;
            TelefonoTextBox.Enabled = true;
            CorreoCTextBox.Enabled = true;
            DireccionCTextBox.Enabled = true;
            FechaNacimientoDateTimePicker.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
            NuevoButton.Enabled = false;
        }

        private void LimpiarControles()
        {
            IdentidadTextBox.Clear();
            NombreCTextBox.Clear();
            TelefonoTextBox.Clear();
            CorreoCTextBox.Clear();
            DireccionCTextBox.Clear();
            EstaActivoCheckBox.Checked = false;
        }

        private void DeshabilitarControles()
        {
            IdentidadTextBox.Enabled = false;
            NombreCTextBox.Enabled = false;
            TelefonoTextBox.Enabled = false;
            CorreoCTextBox.Enabled = false;
            DireccionCTextBox.Enabled = false;
            FechaNacimientoDateTimePicker.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
        }

        private void CancelarButton_Click_1(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }
        private void ModificarButton_Click_1(object sender, EventArgs e)
        {
            tipoOperacion = "Modificar";
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                IdentidadTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                NombreCTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                TelefonoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Telefono"].Value.ToString();
                CorreoCTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                DireccionCTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Direccion"].Value.ToString();
                FechaNacimientoDateTimePicker.Text = ClientesDataGridView.CurrentRow.Cells["FechaNacimiento"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ClientesDataGridView.CurrentRow.Cells["EstaActivo"].Value);

                HabilitarControles();
                IdentidadTextBox.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }
        private void GuardarButton_Click(object sender, EventArgs e)
        {
            cliente = new Cliente();
            cliente.Identidad = IdentidadTextBox.Text;
            cliente.Nombre = NombreCTextBox.Text;
            cliente.Telefono = TelefonoTextBox.Text;
            cliente.Correo = CorreoCTextBox.Text;
            cliente.Direccion = DireccionCTextBox.Text;
            cliente.FechaNacimiento = Convert.ToDateTime(FechaNacimientoDateTimePicker.Text);
            cliente.EstaActivo = EstaActivoCheckBox.Checked;

            if (tipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(IdentidadTextBox.Text))
                {
                    errorProvider1.SetError(IdentidadTextBox, "Ingrese su identidad");
                    IdentidadTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(NombreCTextBox.Text))
                {
                    errorProvider1.SetError(NombreCTextBox, "Ingrese su nombre");
                    NombreCTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(TelefonoTextBox.Text))
                {
                    errorProvider1.SetError(TelefonoTextBox, "Ingrese su telefono");
                    TelefonoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(CorreoCTextBox.Text))
                {
                    errorProvider1.SetError(CorreoCTextBox, "Ingrese su correo");
                    CorreoCTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(DireccionCTextBox.Text))
                {
                    errorProvider1.SetError(DireccionCTextBox, "Ingrese su Direccion");
                    DireccionCTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(FechaNacimientoDateTimePicker.Text))
                {
                    errorProvider1.SetError(FechaNacimientoDateTimePicker, "Ingrese su fecha nacimiento");
                    FechaNacimientoDateTimePicker.Focus();
                    return;
                }
                errorProvider1.Clear();

                bool inserto = clienteDB.Insertar(cliente);
                if (inserto)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    DevolverClientes();
                    MessageBox.Show("Registro guardado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tipoOperacion == "Modificar")
            {
                bool modifico = clienteDB.Editar(cliente);
                if (modifico)
                {
                    IdentidadTextBox.ReadOnly = false;
                    DeshabilitarControles();
                    LimpiarControles();
                    DevolverClientes();
                    MessageBox.Show("Registro actualizado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se puro actualizar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void EliminarButton_Click(object sender, EventArgs e)
        {

            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = clienteDB.Eliminar(ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        DevolverClientes();
                        MessageBox.Show("Registro eliminado");
                    }
                    else
                    { MessageBox.Show("No se pudo eliminar el registro"); }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }


        }
        private void ClientesForm_Load(object sender, EventArgs e)
        {
            DevolverClientes();
        }
        private void DevolverClientes()
        {
            ClientesDataGridView.DataSource = clienteDB.DevolverClientes();
        }




    }

}




