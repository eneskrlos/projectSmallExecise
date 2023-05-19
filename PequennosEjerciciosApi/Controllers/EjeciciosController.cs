using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using PequennosEjerciciosApi.BattleForMiddlerEarth;
using PequennosEjerciciosApi.Controllers.Exceptions;
using PequennosEjerciciosApi.Controllers.Validations;
using PequennosEjerciciosApi.Enums;
using PequennosEjerciciosApi.Utils;
using System.Collections;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PequennosEjerciciosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjeciciosController : ControllerBase
    {

        private readonly ValidationsNumerosPerdidos _validations = new();
        private readonly ValidationsBattlePokemon _battlePokemon = new();
        /*
        // GET: api/<EjeciciosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EjeciciosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EjeciciosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EjeciciosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EjeciciosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */

        //nivel facil Este metodo permite hayar los proximos 30 annos bisiestos
        [HttpPost("/Next30LeapYears")]
        public int[] Next30LeapYears([FromBody] int year)
        {
            int currentYear = year + 1;
            int yearCount = 0;
            int[] leapYears = new int[30];
            int i = 0;
            while (yearCount < 30)
            {
                if ((currentYear % 4 == 0) && (currentYear % 100 != 0 || currentYear % 400 == 0)) {
                    leapYears[yearCount++] = currentYear;
                }
                currentYear++;
            }

            return leapYears;

        }
        // Este metodo encuentra el segundo numero mayor 
        [HttpPost("/findSecoundGreater")]
        public int? FindSecoundGreater([FromBody] List<int> numbers)
        {
            List<int> result = new List<int>(numbers);
            List<int> listaux = new List<int>(numbers.Count);

            foreach (int number in numbers)
            {
                bool found = false;

                for (int i = 0; i < result.Count && !found; i++)
                {
                    if (number >= result[i]) {
                        if (number != result[i])
                        {
                            listaux.Insert(i, number);
                        }
                        found = true;
                    }
                }

                if (!found)
                {
                    listaux.Add(number);
                }
            }

            return (listaux.Count >= 2) ? listaux[1] : null;
        }

        [HttpPost("/calendarioSexagenarioChino")]
        public string CilcoSexagenarioChino([FromBody] int year)
        {
            string[] elementos = new string[] { "madera", "fuego", "tierra", "metal", "agua" };
            string[] animales = new string[] { "rata", "buey", "tigre", "conejo", "dragon",
                "serpiente", "caballo", "oveja", "mono", "gallo", "perro", "cerdo" };
            string result = string.Empty;

            /// El calendario empeso el anno 604
            if (year < 604)
            {
                result = "El ciclo sexagenario comenzó en el año 604";
                return result;
            }

            var sexagenaryYear = (year - 4) % 60;
            int posElemento = (sexagenaryYear % 10) / 2;
            var elemento = elementos[posElemento];
            int posAnimal = sexagenaryYear % 12;
            var animal = animales[posAnimal];

            result = $"{year}:{elemento} {animal}";
            return result;
        }

        //Metodo que busca los numeros perdidos de un arrelgo dado
        [HttpPost("/numerosPerdidos")]
        public List<int> NumerosPerdidos(List<int> numbers)
        {
            try
            {
                _validations.EmptyListOrListSmall(numbers);

                int first = numbers.First();
                int last = numbers.Last();
                bool asc = (first < last);

                // Verifico que esten ordenados y no esten repetidos
                for (int i = 0; i < numbers.Count - 1; i++)
                {
                    for (int j = i + 1; j < numbers.Count; j++)
                    {
                        _validations.ListNumberException(asc, numbers[i], numbers[j]);
                    }
                }

                //Busco los perdidos
                return FindLost(asc, numbers, first, last);

            }
            catch (EmptyListOrListSmallException emp)
            {
                throw new Exception(emp.Message);
            }
            catch (ListNumberExceptionException le)
            {
                throw new Exception(le.Message);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }


        private List<int> FindLost(bool asc,List<int> numbers, int first, int last)
        {
            List<int> lost = new();

            if (asc)
            {

                for (int i = first; i < last; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        lost.Add(i);
                    }
                }

            }
            else
            {

                for (int i = first; i > last; i--)
                {
                    if (!numbers.Contains(i))
                    {
                        lost.Add(i);
                    }
                }
            }
            return lost;
        }

        /**
         * Batalla Pokemon
         * crear un programas que calcule el danno de un ataque durante una batalla Pokemon.
         * -la formula del danno es la siguiente : danno = 50 * (ataque / defensa) * efectividad
         * -Efectividad: x2 ( super efectivo), x1 (neutral), x0.5 (no es muy efectivo)
         * -Solo hay 4 tipos de pokemon: Agua, Fuego, Planta y Ele'ctrico (buscar efectividad)
         * -El programa recibe los siguientes parametros:
         * -Tipo de Pokemon atacante
         * -Tipo de pokemon defensor
         * -Ataque : Entre 1 y 100
         * -Defensa: Entre 1 y 100
        */
        [HttpPost("/batlePokemon")]
        public Double BatlePokemon([FromBody] RequestBattle requestBattle)
        {
            try
            {
                _battlePokemon.ElementIsNull(requestBattle);
                _battlePokemon.IncorrectValueAttackDefender(requestBattle);
                _battlePokemon.IncorrectValue(requestBattle);


                //Creo un diccionario para almacenar los que son efectivoc y no efectivos
                Dictionary<PokemonType, object[]> dict = new Dictionary<PokemonType, object[]>();
                dict.Add(PokemonType.WATER, new object[] { PokemonType.FIRE, PokemonType.GRASS });
                dict.Add(PokemonType.FIRE, new object[] { PokemonType.GRASS, PokemonType.WATER });
                dict.Add(PokemonType.GRASS, new object[] { PokemonType.WATER, PokemonType.FIRE });
                dict.Add(PokemonType.ELECTRIC, new object[] { PokemonType.WATER, PokemonType.GRASS });

                var pokemonAtacador = dict[GetPokemonType(requestBattle.atacante)];
                var efectivo = pokemonAtacador[0];
                var noefectivo = pokemonAtacador[1];

                var pokemonDefensor = GetPokemonType(requestBattle.defensor);

                var efectividadAtaque = 1.0;
                if (requestBattle.atacante == requestBattle.defensor || noefectivo.Equals(pokemonDefensor))
                {
                    efectividadAtaque = 0.5; // no efectiva
                }
                else if (efectivo.Equals(pokemonDefensor))
                {
                    efectividadAtaque = 2.0; // super efectiva
                }


                var calcularDanno = 50 * (Double)requestBattle.atacante / (Double)requestBattle.defensa * efectividadAtaque;
                return calcularDanno;
            }
            catch (ElementNullException e)
            {

                throw new Exception(e.Message);
            }
            catch (IncorrectValuesExcepion e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        private PokemonType GetPokemonType(int pokemon)
        {
           if ( pokemon == 1)
           {
                return PokemonType.WATER;
           }
           else if ( pokemon == 2) 
           { 
                return PokemonType.FIRE;
           }
           else if ( pokemon == 3)
           {
                return PokemonType.GRASS;
           }
           else 
           {
                return PokemonType.ELECTRIC;
           }
        }


        /**
         * Los Anillos del Poder
         * La tierra media esta en guerra en ellas lucharan razas leales a Sauron
         * contra otras bondadosas que no quieren que el mal reine sobre sus tierras.
         * Cada raza tiene asociado a un valor entre 1 y 5
         * -Razas bondadosas: Pelosos(1), Surennos(2), Enanos (3), Numenoriano(4), Elfo (5)
         * -Raza malvadas: Surennos malos(2), Orcos(2), Goblins(2), Huargos(3) , Trolls(5)
         * Crear un programa  que calcule resultado de la batalla entre los 2 tipos de ejercitos;
         * -El resultado puede ser que gane el bien, el mal o exista un empate. Dependiendo del 
         * suma del valor del ejecito y el numero de integrantes
         * -cada ejercito debe estar compuesto por un numero de integrates variables de cada razas
         * -Tienes total libertad para modelar los datos del ejercicio.
         */
        [HttpPost("/battleBetweenGodBad")]
        public string BattleBetweenGodBad(RequestBattleArmy requestBattleArmy )
        {
            int kindArmynPoint = 0;
            int evilArmynPoint = 0;

            
            //int cantKind = requestBattleArmy.kindArmyn.Count;
            //int cantEvil = requestBattleArmy.evilArmyn.Count;

            
            
            requestBattleArmy.kindArmyn.ForEach(army => {
                //Console.WriteLine($"{(int)army.Key} -- {army.Key}");
                kindArmynPoint += (int)army.Key * army.Value;
            });

            requestBattleArmy.evilArmyn.ForEach(army =>
            {
                //Console.WriteLine($"{(int)army.Key} -- {army.Key}");
                evilArmynPoint += ((int)army.Key) * army.Value;
            });

            if(kindArmynPoint > evilArmynPoint)
            {
                return "Gano el Bien";
            }
            else if (kindArmynPoint < evilArmynPoint)
            {
                return "Gano el Mal";
            }
            else
            {
                return "Empate";
            }
        }
         

    }
}
