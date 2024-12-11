

using Firebase.Database;
using Firebase.Database.Query;
using GestionEstudianteEva3.modelos.Modelos;


namespace GestionEstudianteEva3.AppMovil.Vista;

public partial class CrearAlumno : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://gestionestudiante-5fdb4-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; } = new List<Curso>();



    public CrearAlumno()
    {
        InitializeComponent();
        ListarCurso();
        BindingContext = this;
    }

    private void ListarCurso()
    {
        var cursos = client.Child("Cursos").OnceAsync<Curso>();
        Cursos = cursos.Result.Select(x => x.Object).ToList();
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        Curso curso = (Curso)cursoPicker.SelectedItem;
        var alumnos = new Alumnos
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            Edad = Convert.ToInt32(edadEntry.Text),
            Curso = curso
        };
        try
        {
            await client.Child("Alumno").PostAsync(alumnos);
            await DisplayAlert("Guardado", $"El Alumno {alumnos.PrimerNombre} {alumnos.PrimerApellido} fue guardado correctamente", "Ok");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al guardar el Alumno: {ex.Message}", "Ok");
        }


    }
}