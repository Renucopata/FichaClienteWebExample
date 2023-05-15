using FichaCliente.Models;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FichaCliente.Scripts
{
    public class Procedimientos
    {
        CultureInfo formato = new CultureInfo("en-US");

        public List<ModeloResumen> DatosCliente(string srch1, string srch2)
        {
            var ListaDatos = new List<ModeloResumen>();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ResumenCliente", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaDatos.Add(new ModeloResumen()
                        {
                            Documento_de_Identidad = dr["Documento_de_Identidad"].ToString(),
                            Nombre_Titular = dr["Nombre_Titular"].ToString(),
                            Tipo_de_Credito = dr["Tipo_de_Credito"].ToString(),
                            Monto_Desembolsado_en_Bolivianos = dr["Monto_Desembolsado_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Monto_Desembolsado_en_Bolivianos"], formato) : 0,
                            Saldo_en_Bolivianos = dr["Saldo_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Saldo_en_Bolivianos"], formato) : 0,
                            Calificacion = dr["Calificacion"] != DBNull.Value ? Convert.ToChar(dr["Calificacion"]) : '-',
                            Tipo_de_Caja_de_Ahorro = dr["Tipo_de_Caja_de_Ahorro"].ToString(),
                            Estado = dr["Estado"].ToString(),
                            Tipo_de_Dpf_Actual = dr["Tipo_de_Dpf_Actual"].ToString(),
                            Monto_de_Capital_Bolivianos = dr["Monto_de_Capital_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Monto_de_Capital_Bolivianos"], formato) : 0,
                            Fecha_de_Vencimiento = Convert.ToDateTime(dr["Fecha_de_Vencimiento"], CultureInfo.GetCultureInfo("es-BO")),
                            Plazo = dr["Plazo"] != System.DBNull.Value ? Convert.ToInt32(dr["Plazo"]) : 0,
                            Tipo_de_Cuenta_Corriente = dr["Tipo_de_Cuenta_Corriente"].ToString(),
                            Estado_CC = dr["Estado_CC"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            Importe_Capital_Bolivianos = dr["Importe_Capital_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Importe_Capital_Bolivianos"], formato) : 0,
                            Saldo_Cartera_Bolivianos = dr["Saldo_Cartera_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Saldo_Cartera_Bolivianos"], formato) : 0
                        });
                    }
                }
            }
            return ListaDatos;
        }

        public List<ModeloCartera> DatosCartera(string srch1, string srch2)
        {
            var ListaDatos = new List<ModeloCartera>();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("DatosCartera", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime auxREP;
                        DateTime auxVEN;
                        int auxtef;
                        if (dr["Fecha_de_Reprogramaciones"].ToString() != "")
                            auxREP = Convert.ToDateTime(dr["Fecha_de_Reprogramaciones"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxREP = DateTime.Parse("01/01/1999");
                        if (dr["Fecha_de_Vencimiento"].ToString() != "")
                            auxVEN = Convert.ToDateTime(dr["Fecha_de_Vencimiento"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxVEN = DateTime.Parse("01/01/1999");
                        if (dr["Nro_de_Celular"].ToString() != "")
                            auxtef = dr["Nro_de_Celular"] != System.DBNull.Value ? Convert.ToInt32(dr["Nro_de_Celular"]) : 0;
                        else
                            auxtef = 0;
                        ListaDatos.Add(new ModeloCartera()
                        {
                            Numero_de_Prestamo = Convert.ToInt64(dr["Numero_de_Prestamo"]),
                            Documento_de_Identidad = dr["Documento_de_Identidad"].ToString(),
                            Nombre_Titular = dr["Nombre_Titular"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            Tipo_de_Credito = dr["Tipo_de_Credito"].ToString(),
                            Monto_Desembolsado_en_Bolivianos = dr["Monto_Desembolsado_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Monto_Desembolsado_en_Bolivianos"], formato) : 0,
                            Saldo_en_Bolivianos = dr["Saldo_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Saldo_en_Bolivianos"], formato) : 0,
                            Calificacion = Convert.ToChar(dr["Calificacion"]),
                            Cantidad_de_Reprogramaciones = dr["Cantidad_de_Reprogramaciones"] != System.DBNull.Value ? Convert.ToInt32(dr["Cantidad_de_Reprogramaciones"]) : 0,
                            Fecha_de_Reprogramaciones = auxREP,
                            Destino = dr["Destino"].ToString(),
                            Dias_de_Incumplimiento = dr["Dias_de_Incumplimiento"] != System.DBNull.Value ? Convert.ToInt32(dr["Dias_de_Incumplimiento"]) : 0,
                            Dias_de_Pase_a_Vencido = dr["Dias_de_Pase_a_Vencido"] != System.DBNull.Value ? Convert.ToInt32(dr["Dias_de_Pase_a_Vencido"]) : 0,
                            Plazo = dr["Plazo"].ToString(),
                            Fecha_de_Vencimiento = auxVEN,
                            Tasa = Convert.ToDouble(dr["Tasa"].ToString(), formato),
                            CAEDEC_Prestamo = dr["CAEDEC_Prestamo"].ToString(),
                            Tiene_seguro_de_Desgravamen = Convert.ToChar(dr["Tiene_seguro_de_Desgravamen"]),
                            Nro_de_Celular =auxtef
                        });
                    }
                }
            }
            return ListaDatos;
        }

        public ModeloCartera DetalleCreditos(string nmp)
        {
            ModeloCartera credito = new ModeloCartera();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("NumerosOperacion", conexion);
                cmd.Parameters.AddWithValue("@numop", nmp);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime auxREP;
                        DateTime auxVEN;
                        int auxtef;
                        if (dr["Fecha_de_Reprogramaciones"].ToString() != "")
                            auxREP = Convert.ToDateTime(dr["Fecha_de_Reprogramaciones"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxREP = DateTime.Parse("01/01/1999");
                        if (dr["Fecha_de_Vencimiento"].ToString() != "")
                            auxVEN = Convert.ToDateTime(dr["Fecha_de_Vencimiento"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxVEN = DateTime.Parse("01/01/1999");
                        if (dr["Nro_de_Celular"].ToString() != "")
                            auxtef = dr["Nro_de_Celular"] != System.DBNull.Value ? Convert.ToInt32(dr["Nro_de_Celular"]) : 0;
                        else
                            auxtef = 0;
                        credito.Numero_de_Prestamo = Convert.ToInt64(dr["Numero_de_Prestamo"]);
                        credito.Documento_de_Identidad = dr["Documento_de_Identidad"].ToString();
                        credito.Nombre_Titular = dr["Nombre_Titular"].ToString();
                        credito.Direccion = dr["Direccion"].ToString();
                        credito.Tipo_de_Credito = dr["Tipo_de_Credito"].ToString();
                        credito.Monto_Desembolsado_en_Bolivianos = dr["Monto_Desembolsado_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Monto_Desembolsado_en_Bolivianos"], formato) : 0;
                        credito.Saldo_en_Bolivianos = dr["Saldo_en_Bolivianos"] != DBNull.Value ? Convert.ToDecimal(dr["Saldo_en_Bolivianos"], formato) : 0;
                        credito.Calificacion = Convert.ToChar(dr["Calificacion"]);
                        credito.Cantidad_de_Reprogramaciones = dr["Cantidad_de_Reprogramaciones"] != System.DBNull.Value ? Convert.ToInt32(dr["Cantidad_de_Reprogramaciones"]) : 0;
                        credito.Fecha_de_Reprogramaciones = auxREP;
                        credito.Destino = dr["Destino"].ToString();
                        credito.Dias_de_Incumplimiento = dr["Dias_de_Incumplimiento"] != System.DBNull.Value ? Convert.ToInt32(dr["Dias_de_Incumplimiento"]) : 0;
                        credito.Dias_de_Pase_a_Vencido = dr["Dias_de_Pase_a_Vencido"] != System.DBNull.Value ? Convert.ToInt32(dr["Dias_de_Pase_a_Vencido"]) : 0;
                        credito.Plazo = dr["Plazo"].ToString();
                        credito.Fecha_de_Vencimiento = auxVEN;
                        credito.Tasa = Convert.ToDouble(dr["Tasa"].ToString(), formato);
                        credito.CAEDEC_Prestamo = dr["CAEDEC_Prestamo"].ToString();
                        credito.Tiene_seguro_de_Desgravamen = Convert.ToChar(dr["Tiene_seguro_de_Desgravamen"]);
                        credito.Nro_de_Celular = auxtef;
                    }
                }
            }
            return credito;
        }

        public List<ModeloCaja> DatosCajas(string srch1, string srch2)
        {
            var ListaDatos = new List<ModeloCaja>();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("DatosCaja", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaDatos.Add(new ModeloCaja()
                        {
                            Nro_de_Cuenta = Convert.ToInt64(dr["Nro_de_Cuenta"]),
                            Numero_de_Identificacion = dr["Numero_de_Identificacion"].ToString(),
                            Numero_de_Nit = dr["Numero_de_Nit"].ToString(),
                            Nombre_Cliente = dr["Nombre_Cliente"].ToString(),
                            Fecha_de_Apertura = Convert.ToDateTime(dr["Fecha_de_Apertura"], CultureInfo.GetCultureInfo("es-BO")),
                            Tipo_de_Caja_de_Ahorro = dr["Tipo_de_Caja_de_Ahorro"].ToString(),
                            Saldo_en_Bolivianos = Convert.ToDecimal(dr["Saldo_en_Bolivianos"], formato),
                            Tasa = Convert.ToDouble(dr["Tasa"].ToString(), formato),
                            Estado = dr["Estado"].ToString(),
                            Edad_Prestatario = Convert.ToDouble(dr["Edad_Prestatario"].ToString(), formato)
                        });
                    }
                }
            }
            return ListaDatos;
        }

        public List<ModeloDpf> DatosDpf(string srch1, string srch2)
        {
            var ListaDatos = new List<ModeloDpf>();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("DatosDpfs", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaDatos.Add(new ModeloDpf()
                        {
                            Numero_de_Idetificacion = dr["Numero_de_Idetificacion"].ToString(),
                            Codigo_de_Nit = dr["Codigo_de_Nit"].ToString(),
                            Nombre_de_la_Cuenta = dr["Nombre_de_la_Cuenta"].ToString(),
                            Tipo_de_Dpf_Actual = dr["Tipo_de_Dpf_Actual"].ToString(),
                            Monto_de_Capital_Bolivianos = Convert.ToDecimal(dr["Monto_de_Capital_Bolivianos"], formato),
                            Plazo = dr["Plazo"] != System.DBNull.Value ? Convert.ToInt32(dr["Plazo"]) : 0,
                            Fecha_de_Vencimiento = Convert.ToDateTime(dr["Fecha_de_Vencimiento"], CultureInfo.GetCultureInfo("es-BO")),
                            Tasa_de_Interes = Convert.ToDouble(dr["Tasa_de_Interes"].ToString(), formato)
                        });
                    }
                }
            }
            return ListaDatos;
        }

        public ModeloTarjetas DatosTarjeta(string srch1, string srch2)
        {
            var tarjetas = new ModeloTarjetas();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ComboxNI", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime auxVEN;
                        DateTime auxIN;
                        if (dr["Fecha_de_Vencimiento"].ToString() != "")
                            auxVEN = Convert.ToDateTime(dr["Fecha_de_Vencimiento"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxVEN = DateTime.Parse("01/01/1999");
                        if (dr["Fecha_de_incumplimiento"].ToString() != "")
                            auxIN = Convert.ToDateTime(dr["Fecha_de_incumplimiento"], CultureInfo.GetCultureInfo("es-BO"));
                        else
                            auxIN = DateTime.Parse("01/01/1999");
                        int dias = 0;
                        int auxD;
                        if (dr["Dias_de_Incumplimiento"] != System.DBNull.Value && int.TryParse(dr["Dias_de_Incumplimiento"].ToString(), out dias))
                        {
                            auxD = dias;
                        }
                        else
                        {
                            auxD = 0;
                        }
                        tarjetas.Nombre_Cliente = dr["Nombre_Cliente"].ToString();
                        tarjetas.Nro_CI = dr["Nro_CI"].ToString();
                        tarjetas.Calificacion = Convert.ToChar(dr["Calificacion"]);
                        tarjetas.Estado = dr["Estado"].ToString();
                        tarjetas.Importe_Capital_Bolivianos = Convert.ToDecimal(dr["Importe_Capital_Bolivianos"], formato);
                        tarjetas.Saldo_Cartera_Bolivianos = Convert.ToDecimal(dr["Saldo_Cartera_Bolivianos"], formato);
                        tarjetas.Interes_Vigente_Bolivianos = Convert.ToDouble(dr["Interes_Vigente_Bolivianos"].ToString(), formato);
                        tarjetas.Interes_Vencido_Bolivianos = Convert.ToDouble(dr["Interes_Vencido_Bolivianos"].ToString(), formato);
                        tarjetas.Tipo = dr["Tipo"].ToString();
                        tarjetas.Tasa = Convert.ToDouble(dr["Tasa"].ToString(), formato);
                        tarjetas.Fecha_de_Vencimiento = auxVEN;
                        tarjetas.Dias_de_Incumplimiento = auxD;
                        tarjetas.Fecha_de_incumplimiento = auxIN;
                        tarjetas.Estado_Tarjeta = dr["Estado_Tarjeta"].ToString();
                    }
                }
            }
            return tarjetas;
        }

        public List<ModeloCorr> DatosCuentas(string srch1, string srch2)
        {
            var ListaDatos = new List<ModeloCorr>();
            var cn = new ConexionBDD();
            using (var conexion = new SqlConnection(cn.Get_cadConexion()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("DetallesDpfs", conexion);
                cmd.Parameters.AddWithValue("ci", srch1);
                cmd.Parameters.AddWithValue("tit", srch2);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaDatos.Add(new ModeloCorr()
                        {
                            Nro_de_Cuenta = Convert.ToInt64(dr["Nro_de_Cuenta"]),
                            Numero_de_Identificacion = dr["Numero_de_Identificacion"].ToString(),
                            Numero_de_Nit = dr["Numero_de_Nit"].ToString(),
                            Nombre_Cliente = dr["Nombre_Cliente"].ToString(),
                            Tipo_de_Caja_de_Ahorro = dr["Tipo_de_Caja_de_Ahorro"].ToString(),
                            Saldo_en_Bolivianos = Convert.ToDecimal(dr["Saldo_en_Bolivianos"], formato),
                            Tasa = Convert.ToDouble(dr["Tasa"].ToString(), formato),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return ListaDatos;
        }
    }
}
