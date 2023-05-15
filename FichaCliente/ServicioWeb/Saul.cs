using FichaCliente.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace FichaCliente.ServicioWeb
{
    public class Saul : InterfazSaul
    {
        private readonly string baseUrl;
        private readonly string secretKey;
        private static string URLapi;

        public Saul(IConfiguration config)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
            secretKey = builder.GetSection("ApiSettings:secretKey").Value;
            URLapi = builder.GetSection("ApiSettings:baseApi").Value;
        }

        public ModeloUsuario Autenticacion(string usuario, string cont)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(cont))
                return null;
            var token = new JwtSecurityTokenHandler();
            var LlaveTk = Encoding.ASCII.GetBytes(secretKey);
            var credenciales = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("user",usuario),
                    new Claim("pass",cont)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(LlaveTk), SecurityAlgorithms.HmacSha256)
            };
            var salidaTk = token.CreateToken(credenciales);
            var TK = token.WriteToken(salidaTk);
            var API = new RestClient(baseUrl);
            API.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            API.Timeout = -1;
            var consulta = new RestRequest(Method.POST);
            consulta.AddHeader("Content-Type", "application/json");
            var body = "{\r\n\n    \"token\":\"" + TK + "\"\r\n\n}";
            consulta.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse<ModeloUsuario> resp = API.Execute<ModeloUsuario>(consulta);
            if (!string.IsNullOrEmpty(resp.Content))
            {
                var json = JsonConvert.DeserializeObject<ModeloUsuario>(resp.Content.Substring(1, resp.Content.Length - 2));
                return json;
            }
            else
            {
                ModeloUsuario respuesta = new ModeloUsuario() { login = "SIN RESPUESTA", cargo = "SCRUM MASTER A" };
                return respuesta;
            }
        }

        public bool Cargos(Int64 agenda)
        {
            bool valida = false;
            var rol = new RestClient(URLapi + agenda + "/1");
            rol.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            rol.Timeout = -1;
            var menus = new RestRequest(Method.GET);
            IRestResponse resp = rol.Execute(menus);
            if (!string.IsNullOrEmpty(resp.Content))
            {
                JObject json = JObject.Parse(resp.Content);
                JArray roles = (JArray)json["response"];
                //JArray json = JArray.Parse(resp.Content);
                if (json.Count > 0)
                    valida = true;
                else
                    valida = false;
            }
            else
            {
                valida = false;
            }
            return valida;
        }
    }
}
