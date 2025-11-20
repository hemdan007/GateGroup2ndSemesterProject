using simply_database_test.secret;

namespace simply_database_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            conet con = new conet();
            testrepo test = new testrepo(con.connet);

            List<testallergi> allergiw = new List<testallergi>();
            allergiw = test.GetAll();

            foreach (testallergi a in allergiw)
            {
                Console.WriteLine($" it has an id of {a.ID} and name of {a.Name}");

            }
        }
    }
}
