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
        CargarAlumnos();
    }

    private async void CargarAlumnos()
    {
        var alumnos = await client.Child("Alumno").OnceAsync<Alumnos>();
        foreach (var alumno in alumnos)
        {
            Lista.Add(alumno.Object);
        }
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
}