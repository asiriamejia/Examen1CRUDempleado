using System;
using System.Text.RegularExpressions;
using Examen1CRUDempleado.Data;
using Examen1CRUDempleado.Models;

namespace Examen1CRUDempleado.Views
{
    public partial class EmpleadoFormPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private Empleado _empleadoActual;

        public EmpleadoFormPage(Empleado empleado = null)
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _empleadoActual = empleado ?? new Empleado();

            if (empleado != null)
            {
                _empleadoActual = empleado;
                TxtId.Text = _empleadoActual.Id.ToString();
                TxtNombres.Text = _empleadoActual.Nombres;
                TxtApellidos.Text = _empleadoActual.Apellidos;
                TxtPuesto.Text = _empleadoActual.Puesto;
                TxtSalario.Text = _empleadoActual.Salario.ToString();
                TxtCorreo.Text = _empleadoActual.Correo;
                DpFechaIngreso.Date = _empleadoActual.FechaIngreso != default ? _empleadoActual.FechaIngreso : DateTime.Today;

                BtnGuardar.Text = "Actualizar Empleado";
            }
            else
            {
                TxtId.Text = "Nuevo";
                DpFechaIngreso.Date = DateTime.Today;
                BtnGuardar.Text = "Guardar Empleado";
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombres.Text) ||
                string.IsNullOrWhiteSpace(TxtApellidos.Text) ||
                string.IsNullOrWhiteSpace(TxtPuesto.Text) ||
                string.IsNullOrWhiteSpace(TxtSalario.Text) ||
                string.IsNullOrWhiteSpace(TxtCorreo.Text))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            if (!decimal.TryParse(TxtSalario.Text, out decimal salario) || salario <= 0)
            {
                await DisplayAlert("Error", "El salario debe ser un número mayor a cero.", "OK");
                return;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(TxtCorreo.Text, emailPattern))
            {
                await DisplayAlert("Error", "Por favor ingrese un formato de correo válido.", "OK");
                return;
            }

            _empleadoActual.Nombres = TxtNombres.Text.Trim();
            _empleadoActual.Apellidos = TxtApellidos.Text.Trim();
            _empleadoActual.Puesto = TxtPuesto.Text.Trim();
            _empleadoActual.Salario = salario;
            _empleadoActual.Correo = TxtCorreo.Text.Trim();
            _empleadoActual.FechaIngreso = DpFechaIngreso.Date;

            // Faltaban estas líneas para guardar y volver a la lista:
            await _databaseService.SaveEmpleadoAsync(_empleadoActual);
            await DisplayAlert("Éxito", "El empleado se ha guardado correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}