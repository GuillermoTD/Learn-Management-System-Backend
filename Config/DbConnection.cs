using MongoDB.Bson;
using MongoDB.Driver;

namespace Learn_Managment_System_Backend.Config
{
    public class DbConnection
    {

        //se declara variable para cliente de mongodbb
        public MongoClient client;
        private readonly IConfiguration _Configuration;
        private readonly IMongoDatabase _database;

        //contructor
        public DbConnection(IConfiguration configuration )
        {
            try
            {
                _Configuration = configuration;
                //Obteniendo el string de conexion a la base de datos
                var ConnectionString = _Configuration.GetConnectionString("MongoDb");

                //Se crea el cliente desde donde se consultara a la base de datos
                client = new MongoClient(ConnectionString);

                //Selecciono la base de datos a la que quiero consultar
                _database = client.GetDatabase("LearnManagement");

            }
            catch (System.Exception Error)
            {
                Console.WriteLine("****Conexion a base de datos fallida****" + "\n " + Error.Message);
            }
        }

        public DbConnection()
        {
        }

        public void PingDatabase()
        {
            try
            {
                // Ejecutamos un comando de ping a la base de datos 'admin'
                var command = new BsonDocument("ping", 1);
                var result = _database.RunCommand<BsonDocument>(command);

                // Si no lanza excepción, significa que la conexión es exitosa
                Console.WriteLine("**********Conexión exitosa a la base de datos.**********");
            }
            catch (System.Exception error)
            {

                // Si ocurre una excepción, la conexión ha fallado
                Console.WriteLine($"**Error al conectar a la base de datos** :  {error.Message}");
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