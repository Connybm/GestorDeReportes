﻿using System;
using System.Data.Odbc;

namespace capaDatoRpt.Conexion
{
    public class Conexion
    {
        OdbcConnection conexion;

        public Tuple<OdbcConnection, OdbcTransaction> iniciarConexion()
        {
            conexion = new OdbcConnection("Dsn=seguridad");
            conexion.Open();

            OdbcTransaction transaccion = conexion.BeginTransaction();

            return Tuple.Create(conexion, transaccion);
        }

        public void cerrarConexion()
        {
            conexion.Close();
        }
    }
}
