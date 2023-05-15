using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace FichaCliente.Scripts
{
    public class Consultas
    {
        public List<String> consultaDOC()
        {
            List<string> ci = new List<string>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Documento_de_Identidad FROM CARTERA_ACT WHERE Documento_de_Identidad <> '' GROUP BY Documento_de_Identidad " +
                               "UNION SELECT Numero_de_Identificacion FROM CAJA_ACT WHERE Numero_de_Identificacion <> '' GROUP BY Numero_de_Identificacion " +
                               "UNION SELECT Numero_de_Idetificacion FROM DPF_ACT WHERE Numero_de_Idetificacion <> '' GROUP BY Numero_de_Idetificacion " +
                               "UNION SELECT Numero_de_Identificacion FROM CORR_ACT WHERE Numero_de_Identificacion <> '' GROUP BY Numero_de_Identificacion " +
                               "UNION SELECT Nro_CI FROM TARJETAS_ACT WHERE Nro_CI <> '' GROUP BY Nro_CI";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string dato = lecturaBD.GetString(0);
                        ci.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return ci;
        }

        public List<String> consultaNOM()
        {
            List<string> titular = new List<string>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Nombre_Titular FROM CARTERA_ACT WHERE Nombre_Titular <> '' GROUP BY Nombre_Titular " +
                               "UNION SELECT Nombre_Cliente FROM CAJA_ACT WHERE Nombre_Cliente <> '' GROUP BY Nombre_Cliente " +
                               "UNION SELECT Nombre_de_la_Cuenta FROM DPF_ACT WHERE Nombre_de_la_Cuenta <> '' GROUP BY Nombre_de_la_Cuenta " +
                               "UNION SELECT Nombre_Cliente FROM CORR_ACT WHERE Nombre_Cliente <> '' GROUP BY Nombre_Cliente " +
                               "UNION SELECT Nombre_Cliente FROM TARJETAS_ACT WHERE Nombre_Cliente <> '' GROUP BY Nombre_Cliente";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string dato = lecturaBD.GetString(0);
                        titular.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return titular;
        }

        public List<(string, string)> GetEdad(string busca)
        {
            List<(string, string)> extra = new List<(string, string)>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Edad_Prestatario,Fecha_de_Apertura FROM CAJA_ACT WHERE Numero_de_Identificacion= '" + busca + "' OR Nombre_Cliente= '" + busca + "'";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string edad = lecturaBD.GetString(0);
                        string antiguedad = lecturaBD.GetString(1);
                        var dato = (edad, antiguedad);
                        extra.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return extra;
        }

        public List<(string, Int64)> GetTelefono(string busca)
        {
            List<(string, Int64)> extra = new List<(string, Int64)>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Direccion,Nro_de_Celular FROM CARTERA_ACT WHERE Documento_de_Identidad= '" + busca + "' OR Nombre_Titular= '" + busca + "'";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string direccion = lecturaBD.GetString(0);
                        Int64 telefono = Convert.ToInt64(lecturaBD.GetString(1));
                        var dato = (direccion, telefono);
                        extra.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return extra;
        }

        public List<(string, string)> GetBarraCAJA(string busca)
        {
            List<(string, string)> extra = new List<(string, string)>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Nro_de_Cuenta,Saldo_en_Bolivianos FROM CAJA_ACT WHERE Numero_de_Identificacion= '" + busca + "' OR Nombre_Cliente= '" + busca + "' ORDER BY CONVERT(FLOAT, Saldo_en_Bolivianos) DESC";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string cuenta = lecturaBD.GetString(0);
                        string saldo = lecturaBD.GetString(1);
                        var dato = (cuenta, saldo);
                        extra.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return extra;
        }

        public List<(string, string)> GetBarraCUENTAS(string busca)
        {
            List<(string, string)> extra = new List<(string, string)>();
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT Nro_de_Cuenta,Saldo_en_Bolivianos FROM CORR_ACT WHERE Numero_de_Identificacion= '" + busca + "' OR Nombre_Cliente= '" + busca + "' ORDER BY CONVERT(FLOAT, Saldo_en_Bolivianos) DESC";
                using (var conexion = new SqlConnection(con.Get_cadConexion()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        string cuenta = lecturaBD.GetString(0);
                        string saldo = lecturaBD.GetString(1);
                        var dato = (cuenta, saldo);
                        extra.Add(dato);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return extra;
        }

        public String consultaRetenciones(string busca)
        {
            string retenido = "";
            try
            {
                var con = new ConexionBDD();
                string query = "SELECT RETENIDO FROM ARCH_SUSPENCION WHERE CARNET= '" + busca + "' OR NOMBRES= '" + busca + "'";
                using (var conexion = new SqlConnection(con.Get_Retenciones()))
                {
                    conexion.Open();
                    SqlCommand cursor = new SqlCommand(query, conexion);
                    SqlDataReader lecturaBD = cursor.ExecuteReader();
                    while (lecturaBD.Read())
                    {
                        retenido = lecturaBD.GetString(0);
                    }
                }
                if (retenido == "")
                {
                    busca = Regex.Replace(busca, @"[^\d]", "");
                    query = "SELECT RETENIDO FROM ARCH_SUSPENCION WHERE CARNET= '" + busca + "' OR NOMBRES= '" + busca + "'";
                    using (var conexion = new SqlConnection(con.Get_Retenciones()))
                    {
                        conexion.Open();
                        SqlCommand cursor = new SqlCommand(query, conexion);
                        SqlDataReader lecturaBD = cursor.ExecuteReader();
                        while (lecturaBD.Read())
                        {
                            retenido = lecturaBD.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
            }
            return retenido;
        }
    }
}
