

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ProyectoSenaLmour.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ProyectoSenaLmour.Datos
{
    public class DBUsuario
    {
        private static string CadenaSQL = "Server=LAPTOP-GTH4PE47;Initial Catalog=Lmour;integrated security=True; TrustServerCertificate=True";


        public static bool Registrar(Usuario usuario)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {

                    string query = "inser into Usuario( Nombres, Correo, Contraseña,  Restablecer , Confirmado, Token   )";
                    query += "values(@nombres.@correo.@contraseña.@restablecer.@confirmado.@token)";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    cmd.Parameters.AddWithValue("@restablecer", usuario.Restablecer);
                    cmd.Parameters.AddWithValue("@confirmado", usuario.Confirmado);
                    cmd.Parameters.AddWithValue("@token", usuario.Token);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;

                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Usuario validar(string correo, string contraseña)
        {
            Usuario usuario = null;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {

                    string query = "select Nombres,  Restablecer , Confirmado, from Usuario";
                    query += "Where Correo=@correo and clave =@clave";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuario()
                            {

                                Nombres = dr["Nombres"].ToString(),
                                Restablecer = (bool)dr["Restablecer"],
                                Confirmado = (bool)dr["Confirmado"]

                            };

                        }


                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }



        public static Usuario Obtener(string correo, string contraseña)
        {
            Usuario usuario = null;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {

                    string query = "select Nombres, Contraseña, Restablecer , Confirmado, Token from Usuario";
                    query += "Where Correo=@correo";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuario()
                            {

                                Nombres = dr["Nombres"].ToString(),
                                Contraseña = dr["Contraseña"].ToString(),
                                Restablecer = (bool)dr["Restablecer"],
                                Confirmado = (bool)dr["Confirmado"],
                                Token = dr["Token"].ToString(),

                            };

                        }


                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }



        public static bool RestablecerActualizar(int restablecer, string contraseña, string token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {

                    string query = @"update Usuario set" +
                        "restablecer=@restablecer," +
                        "Contraseña=@contraseña" +
                        "where Token=@token";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@restablecer", restablecer);
                    cmd.Parameters.AddWithValue("@contraseña", contraseña);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;

                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static bool Confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {

                    string query = @"update Usuario set" +
                        "restablecer=@restablecer," +
                        "Contraseña=@contraseña" +
                        "where Token=@token";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;

                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}

