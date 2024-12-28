using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using GestionEstudianteEva3.modelos.Modelos;
namespace GestionEstudianteEva3.AppMovil.Vista;

public partial class EditarAlumno : ContentPage
{

    FirebaseClient client = new FirebaseClient("https://gestionestudiante-5fdb4-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; }
    public ObservableCollection<string> ListaCursos { get; set; } = new ObservableCollection<string>();
    private Alumnos alumnoActualizado = new Alumnos();
    private string alumnoId;
    public EditarAlumno(string idAlumno)
    {
        InitializeComponent();
        BindingContext = this;
        alumnoId = idAlumno;
        CargarAlumnos(alumnoId);
        CargarListaCursos();
        
    }

private async void CargarListaCursos()
    {
        try
        {
            var cursos = await client.Child("Cursos").OnceAsync<Curso>();
            ListaCursos.Clear();
            foreach (var curso in cursos)
            {
                ListaCursos.Add(curso.Object.Nombre);
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error!", "Error:" +ex.Message, "OK");
        }

    }


private async void CargarAlumnos(string idAlumno)
    {
        var alumno = await client.Child("Alumno")
            .Child(idAlumno).OnceSingleAsync<Alumnos>();
        if (alumno != null)
        {
            EditPrimerNombreEntry.Text = alumno.PrimerNombre;
            EditSegundoNombreEntry.Text = alumno.SegundoNombre;
            EditPrimerApellidoEntry.Text = alumno.PrimerApellido;
            EditSegundoApellidoEntry.Text = alumno.SegundoApellido;
            EditCorreoEntry.Text = alumno.CorreoElectronico;
            EditEdadEntry.Text = alumno.Edad.ToString();
            EditCursoPicker.SelectedItem = alumno.Curso.Nombre;

        }
    }
    private async void ActualizarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(EditPrimerNombreEntry.Text) ||
                 string.IsNullOrEmpty(EditPrimerApellidoEntry.Text) ||
                 string.IsNullOrEmpty(EditSegundoNombreEntry.Text) ||
                  string.IsNullOrEmpty(EditSegundoApellidoEntry.Text) ||
                 string.IsNullOrEmpty(EditCorreoEntry.Text) ||
                 string.IsNullOrEmpty(EditEdadEntry.Text) ||
                    EditCursoPicker.SelectedItem == null

                 )
                 
            {
                await DisplayAlert("Error!", "Todos los campos son requeridos", "OK");
                return;
            }
            if (!EditCorreoEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error!", "Correo invalido", "OK");
                return;
            }

            if (!int.TryParse(EditEdadEntry.Text, out var edad))
            {
                await DisplayAlert("Error!", "La Edad no es un numero valido", "OK");
                return;
            }
            if (edad <= 0)
            {
                await DisplayAlert("Error!", "La Edad debe ser mayor o igual a 0", "OK");
                return;
            }
            alumnoActualizado.Id = alumnoId;
            alumnoActualizado.PrimerNombre = EditPrimerNombreEntry.Text.Trim();
            alumnoActualizado.SegundoNombre = EditSegundoNombreEntry.Text.Trim();
            alumnoActualizado.PrimerApellido = EditPrimerApellidoEntry.Text.Trim();
            alumnoActualizado.SegundoApellido = EditSegundoApellidoEntry.Text.Trim();
            alumnoActualizado.CorreoElectronico = EditCorreoEntry.Text.Trim();
            alumnoActualizado.Edad = edad;
            alumnoActualizado.Curso = new Curso
            {
                Nombre = EditCursoPicker.SelectedItem.ToString()
            };


            alumnoActualizado.Estado = EditEstadoSwitch.IsToggled;
            await client.Child("Alumnos").Child(alumnoId).PutAsync(alumnoActualizado);
            await DisplayAlert("Exito", $"El Alumno {alumnoActualizado.PrimerNombre} {alumnoActualizado.PrimerApellido} fue actualizado correctamente", "OK");
            await Navigation.PopAsync();


        }
        catch (Exception)
        {

        }
    }
}
