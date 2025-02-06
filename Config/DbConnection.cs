using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using ZstdSharp.Unsafe;

namespace Learn_Managment_System_Backend.Config
{
    public class DbConnection
    {

        //se declara variable para cliente de mongodbb
        public MongoClient client;

        private readonly IMongoDatabase _database;



        //contructor
        public DbConnection(IConfiguration configuration)
        {
            try
            {
                //Obteniendo el string de conexion a la base de datos
                var ConnectionString = configuration.GetConnectionString("MongoDb");

                //Se crea el cliente desde donde se consultara a la base de datos
                client = new MongoClient(ConnectionString);

                //Selecciono la base de datos a la que quiero consultar
                _database = client.GetDatabase("LearnManagement");

            }
            catch (System.Exception Error)
            {
                Console.WriteLine("Conexion a base de datos fallida" + "\n " + Error.Message);
            }
        }

        /*Este metodo utiliza la interfaz IMongoCollection<T> la cual esta hecha para intereactuar 
        con las colecciones de una base de datos pasandole un modelo*/
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}