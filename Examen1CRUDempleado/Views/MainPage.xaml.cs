using System;
using System.Threading.Tasks;
using Examen1CRUDempleado.Data;
using Examen1CRUDempleado.Models;
using Examen1CRUDempleado.Views;

namespace Examen1CRUDempleado.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadEmpleados();
        }

        private async Task LoadEmpleados()
        {
            LvEmpleados.ItemsSource = await _databaseService.GetEmpleadosAsync();
        }

        // ✅ Corregido: Los botones usan EventArgs, no SelectedItemChangedEventArgs
        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmpleadoFormPage());
        }

        private async void OnEmpleadoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Empleado empleado)
            {
                LvEmpleados.SelectedItem = null; // Deseleccionar

                string accion = await DisplayActionSheet(
                    $"Opciones para {empleado.Nombres}",
                    "Cancelar",
                    null,
                    "Editar",
                    "Eliminar");

                if (accion == "Editar")
                {
                    await Navigation.PushAsync(new EmpleadoFormPage(empleado));
                }
                else if (accion == "Eliminar")
                {
                    bool confirmar = await DisplayAlert("Confirmación", $"¿Está seguro de eliminar a {empleado.Nombres}?", "Sí", "No");
                    if (confirmar)
                    {
                        await _databaseService.DeleteEmpleadoAsync(empleado);
                        await DisplayAlert("Éxito", "El empleado ha sido eliminado.", "OK");
                        await LoadEmpleados();
                    }
                }
            }
        }
    }
}
