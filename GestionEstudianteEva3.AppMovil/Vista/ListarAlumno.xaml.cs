using Firebase.Database;
using Firebase.Database.Query;
using GestionEstudianteEva3.modelos.Modelos;
using System.Collections.ObjectModel;

namespace GestionEstudianteEva3.AppMovil.Vista;

public partial class ListarAlumno : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://gestionestudiante-5fdb4-default-rtdb.firebaseio.com/");
    public ObservableCollection<Alumnos> Lista { get; set; } = new ObservableCollection<Alumnos>();

    public ListarAlumno()
    {
        InitializeComponent();
        BindingContext = this;
        CargarCargar();
    }

    private async void CargarCargar()
    {
        Lista.Clear();
        var alumnos = await client.Child
            ("Alumno").OnceAsync<Alumnos>();


        foreach (var alumno in alumnos)
        {
            Lista.Add(new Alumnos
            {
                Id = alumno.Key,
                PrimerNombre = alumno.Object.PrimerNombre,
                SegundoNombre = alumno.Object.SegundoNombre,
                PrimerApellido = alumno.Object.PrimerApellido,
                SegundoApellido = alumno.Object.SegundoApellido,
                CorreoElectronico = alumno.Object.CorreoElectronico,
                Edad = alumno.Object.Edad,
                Estado = alumno.Object.Estado,
                Curso = alumno.Object.Curso



            });




        }
        #region CodigoAntiguo
        //var alumnos = await client.Child("Alumno").OnceAsync<Alumnos>();
        //foreach (var alumno in alumnos)
        //{
        //    Lista.Add(alumno.Object);
        //}
        #endregion
    }



    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();

        if (filtro.Length > 0)
        {
            ListaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else
        {
            ListaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoAlumnoBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearAlumno());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var alumno = boton?.CommandParameter as Alumnos;

        if (alumno != null && !string.IsNullOrEmpty(alumno.Id))
        {
            await Navigation.PushAsync(new EditarAlumno(alumno.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la informacion del alumno", "Ok");
        }
    }

    private async void deshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var alumno = boton?.CommandParameter as Alumnos;

        if (alumno != null && !string.IsNullOrEmpty(alumno.Id))
        {
            var confirm = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas deshabilitar este alumno?", "Sí", "No");
            if (confirm)
            {
                alumno.Estado = false;
                await client.Child("Alumno").Child(alumno.Id).PutAsync(alumno);
                Lista.Remove(alumno);
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la informacion del alumno", "Ok");
        }
    }
}