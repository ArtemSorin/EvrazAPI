using Microsoft.AspNetCore.Mvc;
using ScoredBackend;

namespace EvrazAPI.Controllers
{
    public class WebController : Controller
    {
        [HttpPost]
        [Route("/auth")]
        public ResponseAuth RequestAuth(RequestAuthArgs args)
        {
            DBTable query = DBConnection.Self!.Query(
                "SELECT * FROM user WHERE login = @0 AND password = @1",
                [args.Login, args.Password]
            );

            if (query.IsError || query.rows.Count == 0)
            {
                return new ResponseAuth() { Id = -1, Role = UserRole.Operator };
            }

            return new ResponseAuth() {
                Id = (int)query.rows[0]["id"]!,
                Role = (UserRole)query.rows[0]["role"]!
            };
        }

        [HttpPost]
        [Route("/admin_stations")]
        public ResponseAdminStations RequestAdminStations(RequestAdminStationsArgs args)
        {
            DBTable query = DBConnection.Self!.Query(
                "SELECT * FROM station"
            );

            List<StationInfo> stations = [];
            foreach (Dictionary<string, object?> station in query.rows)
            {
                stations.Add(new StationInfo() { Id = (int)station["id"]!, Name = (string)station["name"]! });
            }
            return new ResponseAdminStations() { Stations = stations };
        }
    }

    public enum UserRole
    { 
        Admin,
        Operator
    }

    public struct StationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public struct RequestAuthArgs
    {
        public string Login { get; set;}
        public string Password { get; set;}
    }

    public struct ResponseAuth
    {
        public int Id { get; set;}
        public UserRole Role { get; set;}
    }

    public struct RequestAdminStationsArgs
    {
        
    }

    public struct ResponseAdminStations
    {
        public List<StationInfo> Stations; 
    }
}
