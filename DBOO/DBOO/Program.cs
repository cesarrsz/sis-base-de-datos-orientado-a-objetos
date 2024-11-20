using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Clase para representar un estudiante
[Serializable]
public class Estudiante
{
    public string ID { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Carrera { get; set; }

    public Estudiante(string id, string nombre, int edad, string carrera)
    {
        ID = id;
        Nombre = nombre;
        Edad = edad;
        Carrera = carrera;
    }

    public void MostrarInfo()
    {
        Console.WriteLine($"ID: {ID}, Nombre: {Nombre}, Edad: {Edad}, Carrera: {Carrera}");
    }
}

// Clase para gestionar la base de datos orientada a objetos
public class BaseDeDatos
{
    private const string ArchivoDB = "base_datos.dat";
    private List<Estudiante> datos;

    public BaseDeDatos()
    {
        datos = CargarDatos();
    }

    // Cargar datos desde el archivo
    private List<Estudiante> CargarDatos()
    {
        if (File.Exists(ArchivoDB))
        {
            try
            {
                using (FileStream fs = new FileStream(ArchivoDB, FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    return (List<Estudiante>)formatter.Deserialize(fs);
                }
            }
            catch
            {
                Console.WriteLine("Error al cargar la base de datos.");
                return new List<Estudiante>();
            }
        }
        return new List<Estudiante>();
    }

    // Guardar datos en el archivo
    private void GuardarDatos()
    {
        using (FileStream fs = new FileStream(ArchivoDB, FileMode.Create))
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, datos);
        }
    }

    // Agregar un estudiante
    public void AgregarEstudiante(Estudiante estudiante)
    {
        datos.Add(estudiante);
        GuardarDatos();
        Console.WriteLine("Estudiante agregado correctamente.");
    }

    // Listar todos los estudiantes
    public void ListarEstudiantes()
    {
        if (datos.Count == 0)
        {
            Console.WriteLine("No hay estudiantes en la base de datos.");
            return;
        }

        Console.WriteLine("Lista de estudiantes:");
        foreach (var estudiante in datos)
        {
            estudiante.MostrarInfo();
        }
    }

    // Buscar un estudiante por ID
    public Estudiante BuscarEstudiante(string id)
    {
        return datos.Find(e => e.ID == id);
    }

    // Eliminar un estudiante por ID
    public void EliminarEstudiante(string id)
    {
        var estudiante = BuscarEstudiante(id);
        if (estudiante != null)
        {
            datos.Remove(estudiante);
            GuardarDatos();
            Console.WriteLine("Estudiante eliminado correctamente.");
        }
        else
        {
            Console.WriteLine("Estudiante no encontrado.");
        }
    }
}

// Clase principal con el menú
class Program
{
    static void Main(string[] args)
    {
        BaseDeDatos baseDatos = new BaseDeDatos();

        while (true)
        {
            Console.WriteLine("\n--- Menú ---");
            Console.WriteLine("1. Agregar estudiante");
            Console.WriteLine("2. Listar estudiantes");
            Console.WriteLine("3. Buscar estudiante");
            Console.WriteLine("4. Eliminar estudiante");
            Console.WriteLine("5. Salir");

            Console.Write("Selecciona una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingresa el ID del estudiante: ");
                    string id = Console.ReadLine();

                    Console.Write("Ingresa el nombre del estudiante: ");
                    string nombre = Console.ReadLine();

                    Console.Write("Ingresa la edad del estudiante: ");
                    int edad = int.Parse(Console.ReadLine());

                    Console.Write("Ingresa la carrera del estudiante: ");
                    string carrera = Console.ReadLine();

                    var estudiante = new Estudiante(id, nombre, edad, carrera);
                    baseDatos.AgregarEstudiante(estudiante);
                    break;

                case "2":
                    baseDatos.ListarEstudiantes();
                    break;

                case "3":
                    Console.Write("Ingresa el ID del estudiante que deseas buscar: ");
                    string idBuscar = Console.ReadLine();

                    var encontrado = baseDatos.BuscarEstudiante(idBuscar);
                    if (encontrado != null)
                    {
                        encontrado.MostrarInfo();
                    }
                    else
                    {
                        Console.WriteLine("Estudiante no encontrado.");
                    }
                    break;

                case "4":
                    Console.Write("Ingresa el ID del estudiante que deseas eliminar: ");
                    string idEliminar = Console.ReadLine();

                    baseDatos.EliminarEstudiante(idEliminar);
                    break;

                case "5":
                    Console.WriteLine("Saliendo del programa.");
                    return;

                default:
                    Console.WriteLine("Opción no válida. Intenta de nuevo.");
                    break;
            }
        }
    }
}

