﻿using System;
using System.Collections.Generic;
using capaDatoRpt.Conexion;
using capaDatoRpt.Entity;
using System.Data.Odbc;
using System.Windows.Forms;

namespace CapaControlRpt.Control
{
    public class ModuloControl
    {
        private Transaccion transaccion = new Transaccion();

        public List<Modulo> obtenerAllModulo()
        {
            List<Modulo> moduloList = new List<Modulo>();
            try
            {
                String sComando = String.Format("SELECT PK_ID_MODULO, NOMBRE_MODULO, DESCRIPCION_MODULO, ESTADO_MODULO " +
                    "FROM TBL_MODULO " +
                    "WHERE ESTADO_MODULO <> 0; ");

                OdbcDataReader reader = transaccion.ConsultarDatos(sComando);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Modulo moduloTmp = new Modulo();
                        moduloTmp.MODULO = reader.GetInt32(0);
                        moduloTmp.NOMBRE= reader.GetString(1);
                        moduloTmp.DESCRIPCION = reader.IsDBNull(2) ? " " : reader.GetString(2);
                        moduloTmp.ESTADO = reader.GetInt32(3);
                        moduloList.Add(moduloTmp);
                    }
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show(ex.ToString(), "Error al obtener lista de modulos.");
                return null;
            }

            return moduloList;
        }

        public Modulo obtenerModulo(int modulo)
        {
            Modulo moduloTmp = new Modulo();
            try
            {
                String sComando = String.Format("SELECT PK_ID_MODULO, NOMBRE_MODULO, DESCRIPCION_MODULO, ESTADO_MODULO " +
                    "FROM TBL_MODULO " +
                    "WHERE ESTADO_MODULO <> 0 " +
                    " AND PK_ID_MODULO = {0}; ", modulo.ToString());

                OdbcDataReader reader = transaccion.ConsultarDatos(sComando);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        moduloTmp.MODULO = reader.GetInt32(0);
                        moduloTmp.NOMBRE = reader.GetString(1);
                        moduloTmp.DESCRIPCION = (reader.IsDBNull(2) ? " " : reader.GetString(2));
                        moduloTmp.ESTADO = reader.GetInt32(3);
                    }
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show(ex.ToString(), "Error al obtener lista de modulos.");
                return null;
            }

            return moduloTmp;
        }
    }
}
