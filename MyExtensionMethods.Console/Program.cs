using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyExtensionMethods.Test
{
    public enum Equipos : int
    {
        // Para tener un valor por defecto
        SinEquipo = 0,

        [Description("San Antonio")]
        SanAntonio = 1,

        Chicago = 2,

        [Description("L.A. Lakers")]
        LaLakers = 3,  
        
        LaClippers = 4,

        Detroit = 5,

        Dallas = 6, 
    }

    [Flags]
    public enum Posiciones : int
    {
        uno = 1,
        dos = 2,
        tres = 4,
        cuatro = 8,
        cinco = 16,
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0}. Desde atributo Description: {1}", Equipos.SanAntonio.GetDescription(), Equipos.SanAntonio.HasDescription());
            Console.WriteLine("{0}. Desde atributo Description: {1}", Equipos.LaClippers.GetDescription(), Equipos.LaClippers.HasDescription());
            Console.WriteLine("{0}. Desde atributo Description: {1}", Equipos.LaLakers.GetDescription(), Equipos.LaLakers.HasDescription());

            var magic = Posiciones.uno | Posiciones.dos | Posiciones.tres | Posiciones.cuatro | Posiciones.cinco;
            var dirk = Posiciones.tres | Posiciones.cuatro | Posiciones.cinco;

            Console.WriteLine(magic.HasFlag(Posiciones.dos));
            Console.WriteLine(dirk.HasFlag(Posiciones.dos));

            var equipo1 = "Chicago".ToEnum<Equipos>();
            Equipos? equipo2 = "Toronto".ToEnum<Equipos>();

            Dictionary<string, string> dicEquipos = typeof(Equipos).ToDictionary();
            Dictionary<string, string> dicPosiciones = typeof(Posiciones).ToDictionary();

            var algo = magic.ParseFromString("Magic", Posiciones.dos);
            algo = magic.ParseFromString<Posiciones>("Magic");
            algo = magic.ParseFromString<Posiciones>("Uno");

            Console.ReadLine();
        }
    }
}
